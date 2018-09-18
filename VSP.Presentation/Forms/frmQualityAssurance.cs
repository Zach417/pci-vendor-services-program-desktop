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
	public partial class frmQualityAssurance : Form, IMessageFilter
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="Close"></param>
        public frmQualityAssurance(frmMain mf, FormClosedEventHandler Close = null)
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

            cboCrashesViews.SelectedIndex = 0;
            cboFeedbackViews.SelectedIndex = 0;
            LoadElements();

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

        public void LoadElements()
        {
            LoadDgvCrashes();
            LoadDgvFeedback();
        }

        private void LoadDgvCrashes()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvCrashes.CurrentCell != null)
            {
                currentCellRow = dgvCrashes.CurrentCell.RowIndex;
                currentCellCol = dgvCrashes.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = ApplicationError.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 0);
            txtCrashCount.Text = dataTableEnum.Count().ToString();

            if (cboCrashesViews.SelectedIndex == 1)
            {
                dataTableEnum = dataTable.AsEnumerable();
                dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 1);
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvCrashes.DataSource = dataTable;

            // Display/order the columns.
            dgvCrashes.Columns["CreatedBy"].Visible = false;
            dgvCrashes.Columns["ModifiedBy"].Visible = false;
            dgvCrashes.Columns["ModifiedOn"].Visible = false;
            dgvCrashes.Columns["StateCode"].Visible = false;
            dgvCrashes.Columns["Data"].Visible = false;
            dgvCrashes.Columns["StackTrace"].Visible = false;
            dgvCrashes.Columns["TimeStamp"].Visible = false;
            dgvCrashes.Columns["ApplicationErrorId"].Visible = false;

            dataTable.Columns.Add("State");
            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr["StateCode"].ToString().Equals("0"))
                {
                    dr["State"] = "Unresolved";
                }
                else
                {
                    dr["State"] = "Resolved";
                }
            }

            dgvCrashes.Columns["Message"].DisplayIndex = 0;
            dgvCrashes.Columns["Source"].DisplayIndex = 1;
            dgvCrashes.Columns["CreatedOn"].DisplayIndex = 2;
            dgvCrashes.Columns["State"].DisplayIndex = 3;

            if (dgvCrashes.RowCount > 0 && dgvCrashes.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvCrashes.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvCrashes.CurrentCell = selectedCell;
                }
                else
                {
                    dgvCrashes.CurrentCell = dgvCrashes.FirstDisplayedCell;
                }
            }
        }

        private void LoadDgvFeedback()
        {
            int currentCellRow = 0;
            int currentCellCol = 0;
            if (dgvFeedback.CurrentCell != null)
            {
                currentCellRow = dgvFeedback.CurrentCell.RowIndex;
                currentCellCol = dgvFeedback.CurrentCell.ColumnIndex;
            }

            DataTable dataTable = Feedback.GetAll();
            var dataTableEnum = dataTable.AsEnumerable();

            dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 0);
            txtFeedbackCount.Text = dataTableEnum.Count().ToString();

            if (cboFeedbackViews.SelectedIndex == 1)
            {
                dataTableEnum = dataTable.AsEnumerable();
                dataTableEnum = dataTableEnum.Where(x => x.Field<int>("StateCode") == 1);
            }

            if (dataTableEnum.Any())
            {
                dataTable = dataTableEnum.CopyToDataTable();
            }
            else
            {
                dataTable.Rows.Clear();
            }

            dgvFeedback.DataSource = dataTable;

            // Display/order the columns.
            dgvFeedback.Columns["CreatedBy"].Visible = false;
            dgvFeedback.Columns["ModifiedBy"].Visible = false;
            dgvFeedback.Columns["ModifiedOn"].Visible = false;
            dgvFeedback.Columns["StateCode"].Visible = false;

            dataTable.Columns.Add("State");
            dataTable.Columns.Add("User");
            int i = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                User user = new User((Guid)dr["CreatedBy"]);
                dr["User"] = user.FullName;

                if (dr["StateCode"].ToString().Equals("0"))
                {
                    dr["State"] = "Unresolved";
                }
                else
                {
                    dr["State"] = "Resolved";
                }
            }

            dgvFeedback.Columns["User"].DisplayIndex = 0;
            dgvFeedback.Columns["Severity"].DisplayIndex = 1;
            dgvFeedback.Columns["Description"].DisplayIndex = 2;
            dgvFeedback.Columns["FeedbackId"].Visible = false;

            if (dgvFeedback.RowCount > 0 && dgvFeedback.ColumnCount > 0)
            {
                DataGridViewCell selectedCell = dgvFeedback.Rows[currentCellRow].Cells[currentCellCol];
                if (selectedCell != null && selectedCell.Visible)
                {
                    dgvFeedback.CurrentCell = selectedCell;
                }
                else
                {
                    dgvFeedback.CurrentCell = dgvFeedback.FirstDisplayedCell;
                }
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
            tabQualityAssurance.SelectedIndex = 0;
            tabQASummary.Focus();

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

        private void label5_Click(object sender, EventArgs e)
        {
            tabQualityAssurance.SelectedTab = tabQualityAssurance.TabPages["tabCrashes"];
            dgvCrashes.Focus();
        }

        private void cboCrashViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvCrashes();
        }

        private void dgvCrashes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvCrashes.CurrentRow.Index;
            Guid crashId = new Guid(dgvCrashes.Rows[index].Cells["ApplicationErrorId"].Value.ToString());
            ApplicationError crash = new ApplicationError(crashId);
            frmQualityAssurancePopup frmQAPopup = new frmQualityAssurancePopup(crash);
            frmQAPopup.FormClosed += frmQualityAssurancePopup_FormClosed;
        }

        private void frmQualityAssurancePopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadElements();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            tabQualityAssurance.SelectedTab = tabQualityAssurance.TabPages["tabFeedback"];
            dgvFeedback.Focus();
        }

        private void cboFeedbackViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvFeedback();
        }

        private void dgvFeedback_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvFeedback.CurrentRow.Index;
            Guid feedbackId = new Guid(dgvFeedback.Rows[index].Cells["FeedbackId"].Value.ToString());
            Feedback feedback = new Feedback(feedbackId);
            frmQualityAssurancePopup frmQAPopup = new frmQualityAssurancePopup(feedback);
            frmQAPopup.FormClosed += frmQualityAssurancePopup_FormClosed;
        }
    }
}
