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
	public partial class frmPlanAdvisor : Form, IMessageFilter
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
        public VSP.Business.Entities.PlanAdvisor CurrentPlanAdvisor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="Close"></param>
        public frmPlanAdvisor(frmMain mf, Plan plan, FormClosedEventHandler Close = null)
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

            CurrentPlanAdvisor = new VSP.Business.Entities.PlanAdvisor();
            CurrentPlanAdvisor.PlanId = plan.PlanId;

            cboPlan.Text = plan.Name + " - " + plan.Description;

            ss.Close();
            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmPlanAdvisor(frmMain mf, VSP.Business.Entities.PlanAdvisor planAdvisor, FormClosedEventHandler Close = null)
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

            CurrentPlanAdvisor = planAdvisor;

            if (CurrentPlanAdvisor.PlanId != null)
            {
                Plan plan = new Plan(CurrentPlanAdvisor.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            if (CurrentPlanAdvisor.AdvisorId != null)
            {
                DataIntegrationHub.Business.Entities.PlanAdvisor advisor = new DataIntegrationHub.Business.Entities.PlanAdvisor(CurrentPlanAdvisor.AdvisorId);
                cboAdvisor.Text = advisor.Name;
            }

            if (CurrentPlanAdvisor.DateAdded != null)
            {
                txtDateAdded.Text = ((DateTime)CurrentPlanAdvisor.DateAdded).ToString("MM/dd/yyyy");
            }

            if (CurrentPlanAdvisor.DateRemoved != null)
            {
                txtDateRemoved.Text = ((DateTime)CurrentPlanAdvisor.DateRemoved).ToString("MM/dd/yyyy");
            }

            cboFeeViews.SelectedIndex = 0;

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
            cboAdvisor.Items.Clear();
            cboPlan.Items.Clear();

            cboAdvisor.Items.Add(new ListItem("", ""));
            cboPlan.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in DataIntegrationHub.Business.Entities.PlanAdvisor.GetAll().Rows)
            {
                Guid advisorId = new Guid(dr["PlanAdvisorId"].ToString());
                string name = dr["Name"].ToString();
                cboAdvisor.Items.Add(new ListItem(name, advisorId));
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
            if (cboAdvisor.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Advisor cannot be left blank.");
                return;
            }
            else
            {
                ListItem li = (ListItem)cboAdvisor.SelectedItem;
                Guid advisorId = (Guid)li.HiddenObject;
                CurrentPlanAdvisor.AdvisorId = advisorId;
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
                CurrentPlanAdvisor.PlanId = plan.PlanId;
            }

            if (String.IsNullOrWhiteSpace(txtDateAdded.Text))
            {
                CurrentPlanAdvisor.DateAdded = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisor.DateAdded = DateTime.Parse(txtDateAdded.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Date Added string not in date format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtDateRemoved.Text))
            {
                CurrentPlanAdvisor.DateRemoved = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisor.DateRemoved = DateTime.Parse(txtDateRemoved.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Date Removed string not in date format");
                    return;
                }
            }

            CurrentPlanAdvisor.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabFees"];
        }

        private void LoadDgvFees()
        {
            DataTable dataTable = new DataTable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboInvestmentViews"/>.
            switch (cboFeeViews.SelectedIndex)
            {
                case 0:
                    dataTable = PlanAdvisorFee.GetAssociatedActive(CurrentPlanAdvisor);
                    break;
                case 1:
                    dataTable = PlanAdvisorFee.GetAssociatedActive(CurrentPlanAdvisor);
                    break;
                default:
                    return;
            }

            dgvFees.DataSource = dataTable;

            // Display/order the columns.
            dgvFees.Columns["PlanAdvisorFeeId"].Visible = false;
            dgvFees.Columns["PlanId"].Visible = false;
            dgvFees.Columns["AdvisorId"].Visible = false;
            dgvFees.Columns["CreatedBy"].Visible = false;
            dgvFees.Columns["ModifiedBy"].Visible = false;
            dgvFees.Columns["StateCode"].Visible = false;

            dgvFees.Columns["Fee"].DisplayIndex = 0;
            dgvFees.Columns["BenchmarkFee"].DisplayIndex = 1;
            dgvFees.Columns["AsOfDate"].DisplayIndex = 2;
            dgvFees.Columns["ModifiedOn"].DisplayIndex = 3;
        }

        private void cboFeeViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvFees();
        }
	}
}
