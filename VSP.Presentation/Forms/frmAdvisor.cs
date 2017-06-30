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
	public partial class frmAdvisor : Form, IMessageFilter
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
        public VSP.Business.Entities.Advisor CurrentPlanAdvisor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="accountId"></param>
        /// <param name="Close"></param>
        public frmAdvisor(frmMain mf, VSP.Business.Entities.Advisor planAdvisor, FormClosedEventHandler Close = null)
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

            DataIntegrationHub.Business.Entities.PlanAdvisor dihPA = new DataIntegrationHub.Business.Entities.PlanAdvisor(planAdvisor.Id);

            CurrentPlanAdvisor = planAdvisor;
            txtName.Text = dihPA.Name;

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
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabSummary"];

        }

        private void lblMenuServices_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlClientDetail.SelectedTab = tabControlClientDetail.TabPages["tabServices"];
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
            CurrentPlanAdvisor.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);

            // loop through dgvservices, and update productservice records for record keeper product
            if (dgvServices.Rows.Count > 0)
            {
                DataTable planAdvisorServices = AdvisorService.GetAssociated(CurrentPlanAdvisor);

                foreach (DataGridViewRow dr in dgvServices.Rows)
                {
                    Guid serviceId = new Guid(dr.Cells["ServiceId"].Value.ToString());

                    bool serviceOffered = false;
                    if (dr.Cells["ServiceOffered"].Value.ToString() != "")
                    {
                        serviceOffered = bool.Parse(dr.Cells["ServiceOffered"].Value.ToString());
                    }

                    var ps = planAdvisorServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        Guid productServiceId = new Guid(ps.CopyToDataTable().Rows[0]["PlanAdvisorServiceId"].ToString());
                        ProductService productService = new ProductService(productServiceId);
                        productService.ServiceOffered = serviceOffered;
                        productService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
                    }
                    else // rk product does not have service record, so create on
                    {
                        AdvisorService planAdvisorService = new AdvisorService();
                        planAdvisorService.ServiceId = serviceId;
                        planAdvisorService.PlanAdvisorId = CurrentPlanAdvisor.Id;
                        planAdvisorService.ServiceOffered = serviceOffered;
                        planAdvisorService.SaveRecordToDatabase(frmMain_Parent.CurrentUser.UserId);
                    }
                }
            }

            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            label23.Text = txtName.Text;
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

            dataTable = dataTable.AsEnumerable().Where(x => x["Type"].ToString() == "Advisor").CopyToDataTable();

            dataTable.Columns.Add("ServiceOffered", typeof(bool));

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
            dgvServices.Columns["ServiceOffered"].DisplayIndex = 2;
            dgvServices.Columns["ServiceOffered"].ReadOnly = false;


            // set service offered values
            if (refresh == true)
            {
                DataTable planAdvisorServices = AdvisorService.GetAssociated(CurrentPlanAdvisor);
                int rowIndex = 0;

                foreach (DataGridViewRow drServices in dgvServices.Rows)
                {
                    Guid serviceId = new Guid(drServices.Cells["ServiceId"].Value.ToString());
                    var ps = planAdvisorServices.AsEnumerable().Where(x => x.Field<Guid>("ServiceId") == serviceId);
                    if (ps.Any()) // rk product already has service record, so update it
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvServices.Rows[rowIndex].Cells["ServiceOffered"];
                        var serviceOffered = SqlBoolean.Parse(ps.CopyToDataTable().Rows[0]["ServiceOffered"].ToString()).IsTrue;
                        dgvServices.Rows[rowIndex].Cells["ServiceOffered"].Value = serviceOffered.ToString();
                    }

                    rowIndex++;
                }
            }
        }

        private void cboServicesView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDgvServices();
        }
	}
}
