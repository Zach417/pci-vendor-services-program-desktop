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
	public partial class frmPlanAdvisorFee : Form, IMessageFilter
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
        public PlanAdvisorFee CurrentPlanAdvisorFee;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="planAdvisor"></param>
        /// <param name="Close"></param>
        public frmPlanAdvisorFee(frmMain mf, VSP.Business.Entities.PlanAdvisor planAdvisor, FormClosedEventHandler Close = null)
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

            Plan plan = new Plan(planAdvisor.PlanId);
            Advisor advisor = new Advisor(planAdvisor.AdvisorId);
            DataIntegrationHub.Business.Entities.PlanAdvisor plAd = new DataIntegrationHub.Business.Entities.PlanAdvisor(planAdvisor.AdvisorId);
            txtAdvisor.Text = plAd.Name;

            CurrentPlanAdvisorFee = new PlanAdvisorFee();
            CurrentPlanAdvisorFee.PlanAdvisorId = planAdvisor.Id;

            txtPlan.Text = plan.Name;

            ss.Close();
            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="planAdvisorFee"></param>
        /// <param name="Close"></param>
        public frmPlanAdvisorFee(frmMain mf, PlanAdvisorFee planAdvisorFee, FormClosedEventHandler Close = null)
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

            Business.Entities.PlanAdvisor planAdvisor = new Business.Entities.PlanAdvisor(planAdvisorFee.PlanAdvisorId);
            Plan plan = new Plan(planAdvisor.PlanId);
            Advisor advisor = new Advisor(planAdvisor.AdvisorId);
            DataIntegrationHub.Business.Entities.PlanAdvisor dihPlanAdvisor = new DataIntegrationHub.Business.Entities.PlanAdvisor(advisor.Id);

            CurrentPlanAdvisorFee = planAdvisorFee;
            txtPlan.Text = plan.Name;
            txtAdvisor.Text = dihPlanAdvisor.Name;
            txtNotes.Text = CurrentPlanAdvisorFee.Notes;
            txtName.Text = CurrentPlanAdvisorFee.Name;

            txtNotes.Focus();

            if (CurrentPlanAdvisorFee.Fee != null)
            {
                txtFee.Text = ((decimal)CurrentPlanAdvisorFee.Fee).ToString("#,##");
            }

            if (CurrentPlanAdvisorFee.Benchmark25Fee != null)
            {
                txtBenchmark25Fee.Text = ((decimal)CurrentPlanAdvisorFee.Benchmark25Fee).ToString("#,##");
            }

            if (CurrentPlanAdvisorFee.Benchmark50Fee != null)
            {
                txtBenchmark50Fee.Text = ((decimal)CurrentPlanAdvisorFee.Benchmark50Fee).ToString("#,##");
            }

            if (CurrentPlanAdvisorFee.Benchmark75Fee != null)
            {
                txtBenchmark75Fee.Text = ((decimal)CurrentPlanAdvisorFee.Benchmark75Fee).ToString("#,##");
            }

            if (CurrentPlanAdvisorFee.AsOfDate != null)
            {
                dateAsOfDate.Value = (DateTime)CurrentPlanAdvisorFee.AsOfDate;
                dateAsOfDate.Checked = true;
            }
            else
            {
                dateAsOfDate.Checked = false;
            }

            chbxRevenueSharingPaid.Checked = CurrentPlanAdvisorFee.RevenueSharingPaid;
            chbxForfeituresPaid.Checked = CurrentPlanAdvisorFee.ForfeituresPaid;
            chbxParticipantsPaid.Checked = CurrentPlanAdvisorFee.ParticipantsPaid;
            chbxPlanSponsorPaid.Checked = CurrentPlanAdvisorFee.PlanSponsorPaid;

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
            if (String.IsNullOrWhiteSpace(txtFee.Text))
            {
                CurrentPlanAdvisorFee.Fee = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisorFee.Fee = Decimal.Parse(txtFee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Fee string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtBenchmark25Fee.Text))
            {
                CurrentPlanAdvisorFee.Benchmark25Fee = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisorFee.Benchmark25Fee = Decimal.Parse(txtBenchmark25Fee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Benchmark 25% fee string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtBenchmark50Fee.Text))
            {
                CurrentPlanAdvisorFee.Benchmark50Fee = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisorFee.Benchmark50Fee = Decimal.Parse(txtBenchmark50Fee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Benchmark 50% fee string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtBenchmark75Fee.Text))
            {
                CurrentPlanAdvisorFee.Benchmark75Fee = null;
            }
            else
            {
                try
                {
                    CurrentPlanAdvisorFee.Benchmark75Fee = Decimal.Parse(txtBenchmark75Fee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Benchmark 75% fee string not in decimal format");
                    return;
                }
            }

            if (dateAsOfDate.Checked)
            {
                CurrentPlanAdvisorFee.AsOfDate = dateAsOfDate.Value;
            }
            else
            {
                CurrentPlanAdvisorFee.AsOfDate = null;
            }
            CurrentPlanAdvisorFee.RevenueSharingPaid = chbxRevenueSharingPaid.Checked;
            CurrentPlanAdvisorFee.ForfeituresPaid = chbxForfeituresPaid.Checked;
            CurrentPlanAdvisorFee.ParticipantsPaid = chbxParticipantsPaid.Checked;
            CurrentPlanAdvisorFee.PlanSponsorPaid = chbxPlanSponsorPaid.Checked;
            CurrentPlanAdvisorFee.Notes = txtNotes.Text;
            CurrentPlanAdvisorFee.Name = txtName.Text;

            CurrentPlanAdvisorFee.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            //this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtPlan.Text;
        }
    }
}
