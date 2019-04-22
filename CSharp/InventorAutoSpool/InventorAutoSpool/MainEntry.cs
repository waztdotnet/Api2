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
				MessageBox.Show("Starting a New Inventor Session");
            }
            if (InvApp == null)
            {
                Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                InvApp = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                InvApp.Visible = true;
                QuitInventor = true;
            }

					txtSaveFolder.Text = folderBrowserDialog1.SelectedPath; 
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

        }



        private void btnAssembly_Click(object sender, EventArgs e)
        {
            
            //CMFlange Flange = new CMFlange("200", "88.9", "20","160","22");
            //Flange.SetFileSavePath("F:\\Test", "Flange-001");
            //Flange.ModelMake(ref InvApp);

            CModelPipe pipe = new CModelPipe("88.9", "6");
            pipe.SetFileSavePath("F:\\Test", "Pipe-001");
            pipe.ModelMake(ref InvApp);
            IList<string> names = new List<string >(); 
            ICollection<string> PartFileNames = names;
            

            CModelAssembly SpoolAssembly = new CModelAssembly();
            SpoolAssembly.Build_AssemblyDocument(ref InvApp ,"F:\\Test", "SpoolAssembly-001", PartFileNames);


        }

        private void btnSaveFolder_Click(object sender, EventArgs e)
        {
			folderBrowserDialog1.ShowDialog();
			txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void toolbtnProjectProperties_Click(object sender, EventArgs e)
        {

        }


     }
}
