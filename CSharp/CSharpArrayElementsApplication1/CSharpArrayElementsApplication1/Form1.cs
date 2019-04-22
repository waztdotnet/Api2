using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpArrayElementsApplication1
{
    public partial class Form1 : Form
    {
        private Inventor.Application InvApp = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

            }
            Inventor.Document Document = InvApp.ActiveDocument;
            if (Document.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)Document;
                foreach (Inventor.OccurrencePattern OccurrencePattern in AssemblyDocument.ComponentDefinition.OccurrencePatterns)
                {
                    if (OccurrencePattern.Type == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                    {
                        Inventor.RectangularOccurrencePattern RectangularOccurrencePattern;
                        RectangularOccurrencePattern = (Inventor.RectangularOccurrencePattern)OccurrencePattern;
                        foreach (Inventor.OccurrencePatternElement OccurrencePatternElement in RectangularOccurrencePattern.OccurrencePatternElements)
                        {
                            cmbElements.Items.Add(OccurrencePatternElement.Name);
                        }
                    }

                }

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Inventor.AssemblyDocument AssemblyDocument;
            Inventor.Document mDocument = InvApp.ActiveDocument;
           // Inventor.PartDocument PartDocument;
            Inventor.RectangularOccurrencePattern RectangularOccurrencePattern;
            Collection<Inventor.ComponentOccurrence> name = new Collection<Inventor.ComponentOccurrence>();

            if (mDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                AssemblyDocument = (Inventor.AssemblyDocument)mDocument;

                foreach (Inventor.OccurrencePattern OccurrencePattern in AssemblyDocument.ComponentDefinition.OccurrencePatterns)
                {
                    if (OccurrencePattern.Type == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                    {
                        RectangularOccurrencePattern = (Inventor.RectangularOccurrencePattern)OccurrencePattern;
                       foreach(Inventor.OccurrencePatternElement OccurrencePatternElement in RectangularOccurrencePattern.OccurrencePatternElements)
                        {
                            if (OccurrencePatternElement.Index > 0 || !OccurrencePatternElement.Suppressed  )
                            { 
                                //OccurrencePatternElement.Independent = true;
                                Inventor.ComponentOccurrences occs;
                                if (OccurrencePatternElement.Name == "Element:7")
                                {
                                            //OccurrencePatternElement.Independent = true;
                                            foreach (Inventor.ComponentOccurrence occ in OccurrencePatternElement.Occurrences)
                                            {
                                                        if (occ.Name != "P00001 - 02:21" || occ.Name != "P00001 - 01:11") { 
                                                                lstNames.Items.Add(occ.Name+": :"+OccurrencePatternElement.Name);
                                                        }
                                                        else {
                                        
                                                            name.Add(occ);
                                                        }

                                            }
                       
                                }
                                // MessageBox.Show(OccurrencePatternElement.Name);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Not an Assembly");
            }
        }
    }
}
