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
        private Label CurrentTabLabel;

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
            CurrentPlanDetail = new PlanDetail(plan.PlanId);
            txtName.Text = CurrentPlan.Name;
            txtNotes.Text = CurrentPlanDetail.Notes;
            CurrentTabLabel = label46; // Summary tab label
            highlightSelectedTabLabel(CurrentTabLabel);
            txtNotes.Focus();

            cboRkProductViews.SelectedIndex = 0;
            cboAdvisorViews.SelectedIndex = 0;
            cboAuditorViews.SelectedIndex = 0;
            cboInvestmentViews.SelectedIndex = 0;
            cboOtherViews.SelectedIndex = 0;
            cboIssueViews.SelectedIndex = 0;
            cboContributionViews.SelectedIndex = 0;
            cboDistributionViews.SelectedIndex = 0;
            cboActiveParticipantViews.SelectedIndex = 0;
            cboEligibleParticipantsViews.SelectedIndex = 0;

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
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedIndex = 0;
            txtNotes.Focus();
        }

        private void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label != CurrentTabLabel)
            {
                label.BackColor = System.Drawing.Color.DarkGray;
            }
        }

        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label != CurrentTabLabel)
            {
                label.BackColor = System.Drawing.Color.Transparent;
            }
        }

        private void highlightSelectedTabLabel(object sender)
        {
            Label label = (Label)sender;
            CurrentTabLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            CurrentTabLabel.BackColor = System.Drawing.Color.Transparent;
            label.ForeColor = System.Drawing.SystemColors.HotTrack;
            label.BackColor = System.Drawing.Color.Gainsboro;
            CurrentTabLabel = label;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CurrentPlanDetail.Notes = txtNotes.Text;
            CurrentPlanDetail.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            //this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtName.Text;
        }

        private void LoadDgvRkPds()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvRKProducts.CurrentCell != null)
            {
                currentCellRow = dgvRKProducts.CurrentCell.RowIndex;
                currentCellCol = dgvRKProducts.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = PlanRecordKeeperProduct.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboRkProductViews.SelectedIndex)
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

            dataTable.Columns.Add("Record Keeper");
            dataTable.Columns.Add("Product");

            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                string s = dr["ProductId"].ToString();
                Guid productId = new Guid(dr["ProductId"].ToString());
                Product pd = new Product(productId);
                dataTable.Rows[i]["Product"] = pd.Name;

                DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(pd.RecordKeeperId);
                dataTable.Rows[i]["Record Keeper"] = recordKeeper.Name;
                i++;
            }

            dgvRKProducts.DataSource = dataTable;

            // Display/order the columns.
            dgvRKProducts.Columns["PlanRecordKeeperProductId"].Visible = false;
            dgvRKProducts.Columns["PlanId"].Visible = false;
            dgvRKProducts.Columns["CreatedBy"].Visible = false;
            dgvRKProducts.Columns["CreatedOn"].Visible = false;
            dgvRKProducts.Columns["ModifiedBy"].Visible = false;
            dgvRKProducts.Columns["ModifiedOn"].Visible = false;
            dgvRKProducts.Columns["StateCode"].Visible = false;
            dgvRKProducts.Columns["ProductId"].Visible = false;

            dgvRKProducts.Columns["Record Keeper"].DisplayIndex = 0;
            dgvRKProducts.Columns["Product"].DisplayIndex = 1;
            dgvRKProducts.Columns["DateAdded"].DisplayIndex = 2;
            dgvRKProducts.Columns["DateRemoved"].DisplayIndex = 3;

            if (dgvRKProducts.RowCount > 0 && dgvRKProducts.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvRKProducts.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvRKProducts.CurrentCell = selectedCell;
                }
                else
                {
                    dgvRKProducts.CurrentCell = dgvRKProducts.FirstDisplayedCell;
                }
            }
        }

        private void cboRkViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvRkPds();
        }

        private void LoadDgvAuditors()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvAuditors.CurrentCell != null)
            {
                currentCellRow = dgvAuditors.CurrentCell.RowIndex;
                currentCellCol = dgvAuditors.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = PlanAuditor.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboAuditorViews.SelectedIndex)
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

            if (dgvAuditors.RowCount > 0 && dgvAuditors.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvAuditors.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvAuditors.CurrentCell = selectedCell;
                }
                else
                {
                    dgvAuditors.CurrentCell = dgvAuditors.FirstDisplayedCell;
                }
            }
        }

        private void cboAuditorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvAuditors();
        }

        private void LoadDgvAdvisors()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvAdvisors.CurrentCell != null)
            {
                currentCellRow = dgvAdvisors.CurrentCell.RowIndex;
                currentCellCol = dgvAdvisors.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = VSP.Business.Entities.PlanAdvisor.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboAdvisorViews.SelectedIndex)
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

            if (dgvAdvisors.RowCount > 0 && dgvAdvisors.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvAdvisors.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvAdvisors.CurrentCell = selectedCell;
                }
                else
                {
                    dgvAdvisors.CurrentCell = dgvAdvisors.FirstDisplayedCell;
                }
            }
        }

        private void cboAdvisorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvAdvisors();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabRKProduct"];
            dgvRKProducts.Focus();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabAuditor"];
            dgvAuditors.Focus();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabAdvisor"];
            dgvAdvisors.Focus();
        }

        private void label20_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabInvestments"];
            dgvInvestments.Focus();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabIssues"];
            dgvIssues.Focus();
        }

        private void label43_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabOther"];
            dgvOther.Focus();
        }

        private void btnNewRKProduct_Click(object sender, EventArgs e)
        {
            frmPlanRecordKeeperProduct frmPlanRecordKeeperProduct = new frmPlanRecordKeeperProduct(frmMain_Parent, CurrentPlan);
            frmPlanRecordKeeperProduct.FormClosed += frmPlanRecordKeeperProduct_FormClosed;
        }

        private void frmPlanRecordKeeperProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvRkPds();
        }

        private void btnDeleteRKProduct_Click(object sender, EventArgs e)
        {
            if (dgvRKProducts.CurrentRow == null)
            {
                return;
            }

            int index = dgvRKProducts.CurrentRow.Index;
            Guid planRecordKeeperProductId = new Guid(dgvRKProducts.Rows[index].Cells[0].Value.ToString());
            PlanRecordKeeperProduct planRecordKeeper = new PlanRecordKeeperProduct(planRecordKeeperProductId);

            DialogResult result = MessageBox.Show("Are you sure you wish to permanently delete the selected record keeper product from the plan?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planRecordKeeper.DeleteRecordFromDatabase();
                LoadDgvRkPds();
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
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvInvestments.CurrentCell != null)
            {
                currentCellRow = dgvInvestments.CurrentCell.RowIndex;
                currentCellCol = dgvInvestments.CurrentCell.ColumnIndex;
            }

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

            dataTable.Columns.Add("Balance", typeof(string));
            dataTable.Columns.Add("BalanceAsOf", typeof(string));

            int rowIndex = 0;
            foreach (DataGridViewRow drInvestments in dgvInvestments.Rows)
            {
                Guid rfpId = new Guid(drInvestments.Cells["Relational_Funds_PlansId"].Value.ToString());
                ISP.Business.Entities.Relational_Funds_Plans rfp = new ISP.Business.Entities.Relational_Funds_Plans(rfpId);

                if (rfp.AssetValue != null)
                {
                    dgvInvestments.Rows[rowIndex].Cells["Balance"].Value = "$" + ((decimal)rfp.AssetValue).ToString("#,##.##");
                }

                if (rfp.AssetValueAsOf != null)
                {
                    dgvInvestments.Rows[rowIndex].Cells["BalanceAsOf"].Value = ((DateTime)rfp.AssetValueAsOf).ToString("MM/dd/yyyy");
                }

                rowIndex++;
            }

            // Display/order the columns.
            dgvInvestments.Columns["FundId"].Visible = false;
            dgvInvestments.Columns["Relational_Funds_PlansId"].Visible = false;
            dgvInvestments.Columns["Ordinal"].Visible = false;

            dgvInvestments.Columns["Ticker"].DisplayIndex = 0;
            dgvInvestments.Columns["Fund Name"].DisplayIndex = 1;
            dgvInvestments.Columns["Balance"].DisplayIndex = 2;
            dgvInvestments.Columns["BalanceAsOf"].DisplayIndex = 3;

            if (dgvInvestments.RowCount > 0 && dgvInvestments.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvInvestments.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvInvestments.CurrentCell = selectedCell;
                }
                else
                {
                    dgvInvestments.CurrentCell = dgvInvestments.FirstDisplayedCell;
                }
            }
        }

        private void cboInvestments_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvInvestments();
        }

        private void LoadDgvIssues()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvIssues.CurrentCell != null)
            {
                currentCellRow = dgvIssues.CurrentCell.RowIndex;
                currentCellCol = dgvIssues.CurrentCell.ColumnIndex;
            }

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
            dataTable.Columns.Add("Product");
            foreach (DataRow dr in dataTable.Rows)
            {
                string rkpIdString = dr["Product"].ToString();
                if (String.IsNullOrWhiteSpace(rkpIdString) == false)
                {
                    Guid rkpId = new Guid(rkpIdString);
                    PlanRecordKeeperProduct rkp = new PlanRecordKeeperProduct(rkpId);
                    Product product = new Product(rkp.ProductId);
                    dr["Product"] = product.Name;
                }
            }

            dgvIssues.DataSource = dataTable;

            // Display/order the columns.
            dgvIssues.Columns["ServiceIssueId"].Visible = false;
            dgvIssues.Columns["PlanId"].Visible = false;
            dgvIssues.Columns["PlanRecordKeeperProductId"].Visible = false;
            dgvIssues.Columns["AuditorId"].Visible = false;
            dgvIssues.Columns["DescriptionValue"].Visible = false;
            dgvIssues.Columns["CreatedBy"].Visible = false;
            dgvIssues.Columns["ModifiedBy"].Visible = false;
            dgvIssues.Columns["StateCode"].Visible = false;

            dgvIssues.Columns["SubjectValue"].DisplayIndex = 0;
            dgvIssues.Columns["Product"].DisplayIndex = 1;
            dgvIssues.Columns["AsOfDate"].DisplayIndex = 2;
            dgvIssues.Columns["ModifiedOn"].DisplayIndex = 3;

            if (dgvIssues.RowCount > 0 && dgvIssues.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvIssues.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvIssues.CurrentCell = selectedCell;
                }
                else
                {
                    dgvIssues.CurrentCell = dgvIssues.FirstDisplayedCell;
                }
            }
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

        private void dgvRKProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvRKProducts.CurrentRow.Index;
            Guid planRkPdId = new Guid(dgvRKProducts.Rows[index].Cells["PlanRecordKeeperProductId"].Value.ToString());
            PlanRecordKeeperProduct planRKProduct = new PlanRecordKeeperProduct(planRkPdId);
            frmPlanRecordKeeperProduct frmPlanRecordKeeperProduct = new frmPlanRecordKeeperProduct(frmMain_Parent, planRKProduct);
            frmPlanRecordKeeperProduct.FormClosed += frmPlanRecordKeeperProduct_FormClosed;
        }

        private void label22_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabContributions;
            dgvContributions.Focus();
        }

        private void label26_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabDistributions;
            dgvDistributions.Focus();
        }

        private void label27_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabActiveParticipants;
            dgvActiveParticipants.Focus();
        }

        private void label28_Click(object sender, EventArgs e)
        {
            highlightSelectedTabLabel(sender);
            tabControlDetail.SelectedTab = tabEligibleParticipants;
            dgvEligibleParticipants.Focus();
        }

        private void LoadDgvContributions()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvContributions.CurrentCell != null)
            {
                currentCellRow = dgvContributions.CurrentCell.RowIndex;
                currentCellCol = dgvContributions.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = VSP.Business.Entities.PlanContribution.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboContributionViews.SelectedIndex)
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

            dgvContributions.DataSource = dataTable;

            // Display/order the columns.
            dgvContributions.Columns["PlanContributionId"].Visible = false;
            dgvContributions.Columns["PlanId"].Visible = false;
            dgvContributions.Columns["CreatedBy"].Visible = false;
            dgvContributions.Columns["ModifiedBy"].Visible = false;
            dgvContributions.Columns["StateCode"].Visible = false;

            dgvContributions.Columns["Contribution"].DisplayIndex = 0;
            dgvContributions.Columns["AsOfDate"].DisplayIndex = 1;
            dgvContributions.Columns["ModifiedOn"].DisplayIndex = 2;
            dgvContributions.Columns["CreatedOn"].DisplayIndex = 3;

            if (dgvContributions.RowCount > 0 && dgvContributions.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvContributions.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvContributions.CurrentCell = selectedCell;
                }
                else
                {
                    dgvContributions.CurrentCell = dgvContributions.FirstDisplayedCell;
                }
            }
        }

        private void cboContributionViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvContributions();
        }

        private void LoadDgvDistributions()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvDistributions.CurrentCell != null)
            {
                currentCellRow = dgvDistributions.CurrentCell.RowIndex;
                currentCellCol = dgvDistributions.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = VSP.Business.Entities.PlanDistribution.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboDistributionViews.SelectedIndex)
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

            dgvDistributions.DataSource = dataTable;

            // Display/order the columns.
            dgvDistributions.Columns["PlanDistributionId"].Visible = false;
            dgvDistributions.Columns["PlanId"].Visible = false;
            dgvDistributions.Columns["CreatedBy"].Visible = false;
            dgvDistributions.Columns["ModifiedBy"].Visible = false;
            dgvDistributions.Columns["StateCode"].Visible = false;

            dgvDistributions.Columns["Distribution"].DisplayIndex = 0;
            dgvDistributions.Columns["AsOfDate"].DisplayIndex = 1;
            dgvDistributions.Columns["ModifiedOn"].DisplayIndex = 2;
            dgvDistributions.Columns["CreatedOn"].DisplayIndex = 3;

            if (dgvDistributions.RowCount > 0 && dgvDistributions.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvDistributions.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvDistributions.CurrentCell = selectedCell;
                }
                else
                {
                    dgvDistributions.CurrentCell = dgvDistributions.FirstDisplayedCell;
                }
            }
        }

        private void cboDistributionViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvDistributions();
        }

        private void LoadDgvEligibleParticipants()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvEligibleParticipants.CurrentCell != null)
            {
                currentCellRow = dgvEligibleParticipants.CurrentCell.RowIndex;
                currentCellCol = dgvEligibleParticipants.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = VSP.Business.Entities.PlanParticipantsEligible.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboEligibleParticipantsViews.SelectedIndex)
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

            dgvEligibleParticipants.DataSource = dataTable;

            // Display/order the columns.
            dgvEligibleParticipants.Columns["PlanParticipantsEligibleId"].Visible = false;
            dgvEligibleParticipants.Columns["PlanId"].Visible = false;
            dgvEligibleParticipants.Columns["CreatedBy"].Visible = false;
            dgvEligibleParticipants.Columns["ModifiedBy"].Visible = false;
            dgvEligibleParticipants.Columns["StateCode"].Visible = false;

            dgvEligibleParticipants.Columns["ParticipantCount"].DisplayIndex = 0;
            dgvEligibleParticipants.Columns["AsOfDate"].DisplayIndex = 1;
            dgvEligibleParticipants.Columns["ModifiedOn"].DisplayIndex = 2;
            dgvEligibleParticipants.Columns["CreatedOn"].DisplayIndex = 3;

            if (dgvEligibleParticipants.RowCount > 0 && dgvEligibleParticipants.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvEligibleParticipants.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvEligibleParticipants.CurrentCell = selectedCell;
                }
                else
                {
                    dgvEligibleParticipants.CurrentCell = dgvEligibleParticipants.FirstDisplayedCell;
                }
            }
        }

        private void cboEligibleParticipantsViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvEligibleParticipants();
        }

        private void LoadDgvActiveParticipants()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvActiveParticipants.CurrentCell != null)
            {
                currentCellRow = dgvActiveParticipants.CurrentCell.RowIndex;
                currentCellCol = dgvActiveParticipants.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = VSP.Business.Entities.PlanParticipantsActive.GetAssociated(CurrentPlan.PlanId);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboRkProductViews"/>.
            switch (cboActiveParticipantViews.SelectedIndex)
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

            dgvActiveParticipants.DataSource = dataTable;

            // Display/order the columns.
            dgvActiveParticipants.Columns["PlanParticipantsActiveId"].Visible = false;
            dgvActiveParticipants.Columns["PlanId"].Visible = false;
            dgvActiveParticipants.Columns["CreatedBy"].Visible = false;
            dgvActiveParticipants.Columns["ModifiedBy"].Visible = false;
            dgvActiveParticipants.Columns["StateCode"].Visible = false;

            dgvActiveParticipants.Columns["ParticipantCount"].DisplayIndex = 0;
            dgvActiveParticipants.Columns["AsOfDate"].DisplayIndex = 1;
            dgvActiveParticipants.Columns["ModifiedOn"].DisplayIndex = 2;
            dgvActiveParticipants.Columns["CreatedOn"].DisplayIndex = 3;

            if (dgvActiveParticipants.RowCount > 0 && dgvActiveParticipants.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvActiveParticipants.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvActiveParticipants.CurrentCell = selectedCell;
                }
                else
                {
                    dgvActiveParticipants.CurrentCell = dgvActiveParticipants.FirstDisplayedCell;
                }
            }
        }

        private void cboActiveParticipantViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvActiveParticipants();
        }

        private void dgvContributions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvContributions.CurrentRow.Index;
            Guid id = new Guid(dgvContributions.Rows[index].Cells["PlanContributionId"].Value.ToString());
            PlanContribution obj = new PlanContribution(id);
            frmPlanContribution frmPlanContribution = new frmPlanContribution(frmMain_Parent, obj);
            frmPlanContribution.FormClosed += frmPlanContribution_FormClosed;
        }

        private void frmPlanContribution_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvContributions();
        }

        private void btnNewContributions_Click(object sender, EventArgs e)
        {
            frmPlanContribution frmPlanContribution = new frmPlanContribution(frmMain_Parent, CurrentPlan);
            frmPlanContribution.FormClosed += frmPlanContribution_FormClosed;
        }

        private void btnDeleteContribution_Click(object sender, EventArgs e)
        {
            if (dgvContributions.CurrentRow == null)
            {
                return;
            }

            int index = dgvContributions.CurrentRow.Index;
            Guid id = new Guid(dgvContributions.Rows[index].Cells["PlanContributionId"].Value.ToString());
            PlanContribution obj = new PlanContribution(id);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete contribution on " + obj.AsOfDate.ToString("MM/dd/yyyy") + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                obj.DeleteRecordFromDatabase();
                LoadDgvContributions();
            }
        }

        private void dgvDistributions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvDistributions.CurrentRow.Index;
            Guid id = new Guid(dgvDistributions.Rows[index].Cells["PlanDistributionId"].Value.ToString());
            PlanDistribution obj = new PlanDistribution(id);
            frmPlanDistribution frmPlanDistribution = new frmPlanDistribution(frmMain_Parent, obj);
            frmPlanDistribution.FormClosed += frmPlanDistribution_FormClosed;
        }

        private void frmPlanDistribution_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvDistributions();
        }

        private void btnNewDistribution_Click(object sender, EventArgs e)
        {
            frmPlanDistribution frmPlanDistribution = new frmPlanDistribution(frmMain_Parent, CurrentPlan);
            frmPlanDistribution.FormClosed += frmPlanDistribution_FormClosed;
        }

        private void btnDeleteDistribution_Click(object sender, EventArgs e)
        {
            if (dgvDistributions.CurrentRow == null)
            {
                return;
            }

            int index = dgvDistributions.CurrentRow.Index;
            Guid id = new Guid(dgvDistributions.Rows[index].Cells["PlanDistributionId"].Value.ToString());
            PlanDistribution obj = new PlanDistribution(id);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete distribution on " + obj.AsOfDate.ToString("MM/dd/yyyy") + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                obj.DeleteRecordFromDatabase();
                LoadDgvDistributions();
            }
        }

        private void dgvActiveParticipants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvActiveParticipants.CurrentRow.Index;
            Guid id = new Guid(dgvActiveParticipants.Rows[index].Cells["PlanParticipantsActiveId"].Value.ToString());
            PlanParticipantsActive obj = new PlanParticipantsActive(id);
            frmPlanParticipantsActive frmPlanParticipantsActive = new frmPlanParticipantsActive(frmMain_Parent, obj);
            frmPlanParticipantsActive.FormClosed += frmPlanParticipantsActive_FormClosed;
        }

        private void frmPlanParticipantsActive_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvActiveParticipants();
        }

        private void btnNewActiveParticipants_Click(object sender, EventArgs e)
        {
            frmPlanParticipantsActive frmPlanParticipantsActive = new frmPlanParticipantsActive(frmMain_Parent, CurrentPlan);
            frmPlanParticipantsActive.FormClosed += frmPlanParticipantsActive_FormClosed;
        }

        private void btnDeleteActiveParticipants_Click(object sender, EventArgs e)
        {
            if (dgvActiveParticipants.CurrentRow == null)
            {
                return;
            }

            int index = dgvActiveParticipants.CurrentRow.Index;
            Guid id = new Guid(dgvActiveParticipants.Rows[index].Cells["PlanParticipantsActiveId"].Value.ToString());
            PlanParticipantsActive obj = new PlanParticipantsActive(id);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete active plan participants on " + obj.AsOfDate.ToString("MM/dd/yyyy") + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                obj.DeleteRecordFromDatabase();
                LoadDgvActiveParticipants();
            }
        }

        private void dgvEligibleParticipants_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvEligibleParticipants.CurrentRow.Index;
            Guid id = new Guid(dgvEligibleParticipants.Rows[index].Cells["PlanParticipantsEligibleId"].Value.ToString());
            PlanParticipantsEligible obj = new PlanParticipantsEligible(id);
            frmPlanParticipantsEligible frmPlanParticipantsEligible = new frmPlanParticipantsEligible(frmMain_Parent, obj);
            frmPlanParticipantsEligible.FormClosed += frmPlanParticipantsEligible_FormClosed;
        }

        private void frmPlanParticipantsEligible_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvEligibleParticipants();
        }

        private void btnNewEligibleParticipants_Click(object sender, EventArgs e)
        {
            frmPlanParticipantsEligible frmPlanParticipantsEligible = new frmPlanParticipantsEligible(frmMain_Parent, CurrentPlan);
            frmPlanParticipantsEligible.FormClosed += frmPlanParticipantsEligible_FormClosed;
        }

        private void btnDeleteEligibleParticipants_Click(object sender, EventArgs e)
        {
            if (dgvEligibleParticipants.CurrentRow == null)
            {
                return;
            }

            int index = dgvEligibleParticipants.CurrentRow.Index;
            Guid id = new Guid(dgvEligibleParticipants.Rows[index].Cells["PlanParticipantsEligibleId"].Value.ToString());
            PlanParticipantsEligible obj = new PlanParticipantsEligible(id);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete eligible plan participants on " + obj.AsOfDate.ToString("MM/dd/yyyy") + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                obj.DeleteRecordFromDatabase();
                LoadDgvEligibleParticipants();
            }
        }

        private void dgvAuditors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvAuditors.CurrentRow.Index;
            Guid planAuditorId = new Guid(dgvAuditors.Rows[index].Cells["PlanAuditorId"].Value.ToString());
            PlanAuditor planAuditor = new PlanAuditor(planAuditorId);
            frmPlanAuditor frmPlanAuditor = new frmPlanAuditor(frmMain_Parent, planAuditor);
            frmPlanAuditor.FormClosed += frmPlanAuditor_FormClosed;
        }

        private void dgvAdvisor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvAdvisors.CurrentRow.Index;
            Guid planAdvisorId = new Guid(dgvAdvisors.Rows[index].Cells["PlanAdvisorId"].Value.ToString());
            VSP.Business.Entities.PlanAdvisor planAdvisor = new VSP.Business.Entities.PlanAdvisor(planAdvisorId);
            frmPlanAdvisor frmPlanAdvisor = new frmPlanAdvisor(frmMain_Parent, planAdvisor);
            frmPlanAdvisor.FormClosed += frmPlanAdvisor_FormClosed;
        }

        private void btnNewOther_Click(object sender, EventArgs e)
        {
            frmPlanOtherFee frmPlanOtherFee = new frmPlanOtherFee(frmMain_Parent, CurrentPlan);
            frmPlanOtherFee.FormClosed += frmPlanOtherFee_FormClosed;
        }

        private void frmPlanOtherFee_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvOther();
        }

        private void btnDeleteOther_Click(object sender, EventArgs e)
        {
            int index = dgvOther.CurrentRow.Index;
            Guid planAdvFeeId = new Guid(dgvOther.Rows[index].Cells["PlanOtherFeeId"].Value.ToString());
            PlanAdvisorFee planAdvFee = new PlanAdvisorFee(planAdvFeeId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected plan other fee?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planAdvFee.DeleteRecordFromDatabase();
                LoadDgvOther();
            }
        }

        private void LoadDgvOther()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvOther.CurrentCell != null)
            {
                currentCellRow = dgvOther.CurrentCell.RowIndex;
                currentCellCol = dgvOther.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboOtherViews"/>.
            switch (cboOtherViews.SelectedIndex)
            {
                case 0:
                    dataTable = PlanOtherFee.GetAssociatedActive(CurrentPlan);
                    break;
                case 1:
                    dataTable = PlanOtherFee.GetAssociatedActive(CurrentPlan);
                    break;
                default:
                    return;
            }

            dgvOther.DataSource = dataTable;

            // Display/order the columns.
            dgvOther.Columns["PlanOtherFeeId"].Visible = false;
            dgvOther.Columns["PlanId"].Visible = false;
            dgvOther.Columns["CreatedBy"].Visible = false;
            dgvOther.Columns["ModifiedBy"].Visible = false;
            dgvOther.Columns["StateCode"].Visible = false;
            dgvOther.Columns["Benchmark25Fee"].Visible = false;
            dgvOther.Columns["Benchmark50Fee"].Visible = false;
            dgvOther.Columns["Benchmark75Fee"].Visible = false;

            // Display/order the columns.
            dgvOther.Columns["Notes"].Visible = false;

            dgvOther.Columns["Name"].DisplayIndex = 0;

            dgvOther.Columns["Fee"].DisplayIndex = 1;

            dgvOther.Columns["RevenueSharingPaid"].DisplayIndex = 2;
            dgvOther.Columns["RevenueSharingPaid"].HeaderText = "Revenue Sharing Paid";

            dgvOther.Columns["ForfeituresPaid"].DisplayIndex = 3;
            dgvOther.Columns["ForfeituresPaid"].HeaderText = "Forfeitures Paid";

            dgvOther.Columns["ParticipantsPaid"].DisplayIndex = 4;
            dgvOther.Columns["ParticipantsPaid"].HeaderText = "Participants Paid";

            dgvOther.Columns["PlanSponsorPaid"].DisplayIndex = 5;
            dgvOther.Columns["PlanSponsorPaid"].HeaderText = "Plan Sponsor Paid";

            dgvOther.Columns["AsOfDate"].DisplayIndex = 6;
            dgvOther.Columns["ModifiedOn"].DisplayIndex = 7;
            dgvOther.Columns["CreatedOn"].DisplayIndex = 8;

            if (dgvOther.RowCount > 0 && dgvOther.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvOther.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvOther.CurrentCell = selectedCell;
                }
                else
                {
                    dgvOther.CurrentCell = dgvOther.FirstDisplayedCell;
                }
            }
        }

        private void cboOtherViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvOther();
        }

        private void dgvIssues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvIssues.CurrentRow.Index;
            Guid issueId = new Guid(dgvIssues.Rows[index].Cells["ServiceIssueId"].Value.ToString());
            ServiceIssue issue = new ServiceIssue(issueId);
            frmServiceIssue frmServiceIssue = new frmServiceIssue(frmMain_Parent, issue);
            frmServiceIssue.FormClosed += frmServiceIssue_FormClosed;
        }
    }
}
