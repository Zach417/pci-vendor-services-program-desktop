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
	public partial class frmSearchBid : Form, IMessageFilter
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
        public SearchBid CurrentSearchBid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmSearchBid(frmMain mf, SearchBid searchBid, FormClosedEventHandler Close = null)
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

            CurrentSearchBid = searchBid;
            txtRecordKeeper.Text = new DataIntegrationHub.Business.Entities.RecordKeeper(searchBid.RecordKeeperId).Name;
            txtFullName.Text = searchBid.FullName;
            txtEmail.Text = searchBid.Email;
            txtConfirmInvestments.Text = searchBid.ConfirmInvestments.ToString();
            txtConfirmServices.Text = searchBid.ConfirmServices.ToString();
            txtRequiredRevenue.Text = searchBid.RequiredRevenue.ToString("#,##.##");
            txtRequiredRevenueExplanation.Text = searchBid.RequiredRevenueExplanation;
            txtAncillaryServices.Text = searchBid.AncillaryServices;
            txtNotes.Text = searchBid.Notes;

            if (searchBid.IsFinalist == null)
            {
                cboIsFinalist.SelectedIndex = 0;
            }
            else if (((SqlBoolean)searchBid.IsFinalist) == SqlBoolean.True)
            {
                cboIsFinalist.SelectedIndex = 1;
            }
            else
            {
                cboIsFinalist.SelectedIndex = 2;
            }

            if (searchBid.IsRecommended == null)
            {
                cboIsRecommended.SelectedIndex = 0;
            }
            else if (((SqlBoolean)searchBid.IsRecommended) == SqlBoolean.True)
            {
                cboIsRecommended.SelectedIndex = 1;
            }
            else
            {
                cboIsRecommended.SelectedIndex = 2;
            }

            cboQuestionViews.SelectedIndex = 0;

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

        private void label3_Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedTab = tabQuestions;
        }

        private void LoadDgvQuestions()
        {
            DataTable dataTable = SearchBidQuestion.GetAssociated(CurrentSearchBid);
            var dataTableEnum = dataTable.AsEnumerable();

            /// Set the datatable based on the SelectedIndex of <see cref="cboQuestionViews"/>.
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

            dataTable.Columns.Add("Question", typeof(string));

            dgvQuestions.DataSource = dataTable;

            // Display/order the columns.
            dgvQuestions.Columns["SearchBidQuestionId"].Visible = false;
            dgvQuestions.Columns["SearchBidId"].Visible = false;
            dgvQuestions.Columns["SearchQuestionId"].Visible = false;
            dgvQuestions.Columns["ModifiedBy"].Visible = false;
            dgvQuestions.Columns["ModifiedOn"].Visible = false;
            dgvQuestions.Columns["CreatedBy"].Visible = false;
            dgvQuestions.Columns["CreatedOn"].Visible = false;
            dgvQuestions.Columns["StateCode"].Visible = false;

            dgvQuestions.Columns["Question"].DisplayIndex = 0;
            dgvQuestions.Columns["AnswerValue"].DisplayIndex = 1;

            int rowIndex = 0;
            foreach (DataGridViewRow dr in dgvQuestions.Rows)
            {
                Guid searchQuestionId = new Guid(dr.Cells["SearchQuestionId"].Value.ToString());
                SearchQuestion searchQuestion = new SearchQuestion(searchQuestionId);
                dgvQuestions.Rows[rowIndex].Cells["Question"].Value = searchQuestion.SubjectValue;
                rowIndex++;
            }
        }

        private void cboQuestionViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvQuestions();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(cboIsFinalist.Text))
            {
                CurrentSearchBid.IsFinalist = null;
            }
            else
            {
                CurrentSearchBid.IsFinalist = SqlBoolean.Parse(cboIsFinalist.Text.ToLower());
            }

            if (String.IsNullOrWhiteSpace(cboIsRecommended.Text))
            {
                CurrentSearchBid.IsRecommended = null;
            }
            else
            {
                CurrentSearchBid.IsRecommended = SqlBoolean.Parse(cboIsRecommended.Text.ToLower());
            }

            CurrentSearchBid.Notes = txtNotes.Text;
            CurrentSearchBid.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
            this.Close();
        }
	}
}
