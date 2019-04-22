using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorAutoSpool
{
    public partial class FrmAutoSpoolMain : Form
    {
        private Inventor.Application InvApp = null;
        private bool QuitInventor = false;
        public FrmAutoSpoolMain()
        {
            InitializeComponent();
            try
            {
                InvApp = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Starting a New Inventor Session");
            }
            if (InvApp == null)
            {
                Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                InvApp = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                InvApp.Visible = true;
                QuitInventor = true;
            }
        }

        private void FrmAutoSpoolMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (QuitInventor == true)
            {
                var result = MessageBox.Show("Do You Want to Quit Inventor (Yes) ", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    InvApp.Quit();
                    InvApp = null;
                }

            }
        }

        private void btnWork_Click(object sender, EventArgs e)
        {
        

            //CModelFlange Flange = new CModelFlange("200","88.9", "20");
            //Flange.SetFileSavePath("H:\\InvProjects", "Flange-001"); 
            //Flange .ModelMake(ref InvApp);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CModelPipe pipe = new CModelPipe("88.9", "6");
            pipe.SetFileSavePath("H:\\InvProjects", "Pipe-001");
            pipe.ModelMake(ref InvApp);
        }

        private void btnAssembly_Click(object sender, EventArgs e)
        {
            CModelFlange Flange = new CModelFlange("200", "88.9", "20");
            Flange.SetFileSavePath("H:\\InvProjects", "Flange-001");
            Flange.ModelMake(ref InvApp);

            CModelPipe pipe = new CModelPipe("88.9", "6");
            pipe.SetFileSavePath("H:\\InvProjects", "Pipe-001");
            pipe.ModelMake(ref InvApp);

            CModelAssembly SpoolAssembly = new CModelAssembly();
            SpoolAssembly.SetFileSavePath("H:\\InvProjects", "SpoolAssembly-001");

        }
    }
}
