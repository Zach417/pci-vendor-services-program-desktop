using DataIntegrationHub.Business.Entities;
using DataIntegrationHub.Business.Components;

using VSP;
using VSP.Business.Components;
using VSP.Business.Entities;
using VSP.Presentation;
using VSP.Presentation.Utilities;
using VSP.Security;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using FastMember;

namespace VSP.Presentation.Forms
{
    public partial class frmMain : Form, IMessageFilter
    {
        #region IMessageFilter Members

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> controlsToMove = new HashSet<Control>();

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }

        #endregion

        public SecurityComponent Security;

        internal int openTaskNotifications = 0;
        internal User CurrentUser;
        internal UserLogin CurrentLoginSession;

        private Stopwatch stopWatch = new Stopwatch();
        private Pagination paginationAdvisors;

        /// <summary>
        /// Represents the main form of the ISP application.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;

            #region IMessageFilter Methods

            //Add controls to move the form
            Application.AddMessageFilter(this);
            controlsToMove.Add(this.lblFormHeader);
            controlsToMove.Add(this.panel6);
            controlsToMove.Add(this.panel10);
            controlsToMove.Add(this.pnlMainHeader);

            #endregion

            if (!ConnectionSucceeded())
            {
                return;
            }

            bool isAccessUser = LoginCurrentUser();

            if (isAccessUser == false)
            {
                this.Enabled = false;
                this.Hide();
                return;
            }

            LoadAutoCompleteAdvisors();

            HandleAppVersion();

            //Start app with the dashboard tab
            tabMain.SelectedIndex = 8;

            SetDefaultComboBoxValues();
        }

        /// <summary>
        /// Sets the default values for all ComboBoxes within the VSP.
        /// </summary>
        private void SetDefaultComboBoxValues()
        {
            cboRecordKeeperViews.SelectedIndex = 0;
            cboAdvisorViews.SelectedIndex = 0;
            cboViewsCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// Checks the version of the application and sets <see cref="lblFormHeader"/>.
        /// </summary>
        private void HandleAppVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                lblFormHeader.Text = lblFormHeader.Text + " - v" + version;
            }
        }

        /// <summary>
        /// Gets the security status of the current user and enables features based on their status.
        /// </summary>
        /// <returns>true if the current user has sufficient permission to use the application.</returns>
        /// <remarks>
        /// This method uses the <see cref="SecurityComponent(User)"/> to grab security roles from the
        /// Data Integration Hub on PCI-DB. It closes frmMain if an error occurs.
        /// </remarks>
        private bool LoginCurrentUser()
        {
            // Attempt to log the user in
            try
            {
                CurrentUser = new User(Environment.UserDomainName + "\\" + Environment.UserName);
                Security = new SecurityComponent(CurrentUser);
                lblLoginStatus.Text = "You are logged in as: " + CurrentUser.DomainName;

                if (Security.IsAdmin() == false)
                {
                    lblSettings.Enabled = false;
                    lblSettings.Visible = false;
                }

                if (Security.IsUser() == false && Security.IsAdmin() == false)
                {
                    MessageBox.Show("You do not sufficient security privileges to user this application. Please see your system administrator.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                lblLoginStatus.Text = "An error occurred while logging you in.";
                frmError frmError = new frmError(this, ex);
                frmError.FormClosed += frmError_FormClosedEventHandler;
                return false;
            }

            BeginCurrentSession();

            return true;
        }

        private void BeginCurrentSession()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string publishVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();

                CurrentLoginSession = new UserLogin();
                CurrentLoginSession.UserId = CurrentUser.UserId;
                CurrentLoginSession.PublishVersion = publishVersion;
                CurrentLoginSession.SessionStart = DateTime.Now;
                CurrentLoginSession.SaveRecordToDatabase(CurrentUser.UserId);
            }
        }

        private void UpdateCurrentSessionLength()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                CurrentLoginSession.SessionEnd = DateTime.Now;
                CurrentLoginSession.SaveRecordToDatabase(CurrentUser.UserId);
            }
        }

        private void EndCurrentSession()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                CurrentLoginSession.SessionEnd = DateTime.Now;
                CurrentLoginSession.SaveRecordToDatabase(CurrentUser.UserId);
            }
        }

        /// <summary>
        /// Closes <see cref="frmMain"/> when <see cref="frmError"/> is closed.
        /// </summary>
        /// <param name="sender">Provides a reference to the frmError sender instance.</param>
        /// <param name="e">Provides data for the System.Windows.Forms.Form.FormClosed event.</param>
        /// <remarks>
        /// This should only be used when the thrown exception would prevent the user from using the application.
        /// For example, if the application could not connect to the database or if the current user could
        /// not be logged in, then the form should be closed because it would be unusable.
        /// </remarks>
        private void frmError_FormClosedEventHandler(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clears and adds string values to the AutoCompleteCustomSource of <see cref="txtAdvisorSearch"/>.
        /// </summary>
        private void LoadAutoCompleteAdvisors()
        {
            txtAdvisorSearch.AutoCompleteCustomSource.Clear();

            foreach (DataRow dr in UserSearches.GetAssociatedFromTable(CurrentUser.UserId, "Advisors").Rows)
            {
                txtAdvisorSearch.AutoCompleteCustomSource.Add(dr["SearchText"].ToString());
            }
        }

        /// <summary>
        /// Checks if the application can connect to the relevant databases and servers.
        /// </summary>
        /// <returns>Returns true if the connection did succeed.</returns>
        private bool ConnectionSucceeded()
        {
            this.Enabled = false;

            if (Access.ConnectionSucceeded())
            {
                this.Enabled = true;
                return true;
            }

            return false;
        }

        private void cboAdvViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            paginationAdvisors = new Pagination(dgvAdvisors, Business.Entities.Advisors.GetActive());
            dgvAdvisors.Columns[0].Visible = false;
        }

        private void dgvAdvisors_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvAdvisors.CurrentRow.Index;
            lblSelectedAdvisor.Text = dgvAdvisors.Rows[index].Cells[1].Value.ToString();
        }

        private void frmAdvisor_FormClosed(object sender, FormClosedEventArgs e)
        {
            paginationAdvisors = new Pagination(dgvAdvisors, Business.Entities.Advisors.GetActive());
            dgvAdvisors.Columns[0].Visible = false;
        }

        private void NewAdvisor(object sender, EventArgs e)
        {
            frmAdvisor frmAdvisor = new frmAdvisor(this);
            frmAdvisor.FormClosed += frmAdvisor_FormClosed;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = dgvAdvisors.CurrentRow.Index;
            Guid advisorId = new Guid(dgvAdvisors.Rows[index].Cells[0].Value.ToString());
            frmAdvisor frmAdvisor = new frmAdvisor(this, advisorId);
            frmAdvisor.FormClosed += frmAdvisor_FormClosed;
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvAdvisors.CurrentRow.Index;
            Guid advisorId = new Guid(dgvAdvisors.Rows[index].Cells[0].Value.ToString());
            frmAdvisor frmAdvisor = new frmAdvisor(this, advisorId);
            frmAdvisor.FormClosed += frmAdvisor_FormClosed;
        }

        private void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.HotTrack;
            label.BackColor = System.Drawing.Color.Gainsboro;
        }

        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.ControlText;
            label.BackColor = System.Drawing.Color.Silver;
        }

        private void ReportTypeMenuItem_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.HotTrack;
            label.BackColor = System.Drawing.Color.Silver;
        }

        private void ReportTypeMenuItem_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.ControlText;
            label.BackColor = System.Drawing.Color.DarkGray;
        }

        private void ReportTypeSubMenuItem_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.HotTrack;
            label.BackColor = System.Drawing.Color.LightSteelBlue;
        }

        private void ReportTypeSubMenuItem_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.Color.White;
            label.BackColor = System.Drawing.Color.LightSlateGray;
        }

        private void lblAdvisorsClients_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 2;
        }

        private void label49_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 4;
        }

        private void menuUnderlineHandler(Label label)
        {
            lblClients.Font = new Font(lblClients.Font.Name, lblClients.Font.SizeInPoints, FontStyle.Regular);
            lblSettings.Font = new Font(lblSettings.Font.Name, lblSettings.Font.SizeInPoints, FontStyle.Regular);

            label.Font = new Font(label.Font.Name, label.Font.SizeInPoints, FontStyle.Underline);
        }

        private void CloseFormButton_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.LightGray;
        }

        private void CloseFormButton_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.Color.White;
            label.BackColor = System.Drawing.Color.Transparent;
        }

        private void CloseForm(object sender, EventArgs e)
        {
            EndCurrentSession();
            this.Close();
        }

        private void MaximizeForm(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

            // We have to refresh ComboBoxes because they don't draw well after performing this function.
            foreach (ComboBox comboBox in GetAll(this, typeof(ComboBox)))
            {
                comboBox.Refresh();
            }

            Application.DoEvents();
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void label33_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 3;
        }

        private void label29_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 0;
        }

        private void dgvAdvBack_Click(object sender, EventArgs e)
        {
            paginationAdvisors.PageBackward();
        }

        private void dgvAdvForward_Click(object sender, EventArgs e)
        {
            paginationAdvisors.PageForward();
        }

        private void dgvAdvisors_Sorted(object sender, EventArgs e)
        {
            DataGridViewColumn column = dgvAdvisors.SortedColumn;
            System.Windows.Forms.SortOrder sortOrder = dgvAdvisors.SortOrder;

            paginationAdvisors.Sort(column.Name, sortOrder.ToString());
        }

        private void txtAdvSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                paginationAdvisors = new Pagination(dgvAdvisors, Business.Entities.Advisors.SearchActive(txtAdvisorSearch.Text));
                dgvAdvisors.Columns[0].Visible = false;

                if (!String.IsNullOrEmpty(txtAdvisorSearch.Text))
                {
                    UserSearches userSearch = new UserSearches();
                    userSearch.SearchText = txtAdvisorSearch.Text;
                    userSearch.SearchTable = "Advisors";
                    userSearch.SaveRecordToDatabase(CurrentUser.UserId);

                    txtAdvisorSearch.AutoCompleteCustomSource.Clear();

                    foreach (DataRow dr in Business.Entities.UserSearches.GetAssociatedFromTable(CurrentUser.UserId, "Advisors").Rows)
                        txtAdvisorSearch.AutoCompleteCustomSource.Add(dr["SearchText"].ToString());
                }
            }
        }

        private void lblMinForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// Recursively sets the text of a control with the text "Loading".
        /// </summary>
        /// <param name="label">Used to get and set loading text.</param>
        /// <remarks>
        /// Will automatically set the text of the control to "Loading" if
        /// it does not already start with "Loading".
        /// </remarks>
        private void CalculateLoadingAnimation(Label label)
        {
            if (label.Text == "Loading..." || label.Text.StartsWith("Loading") == false)
            {
                label.Text = "Loading";
            }
            else
            {
                label.Text = label.Text + ".";
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int index = dgvAdvisors.CurrentRow.Index;
            Guid advisorId = new Guid(dgvAdvisors.Rows[index].Cells[0].Value.ToString());
            Advisors advisor = new Advisors(advisorId);
            advisor.DeleteRecordFromDatabase();
            paginationAdvisors = new Pagination(dgvAdvisors, Business.Entities.Advisors.GetActive());
            dgvAdvisors.Columns[0].Visible = false;
        }

        private void lblClients_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabClients"];
        }

        private void lblRks_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabRks"];
        }

        private void lblAuditors_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabAuditors"];
        }

        private void lblAdvisors_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabAdvisors"];
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabSearch"];
        }

        private void lblSettings_Click(object sender, EventArgs e)
        {
            tabMain.SelectedTab = tabMain.TabPages["tabSettings"];
            cboUsersViews.SelectedIndex = 0;
            LoadDgvUsers();
        }

        private void label89_Click(object sender, EventArgs e)
        {
            tabControlSettings.SelectedTab = tabControlSettings.TabPages["tabUsers"];
            cboUsersViews.SelectedIndex = 0;
            LoadDgvUsers();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            tabControlSettings.SelectedTab = tabControlSettings.TabPages["tabServices"];
            cboServiceViews.SelectedIndex = 0;
            LoadDgvServices();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            frmService frmService = new frmService(this);
            frmService.FormClosed += frmService_FormClosed;
        }

        private void frmService_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvServices();
        }

        private void LoadDgvServices()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboServiceViews"/>.
            switch (cboServiceViews.SelectedIndex)
            {
                case 0:
                    dataTable = Service.GetActive();
                    break;
                case 1:
                    dataTable = Service.GetInactive();
                    break;
                default:
                    return;
            }

            dgvServices.DataSource = dataTable;

            // Display/order the columns.
            dgvServices.Columns["ServiceId"].Visible = false;
            dgvServices.Columns["CreatedBy"].Visible = false;
            dgvServices.Columns["ModifiedBy"].Visible = false;
            dgvServices.Columns["StateCode"].Visible = false;

            dgvServices.Columns["Name"].DisplayIndex = 0;
            dgvServices.Columns["Category"].DisplayIndex = 1;
            dgvServices.Columns["CreatedOn"].DisplayIndex = 2;
            dgvServices.Columns["ModifiedOn"].DisplayIndex = 3;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (dgvServices.CurrentRow == null)
            {
                return;
            }

            int index = dgvServices.CurrentRow.Index;
            Guid serviceId = new Guid(dgvServices.Rows[index].Cells[0].Value.ToString());
            Service service = new Service(serviceId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete " + service.Name + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                service.DeleteRecordFromDatabase();
                LoadDgvServices();
            }
        }

        private void cboServiceViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvServices();
        }

        private void LoadDgvUsers()
        {
            List<User> list = new List<User>();

            /// Set the datatable based on the SelectedIndex of <see cref="cboUsersViews"/>.
            switch (cboUsersViews.SelectedIndex)
            {
                case 0:
                    list = User.ActiveUsers();
                    break;
                case 1:
                    list = User.AllUsers().Where(x => x.StateCode == 1).ToList();
                    break;
                default:
                    return;
            }

            // convert list to datatable
            DataTable dataTable = new DataTable();
            using (var reader = ObjectReader.Create(list)) {
                dataTable.Load(reader);
            }

            dgvUsers.DataSource = dataTable;

            // Display/order the columns.
            dgvUsers.Columns["UserId"].Visible = false;
            dgvUsers.Columns["FirstName"].Visible = false;
            dgvUsers.Columns["LastName"].Visible = false;
            dgvUsers.Columns["EmailAddress"].Visible = false;
            dgvUsers.Columns["MainPhone"].Visible = false;
            dgvUsers.Columns["CreatedBy"].Visible = false;
            dgvUsers.Columns["CreatedOn"].Visible = false;
            dgvUsers.Columns["ModifiedBy"].Visible = false;
            dgvUsers.Columns["ModifiedOn"].Visible = false;
            dgvUsers.Columns["StateCode"].Visible = false;

            dgvUsers.Columns["DomainName"].DisplayIndex = 0;
            dgvUsers.Columns["FullName"].DisplayIndex = 1;
            dgvUsers.Columns["Title"].DisplayIndex = 2;
        }

        private void cboUsersViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvUsers();
        }

        private void frmUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvUsers();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvUsers.CurrentRow.Index;
            Guid userId = new Guid(dgvUsers.Rows[index].Cells["UserId"].Value.ToString());
            User user = new User(userId);
            frmUser frmUser = new frmUser(this, user);
            frmUser.FormClosed += frmUser_FormClosed;
        }

        private void dgvServices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvServices.CurrentRow.Index;
            Guid serviceId = new Guid(dgvServices.Rows[index].Cells["ServiceId"].Value.ToString());
            Service service = new Service(serviceId);
            frmService frmService = new frmService(this, service);
            frmService.FormClosed += frmService_FormClosed;
        }

        private void frmRecordKeeper_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvRecordKeeper();
        }

        private void LoadDgvRecordKeeper()
        {
            DataTable dataTable = DataIntegrationHub.Business.Entities.RecordKeeper.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRecordKeeperViews"/>.
            switch (cboRecordKeeperViews.SelectedIndex)
            {
                case 0:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<byte>("StateCode") == 0);
                    break;
                case 1:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<byte>("StateCode") == 1);
                    break;
                default:
                    return;
            }

            if (String.IsNullOrWhiteSpace(txtRkSearch.Text) == false)
            {
                dataTableEnum = dataTableEnum.Where(x => x.Field<string>("Name").ToUpper().Contains(txtRkSearch.Text.ToUpper()));
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvRecordKeepers.DataSource = dataTable;

            // Display/order the columns.
            dgvRecordKeepers.Columns["RecordKeeperId"].Visible = false;
            dgvRecordKeepers.Columns["PrimaryRfpContactName"].Visible = false;
            dgvRecordKeepers.Columns["PrimaryRfpContactEmail"].Visible = false;
            dgvRecordKeepers.Columns["Name"].DisplayIndex = 0;
            dgvRecordKeepers.Columns["CreatedBy"].Visible = false;
            dgvRecordKeepers.Columns["ModifiedBy"].Visible = false;
            dgvRecordKeepers.Columns["StateCode"].Visible = false;

            dgvRecordKeepers.Columns["Name"].DisplayIndex = 0;
            dgvRecordKeepers.Columns["CreatedOn"].DisplayIndex = 1;
            dgvRecordKeepers.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        private void cboRecordKeeperViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvRecordKeeper();
        }

        private void dgvRecordKeepers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvRecordKeepers.CurrentRow.Index;
            Guid recordKeeperId = new Guid(dgvRecordKeepers.Rows[index].Cells["RecordKeeperId"].Value.ToString());
            VSP.Business.Entities.RecordKeeper recordKeeper = new VSP.Business.Entities.RecordKeeper(recordKeeperId);
            frmRecordKeeper frmRecordKeeper = new frmRecordKeeper(this, recordKeeper);
            frmRecordKeeper.FormClosed += frmRecordKeeper_FormClosed;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadDgvRecordKeeper();
            }
        }
    }
}