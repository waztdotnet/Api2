using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventor;

namespace PowerViewsAndGroups
{
    public partial class UIMain : Form, IComDialog
    {
        private Inventor.Application main_inventorApplication;
        public UIMain()
        {
            InitializeComponent();
        }



        public void RunUI(Inventor.Application m_inventorApplication)
        {
            main_inventorApplication = m_inventorApplication;
            Activate();
            Show();
            TopMost = true;
           // CEventsHelper eventsHelper = new CEventsHelper(m_inventorApplication);
        }


    }
}
