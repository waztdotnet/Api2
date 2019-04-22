using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvAddIn
{
    public partial class SetReps : Form
    {
        private Inventor.PartDocument partDocument;
        private InventorViewsServer.IAutomationInterface inter;

        public SetReps()
        {
            InitializeComponent();
        }

        public void SetSettings(ref Inventor.PartDocument partdocument)
        {
            partDocument = partdocument;
        }

        private static void UnlockViews(Inventor.PartDocument partDocument)
        {
            Inventor.PartComponentDefinition partComponentDefinition = partDocument.ComponentDefinition;

            if (partComponentDefinition.RepresentationsManager.DesignViewRepresentations.Count >= 1)
            {
                foreach (Inventor.DesignViewRepresentation designViewRepresentation in partComponentDefinition.RepresentationsManager.DesignViewRepresentations)
                {
                    try
                    {
                        designViewRepresentation.Locked = false;
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private void SetReps_Load(object sender, EventArgs e)
        {
            
        }

        private void Btn_Run_Click(object sender, EventArgs e)
        {
            UnlockViews(partDocument);

        }
    }
}
