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
	public partial class frmSearch : Form, IMessageFilter
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
        public Search CurrentSearch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="Close"></param>
        public frmSearch(frmMain mf, FormClosedEventHandler Close = null)
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

            CurrentSearch = new Search();
            txtName.Text = CurrentSearch.Name;
            txtCurrentRkNotes.Text = CurrentSearch.CurrentRkNotes;

            if (CurrentSearch.AsOfDate != null)
            {
                txtAsOfDate.Text = ((DateTime)CurrentSearch.AsOfDate).ToString("MM/dd/yyyy");
            }


            ss.Close();
            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmSearch(frmMain mf, Search search, FormClosedEventHandler Close = null)
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

            CurrentSearch = search;

            try
            {
                Plan plan = new Plan(CurrentSearch.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }
            catch
            {
                MessageBox.Show("This search does not have a valid plan selected.");
            }

            txtName.Text = CurrentSearch.Name;
            txtCurrentRkNotes.Text = CurrentSearch.CurrentRkNotes;

            if (CurrentSearch.AsOfDate != null)
            {
                txtAsOfDate.Text = ((DateTime)CurrentSearch.AsOfDate).ToString("MM/dd/yyyy");
            }

            cboResultsView.SelectedIndex = 0;
            cboBidViews.SelectedIndex = 0;
            cboFundViews.SelectedIndex = 0;
            cboQuestionViews.SelectedIndex = 0;
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
            cboPlan.Items.Clear();

            cboPlan.Items.Add(new ListItem("", ""));

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
            Save();
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtName.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabServices"];
        }

        private void label4_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabResults"];
        }

        private void label5_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabQuestions"];
        }

        private void label6_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabFunds"];
        }

        private void LoadDgvServices(bool refresh = false)
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboServicesView"/>.
            switch (cboServicesView.SelectedIndex)
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

            dataTable = dataTable.AsEnumerable().Where(x => x["Type"].ToString() == "Record Keeper").CopyToDataTable();

            dataTable.Columns.Add("ServiceRequired", typeof(bool));
            dataTable.Columns.Add("ServicePreferred", typeof(bool));
            dataTable.Columns.Add("ServiceOptional", typeof(bool));

            dgvServices.DataSource = dataTable;

            // Display/order the columns.
            dgvServices.Columns["ServiceId"].Visible = false;
            dgvServices.Columns["Type"].Visible = false;
            dgvServices.Columns["CreatedBy"].Visible = false;
            dgvServices.Columns["CreatedOn"].Visible = false;
            dgvServices.Columns["ModifiedBy"].Visible = false;
            dgvServices.Columns["ModifiedOn"].Visible = false;
            dgvServices.Columns["StateCode"].Visible = false;

            dgvServices.Columns["Name"].DisplayIndex = 0;
            dgvServices.Columns["Name"].ReadOnly = true;
            dgvServices.Columns["Category"].DisplayIndex = 1;
            dgvServices.Columns["Category"].ReadOnly = true;
            dgvServices.Columns["ServiceRequired"].DisplayIndex = 2;
            dgvServices.Columns["ServiceRequired"].ReadOnly = false;
            dgvServices.Columns["ServicePreferred"].DisplayIndex = 3;
            dgvServices.Columns["ServicePreferred"].ReadOnly = false;
            dgvServices.Columns["ServiceOptional"].DisplayIndex = 4;
            dgvServices.Columns["ServiceOptional"].ReadOnly = false;


            // set service offered values
            if (refresh == true)
            {
                DataTable planRkServices = SearchService.GetAssociated(CurrentSearch);
                int rowIndex = 0;

                foreach (DataGridViewRow drServices in dgvServices.Rows)
                {
                    Guid serviceId = new Guid(drServices.Cells["ServiceId"].Value.ToString());
                    var ps = planRkServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        var serviceRequired = SqlBoolean.Parse(ps.CopyToDataTable().Rows[0]["ServiceRequired"].ToString()).IsTrue;
                        dgvServices.Rows[rowIndex].Cells["ServiceRequired"].Value = serviceRequired.ToString();

                        var servicePreferred = SqlBoolean.Parse(ps.CopyToDataTable().Rows[0]["ServicePreferred"].ToString()).IsTrue;
                        dgvServices.Rows[rowIndex].Cells["ServicePreferred"].Value = servicePreferred.ToString();

                        var serviceOptional = SqlBoolean.Parse(ps.CopyToDataTable().Rows[0]["ServiceOptional"].ToString()).IsTrue;
                        dgvServices.Rows[rowIndex].Cells["ServiceOptional"].Value = serviceOptional.ToString();
                    }

                    rowIndex++;
                }
            }
        }

        private void cboServicesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvServices();
        }

        private void LoadDgvResults()
        {
            DataTable dataTable = SearchRecordKeeper.GetAssociated(CurrentSearch);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboResultsView"/>.
            switch (cboResultsView.SelectedIndex)
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

            dataTable.Columns.Add("RecordKeeper", typeof(string));
            dataTable.Columns.Add("URL", typeof(string));

            dgvResults.DataSource = dataTable;

            // Display/order the columns.
            dgvResults.Columns["SearchRecordKeeperId"].Visible = false;
            dgvResults.Columns["SearchId"].Visible = false;
            dgvResults.Columns["RecordKeeperId"].Visible = false;
            dgvResults.Columns["ModifiedBy"].Visible = false;
            dgvResults.Columns["ModifiedOn"].Visible = false;
            dgvResults.Columns["CreatedBy"].Visible = false;
            dgvResults.Columns["CreatedOn"].Visible = false;
            dgvResults.Columns["StateCode"].Visible = false;

            dgvResults.Columns["Ordinal"].DisplayIndex = 0;
            dgvResults.Columns["RecordKeeper"].DisplayIndex = 1;
            dgvResults.Columns["URL"].DisplayIndex = 2;


            int rowIndex = 0;
            foreach (DataGridViewRow dr in dgvResults.Rows)
            {
                Guid recordKeeperId = new Guid(dr.Cells["RecordKeeperId"].Value.ToString());
                DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(recordKeeperId);
                dgvResults.Rows[rowIndex].Cells["RecordKeeper"].Value = recordKeeper.Name;
                dgvResults.Rows[rowIndex].Cells["URL"].Value = "http://vendor.pension-consultants.com/bid/?rk=" + recordKeeper.RecordKeeperId.ToString() + "&s=" + CurrentSearch.Id.ToString();
                rowIndex++;
            }
        }

        private void cboResultsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvResults();
        }

        private void SaveServices()
        {
            // loop through dgvservices, and update productservice records for record keeper product
            if (dgvServices.Rows.Count > 0)
            {
                DataTable searchServices = SearchService.GetAssociated(CurrentSearch);

                foreach (DataGridViewRow dr in dgvServices.Rows)
                {
                    Guid serviceId = new Guid(dr.Cells["ServiceId"].Value.ToString());

                    bool serviceRequired = false;
                    bool servicePreferred = false;
                    bool serviceOptional = false;

                    if (dr.Cells["ServiceRequired"].Value.ToString() != "")
                    {
                        serviceRequired = bool.Parse(dr.Cells["ServiceRequired"].Value.ToString());
                    }

                    if (dr.Cells["ServicePreferred"].Value.ToString() != "")
                    {
                        servicePreferred = bool.Parse(dr.Cells["ServicePreferred"].Value.ToString());
                    }

                    if (dr.Cells["ServiceOptional"].Value.ToString() != "")
                    {
                        serviceOptional = bool.Parse(dr.Cells["ServiceOptional"].Value.ToString());
                    }

                    var ps = searchServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        Guid searchServiceId = new Guid(ps.CopyToDataTable().Rows[0]["SearchServiceId"].ToString());
                        SearchService searchService = new SearchService(searchServiceId);
                        searchService.ServiceRequired = serviceRequired;
                        searchService.ServicePreferred = servicePreferred;
                        searchService.ServiceOptional = serviceOptional;
                        searchService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
                    }
                    else // rk product does not have service record, so create one
                    {
                        SearchService searchService = new SearchService();
                        searchService.ServiceId = serviceId;
                        searchService.SearchId = CurrentSearch.Id;
                        searchService.ServiceRequired = serviceRequired;
                        searchService.ServicePreferred = servicePreferred;
                        searchService.ServiceOptional = serviceOptional;
                        searchService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
                    }
                }
            }
        }

        private void Save()
        {
            CurrentSearch.Name = txtName.Text;
            CurrentSearch.CurrentRkNotes = txtCurrentRkNotes.Text;

            if (cboPlan.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Selected plan in plan list box cannot be left blank");
                return;
            }
            else
            {
                ListItem li = (ListItem)cboPlan.SelectedItem;
                Plan plan = (Plan)li.HiddenObject;
                CurrentSearch.PlanId = plan.PlanId;
            }

            if (String.IsNullOrWhiteSpace(txtAsOfDate.Text))
            {
                CurrentSearch.AsOfDate = null;
            }
            else
            {
                try
                {
                    CurrentSearch.AsOfDate = DateTime.Parse(txtAsOfDate.Text);
                }
                catch
                {
                    MessageBox.Show("Error: As Of Date field is not in a date format.");
                    return;
                }
            }

            CurrentSearch.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            SaveServices();
        }

        private void btnExecuteSearch_Click(object sender, EventArgs e)
        {
            frmSplashScreen frmSplashScreen = new frmSplashScreen();
            frmSplashScreen.Show();
            Application.DoEvents();

            SaveServices();
            CurrentSearch.ExecuteSearch(frmMain_Parent.CurrentUser.UserId);
            LoadDgvResults();
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabResults"];

            frmSplashScreen.Close();
        }

        private void LoadDgvQuestions()
        {
            DataTable dataTable = SearchQuestion.GetAssociated(CurrentSearch);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboResultsView"/>.
            switch (cboQuestionViews.SelectedIndex)
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

            dgvQuestions.DataSource = dataTable;

            // Display/order the columns.
            dgvQuestions.Columns["SearchQuestionId"].Visible = false;
            dgvQuestions.Columns["SearchId"].Visible = false;
            dgvQuestions.Columns["ModifiedBy"].Visible = false;
            dgvQuestions.Columns["ModifiedOn"].Visible = false;
            dgvQuestions.Columns["CreatedBy"].Visible = false;
            dgvQuestions.Columns["CreatedOn"].Visible = false;
            dgvQuestions.Columns["StateCode"].Visible = false;

            dgvQuestions.Columns["SubjectValue"].DisplayIndex = 0;
            dgvQuestions.Columns["AnswerValue"].DisplayIndex = 1;
        }

        private void cboQuestionViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvQuestions();
        }

        private void LoadDgvFunds()
        {
            DataTable dataTable = SearchFund.GetAssociated(CurrentSearch);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboResultsView"/>.
            switch (cboFundViews.SelectedIndex)
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

            dgvFunds.DataSource = dataTable;

            // Display/order the columns.
            dgvFunds.Columns["SearchFundId"].Visible = false;
            dgvFunds.Columns["SearchId"].Visible = false;
            dgvFunds.Columns["ModifiedBy"].Visible = false;
            dgvFunds.Columns["ModifiedOn"].Visible = false;
            dgvFunds.Columns["CreatedBy"].Visible = false;
            dgvFunds.Columns["CreatedOn"].Visible = false;
            dgvFunds.Columns["StateCode"].Visible = false;

            dgvFunds.Columns["Ticker"].DisplayIndex = 0;
            dgvFunds.Columns["FundName"].DisplayIndex = 1;
        }

        private void cboFundViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvFunds();
        }

        private void btnNewQuestion_Click(object sender, EventArgs e)
        {
            frmSearchQuestion frmSearchQuestion = new frmSearchQuestion(frmMain_Parent, CurrentSearch);
            frmSearchQuestion.FormClosed += frmSearchQuestion_FormClosed;
        }

        void frmSearchQuestion_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvQuestions();
        }

        private void btnNewFund_Click(object sender, EventArgs e)
        {
            frmSearchFund frmSearchFund = new frmSearchFund(frmMain_Parent, CurrentSearch);
            frmSearchFund.FormClosed += frmSearchFund_FormClosed;
        }

        private void frmSearchFund_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvFunds();
        }

        private void dgvQuestions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvQuestions.CurrentRow.Index;
            Guid searchQuestionId = new Guid(dgvQuestions.Rows[index].Cells["SearchQuestionId"].Value.ToString());
            SearchQuestion searchQuestion = new SearchQuestion(searchQuestionId);
            frmSearchQuestion frmSearchQuestion = new frmSearchQuestion(frmMain_Parent, searchQuestion);
            frmSearchQuestion.FormClosed += frmSearchQuestion_FormClosed;
        }

        private void dgvFunds_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvFunds.CurrentRow.Index;
            Guid searchFundId = new Guid(dgvFunds.Rows[index].Cells["SearchFundId"].Value.ToString());
            SearchFund searchFund = new SearchFund(searchFundId);
            frmSearchFund frmSearchFund = new frmSearchFund(frmMain_Parent, searchFund);
            frmSearchFund.FormClosed += frmSearchFund_FormClosed;
        }

        private void btnDeleteFund_Click(object sender, EventArgs e)
        {
            int index = dgvFunds.CurrentRow.Index;
            Guid searchFundId = new Guid(dgvFunds.Rows[index].Cells["SearchFundId"].Value.ToString());
            SearchFund searchFund = new SearchFund(searchFundId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete " + searchFund.FundName + "?", "Attention", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                searchFund.DeleteRecordFromDatabase();
                LoadDgvFunds();
            }
        }

        private void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            int index = dgvQuestions.CurrentRow.Index;
            Guid searchQuestionId = new Guid(dgvQuestions.Rows[index].Cells["SearchQuestionId"].Value.ToString());
            SearchQuestion searchQuestion = new SearchQuestion(searchQuestionId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete \"" + searchQuestion.SubjectValue + "\"?", "Attention", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                searchQuestion.DeleteRecordFromDatabase();
                LoadDgvQuestions();
            }
        }

        private void btnNewRk_Click(object sender, EventArgs e)
        {
            frmSearchRecordKeeper frmSearchRecordKeeper = new frmSearchRecordKeeper(frmMain_Parent, CurrentSearch);
            frmSearchRecordKeeper.FormClosed += frmSearchRecordKeeper_FormClosed;
        }

        private void btnDeleteRk_Click(object sender, EventArgs e)
        {
            int index = dgvResults.CurrentRow.Index;
            Guid searchRecordKeeperId = new Guid(dgvResults.Rows[index].Cells["SearchRecordKeeperId"].Value.ToString());
            SearchRecordKeeper searchRecordKeeper = new SearchRecordKeeper(searchRecordKeeperId);
            DataIntegrationHub.Business.Entities.RecordKeeper rk = new DataIntegrationHub.Business.Entities.RecordKeeper(searchRecordKeeper.RecordKeeperId);

            DialogResult result = MessageBox.Show("Are you sure you wish to remove " + rk.Name + " from the search results?", "Attention", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                searchRecordKeeper.DeleteRecordFromDatabase();
                LoadDgvResults();
            }
        }

        private void dgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvResults.CurrentRow.Index;
            Guid searchRecordKeeperId = new Guid(dgvResults.Rows[index].Cells["SearchRecordKeeperId"].Value.ToString());
            SearchRecordKeeper searchRecordKeeper = new SearchRecordKeeper(searchRecordKeeperId);

            frmSearchRecordKeeper frmSearchRecordKeeper = new frmSearchRecordKeeper(frmMain_Parent, searchRecordKeeper);
            frmSearchRecordKeeper.FormClosed += frmSearchRecordKeeper_FormClosed;
        }

        private void frmSearchRecordKeeper_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvResults();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            tabControlDetail.SelectedTab = tabBids;
        }

        private void LoadDgvBids()
        {
            DataTable dataTable = SearchBid.GetAssociated(CurrentSearch);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboBidViews"/>.
            switch (cboBidViews.SelectedIndex)
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

            dataTable.Columns.Add("RecordKeeper", typeof(string));

            dgvBids.DataSource = dataTable;

            // Display/order the columns.
            dgvBids.Columns["SearchBidId"].Visible = false;
            dgvBids.Columns["SearchId"].Visible = false;
            dgvBids.Columns["RecordKeeperId"].Visible = false;
            dgvBids.Columns["Email"].Visible = false;
            dgvBids.Columns["ConfirmInvestments"].Visible = false;
            dgvBids.Columns["ConfirmServices"].Visible = false;
            dgvBids.Columns["RequiredRevenueExplanation"].Visible = false;
            dgvBids.Columns["AncillaryServices"].Visible = false;
            dgvBids.Columns["Notes"].Visible = false;
            dgvBids.Columns["ModifiedBy"].Visible = false;
            dgvBids.Columns["ModifiedOn"].Visible = false;
            dgvBids.Columns["CreatedBy"].Visible = false;
            dgvBids.Columns["CreatedOn"].Visible = false;
            dgvBids.Columns["StateCode"].Visible = false;

            dgvBids.Columns["FullName"].DisplayIndex = 0;
            dgvBids.Columns["RecordKeeper"].DisplayIndex = 1;
            dgvBids.Columns["RequiredRevenue"].DisplayIndex = 2;
            dgvBids.Columns["IsFinalist"].DisplayIndex = 3;
            dgvBids.Columns["IsRecommended"].DisplayIndex = 4;

            int rowIndex = 0;
            foreach (DataGridViewRow dr in dgvBids.Rows)
            {
                Guid recordKeeperId = new Guid(dr.Cells["RecordKeeperId"].Value.ToString());
                DataIntegrationHub.Business.Entities.RecordKeeper recordKeeper = new DataIntegrationHub.Business.Entities.RecordKeeper(recordKeeperId);
                dgvBids.Rows[rowIndex].Cells["RecordKeeper"].Value = recordKeeper.Name;
                rowIndex++;
            }
        }

        private void cboBidViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvBids();
        }

        private void dgvBids_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvBids.CurrentRow.Index;
            Guid searchBidId = new Guid(dgvBids.Rows[index].Cells["SearchBidId"].Value.ToString());
            SearchBid searchBid = new SearchBid(searchBidId);

            frmSearchBid frmSearchBid = new frmSearchBid(frmMain_Parent, searchBid);
            frmSearchBid.FormClosed += frmSearchBid_FormClosed;
        }

        void frmSearchBid_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvBids();
        }

        private void btnDeleteBid_Click(object sender, EventArgs e)
        {
            int index = dgvBids.CurrentRow.Index;
            Guid searchBidId = new Guid(dgvBids.Rows[index].Cells["SearchBidId"].Value.ToString());
            SearchBid searchBid = new SearchBid(searchBidId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected bid?", "Attention", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                searchBid.DeleteRecordFromDatabase();
                LoadDgvBids();
            }
        }
	}
}
