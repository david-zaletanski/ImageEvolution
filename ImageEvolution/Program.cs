using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            IEvo.MainForm = new frmMain();
            Application.Run(IEvo.MainForm);
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            IEvo.OnExit();
        }
    }
}
