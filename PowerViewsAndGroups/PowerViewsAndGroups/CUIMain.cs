using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace PowerViewsAndGroups
{
    class CUIMain : IComDialog
    {
        public CUIMain()
        {

        }


        public void RunUI(Inventor.Application m_inventorApplication)
        {
            UIMain uIMain = new UIMain();
            uIMain.Activate();
            uIMain.Show();
            uIMain.TopMost = true;
        }

        public void Activate(ApplicationAddInSite AddInSiteObject, bool FirstTime)
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
            throw new NotImplementedException();
        }

        public void ExecuteCommand(int CommandID)
        {
            throw new NotImplementedException();
        }

        public dynamic Automation { get; }
    }
}
