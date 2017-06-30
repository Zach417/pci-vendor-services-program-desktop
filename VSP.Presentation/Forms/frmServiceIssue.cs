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
	public partial class frmServiceIssue : Form, IMessageFilter
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
        public VSP.Business.Entities.ServiceIssue CurrentServiceIssue;

        public frmServiceIssue(frmMain mf, DataIntegrationHub.Business.Entities.Plan plan, FormClosedEventHandler Close = null)
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

            CurrentServiceIssue = new ServiceIssue();
            CurrentServiceIssue.PlanId = plan.PlanId;
            CurrentServiceIssue.AsOfDate = DateTime.Now;

            if (CurrentServiceIssue.RecordKeeperId != null)
            {
                cboRecordKeeper.Text = new DataIntegrationHub.Business.Entities.RecordKeeper((Guid)CurrentServiceIssue.RecordKeeperId).Name;
            }

            if (CurrentServiceIssue.AuditorId != null)
            {
                cboAuditor.Text = new DataIntegrationHub.Business.Entities.Auditor((Guid)CurrentServiceIssue.AuditorId).Name;
            }

            if (CurrentServiceIssue.PlanId != null)
            {
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            txtAsOfDate.Text = CurrentServiceIssue.AsOfDate.ToString("MM/dd/yyyy");

            ss.Close();
            this.Show();
        }

        public frmServiceIssue(frmMain mf, VSP.Business.Entities.RecordKeeper recordKeeper, FormClosedEventHandler Close = null)
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

            CurrentServiceIssue = new ServiceIssue();
            CurrentServiceIssue.RecordKeeperId = recordKeeper.Id;
            CurrentServiceIssue.AsOfDate = DateTime.Now;

            if (CurrentServiceIssue.RecordKeeperId != null)
            {
                cboRecordKeeper.Text = new DataIntegrationHub.Business.Entities.RecordKeeper((Guid)CurrentServiceIssue.RecordKeeperId).Name;
            }

            if (CurrentServiceIssue.AuditorId != null)
            {
                cboAuditor.Text = new DataIntegrationHub.Business.Entities.Auditor((Guid)CurrentServiceIssue.AuditorId).Name;
            }

            if (CurrentServiceIssue.PlanId != null)
            {
                Plan plan = new Plan((Guid)CurrentServiceIssue.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            txtAsOfDate.Text = CurrentServiceIssue.AsOfDate.ToString("MM/dd/yyyy");

            ss.Close();
            this.Show();
        }

        public frmServiceIssue(frmMain mf, VSP.Business.Entities.Auditor auditor, FormClosedEventHandler Close = null)
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

            CurrentServiceIssue = new ServiceIssue();
            CurrentServiceIssue.AuditorId = auditor.Id;
            CurrentServiceIssue.AsOfDate = DateTime.Now;

            if (CurrentServiceIssue.RecordKeeperId != null)
            {
                cboRecordKeeper.Text = new DataIntegrationHub.Business.Entities.RecordKeeper((Guid)CurrentServiceIssue.RecordKeeperId).Name;
            }

            if (CurrentServiceIssue.AuditorId != null)
            {
                cboAuditor.Text = new DataIntegrationHub.Business.Entities.Auditor((Guid)CurrentServiceIssue.AuditorId).Name;
            }

            if (CurrentServiceIssue.PlanId != null)
            {
                Plan plan = new Plan((Guid)CurrentServiceIssue.PlanId);
                cboPlan.Text = plan.Name + " - " + plan.Description;
            }

            txtAsOfDate.Text = CurrentServiceIssue.AsOfDate.ToString("MM/dd/yyyy");

            ss.Close();
            this.Show();
        }

        public frmServiceIssue(frmMain mf, ServiceIssue serviceIssue, FormClosedEventHandler Close = null)
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

            CurrentServiceIssue = serviceIssue;
            CurrentServiceIssue.AsOfDate = DateTime.Now;

            if (CurrentServiceIssue.RecordKeeperId != null)
            {
                cboRecordKeeper.Text = new DataIntegrationHub.Business.Entities.RecordKeeper((Guid)CurrentServiceIssue.RecordKeeperId).Name;
            }

            if (CurrentServiceIssue.AuditorId != null)
            {
                cboAuditor.Text = new DataIntegrationHub.Business.Entities.Auditor((Guid)CurrentServiceIssue.AuditorId).Name;
            }

            if (CurrentServiceIssue.PlanId != null)
            {
                cboPlan.Text = new Plan((Guid)CurrentServiceIssue.PlanId).Name;
            }

            txtSubject.Text = CurrentServiceIssue.SubjectValue;
            txtAsOfDate.Text = CurrentServiceIssue.AsOfDate.ToString("MM/dd/yyyy");
            txtDescription.Text = CurrentServiceIssue.DescriptionValue;

            ss.Close();
            this.Show();
		}

        public void PreloadCbos()
        {
            cboRecordKeeper.Items.Clear();
            cboAuditor.Items.Clear();
            cboPlan.Items.Clear();

            cboRecordKeeper.Items.Add(new ListItem("", ""));
            cboAuditor.Items.Add(new ListItem("", ""));
            cboPlan.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in DataIntegrationHub.Business.Entities.RecordKeeper.GetAll().Rows)
            {
                Guid recordKeeperId = new Guid(dr["RecordKeeperId"].ToString());
                string name = dr["Name"].ToString();
                cboRecordKeeper.Items.Add(new ListItem(name, recordKeeperId));
            }

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
            CurrentServiceIssue.SubjectValue = txtSubject.Text;
            CurrentServiceIssue.DescriptionValue = txtDescription.Text;
            CurrentServiceIssue.AsOfDate = DateTime.Parse(txtAsOfDate.Text);

            if (cboRecordKeeper.SelectedIndex <= 0)
            {
                CurrentServiceIssue.RecordKeeperId =  null;
            }
            else
            {
                ListItem li = (ListItem)cboRecordKeeper.SelectedItem;
                Guid recordKeeperId = (Guid)li.HiddenObject;
                CurrentServiceIssue.RecordKeeperId = recordKeeperId;
            }

            if (cboAuditor.SelectedIndex <= 0)
            {
                CurrentServiceIssue.AuditorId = null;
            }
            else
            {
                ListItem li = (ListItem)cboAuditor.SelectedItem;
                Guid auditorId = (Guid)li.HiddenObject;
                CurrentServiceIssue.AuditorId = auditorId;
            }

            if (cboPlan.SelectedIndex <= 0)
            {
                CurrentServiceIssue.PlanId = null;
            }
            else
            {
                ListItem li = (ListItem)cboPlan.SelectedItem;
                Plan plan = (Plan)li.HiddenObject;
                CurrentServiceIssue.PlanId = plan.PlanId;
            }

            CurrentServiceIssue.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtSubject.Text;
        }
	}
}
