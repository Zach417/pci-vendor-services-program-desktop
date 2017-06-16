using DataIntegrationHub.Business.Entities;

using VSP.Business.Entities;
using VSP.Presentation;
using VSP.Presentation.Utilities;
using PensionConsultants.Data.Utilities;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using FastMember;

namespace VSP.Presentation.Forms
{
	public partial class frmUser : Form, IMessageFilter
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> controlsToMove = new HashSet<Control>();

        private frmMain frmMain_Parent;
        public User CurrentUser;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmUser(frmMain mf, User user, FormClosedEventHandler Close = null)
        {
            frmSplashScreen ss = new frmSplashScreen();
            ss.Show();
            Application.DoEvents();

            InitializeComponent();

            frmMain_Parent = mf;

            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;

            Application.AddMessageFilter(this);
            controlsToMove.Add(this.pnlSummaryTabHeader);
            controlsToMove.Add(this.panel16);
            controlsToMove.Add(this.label1);
            controlsToMove.Add(this.label23);

            FormClosed += Close;

            CurrentUser = user;
            txtFullName.Text = CurrentUser.FullName;
            txtDomainName.Text = CurrentUser.DomainName;

            cboRolesViews.SelectedIndex = 0;

            LoadDgvRoles();

            ss.Close();
            this.Show();
		}

        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
        /// </returns>
        /// <remarks>
        /// Use <see cref="PreFilterMessage"/> to filter out a message before it is dispatched to a control or form.
        /// For example, to stop the Click event of a Button control from being dispatched to the control, you implement
        /// the <see cref="PreFilterMessage"/> method and return a true value when the Click message occurs. You can
        /// also use this method to perform code work that you might need to do before the message is dispatched.
        /// </remarks>
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

        private void CloseFormButton_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.Color.Black;
            label.BackColor = System.Drawing.Color.Lavender;
        }

        private void CloseFormButton_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.Color.White;
            label.BackColor = System.Drawing.Color.Transparent;
        }

        private void CloseForm(object sender, EventArgs e)
        {
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

        private void lblSummary(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabSummary"];
        }

        private void lblRoles(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlMain.SelectedTab = tabControlMain.TabPages["tabRoles"];
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
            label.BackColor = System.Drawing.Color.Transparent;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtFullName.Text;
        }

        private void LoadDgvRoles()
        {
            List<VSP.Business.Entities.SecurityRole> list = new List<VSP.Business.Entities.SecurityRole>();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRolesViews"/>.
            switch (cboRolesViews.SelectedIndex)
            {
                case 0:
                    list = VSP.Business.Entities.UserSecurityRole.AssociatedSecurityRolesFromUser(CurrentUser.UserId);
                    break;
                default:
                    return;
            }

            // convert list to datatable
            DataTable dataTable = new DataTable();
            using (var reader = ObjectReader.Create(list)) {
                dataTable.Load(reader);
            }

            dgvRoles.DataSource = dataTable;

            // Display/order the columns.
            dgvRoles.Columns["Id"].Visible = false;
            dgvRoles.Columns["CreatedBy"].Visible = false;
            dgvRoles.Columns["CreatedOn"].Visible = false;
            dgvRoles.Columns["ModifiedBy"].Visible = false;
            dgvRoles.Columns["ModifiedOn"].Visible = false;
            dgvRoles.Columns["StateCode"].Visible = false;

            dgvRoles.Columns["DbAccess"].Visible = false;
            dgvRoles.Columns["EntityMembersAsOf"].Visible = false;
            dgvRoles.Columns["ExistingRecord"].Visible = false;
            dgvRoles.Columns["TableName"].Visible = false;

            dgvRoles.Columns["Name"].DisplayIndex = 0;
            dgvRoles.Columns["Description"].DisplayIndex = 1;
        }

        private void cboRolesViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvRoles();
        }

        private void frmSelectRecord_RecordSelected(object sender, EventArgs e)
        {
            Guid securityRoleId = (Guid)sender;
            VSP.Business.Entities.UserSecurityRole userSecurityRole = new VSP.Business.Entities.UserSecurityRole();
            userSecurityRole.SecurityRoleId = securityRoleId;
            userSecurityRole.UserId = CurrentUser.UserId;
            userSecurityRole.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            LoadDgvRoles();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            // convert list to datatable
            DataTable dataTable = new DataTable();
            List<VSP.Business.Entities.SecurityRole> list = VSP.Business.Entities.SecurityRole.ActiveSecurityRoles();
            using (var reader = ObjectReader.Create(list)) {
                dataTable.Load(reader);
            }

            dataTable.Columns.Remove("DbAccess");
            dataTable.Columns.Remove("EntityMembersAsOf");
            dataTable.Columns.Remove("ExistingRecord");
            dataTable.Columns.Remove("TableName");
            dataTable.Columns.Remove("RegisteredColumns");
            dataTable.Columns.Remove("Description");
            dataTable.Columns.Remove("CreatedBy");
            dataTable.Columns.Remove("CreatedOn");
            dataTable.Columns.Remove("ModifiedBy");
            dataTable.Columns.Remove("ModifiedOn");
            dataTable.Columns.Remove("StateCode");

            frmSelectRecord frmSelectRecord = new frmSelectRecord(frmMain_Parent, frmSelectRecord.RecordType.SecurityRole, CurrentUser, dataTable);
            frmSelectRecord.RecordSelected += frmSelectRecord_RecordSelected;
        }

        private void btnRemoveRole_Click(object sender, EventArgs e)
        {
            // get selected security role id
            int index = dgvRoles.CurrentRow.Index;
            Guid securityRoleId = new Guid(dgvRoles.Rows[index].Cells["Id"].Value.ToString());

            // get all user security roles in list data type
            DataTable dtUserSecurityRole = VSP.Business.Entities.UserSecurityRole.GetActive();
            List<VSP.Business.Entities.UserSecurityRole> userSecurityRoles = new List<VSP.Business.Entities.UserSecurityRole>();
            foreach (DataRow row in dtUserSecurityRole.Rows)
            {
                Guid userSecurityRoleId = new Guid(row[0].ToString());
                var userSecurityRole = new VSP.Business.Entities.UserSecurityRole(userSecurityRoleId);
                userSecurityRoles.Add(userSecurityRole);
            }

            // delete associated user security roles w/ selected security role
            var userSecurityRolesMatch = userSecurityRoles.FindAll(x => x.UserId == CurrentUser.UserId && x.SecurityRoleId == securityRoleId);
            foreach (VSP.Business.Entities.UserSecurityRole userSecurityRole in userSecurityRolesMatch)
            {
                userSecurityRole.DeleteRecordFromDatabase();
            }

            LoadDgvRoles();
        }
	}
}
