using DataIntegrationHub.Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VSP.Business.Entities;
using VSP.Presentation.Utilities;

namespace VSP.Presentation.Forms
{
    public partial class frmPlanRecordKeeperProduct : Form, IMessageFilter
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
        public PlanRecordKeeperProduct CurrentPlanRecordKeeperProduct;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="Close"></param>
        public frmPlanRecordKeeperProduct(frmMain mf, Plan plan, FormClosedEventHandler Close = null)
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

            PreloadCbos();

            CurrentPlanRecordKeeperProduct = new PlanRecordKeeperProduct();
            CurrentPlanRecordKeeperProduct.PlanId = plan.PlanId;

            cboFeeViews.SelectedIndex = 0;

            cboPlan.Text = plan.Name + " - " + plan.Description;

            lblMenuFees.Visible = false;
            lblMenuServices.Visible = false;

            ss.Close();
            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmPlanRecordKeeperProduct(frmMain mf, PlanRecordKeeperProduct planRecordKeeperProduct, FormClosedEventHandler Close = null)
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

            PreloadCbos();

            CurrentPlanRecordKeeperProduct = planRecordKeeperProduct;

            if (CurrentPlanRecordKeeperProduct.PlanId != null)
            {
                Plan plan = new Plan(CurrentPlanRecordKeeperProduct.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            if (CurrentPlanRecordKeeperProduct.ProductId != null)
            {
                Product pd = new Product(CurrentPlanRecordKeeperProduct.ProductId);
                DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(pd.RecordKeeperId);
                cboRecordKeeper.Text = recordKeeper.Name;

                LoadCboProduct();
                cboProduct.Text = pd.Name;
            }
            else
            {
                LoadCboProduct();
            }

            if (CurrentPlanRecordKeeperProduct.DateAdded != null)
            {
                dateAdded.Value = (DateTime)CurrentPlanRecordKeeperProduct.DateAdded;
                dateAdded.Checked = true;
            }
            else
            {
                dateAdded.Checked = false;
            }

            if (CurrentPlanRecordKeeperProduct.DateRemoved != null)
            {
                dateRemoved.Value = (DateTime)CurrentPlanRecordKeeperProduct.DateRemoved;
                dateRemoved.Checked = true;
            }
            else
            {
                dateRemoved.Checked = false;
            }

            cboFeeViews.SelectedIndex = 0;
            cboServicesView.SelectedIndex = 0;
            LoadDgvServices(true);

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

        public void PreloadCbos()
        {
            cboRecordKeeper.Items.Clear();
            cboPlan.Items.Clear();

            cboRecordKeeper.Items.Add(new ListItem("", ""));
            cboPlan.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in DataIntegrationHub.Business.Entities.RecordKeeper.GetAll().Rows)
            {
                Guid recordKeeperId = new Guid(dr["RecordKeeperId"].ToString());
                string name = dr["Name"].ToString();
                cboRecordKeeper.Items.Add(new ListItem(name, recordKeeperId));
            }

            foreach (Plan plan in Plan.Get().OrderBy(x => x.Name))
            {
                cboPlan.Items.Add(new ListItem(plan.Name + " - " + plan.Description, plan));
            }
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
            tabControlClientDetail.SelectedIndex = 0;
            tabClientSummary.Focus();

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
            if (cboRecordKeeper.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Record keeper cannot be left blank.");
                return;
            }

            if (cboProduct.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Product cannot be left blank.");
                return;
            }
            else
            {
                ListItem li = (ListItem)cboProduct.SelectedItem;
                Guid productId = (Guid)li.HiddenObject;
                CurrentPlanRecordKeeperProduct.ProductId = productId;
            }

            if (cboPlan.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Plan cannot be left blank.");
                return;
            }
            else
            {
                ListItem li = (ListItem)cboPlan.SelectedItem;
                Plan plan = (Plan)li.HiddenObject;
                CurrentPlanRecordKeeperProduct.PlanId = plan.PlanId;
            }

            if (dateAdded.Checked == false)
            {
                CurrentPlanRecordKeeperProduct.DateAdded = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperProduct.DateAdded = dateAdded.Value;
                }
                catch
                {
                    MessageBox.Show("Error: Date Added string not in date format");
                    return;
                }
            }

            if (dateRemoved.Checked == false)
            {
                CurrentPlanRecordKeeperProduct.DateRemoved = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperProduct.DateRemoved = dateRemoved.Value;
                }
                catch
                {
                    MessageBox.Show("Error: Date Removed string not in date format");
                    return;
                }
            }

            // loop through dgvservices, and update productservice records for record keeper product
            if (dgvServices.Rows.Count > 0)
            {
                DataTable productServices = PlanRecordKeeperProductService.GetAssociated(CurrentPlanRecordKeeperProduct);

                foreach (DataGridViewRow dr in dgvServices.Rows)
                {
                    Guid serviceId = new Guid(dr.Cells["ServiceId"].Value.ToString());

                    bool serviceOffered = false;
                    if (dr.Cells["ServiceOffered"].Value.ToString() != "")
                    {
                        serviceOffered = bool.Parse(dr.Cells["ServiceOffered"].Value.ToString());
                    }

                    string notes = dr.Cells["Notes"].Value.ToString();


                    var ps = productServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);

                    PlanRecordKeeperProductService planRkPdService;
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        Guid planRkPdServiceId = new Guid(ps.CopyToDataTable().Rows[0]["PlanRecordKeeperProductServiceId"].ToString());
                        planRkPdService = new PlanRecordKeeperProductService(planRkPdServiceId);
                    }
                    else // rk product does not have service record, so create on
                    {
                        planRkPdService = new PlanRecordKeeperProductService();
                        planRkPdService.ServiceId = serviceId;
                        planRkPdService.PlanRecordKeeperProductId = CurrentPlanRecordKeeperProduct.Id;
                    }
                    planRkPdService.ServiceOffered = serviceOffered;
                    planRkPdService.Notes = notes;
                    planRkPdService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
                }
            }

            CurrentPlanRecordKeeperProduct.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            // Do not close form, so users can edit, save, and continue editing
            //this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabServices"];
            dgvServices.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabFees"];
            dgvFees.Focus();
        }

        private void LoadDgvFees()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvFees.CurrentCell != null)
            {
                currentCellRow = dgvFees.CurrentCell.RowIndex;
                currentCellCol = dgvFees.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboInvestmentViews"/>.
            switch (cboFeeViews.SelectedIndex)
            {
                case 0:
                    dataTable = PlanRecordKeeperProductFee.GetAssociatedActive(CurrentPlanRecordKeeperProduct);
                    break;
                case 1:
                    dataTable = PlanRecordKeeperProductFee.GetAssociatedActive(CurrentPlanRecordKeeperProduct);
                    break;
                default:
                    return;
            }

            dgvFees.DataSource = dataTable;

            // Display/order the columns.
            dgvFees.Columns["PlanRecordKeeperProductFeeId"].Visible = false;
            dgvFees.Columns["PlanId"].Visible = false;
            dgvFees.Columns["RecordKeeperProductId"].Visible = false;
            dgvFees.Columns["CreatedBy"].Visible = false;
            dgvFees.Columns["ModifiedBy"].Visible = false;
            dgvFees.Columns["StateCode"].Visible = false;
            dgvFees.Columns["Notes"].Visible = false;

            dgvFees.Columns["Fee"].DisplayIndex = 0;

            dgvFees.Columns["Benchmark25Fee"].DisplayIndex = 1;
            dgvFees.Columns["Benchmark25Fee"].HeaderText = "25% Benchmark";

            dgvFees.Columns["Benchmark50Fee"].DisplayIndex = 2;
            dgvFees.Columns["Benchmark50Fee"].HeaderText = "50% Benchmark";

            dgvFees.Columns["Benchmark75Fee"].DisplayIndex = 3;
            dgvFees.Columns["Benchmark75Fee"].HeaderText = "75% Benchmark";
            
            dgvFees.Columns["RevenueSharingPaid"].DisplayIndex = 4;
            dgvFees.Columns["RevenueSharingPaid"].HeaderText = "Revenue Sharing Paid";

            dgvFees.Columns["ForfeituresPaid"].DisplayIndex = 5;
            dgvFees.Columns["ForfeituresPaid"].HeaderText = "Forfeitures Paid";

            dgvFees.Columns["ParticipantsPaid"].DisplayIndex = 6;
            dgvFees.Columns["ParticipantsPaid"].HeaderText = "Participants Paid";

            dgvFees.Columns["PlanSponsorPaid"].DisplayIndex = 7;
            dgvFees.Columns["PlanSponsorPaid"].HeaderText = "Plan Sponsor Paid";

            dgvFees.Columns["AsOfDate"].DisplayIndex = 8;
            dgvFees.Columns["ModifiedOn"].DisplayIndex = 9;
            dgvFees.Columns["CreatedOn"].DisplayIndex = 10;

            if (dgvFees.RowCount > 0 && dgvFees.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvFees.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvFees.CurrentCell = selectedCell;
                }
                else
                {
                    dgvFees.CurrentCell = dgvFees.FirstDisplayedCell;
                }
            }
        }

        private void cboFeeViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvFees();
        }

        private void btnNewFee_Click(object sender, EventArgs e)
        {
            frmPlanRecordKeeperProductFee frmPlanRecordKeeperProductFee = new frmPlanRecordKeeperProductFee(frmMain_Parent, CurrentPlanRecordKeeperProduct);
            frmPlanRecordKeeperProductFee.FormClosed += frmPlanRecordKeeperProductFee_FormClosed;
        }

        private void frmPlanRecordKeeperProductFee_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvFees();
        }

        private void dgvFees_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvFees.CurrentRow.Index;
            Guid planRkFeeId = new Guid(dgvFees.Rows[index].Cells["PlanRecordKeeperProductFeeId"].Value.ToString());
            PlanRecordKeeperProductFee planRkPdFee = new PlanRecordKeeperProductFee(planRkFeeId);
            frmPlanRecordKeeperProductFee frmPlanRecordKeeperProductFee = new frmPlanRecordKeeperProductFee(frmMain_Parent, planRkPdFee);
            frmPlanRecordKeeperProductFee.FormClosed += frmPlanRecordKeeperProductFee_FormClosed;
        }

        private void btnDeleteFee_Click(object sender, EventArgs e)
        {
            int index = dgvFees.CurrentRow.Index;
            Guid planRkPdFeeId = new Guid(dgvFees.Rows[index].Cells["PlanRecordKeeperProductFeeId"].Value.ToString());
            PlanRecordKeeperProductFee planRkPdFee = new PlanRecordKeeperProductFee(planRkPdFeeId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected plan record keeper product fee?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                planRkPdFee.DeleteRecordFromDatabase();
                LoadDgvFees();
            }
        }

        private void LoadDgvServices(bool refresh = false)
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvServices.CurrentCell != null)
            {
                currentCellRow = dgvServices.CurrentCell.RowIndex;
                currentCellCol = dgvServices.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboServicesView"/>.
            /*switch (cboServicesView.SelectedIndex)
            {
                case 0:
                    dataTable = Service.GetActive();
                    break;
                case 1:
                    dataTable = Service.GetInactive();
                    break;
                default:
                    return;
            }*/
            switch (cboServicesView.SelectedIndex)
            {
                case 0:
                    dataTable = ProductService.GetAssociatedOfferedActive(new Product(CurrentPlanRecordKeeperProduct.ProductId));
                    break;
                case 1:
                    dataTable = ProductService.GetAssociatedOfferedInactive(new Product(CurrentPlanRecordKeeperProduct.ProductId));
                    break;
                default:
                    return;
            }

            dataTable = dataTable.AsEnumerable().Where(x => x["Type"].ToString() == "Record Keeper").CopyToDataTable();

            //dataTable.Columns.Add("ServiceOffered", typeof(bool));
            dataTable.Columns.Add("Notes");

            dgvServices.DataSource = dataTable;

            // Display/order the columns.
            dgvServices.Columns["ServiceId"].Visible = false;
            dgvServices.Columns["Type"].Visible = false;
            dgvServices.Columns["CreatedBy"].Visible = false;
            dgvServices.Columns["CreatedOn"].Visible = false;
            dgvServices.Columns["ModifiedBy"].Visible = false;
            dgvServices.Columns["ModifiedOn"].Visible = false;
            dgvServices.Columns["StateCode"].Visible = false;
            dgvServices.Columns["ServiceId1"].Visible = false;
            dgvServices.Columns["ProductId"].Visible = false;
            dgvServices.Columns["ProductServiceId"].Visible = false;
            dgvServices.Columns["CreatedBy1"].Visible = false;
            dgvServices.Columns["CreatedOn1"].Visible = false;
            dgvServices.Columns["ModifiedBy1"].Visible = false;
            dgvServices.Columns["ModifiedOn1"].Visible = false;
            dgvServices.Columns["StateCode1"].Visible = false;

            dgvServices.Columns["Name"].DisplayIndex = 0;
            dgvServices.Columns["Name"].ReadOnly = true;
            dgvServices.Columns["Category"].DisplayIndex = 1;
            dgvServices.Columns["Category"].ReadOnly = true;
            dgvServices.Columns["ServiceOffered"].DisplayIndex = 2;
            dgvServices.Columns["ServiceOffered"].ReadOnly = false;
            
            dgvServices.Columns["Notes"].DisplayIndex = 3;
            dgvServices.Columns["Notes"].ReadOnly = false;


            // set service offered values
            if (refresh == true)
            {
                DataTable planRkPdServices = PlanRecordKeeperProductService.GetAssociated(CurrentPlanRecordKeeperProduct);

                //foreach (DataGridViewRow drServices in dgvServices.Rows)
                for (int rowIndex = 0; rowIndex < dgvServices.Rows.Count; rowIndex++)
                {
                    DataGridViewRow drServices = dgvServices.Rows[rowIndex];
                    Guid serviceId = new Guid(drServices.Cells["ServiceId"].Value.ToString());
                    var ps = planRkPdServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)drServices.Cells["ServiceOffered"];
                        var serviceOffered = SqlBoolean.Parse(ps.CopyToDataTable().Rows[0]["ServiceOffered"].ToString()).IsTrue;
                        drServices.Cells["ServiceOffered"].Value = serviceOffered.ToString();

                        var notes = ps.CopyToDataTable().Rows[0]["Notes"].ToString();
                        drServices.Cells["Notes"].Value = notes;
                    }

                }
            }

            if (dgvServices.RowCount > 0 && dgvServices.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvServices.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvServices.CurrentCell = selectedCell;
                }
                else
                {
                    dgvServices.CurrentCell = dgvServices.FirstDisplayedCell;
                }
            }
        }

        private void cboServicesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvServices();
        }

        private void cboRecordKeeper_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCboProduct();
        }

        private void LoadCboProduct()
        {
            cboProduct.Items.Clear();

            if (null != cboRecordKeeper.SelectedItem && 0 < cboRecordKeeper.SelectedIndex)
            {
                ListItem li = (ListItem)cboRecordKeeper.SelectedItem;
                Guid recordKeeperId = (Guid)li.HiddenObject;

                DataTable productTable = Product.GetAllWithRecordKeeper(recordKeeperId);

                cboProduct.Items.Add(new ListItem("", ""));

                foreach (DataRow dr in productTable.Rows)
                {
                    Guid productId = new Guid(dr["ProductId"].ToString());
                    string name = dr["Name"].ToString();
                    cboProduct.Items.Add(new ListItem(name, productId));
                }

                cboProduct.ResetText();
                cboProduct.ForeColor = Color.Empty;
            }
            else
            {
                cboProduct.Items.Insert(0," - Select a Record Keeper to generate Product options - ");
                cboProduct.SelectedIndex = 0;
                cboProduct.ForeColor = Color.Gray;
            }
        }

        private void lblMenuIssues_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabIssues"];
            dgvIssues.Focus();
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
                var dt = dataTable.AsEnumerable().Where(x => x.Field<Guid?>("PlanRecordKeeperProductId") == CurrentPlanRecordKeeperProduct.Id);
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
            dataTable.Columns.Add("Plan");
            foreach (DataRow dr in dataTable.Rows)
            {
                string planIdString = dr["PlanId"].ToString();
                if (String.IsNullOrWhiteSpace(planIdString) == false)
                {
                    Guid planId = new Guid(planIdString);
                    Plan plan = new Plan(planId);
                    dr["Plan"] = plan.Name;
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
            dgvIssues.Columns["Plan"].DisplayIndex = 1;
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

        private void frmServiceIssue_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvIssues();
        }

        private void btnNewIssue_Click(object sender, EventArgs e)
        {
            frmServiceIssue frmServiceIssue = new frmServiceIssue(frmMain_Parent, CurrentPlanRecordKeeperProduct);
            frmServiceIssue.FormClosed += frmServiceIssue_FormClosed;
        }

        private void cboIssueViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvIssues();
        }

        private void dgvIssues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvIssues.CurrentRow.Index;
            Guid serviceIssueId = new Guid(dgvIssues.Rows[index].Cells["ServiceIssueId"].Value.ToString());
            ServiceIssue serviceIssue = new ServiceIssue(serviceIssueId);
            frmServiceIssue frmServiceIssue = new frmServiceIssue(frmMain_Parent, serviceIssue);
            frmServiceIssue.FormClosed += frmServiceIssue_FormClosed;
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

        private void btnSelectAllServices_Click(object sender, EventArgs e)
        {
            bool allSelected = true;

            foreach (DataGridViewRow row in dgvServices.Rows)
            {
                if (row.Cells["ServiceOffered"].Value.ToString() != ""
                     && bool.Parse(row.Cells["ServiceOffered"].Value.ToString()) == false)
                {
                    System.Console.WriteLine("ServiceOffered Not Selected: " + row.Index);
                    allSelected = false;
                }
            }

            // Invert all selected for updates
            allSelected = !allSelected;

            foreach (DataGridViewRow row in dgvServices.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["ServiceOffered"];
                if (allSelected)
                {
                    chk.Value = true;
                }
                else
                {
                    chk.Value = false;
                }
            }
        }
    }
}
