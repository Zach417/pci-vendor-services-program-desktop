using DataIntegrationHub.Business.Entities;
using DataIntegrationHub.Business.Components;

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
using System.Data.SqlTypes;
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
        private Pagination paginationPlanAdvisors;

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

            SecurityComponent securityComponent = new SecurityComponent(CurrentUser);
            if (securityComponent.IsAdmin() == false)
            {
                lblSettings.Visible = false;
            }

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
            cboPlanAdvisorViews.SelectedIndex = 0;
            cboViewsCategory.SelectedIndex = 0;
            cboAuditorViews.SelectedIndex = 0;
            cboClientViews.SelectedIndex = 0;
            cboSearchViews.SelectedIndex = 0;
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
                    MessageBox.Show("You do not sufficient security privileges to use this application. Please see your system administrator.");
                    return false;
                }
            }
            catch (Exception ex)
            {
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
            dgvServices.Columns["CreatedOn"].Visible = false;
            dgvServices.Columns["ModifiedBy"].Visible = false;
            dgvServices.Columns["StateCode"].Visible = false;

            dgvServices.Columns["Name"].DisplayIndex = 0;
            dgvServices.Columns["Category"].DisplayIndex = 1;
            dgvServices.Columns["Type"].DisplayIndex = 2;
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

        private void txtRkSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadDgvRecordKeeper();
            }
        }

        private void LoadDgvAuditors()
        {
            DataTable dataTable = DataIntegrationHub.Business.Entities.Auditor.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRecordKeeperViews"/>.
            switch (cboAuditorViews.SelectedIndex)
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

            if (String.IsNullOrWhiteSpace(txtAuditorSearch.Text) == false)
            {
                dataTableEnum = dataTableEnum.Where(x => x.Field<string>("Name").ToUpper().Contains(txtAuditorSearch.Text.ToUpper()));
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvAuditors.DataSource = dataTable;

            // Display/order the columns.
            dgvAuditors.Columns["AuditorId"].Visible = false;
            dgvAuditors.Columns["Name"].DisplayIndex = 0;
            dgvAuditors.Columns["CreatedBy"].Visible = false;
            dgvAuditors.Columns["ModifiedBy"].Visible = false;
            dgvAuditors.Columns["StateCode"].Visible = false;

            dgvAuditors.Columns["Name"].DisplayIndex = 0;
            dgvAuditors.Columns["CreatedOn"].DisplayIndex = 1;
            dgvAuditors.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        private void cboAuditorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvAuditors();
        }

        private void txtAuditorSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadDgvAuditors();
            }
        }

        private void frmAuditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvAuditors();
        }

        private void dgvAuditors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvAuditors.CurrentRow.Index;
            Guid auditorId = new Guid(dgvAuditors.Rows[index].Cells["AuditorId"].Value.ToString());
            VSP.Business.Entities.Auditor auditor = new VSP.Business.Entities.Auditor(auditorId);
            frmAuditor frmAuditor = new frmAuditor(this, auditor);
            frmAuditor.FormClosed += frmAuditor_FormClosed;
        }

        private void LoadDgvPlanAdvisors()
        {
            DataTable dataTable = DataIntegrationHub.Business.Entities.PlanAdvisor.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboPlanAdvisorViews"/>.
            switch (cboPlanAdvisorViews.SelectedIndex)
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

            if (String.IsNullOrWhiteSpace(txtPlanAdvisorSearch.Text) == false)
            {
                dataTableEnum = dataTableEnum.Where(x => x.Field<string>("Name").ToUpper().Contains(txtPlanAdvisorSearch.Text.ToUpper()));
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvPlanAdvisors.DataSource = dataTable;

            // Display/order the columns.
            dgvPlanAdvisors.Columns["PlanAdvisorId"].Visible = false;
            dgvPlanAdvisors.Columns["CreatedBy"].Visible = false;
            dgvPlanAdvisors.Columns["ModifiedBy"].Visible = false;
            dgvPlanAdvisors.Columns["StateCode"].Visible = false;

            dgvPlanAdvisors.Columns["Name"].DisplayIndex = 0;
            dgvPlanAdvisors.Columns["CreatedOn"].DisplayIndex = 1;
            dgvPlanAdvisors.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        private void cboAdvisorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvPlanAdvisors();
        }

        private void txtAdvSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadDgvPlanAdvisors();
            }
        }

        private void frmPlanAdvisor_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvPlanAdvisors();
        }

        private void dgvPlanAdvisors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvPlanAdvisors.CurrentRow.Index;
            Guid planAdvisorId = new Guid(dgvPlanAdvisors.Rows[index].Cells["PlanAdvisorId"].Value.ToString());
            VSP.Business.Entities.Advisor planAdvisor = new VSP.Business.Entities.Advisor(planAdvisorId);
            frmAdvisor frmPlanAdvisor = new frmAdvisor(this, planAdvisor);
            frmPlanAdvisor.FormClosed += frmPlanAdvisor_FormClosed;
        }

        private void LoadDgvClients()
        {
            DataTable dataTable = DataIntegrationHub.Business.Entities.Customer.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboClientViews"/>.
            switch (cboClientViews.SelectedIndex)
            {
                case 0: //Active Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<byte>("StateCode") == 0);
                    break;
                case 1: // Active 3(21) Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<bool>("Is321"));
                    break;
                case 2: // Active 3(38) Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<bool>("Is338"));
                    break;
                case 3: // Active EC Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<bool>("IsEC"));
                    break;
                case 4: // Active VM Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<bool>("IsVM"));
                    break;
                case 5: // Inactive Clients
                    dataTableEnum = dataTableEnum.Where(x => x.Field<byte>("StateCode") == 1);
                    break;
                default:
                    return;
            }

            if (String.IsNullOrWhiteSpace(txtClientSearch.Text) == false)
            {
                dataTableEnum = dataTableEnum.Where(x => x.Field<string>("Name").ToUpper().Contains(txtClientSearch.Text.ToUpper()));
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvClients.DataSource = dataTable;

            // Display/order the columns.
            dgvClients.Columns["CustomerId"].Visible = false;
            dgvClients.Columns["PrimaryContactId"].Visible = false;
            dgvClients.Columns["AssetValue"].Visible = false;
            dgvClients.Columns["MainPhone"].Visible = false;
            dgvClients.Columns["Fax"].Visible = false;
            dgvClients.Columns["AddressLine1"].Visible = false;
            dgvClients.Columns["AddressLine2"].Visible = false;
            dgvClients.Columns["AddressCity"].Visible = false;
            dgvClients.Columns["AddressState"].Visible = false;
            dgvClients.Columns["AddressZip"].Visible = false;
            dgvClients.Columns["Is321"].Visible = false;
            dgvClients.Columns["Is338"].Visible = false;
            dgvClients.Columns["IsEC"].Visible = false;
            dgvClients.Columns["IsVM"].Visible = false;
            dgvClients.Columns["ModifiedBy"].Visible = false;
            dgvClients.Columns["Createdon"].Visible = false;
            dgvClients.Columns["CreatedBy"].Visible = false;
            dgvClients.Columns["StateCode"].Visible = false;

            dgvClients.Columns["Name"].DisplayIndex = 0;
            dgvClients.Columns["BusinessUnit"].DisplayIndex = 1;
            dgvClients.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        private void cboClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvClients();
        }

        private void txtClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadDgvClients();
            }
        }

        private void dgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvClients.CurrentRow.Index;
            Guid customerId = new Guid(dgvClients.Rows[index].Cells["CustomerId"].Value.ToString());
            Customer customer = new Customer(customerId);
            frmAccount frmAccount = new frmAccount(this, customer);
            frmAccount.FormClosed += frmAccount_FormClosed;
        }

        private void frmAccount_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvClients();
        }

        private void btnNewSearch_Click(object sender, EventArgs e)
        {
            frmSearch frmSearch = new frmSearch(this);
            frmSearch.FormClosed += frmSearch_FormClosed;
        }

        private void LoadDgvSearches()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboSearchViews"/>.
            switch (cboSearchViews.SelectedIndex)
            {
                case 0:
                    dataTable = Search.GetActive();
                    break;
                case 1:
                    dataTable = Search.GetInactive();
                    break;
                default:
                    return;
            }

            dgvSearches.DataSource = dataTable;

            // Display/order the columns.
            dgvSearches.Columns["SearchId"].Visible = false;
            dgvSearches.Columns["PlanId"].Visible = false;
            dgvSearches.Columns["CurrentRkNotes"].Visible = false;
            dgvSearches.Columns["CreatedBy"].Visible = false;
            dgvSearches.Columns["ModifiedBy"].Visible = false;
            dgvSearches.Columns["StateCode"].Visible = false;

            dgvSearches.Columns["Name"].DisplayIndex = 0;
            dgvSearches.Columns["CreatedOn"].DisplayIndex = 1;
            dgvSearches.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        void frmSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvSearches();
        }

        private void cboSearchViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvSearches();
        }

        private void dgvSearches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvSearches.CurrentRow.Index;
            Guid searchId = new Guid(dgvSearches.Rows[index].Cells["SearchId"].Value.ToString());
            Search search = new Search(searchId);
            frmSearch frmSearch = new frmSearch(this, search);
            frmSearch.FormClosed += frmSearch_FormClosed;
        }

        private void btnDeleteSearch_Click(object sender, EventArgs e)
        {
            if (dgvSearches.CurrentRow == null)
            {
                return;
            }

            int index = dgvSearches.CurrentRow.Index;
            Guid searchId = new Guid(dgvSearches.Rows[index].Cells[0].Value.ToString());
            Search search = new Search(searchId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete " + search.Name + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                search.DeleteRecordFromDatabase();
                LoadDgvSearches();
            }
        }
    }
}