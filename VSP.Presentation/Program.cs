using VSP.Presentation.Forms;
using VSP.Business.Components;
using System.Threading;
using System;
using System.Windows.Forms;

namespace VSP
{
    internal sealed class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.ThreadException +=
                new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                Exception exception = e.Exception;
                ErrorHandler.ProcessException(exception);
            }
            catch
            {
                return;
            }
            finally
            {
                ProcessFatalError();

            }
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception exception = (Exception)e.ExceptionObject;
                ErrorHandler.ProcessException(exception);
            }
            catch
            {
                return;
            }
            finally
            {
                ProcessFatalError();
            }
        }

        public static void ProcessFatalError()
        {
            // Announce error
            MessageBox.Show("An error occurred and the application must close.\n\nThe error has been logged for review.", "Application Error",
             MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Terminate Program
            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                System.Environment.Exit(1);
            }
        }
    }
}
