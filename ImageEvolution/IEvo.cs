using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.dzale.ImageEvolution
{
    class IEvo
    {
        public static frmMain MainForm;

        public static void Output(string msg)
        {
            if (MainForm != null)
                MainForm.AddOutput(msg);
        }

        public static void OnExit()
        {
            MainForm.OnExit();
        }

        public static void ReportException(Exception ex, string Source)
        {
            //System.Windows.Forms.MessageBox.Show("Exception occured in " + Source + "\n" + ex.Message);
            Output("Exception occured in " + Source + "\n" + ex.Message);
        }
    }
}
