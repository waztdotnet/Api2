using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorTestWinApp
{
	public partial class TestFrm : Form
	{
		private Inventor.Application InvApp = null;
		private bool QuitInventor = false;
		public TestFrm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				InvApp = (Inventor.Application) System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
			}
			catch
			{
				MessageBox.Show("Starting a New Inventor Session");
			}
			if(InvApp == null)
			{
				Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
				InvApp = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
				InvApp.Visible = true;
				QuitInventor = true;
			}
		}

		private void TestFrm_FormClosed(object sender, FormClosedEventArgs e)
		{
			while(System.Runtime.InteropServices.Marshal.ReleaseComObject(InvApp) > 0)
			{
			}
			InvApp = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void btnTest_Click(object sender, EventArgs e)
		{

			Inventor.PartDocument PDoc = (Inventor.PartDocument) InvApp.ActiveDocument;
			Inventor.UnitsTypeEnum units = Inventor.UnitsTypeEnum.kMillimeterLengthUnits;
			CFlange flange = new CFlange(ref InvApp,ref PDoc,ref units,"XY","300","150","30","200","18","8");
			flange.Extrude_FlangeBody(); 
			
		}
	}
}
