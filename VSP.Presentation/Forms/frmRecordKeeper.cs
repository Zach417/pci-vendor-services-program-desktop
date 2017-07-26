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
	public partial class frmRecordKeeper : Form, IMessageFilter
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
        public VSP.Business.Entities.RecordKeeper CurrentRK;

        public frmRecordKeeper(frmMain mf, VSP.Business.Entities.RecordKeeper recordKeeper, FormClosedEventHandler Close = null)
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

            DataIntegrationHub.Business.Entities.RecordKeeper dihRk = new DataIntegrationHub.Business.Entities.RecordKeeper(recordKeeper.Id);

            CurrentRK = recordKeeper;
            txtName.Text = dihRk.Name;
            txtRfpName.Text = dihRk.RfpContactName;
            txtRfpEmail.Text = dihRk.RfpContactEmail;
            txtPhilosophyStrategy.Text = CurrentRK.PhilosophyStrategy;
            txtPoliciesProcedures.Text = CurrentRK.PoliciesProcedures;
            txtSecurityTechnology.Text = CurrentRK.SecurityTechnology;
            txtCredentialsExperience.Text = CurrentRK.CredentialsExperiences;
            txtActionsPenalties.Text = CurrentRK.ActionsPenalties;
            txtMergersAcquisitions.Text = CurrentRK.MergersAcquisitions;
            txtFeeArrangementsRelationships.Text = CurrentRK.FeeArrangementsRelationships;

            if (CurrentRK.PlansServiced != null)
            {
                txtPlansServiced.Text = ((int)CurrentRK.PlansServiced).ToString();
            }

            if (CurrentRK.AssetsServiced != null)
            {
                txtAssetsServiced.Text = ((decimal)CurrentRK.AssetsServiced).ToString();
            }

            cboIssueViews.SelectedIndex = 0;
            cboProductViews.SelectedIndex = 0;

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

        private void lblMenuSummary_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabSummary"];

        }

        private void lblMenuProducts_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabProducts"];
        }

        private void lblMenuIssues_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlDetail.SelectedTab = tabControlDetail.TabPages["tabIssues"];
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
            CurrentRK.PhilosophyStrategy = txtPhilosophyStrategy.Text;
            CurrentRK.PoliciesProcedures = txtPoliciesProcedures.Text;
            CurrentRK.SecurityTechnology = txtSecurityTechnology.Text;
            CurrentRK.CredentialsExperiences = txtCredentialsExperience.Text;
            CurrentRK.ActionsPenalties = txtActionsPenalties.Text;
            CurrentRK.MergersAcquisitions = txtMergersAcquisitions.Text;
            CurrentRK.FeeArrangementsRelationships = txtFeeArrangementsRelationships.Text;

            if (String.IsNullOrWhiteSpace(txtPlansServiced.Text))
            {
                CurrentRK.PlansServiced = null;
            }
            else
            {
                try
                {
                    CurrentRK.PlansServiced = int.Parse(txtPlansServiced.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Plans Serviced field is not in an integer format.");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtAssetsServiced.Text))
            {
                CurrentRK.AssetsServiced = null;
            }
            else
            {
                try
                {
                    CurrentRK.AssetsServiced = decimal.Parse(txtAssetsServiced.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Assets Serviced field is not in a decimal format.");
                    return;
                }
            }

            CurrentRK.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtName.Text;
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
                var dt = dataTable.AsEnumerable().Where(x => x.Field<Guid>("RecordKeeperId") == CurrentRK.Id);
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
        }

        private void frmServiceIssue_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvIssues();
        }

        private void btnNewIssue_Click(object sender, EventArgs e)
        {
            frmServiceIssue frmServiceIssue = new frmServiceIssue(frmMain_Parent, CurrentRK);
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

        private void LoadDgvProducts()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboIssueViews"/>.
            switch (cboIssueViews.SelectedIndex)
            {
                case 0:
                    dataTable = Product.GetActive();
                    break;
                case 1:
                    dataTable = Product.GetInactive();
                    break;
                default:
                    return;
            }

            if (dataTable.Rows.Count > 0)
            {
                var dt = dataTable.AsEnumerable().Where(x => x.Field<Guid>("RecordKeeperId") == CurrentRK.Id);
                if (dt.Any())
                {
                    dataTable = dt.CopyToDataTable();
                }
                else
                {
                    dataTable.Rows.Clear();
                }
            }

            dgvProducts.DataSource = dataTable;

            // Display/order the columns.
            dgvProducts.Columns["ProductId"].Visible = false;
            dgvProducts.Columns["RecordKeeperId"].Visible = false;
            dgvProducts.Columns["CreatedBy"].Visible = false;
            dgvProducts.Columns["ModifiedBy"].Visible = false;
            dgvProducts.Columns["StateCode"].Visible = false;

            dgvProducts.Columns["Name"].DisplayIndex = 0;
            dgvProducts.Columns["CreatedOn"].DisplayIndex = 1;
            dgvProducts.Columns["ModifiedOn"].DisplayIndex = 2;
        }

        private void frmProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDgvProducts();
        }

        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            frmProduct frmProduct = new frmProduct(frmMain_Parent, CurrentRK);
            frmProduct.FormClosed += frmProduct_FormClosed;
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvProducts.CurrentRow.Index;
            Guid productId = new Guid(dgvProducts.Rows[index].Cells["ProductId"].Value.ToString());
            Product product = new Product(productId);
            frmProduct frmProduct = new frmProduct(frmMain_Parent, product);
            frmProduct.FormClosed += frmProduct_FormClosed;
        }

        private void cboProductViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvProducts();
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                return;
            }

            int index = dgvProducts.CurrentRow.Index;
            Guid productId = new Guid(dgvProducts.Rows[index].Cells[0].Value.ToString());
            Product product = new Product(productId);

            DialogResult result = MessageBox.Show("Are you sure you wish to delete " + product.Name + "?", "Attention", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                product.DeleteRecordFromDatabase();
                LoadDgvProducts();
            }
        }
	}
}
