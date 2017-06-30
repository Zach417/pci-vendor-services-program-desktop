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
	public partial class frmPlanAuditor : Form, IMessageFilter
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
        public PlanAuditor CurrentPlanAuditor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="Close"></param>
        public frmPlanAuditor(frmMain mf, Plan plan, FormClosedEventHandler Close = null)
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

            CurrentPlanAuditor = new PlanAuditor();
            CurrentPlanAuditor.PlanId = plan.PlanId;

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
        public frmPlanAuditor(frmMain mf, PlanAuditor planAuditor, FormClosedEventHandler Close = null)
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

            CurrentPlanAuditor = planAuditor;

            if (CurrentPlanAuditor.PlanId != null)
            {
                Plan plan = new Plan(CurrentPlanAuditor.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            if (CurrentPlanAuditor.AuditorId != null)
            {
                DataIntegrationHub.Business.Entities.Auditor auditor = new DataIntegrationHub.Business.Entities.Auditor(CurrentPlanAuditor.AuditorId);
                cboAuditor.Text = auditor.Name;
            }

            if (CurrentPlanAuditor.DateAdded != null)
            {
                txtDateAdded.Text = ((DateTime)CurrentPlanAuditor.DateAdded).ToString("MM/dd/yyyy");
            }

            if (CurrentPlanAuditor.DateRemoved != null)
            {
                txtDateRemoved.Text = ((DateTime)CurrentPlanAuditor.DateRemoved).ToString("MM/dd/yyyy");
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

        public void PreloadCbos()
        {
            cboAuditor.Items.Clear();
            cboPlan.Items.Clear();

            cboAuditor.Items.Add(new ListItem("", ""));
            cboPlan.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in DataIntegrationHub.Business.Entities.Auditor.GetAll().Rows)
            {
                Guid auditorId = new Guid(dr["AuditorId"].ToString());
                string name = dr["Name"].ToString();
                cboAuditor.Items.Add(new ListItem(name, auditorId));
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
            if (cboAuditor.SelectedIndex <= 0)
            {
                MessageBox.Show("Error: Auditor cannot be left blank.");
                return;
            }
            else
            {
                ListItem li = (ListItem)cboAuditor.SelectedItem;
                Guid auditorId = (Guid)li.HiddenObject;
                CurrentPlanAuditor.AuditorId = auditorId;
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
                CurrentPlanAuditor.PlanId = plan.PlanId;
            }

            if (String.IsNullOrWhiteSpace(txtDateAdded.Text))
            {
                CurrentPlanAuditor.DateAdded = null;
            }
            else
            {
                try
                {
                    CurrentPlanAuditor.DateAdded = DateTime.Parse(txtDateAdded.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Date Added string not in date format");
                    return;
                }
            }

            if (String.IsNullOrWhiteSpace(txtDateRemoved.Text))
            {
                CurrentPlanAuditor.DateRemoved = null;
            }
            else
            {
                try
                {
                    CurrentPlanAuditor.DateRemoved = DateTime.Parse(txtDateRemoved.Text);
                }
                catch
                {
                    MessageBox.Show("Error: Date Removed string not in date format");
                    return;
                }
            }

            CurrentPlanAuditor.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            this.Close();
        }
	}
}
