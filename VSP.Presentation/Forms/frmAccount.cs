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
	public partial class frmAccount : Form, IMessageFilter
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
        public Customer CurrentCustomer;

        public frmAccount(frmMain mf, Customer customer, FormClosedEventHandler Close = null)
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

            CurrentCustomer = customer;
            if (customer.PrimaryContactId != null)
            {
                Contact contact = new Contact((Guid)CurrentCustomer.PrimaryContactId);
                txtPrimaryContactName.Text = contact.FirstName + ' ' + contact.LastName;
                txtPrimaryContactEmail.Text = contact.EmailAddress;
            }

            txtName.Text = CurrentCustomer.Name;
            this.Text = CurrentCustomer.Name;
            label23.Text = CurrentCustomer.Name;
            assetvalue.Text = CurrentCustomer.AssetValue.ToString();
            txtPhone.Text = CurrentCustomer.MainPhone;
            txtFax.Text = CurrentCustomer.Fax;
            txtAddressLine1.Text = CurrentCustomer.Address.Line1;
            txtAddressLine2.Text = CurrentCustomer.Address.Line2;
            txtAddressCity.Text = CurrentCustomer.Address.City;
            txtAddressState.Text = CurrentCustomer.Address.State;
            txtAddressZip.Text = CurrentCustomer.Address.ZipCode;

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

        void pictureBox1Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedIndex = 0;
        }

        void pictureBox1MouseEnter(object sender, EventArgs e)
        {
            label25.Text = "Account Details";
        }

        void pictureBox1MouseLeave(object sender, EventArgs e)
        {
            label25.Text = "Ready";
        }

        void pictureBox2Click(object sender, EventArgs e)
        {
            tabControlClientDetail.SelectedIndex = 1;
        }

        private void label23_TextChanged(object sender, EventArgs e)
        {
            string CompanyName = label23.Text;
        }

        private void label46_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlClientDetail.SelectedIndex = 0;

        }

        private void label47_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlClientDetail.SelectedIndex = 1;
        }

        private void label49_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            tabControlClientDetail.SelectedIndex = 2;
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
            label.ForeColor = System.Drawing.SystemColors.HotTrack;
            label.BackColor = System.Drawing.Color.Gainsboro;
        }

        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            label.ForeColor = System.Drawing.SystemColors.ControlText;
            label.BackColor = System.Drawing.Color.Transparent;
        }

        private void label38_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
