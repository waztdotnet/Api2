using ClientGraphicsLib;
using System;
using System.Windows.Forms;

namespace UserGraphicsApp
{
    public partial class UserGUI : Form
    {
        private Inventor.Application _InvApplication;
        private CClientGraphic cClientGraphics;

        public UserGUI()
        {
            InitializeComponent();
            _InvApplication = null;
            try
            {
                _InvApplication = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch
            {
                if (System.Windows.Forms.MessageBox.Show("Trying to Open a new instance of Autodesk Inventor", "Inventor Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return; //To Caller
                }
            }
            if (_InvApplication == null)
            {
                try
                {
                    Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                    _InvApplication = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Can Not Open Autodesk Inventor " + Environment.NewLine + "See Your Administrator", "Instalation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Question);
                    return;
                }
            }
            cClientGraphics = new CClientGraphic(ref _InvApplication);
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (_InvApplication != null)
            {
                //cClientGraphics = new CClientGraphic(ref _InvApplication);
                // CClientGraphic.Draw(ref _InvApplication, "Run");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //cClientGraphics = new CClientGraphic(ref _InvApplication);
            // CClientGraphic.Draw(ref _InvApplication, "");
        }
    }
}