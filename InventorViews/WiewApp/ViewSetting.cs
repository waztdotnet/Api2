using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ViewApp
{
    public partial class ViewsForm : Form
    {
        private Inventor.Application _InvApplication;
      // private InventorViewsServer.IAutomationInterface _InvAddInInterface;


        public ViewsForm()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            try
            {
                _InvApplication = System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application") as Inventor.Application;
            }

            catch
            {
                Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                _InvApplication = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
            }

            _InvApplication.Visible = true;

            string addInCLSID = "{55488e4a-45de-4630-a00c-b31e0eb17ba8}";

            Inventor.ApplicationAddIn addIn = _InvApplication.ApplicationAddIns.get_ItemById(addInCLSID.ToUpper());

            //Make sure addin is activated
            if (!addIn.Activated)
            {
                addIn.Activate();
            }
         // _InvAddInInterface = addIn.Automation as InventorViewsServer.IAutomationInterface;
        }
    }
}
