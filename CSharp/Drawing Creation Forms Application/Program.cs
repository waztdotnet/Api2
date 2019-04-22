using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Drawing_Creation_Forms_Application
{
    static class Program
    {
        private static DrawingCreationForm mWinForm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Boolean retry = false;

            do
            {
                try
                {
                    retry = false;

                    Inventor.Application oApp = System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application") as Inventor.Application;
                    mWinForm = new DrawingCreationForm(oApp);
                    mWinForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    string Message = "Inventor is not running...";
                    string Caption = "Appication Error";
                    MessageBoxButtons Buttons = MessageBoxButtons.RetryCancel;

                    DialogResult Result = MessageBox.Show(Message, Caption, Buttons, MessageBoxIcon.Exclamation);

                    retry = false;

                    switch (Result)
                    {
                        case DialogResult.Retry:
                            retry = true;
                            break;

                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            while (retry == true);

           
        }
    }
}
