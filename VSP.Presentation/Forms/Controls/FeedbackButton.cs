using System;
using System.Windows.Forms;
using VSP.Presentation.Forms;

namespace VSP.Presentation.Forms
{
    class FeedbackButton : Button
    {
        public FeedbackButton() : base() { }

        protected override void OnClick(EventArgs e)
        {
            frmFeedback frmFeedback = new frmFeedback();
            base.OnClick(e);
        }
    }
}
