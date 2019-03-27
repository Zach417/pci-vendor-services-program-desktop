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
    public partial class frmQualityAssurancePopup : Form, IMessageFilter
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private HashSet<Control> controlsToMove = new HashSet<Control>();

        public Feedback CurrentFeedback;
        public ApplicationError CurrentError;
        public User CurrentUser;
        public Label CurrentTabLabel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fdbk"></param>
        /// <param name="Close"></param>
        public frmQualityAssurancePopup(Feedback fdbk, FormClosedEventHandler Close = null)
        {
            frmSplashScreen ss = new frmSplashScreen();
            ss.Show();
            Application.DoEvents();

            InitializeComponent();

            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;

            Application.AddMessageFilter(this);
            controlsToMove.Add(this.pnlSummaryTabHeader);
            controlsToMove.Add(this.panel16);
            controlsToMove.Add(this.label1);
            controlsToMove.Add(this.label23);

            FormClosed += Close;

            CurrentUser = new User(Environment.UserDomainName + "\\" + Environment.UserName);
            CurrentError = null;
            CurrentFeedback = fdbk;
            txtFdbkDescription.Text = CurrentFeedback.Description;
            txtFdbkSeverity.Text = CurrentFeedback.Severity;
            txtFdbkTimestamp.Text = CurrentFeedback.CreatedOn.ToString();
            User submittingUser = new User((Guid)CurrentFeedback.CreatedBy);
            txtFdbkUser.Text = submittingUser.FullName;
            if (CurrentFeedback.StateCode == 1)
            {
                ckbxFdbkResolved.Checked = true;
            }
            else
            {
                ckbxFdbkResolved.Checked = false;
            }

            this.ckbxFdbkResolved.CheckedChanged += new System.EventHandler(this.ckbxFdbkResolved_CheckedChanged);

            ss.Close();

            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabFeedback"];

            this.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="err"></param>
        /// <param name="Close"></param>
        public frmQualityAssurancePopup(ApplicationError err, FormClosedEventHandler Close = null)
        {
            frmSplashScreen ss = new frmSplashScreen();
            ss.Show();
            Application.DoEvents();

            InitializeComponent();

            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;

            Application.AddMessageFilter(this);
            controlsToMove.Add(this.pnlSummaryTabHeader);
            controlsToMove.Add(this.panel16);
            controlsToMove.Add(this.label1);
            controlsToMove.Add(this.label23);

            FormClosed += Close;

            CurrentUser = new User(Environment.UserDomainName + "\\" + Environment.UserName);
            CurrentFeedback = null;
            CurrentError = err;
            txtCrashData.Text = CurrentError.Data;
            txtCrashMessage.Text = CurrentError.Message;
            txtCrashSource.Text = CurrentError.Source;
            txtCrashStackTrace.Text = CurrentError.StackTrace;
            txtCrashTimestamp.Text = CurrentError.CreatedOn.ToString();
            if (CurrentError.StateCode == 1)
            {
                ckbxCrashResolved.Checked = true;
            }
            else
            {
                ckbxCrashResolved.Checked = false;
            }

            this.ckbxCrashResolved.CheckedChanged += new System.EventHandler(this.ckbxCrashResolved_CheckedChanged);

            ss.Close();

            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabCrashes"];

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

        private void ckbxFdbkResolved_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxFdbkResolved.Checked)
            {
                CurrentFeedback.StateCode = 1;
            }
            else
            {
                CurrentFeedback.StateCode = 0;
            }
            CurrentFeedback.SaveRecordToDatabase(CurrentUser.UserId);
            MessageBox.Show("Resolution state updated!");
        }

        private void ckbxCrashResolved_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxCrashResolved.Checked)
            {
                CurrentError.StateCode = 1;
            }
            else
            {
                CurrentError.StateCode = 0;
            }
            CurrentError.SaveRecordToDatabase(CurrentUser.UserId);
            MessageBox.Show("Resolution state updated!");
        }
    }
}
