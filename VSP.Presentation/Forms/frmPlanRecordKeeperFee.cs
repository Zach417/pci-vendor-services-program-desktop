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
	public partial class frmPlanRecordKeeperFee : Form, IMessageFilter
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
        public PlanRecordKeeperFee CurrentPlanRecordKeeperFee;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmPlanRecordKeeperFee(frmMain mf, PlanRecordKeeper planRecordKeeper, FormClosedEventHandler Close = null)
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

            Plan plan = new Plan(planRecordKeeper.PlanId);
            DataIntegrationHub.Business.Entities.RecordKeeper rk = new DataIntegrationHub.Business.Entities.RecordKeeper(planRecordKeeper.RecordKeeperId);

            CurrentPlanRecordKeeperFee = new PlanRecordKeeperFee();
            CurrentPlanRecordKeeperFee.PlanId = plan.PlanId;
            CurrentPlanRecordKeeperFee.RecordKeeperId = rk.RecordKeeperId;

            txtPlan.Text = plan.Name;
            txtRecordKeeper.Text = rk.Name;

            ss.Close();
            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmPlanRecordKeeperFee(frmMain mf, PlanRecordKeeperFee planRecordKeeperFee, FormClosedEventHandler Close = null)
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

            Plan plan = new Plan(planRecordKeeperFee.PlanId);
            DataIntegrationHub.Business.Entities.RecordKeeper rk = new DataIntegrationHub.Business.Entities.RecordKeeper(planRecordKeeperFee.RecordKeeperId);

            CurrentPlanRecordKeeperFee = planRecordKeeperFee;
            txtPlan.Text = plan.Name;
            txtRecordKeeper.Text = rk.Name;

            if (CurrentPlanRecordKeeperFee.Fee != null)
            {
                txtFee.Text = ((decimal)CurrentPlanRecordKeeperFee.Fee).ToString("#,##");
            }

            if (CurrentPlanRecordKeeperFee.BenchmarkFee != null)
            {
                txtBenchmarkFee.Text = ((decimal)CurrentPlanRecordKeeperFee.BenchmarkFee).ToString("#,##");
            }

            if (CurrentPlanRecordKeeperFee.AsOfDate != null)
            {
                txtAsOfDate.Text = ((DateTime)CurrentPlanRecordKeeperFee.AsOfDate).ToString("MM/dd/yyyy");
            }

            if (CurrentPlanRecordKeeperFee.PaymentRevenueSharing != null)
            {
                txtPaymentRevenueSharing.Text = ((decimal)CurrentPlanRecordKeeperFee.PaymentRevenueSharing).ToString("#,##");
            }

            if (CurrentPlanRecordKeeperFee.PaymentForfeitures != null)
            {
                txtPaymentForfeitures.Text = ((decimal)CurrentPlanRecordKeeperFee.PaymentForfeitures).ToString("#,##");
            }

            if (CurrentPlanRecordKeeperFee.PaymentParticipants != null)
            {
                txtPaymentParticipants.Text = ((decimal)CurrentPlanRecordKeeperFee.PaymentParticipants).ToString("#,##");
            }

            if (CurrentPlanRecordKeeperFee.PaymentPlanSponsor != null)
            {
                txtPaymentPlanSponsor.Text = ((decimal)CurrentPlanRecordKeeperFee.PaymentPlanSponsor).ToString("#,##");
            }

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
                CurrentPlanRecordKeeperFee.Fee = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.Fee = Decimal.Parse(txtFee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Fee string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtBenchmarkFee.Text))
            {
                CurrentPlanRecordKeeperFee.BenchmarkFee = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.BenchmarkFee = Decimal.Parse(txtBenchmarkFee.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Benchmark fee string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtAsOfDate.Text))
            {
                CurrentPlanRecordKeeperFee.AsOfDate = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.AsOfDate = DateTime.Parse(txtAsOfDate.Text);
                }
                catch
                {
                    MessageBox.Show("Error: As Of Date string not in date format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtPaymentRevenueSharing.Text))
            {
                CurrentPlanRecordKeeperFee.PaymentRevenueSharing = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.PaymentRevenueSharing = Decimal.Parse(txtPaymentRevenueSharing.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Revenue sharing payment string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtPaymentForfeitures.Text))
            {
                CurrentPlanRecordKeeperFee.PaymentForfeitures = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.PaymentForfeitures = Decimal.Parse(txtPaymentForfeitures.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Forfeiture payment string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtPaymentParticipants.Text))
            {
                CurrentPlanRecordKeeperFee.PaymentParticipants = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.PaymentParticipants = Decimal.Parse(txtPaymentParticipants.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Participant payment string not in decimal format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtPaymentPlanSponsor.Text))
            {
                CurrentPlanRecordKeeperFee.PaymentPlanSponsor = null;
            }
            else
            {
                try
                {
                    CurrentPlanRecordKeeperFee.PaymentPlanSponsor = Decimal.Parse(txtPaymentPlanSponsor.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Plan Sponsor payment string not in decimal format");
                    return;
                }
            }

            CurrentPlanRecordKeeperFee.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtPlan.Text;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
	}
}
