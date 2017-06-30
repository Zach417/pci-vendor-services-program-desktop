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

namespace VSP.Presentation.Forms
{
	public partial class frmPlan : Form, IMessageFilter
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
        public Plan CurrentPlan;
        public PlanDetail CurrentPlanDetail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmPlan(frmMain mf, Plan plan, FormClosedEventHandler Close = null)
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

            CurrentPlan = plan;
            txtName.Text = CurrentPlan.Name;

            cboRkViews.SelectedIndex = 0;
            cboAdvisorViews.SelectedIndex = 0;
            cboAuditorViews.SelectedIndex = 0;
            cboInvestmentViews.SelectedIndex = 0;
            cboIssueViews.SelectedIndex = 0;

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

        private void label46_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlDetail.SelectedIndex = 0;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            //CurrentService.Name = txtName.Text;
            //CurrentService.Category = txtCategory.Text;
            //CurrentService.Type = txtType.Text;
            //CurrentService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            //this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtName.Text;
        }

        private void LoadDgvRks()
        {
            DataTable dataTable = PlanRecordKeeper.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkViews"/>.
            switch (cboRkViews.SelectedIndex)
            {
                case 0:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 0);
                    break;
                case 1:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 1);
                    break;
                default:
                    return;
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dataTable.Columns.Add("Name");

            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                Guid recordKeeperId = new Guid(dr["RecordKeeperId"].ToString());
                DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(recordKeeperId);
                dataTable.Rows[i]["Name"] = recordKeeper.Name;
                i++;
            }

            dgvRKs.DataSource = dataTable;

            // Display/order the columns.
            dgvRKs.Columns["PlanRecordKeeperId"].Visible = false;
            dgvRKs.Columns["RecordKeeperId"].Visible = false;
            dgvRKs.Columns["PlanId"].Visible = false;
            dgvRKs.Columns["CreatedBy"].Visible = false;
            dgvRKs.Columns["CreatedOn"].Visible = false;
            dgvRKs.Columns["ModifiedBy"].Visible = false;
            dgvRKs.Columns["ModifiedOn"].Visible = false;
            dgvRKs.Columns["StateCode"].Visible = false;

            dgvRKs.Columns["Name"].DisplayIndex = 0;
            dgvRKs.Columns["DateAdded"].DisplayIndex = 1;
            dgvRKs.Columns["DateRemoved"].DisplayIndex = 2;
        }

        private void cboRkViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvRks();
        }

        private void LoadDgvAuditors()
        {
            DataTable dataTable = PlanAuditor.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkViews"/>.
            switch (cboRkViews.SelectedIndex)
            {
                case 0:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 0);
                    break;
                case 1:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 1);
                    break;
                default:
                    return;
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dataTable.Columns.Add("Name");

            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                Guid auditorId = new Guid(dr["AuditorId"].ToString());
                DataIntegrationHub.Business.Entities.Auditor auditor = new DataIntegrationHub.Business.Entities.Auditor(auditorId);
                dataTable.Rows[i]["Name"] = auditor.Name;
                i++;
            }

            dgvAuditors.DataSource = dataTable;

            // Display/order the columns.
            dgvAuditors.Columns["PlanAuditorId"].Visible = false;
            dgvAuditors.Columns["AuditorId"].Visible = false;
            dgvAuditors.Columns["PlanId"].Visible = false;
            dgvAuditors.Columns["CreatedBy"].Visible = false;
            dgvAuditors.Columns["CreatedOn"].Visible = false;
            dgvAuditors.Columns["ModifiedBy"].Visible = false;
            dgvAuditors.Columns["ModifiedOn"].Visible = false;
            dgvAuditors.Columns["StateCode"].Visible = false;

            dgvAuditors.Columns["Name"].DisplayIndex = 0;
            dgvAuditors.Columns["DateAdded"].DisplayIndex = 1;
            dgvAuditors.Columns["DateRemoved"].DisplayIndex = 2;
        }

        private void cboAuditorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvAuditors();
        }

        private void LoadDgvAdvisors()
        {
            DataTable dataTable = VSP.Business.Entities.PlanAdvisor.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkViews"/>.
            switch (cboRkViews.SelectedIndex)
            {
                case 0:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 0);
                    break;
                case 1:
                    dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 1);
                    break;
                default:
                    return;
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dataTable.Columns.Add("Name");

            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                Guid advisorId = new Guid(dr["AdvisorId"].ToString());
                DataIntegrationHub.Business.Entities.PlanAdvisor advisor = new DataIntegrationHub.Business.Entities.PlanAdvisor(advisorId);
                dataTable.Rows[i]["Name"] = advisor.Name;
                i++;
            }

            dgvAdvisors.DataSource = dataTable;

            // Display/order the columns.
            dgvAdvisors.Columns["PlanAdvisorId"].Visible = false;
            dgvAdvisors.Columns["AdvisorId"].Visible = false;
            dgvAdvisors.Columns["PlanId"].Visible = false;
            dgvAdvisors.Columns["CreatedBy"].Visible = false;
            dgvAdvisors.Columns["CreatedOn"].Visible = false;
            dgvAdvisors.Columns["ModifiedBy"].Visible = false;
            dgvAdvisors.Columns["ModifiedOn"].Visible = false;
            dgvAdvisors.Columns["StateCode"].Visible = false;

            dgvAdvisors.Columns["Name"].DisplayIndex = 0;
            dgvAdvisors.Columns["DateAdded"].DisplayIndex = 1;
            dgvAdvisors.Columns["DateRemoved"].DisplayIndex = 2;
        }

        private void cboAdvisorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvAdvisors();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabRK"];
        }

        private void label3_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabAuditor"];
        }

        private void label4_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabAdvisor"];
        }

        private void label20_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabInvestments"];
        }

        private void label21_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabIssues"];
        }

        private void btnNewRK_Click(object sender, EventArgs e)
        {
            frmPlanRecordKeeper frmPlanRecordKeeper = new frmPlanRecordKeeper(frmMain_Parent, CurrentPlan);
            frmPlanRecordKeeper.FormClosed += frmPlanRecordKeeper_FormClosed;
        }

        private void frmPlanRecordKeeper_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvRks();
        }

        private void btnDeleteRK_Click(object sender, EventArgs e)
        {
            if (dgvRKs.CurrentRow == null)
            {
                return;
            }

            int index = dgvRKs.CurrentRow.Index;
            Guid planRecordKeeperId = new Guid(dgvRKs.Rows[index].Cells[0].Value.ToString());
            PlanRecordKeeper planRecordKeeper = new PlanRecordKeeper(planRecordKeeperId);

            DialogResult result = MessageBox.Show("Are you sure you wish to permanently delete the selected record keeper from the plan?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planRecordKeeper.DeleteRecordFromDatabase();
                LoadDgvRks();
            }
        }

        private void btnNewAuditor_Click(object sender, EventArgs e)
        {
            frmPlanAuditor frmPlanAuditor = new frmPlanAuditor(frmMain_Parent, CurrentPlan);
            frmPlanAuditor.FormClosed += frmPlanAuditor_FormClosed;
        }

        private void btnDeleteAuditor_Click(object sender, EventArgs e)
        {
            if (dgvAuditors.CurrentRow == null)
            {
                return;
            }

            int index = dgvAuditors.CurrentRow.Index;
            Guid planAuditorId = new Guid(dgvAuditors.Rows[index].Cells[0].Value.ToString());
            PlanAuditor planAuditor = new PlanAuditor(planAuditorId);

            DialogResult result = MessageBox.Show("Are you sure you wish to permanently delete the selected auditor from the plan?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planAuditor.DeleteRecordFromDatabase();
                LoadDgvAuditors();
            }
        }

        private void frmPlanAuditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvAuditors();
        }

        private void btnNewAdvisor_Click(object sender, EventArgs e)
        {
            frmPlanAdvisor frmPlanAdvisor = new frmPlanAdvisor(frmMain_Parent, CurrentPlan);
            frmPlanAdvisor.FormClosed += frmPlanAdvisor_FormClosed;
        }

        private void btnDeleteAdvisor_Click(object sender, EventArgs e)
        {
            if (dgvAdvisors.CurrentRow == null)
            {
                return;
            }

            int index = dgvAdvisors.CurrentRow.Index;
            Guid planAdvisorId = new Guid(dgvAdvisors.Rows[index].Cells[0].Value.ToString());
            VSP.Business.Entities.PlanAdvisor planAdvisor = new VSP.Business.Entities.PlanAdvisor(planAdvisorId);

            DialogResult result = MessageBox.Show("Are you sure you wish to permanently delete the selected advisor from the plan?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planAdvisor.DeleteRecordFromDatabase();
                LoadDgvAdvisors();
            }
        }

        private void frmPlanAdvisor_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvAdvisors();
        }

        private void LoadDgvInvestments()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboInvestmentViews"/>.
            switch (cboInvestmentViews.SelectedIndex)
            {
                case 0:
                    dataTable = ISP.Business.Entities.Fund.GetActiveAssociatedFromPlan(CurrentPlan.PlanId);
                    break;
                case 1:
                    dataTable = ISP.Business.Entities.Fund.GetInactiveAssociatedFromPlan(CurrentPlan.PlanId);
                    break;
                default:
                    return;
            }

            dgvInvestments.DataSource = dataTable;

            // Display/order the columns.
            dgvInvestments.Columns["FundId"].Visible = false;
            dgvInvestments.Columns["Relational_Funds_PlansId"].Visible = false;
            dgvInvestments.Columns["Ordinal"].Visible = false;

            //dgvInvestments.Columns["FundName"].DisplayIndex = 0;
        }

        private void cboInvestments_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvInvestments();
        }

        private void LoadDgvIssues()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboIssueViews"/>.
            switch (cboIssueViews.SelectedIndex)
            {
                case 0:
                    dataTable = ServiceIssue.GetActive();
                    break;
                case 1:
                    dataTable = ServiceIssue.GetInactive();
                    break;
                default:
                    return;
            }

            if (dataTable.Rows.Count > 0)
            {
                var dt = dataTable.AsEnumerable().Where(x => x.Field<Guid?>("PlanId") == CurrentPlan.PlanId);
                if (dt.Any())
                {
                    dataTable = dt.CopyToDataTable();
                }
                else
                {
                    dataTable.Rows.Clear();
                }
            }

            // Add plan name column and fill data
            dataTable.Columns.Add("RecordKeeper");
            foreach (DataRow dr in dataTable.Rows)
            {
                string recordKeeperIdString = dr["RecordKeeperId"].ToString();
                if (String.IsNullOrWhiteSpace(recordKeeperIdString) == false)
                {
                    Guid recordKeeperId = new Guid(recordKeeperIdString);
                    DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(recordKeeperId);
                    dr["RecordKeeper"] = recordKeeper.Name;
                }
            }

            dgvIssues.DataSource = dataTable;

            // Display/order the columns.
            dgvIssues.Columns["ServiceIssueId"].Visible = false;
            dgvIssues.Columns["PlanId"].Visible = false;
            dgvIssues.Columns["RecordKeeperId"].Visible = false;
            dgvIssues.Columns["AuditorId"].Visible = false;
            dgvIssues.Columns["DescriptionValue"].Visible = false;
            dgvIssues.Columns["CreatedBy"].Visible = false;
            dgvIssues.Columns["ModifiedBy"].Visible = false;
            dgvIssues.Columns["StateCode"].Visible = false;

            dgvIssues.Columns["SubjectValue"].DisplayIndex = 0;
            dgvIssues.Columns["RecordKeeper"].DisplayIndex = 1;
            dgvIssues.Columns["AsOfDate"].DisplayIndex = 2;
            dgvIssues.Columns["ModifiedOn"].DisplayIndex = 3;
        }

        private void cboIssueViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvIssues();
        }

        private void btnNewIssue_Click(object sender, EventArgs e)
        {
            frmServiceIssue frmServiceIssue = new frmServiceIssue(frmMain_Parent, CurrentPlan);
            frmServiceIssue.FormClosed += frmServiceIssue_FormClosed;
        }

        void frmServiceIssue_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvIssues();
        }

        private void btnDeleteIssue_Click(object sender, EventArgs e)
        {
            if (dgvIssues.CurrentRow == null)
            {
                return;
            }

            int index = dgvIssues.CurrentRow.Index;
            Guid serviceIssueId = new Guid(dgvIssues.Rows[index].Cells[0].Value.ToString());
            ServiceIssue serviceIssue = new ServiceIssue(serviceIssueId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete " + serviceIssue.SubjectValue + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                serviceIssue.DeleteRecordFromDatabase();
                LoadDgvIssues();
            }
        }
	}
}
