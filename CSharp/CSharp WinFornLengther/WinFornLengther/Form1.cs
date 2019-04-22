using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFornLengther
{
    public partial class Form1 : Form
    {
        private Inventor.Application InvApp = null;
        private bool QuitInventor = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ////////private Inventor.Application InvApp = null;
            ////////private bool QuitInventor = false;

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
        private void btnStart_Click(object sender, EventArgs e)
        {
            Inventor.Document Document;
            Inventor.PartDocument PartDocument;
            Inventor.AssemblyDocument AssemblyDocument;
            Inventor.PresentationDocument PresentationDocument;
            Inventor.DrawingDocument DrawingDocument;

            CreateDocument(out Document , Inventor.DocumentTypeEnum.kPartDocumentObject, "\\Standard.ipt");
            
            if (Document.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject  )
            { 
                PartDocument = (Inventor.PartDocument)Document;
            }
            else if (Document .DocumentType ==  Inventor.DocumentTypeEnum.kAssemblyDocumentObject )
            {
                AssemblyDocument = (Inventor.AssemblyDocument)Document;

            }
            else if (Document.DocumentType == Inventor.DocumentTypeEnum.kPresentationDocumentObject )
            {
                PresentationDocument = (Inventor.PresentationDocument)Document;

            }
            else if (Document.DocumentType == Inventor.DocumentTypeEnum.kDrawingDocumentObject )
            {
                DrawingDocument = (Inventor.DrawingDocument)Document;

            }
        }

        private void CreateDocument(out Inventor.Document Document, Inventor.DocumentTypeEnum DocumentType, string TemplateFileName)
        {
            
            Inventor.UnitsOfMeasure UnitsOfMeasure;
            if(TemplateFileName != "")
            {
                Inventor.FileOptions FileOptions = InvApp.FileOptions  ;
                TemplateFileName = FileOptions.TemplatesPath + "\\" + TemplateFileName;
                Document = InvApp.Documents.Add(DocumentType, TemplateFileName, true);
            }
            else
            {
                Document = InvApp.Documents.Add(DocumentType, "", true);
            }

            UnitsOfMeasure = Document.UnitsOfMeasure;
            UnitsOfMeasure.LengthUnits = Inventor.UnitsTypeEnum.kMillimeterLengthUnits;
            UnitsOfMeasure.AngleUnits = Inventor.UnitsTypeEnum.kDegreeAngleUnits;
            UnitsOfMeasure.MassUnits = Inventor.UnitsTypeEnum.kKilogramMassUnits;
            UnitsOfMeasure.TimeUnits = Inventor.UnitsTypeEnum.kSecondTimeUnits;
            UnitsOfMeasure.LengthDisplayPrecision = 3;
            UnitsOfMeasure.AngleDisplayPrecision = 3;
        }



        private void TriagleLate(double Width, double Height, double ComponantWidth, double Centers)
        {
            double Angle = Math.Atan ( Height / Width); //inverse tangent

            double newHeight = (Math.Tan(Angle)) * Width; //Normal tangent

        }
    }
}
