using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsDrawApp
{
    public partial class DrawParts : Form
    {
        private Inventor.Application mInvApplication = null;
        private string UserDrawingTemplatePath = "";
        private string UserViewOrientationAngel;
        private string UserSaveFolderName;
        private bool bInventorVisable = true;
        FolderBrowserDialog FolderBrowserDiag ;
        CDraw Draw;
        public DrawParts()
        {
            InitializeComponent();
        }

        private void DrawParts_Load(object sender, EventArgs e)
        {
            FolderBrowserDiag = new FolderBrowserDialog();

            try
            {
                mInvApplication = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch
            {
                if (System.Windows.Forms.MessageBox.Show("Try to Open Autodesk Inventor", "Inventor Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return; //To Caller
                }
            }
            if (mInvApplication == null)
            {
                try
                {
                    Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                    mInvApplication = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                    mInvApplication.Visible = bInventorVisable;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Can Not Open Autodesk Inventor " + Environment.NewLine + "See Your Administrator", "Instalation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Question);
                    return;
                }

            }

            UserDrawingTemplatePath = mInvApplication.DesignProjectManager.ActiveDesignProject.TemplatesPath;
            string Search = UserDrawingTemplatePath;
            string[] filePaths = Directory.GetFiles(Search);
            foreach (string path in filePaths )
            {
                cmbDrgTemplates.Items.Add(path);
               
            }
            cmbDrgTemplates.SelectedIndex = 3;
            cmbScales.SelectedIndex = 3;
            cmbSheetSize.SelectedIndex = 3;
            cmbView.SelectedIndex = 0;
            UserDrawingTemplatePath = (string)cmbDrgTemplates.Items[cmbDrgTemplates.SelectedIndex];
            Draw = new CDraw(mInvApplication);
        }

        private void BtnMultiPart_Click(object sender, EventArgs e)
        {
            Draw.SetDrawingTemplateFilePath(UserDrawingTemplatePath);
            Draw.SetViewTypeOrientation(UserViewOrientationAngel);
           
            Draw.SetViewTypeOrientation(UserViewOrientationAngel);
            Draw.DrawMultiPart();

        }
        private void BtnSinglePart_Click(object sender, EventArgs e)
        {
            Draw.SetDrawingTemplateFilePath(UserDrawingTemplatePath);
            Draw.SetViewTypeOrientation(UserViewOrientationAngel);
            Draw.DrawSingelPart();

           

        }
        private void RdDrawBy_CheckedChanged(object sender, EventArgs e)
        {
            if (RdDrawBy.Checked == false)
            { btnMultiPart.Enabled = true; }
            else { btnMultiPart.Enabled = false; }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            CDXFExport ExportDXF = new CDXFExport(ref ListComponents,txtPartID.Text, "", ref mInvApplication);
            
            

            //CExportDXF ExportDXF = new CExportDXF(txtPartID.Text, "",ref mInvApplication);
            //ExportDXF.IsQtyAdded(true);
            //ExportDXF.GetFileRefs();
        }

        private void BtnSetMultiPartView_Click(object sender, EventArgs e)
        {
            Inventor.File pFile = mInvApplication.ActiveDocument.File;
            CModelViews ModelView = new CModelViews(ref mInvApplication);
            ModelView.ProcessFileRefs(pFile);
        }
        private void BtnSetSingelPartView_Click(object sender, EventArgs e)
        {
            Inventor.Document mDocument = mInvApplication.ActiveDocument;
            CModelViews ModelView = new CModelViews(ref mInvApplication);
            ModelView.SetSingelView(ref mDocument);
        }
        //string FileName = "";
        //string Issue = "";


        //OpenFileDialog mOpenFileDialog1 = new OpenFileDialog();

        //mOpenFileDialog1.InitialDirectory = "D:\\Working Folder\\Designs\\Projects\\E376-Billian";
        //    mOpenFileDialog1.Filter = "Part Files (*.ipt)|*.ipt|Assembly Files (*.iam)|*.iam*|All files (*.*)|*.*";
        //    mOpenFileDialog1.FilterIndex = 2;
        //    mOpenFileDialog1.RestoreDirectory = true;
        //    if (mOpenFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            FileName = mOpenFileDialog1.FileName;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
        //        }
        //    }

        //private void ProcessFileReferancesInAssembly(Inventor.File File)
        //{
        //    foreach (Inventor.FileDescriptor DescriptedFile in File.ReferencedFileDescriptors)
        //    {
        //        if (!DescriptedFile.ReferenceMissing)
        //        {

        //            if (DescriptedFile.ReferencedFileType != Inventor.FileTypeEnum.kForeignFileType)
        //            {

        //                if (DescriptedFile.ReferencedFileType == Inventor.FileTypeEnum.kPartFileType) //part or sub;
        //                {


        //                    string TartgetPartNumber;
        //                    Inventor.PartDocument PartDocument = (Inventor.PartDocument)mInvApplication.Documents.Open(DescriptedFile.FullFileName, false);
        //                    TartgetPartNumber = PartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value;

        //                    if (TartgetPartNumber.StartsWith(txtPartID.Text) && PartDocument.ComponentDefinition.Features.ExtrudeFeatures.Count >= 1)
        //                    {


        //                        DrawPartDoc(PartDocument);

        //                           // PartDocument.SubType = "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}";


        //                            PartDocument.Close(false );

        //                    }
        //                }
        //                else if (DescriptedFile.ReferencedFileType == Inventor.FileTypeEnum.kAssemblyFileType) { }

        //               // ProcessFileReferancesInAssembly(DescriptedFile.ReferencedFile);
        //            }
        //        }
        //    }
        //}
        //private void ViewAngel(ref Inventor.ViewOrientationTypeEnum ViewAngel)
        //{
        //    ViewAngel = Inventor.ViewOrientationTypeEnum.kTopViewOrientation;

        //    if (cmbView.Items[cmbFilePaths.SelectedIndex].ToString() == "Bottom")
        //    {
        //        ViewAngel = Inventor.ViewOrientationTypeEnum.kBottomViewOrientation;
        //    }
        //    else if (cmbView.Items[cmbFilePaths.SelectedIndex].ToString() == "Left")
        //    {
        //        ViewAngel = Inventor.ViewOrientationTypeEnum.kLeftViewOrientation;
        //    }
        //    else if (cmbView.Items[cmbFilePaths.SelectedIndex].ToString() == "Right")
        //    {
        //        ViewAngel = Inventor.ViewOrientationTypeEnum.kRightViewOrientation;
        //    }
        //    else if (cmbView.Items[cmbFilePaths.SelectedIndex].ToString() == "Front")
        //    {
        //        ViewAngel = Inventor.ViewOrientationTypeEnum.kFrontViewOrientation;
        //    }
        //    else if (cmbView.Items[cmbFilePaths.SelectedIndex].ToString() == "Back")
        //    {
        //        ViewAngel = Inventor.ViewOrientationTypeEnum.kBackViewOrientation;
        //    }

        //}
        //private double SetFitPartToDrawing(ref Inventor.DrawingDocument DrawingDocument)
        //{

        //    Inventor.Sheet CurrentSheet = DrawingDocument.Sheets[1]; ;
        //    Inventor.DrawingView DrawingView = CurrentSheet.DrawingViews[1];
        //    double DrawingTitelBlockHeight = 20;
        //    double ViewPositionX = 0;
        //    double ViewPositionY = 0;
        //    double SheetWidth = 0;
        //    double SheetHeight = 0;
        //    double ViewSizeWidthGap = 30;
        //    double ViewSizeHeightGap = 30;


        //    SheetWidth = CurrentSheet.Width;
        //    SheetHeight = CurrentSheet.Height;

        //    ViewPositionX = DrawingView.Center.X;
        //    ViewPositionY = DrawingView.Center.Y;
        //    double NewViewCenterX = (SheetHeight- DrawingTitelBlockHeight) /2;
        //    double NewViewCenterY = (SheetWidth) / 2;
        //    DrawingView.Center.X = NewViewCenterX;
        //    DrawingView.Center.Y = NewViewCenterY;


        //    //foreach (Inventor.DrawingView DrawingView in CurrentSheet.DrawingViews )
        //    //{
        //    //    ViewSizeWidth = DrawingView.Width;
        //    //}

        //    //Current width
        //    //Dim cw As Double
        //    //cw = dv.Width
        //    //New width we want
        //    //Set in 'cm' (internal length unit)
        //    //Dim nw As Double
        //    //nw = 10
        //    //New scale
        //    //Dim ns As Double
        //    //ns = nw / cw * dv.Scale
        //    //dv.[Scale] = ns
        //    return 0;
        //}
        //private void GetStyles()
        //{
        //    string SelectedTemplate = "";
        //    SelectedTemplate = cmbFilePaths.Items[cmbFilePaths.SelectedIndex].ToString();
        //}
        //private void DrawPartDoc(Inventor.PartDocument PartDocument)
        //{
        //    Inventor.DrawingDocument DrawingDocument;
        //    Inventor.Sheet Sheet;
        //    Inventor.TransientGeometry oTG = mInvApplication.TransientGeometry;
        //    CreateDrawingDocument(out DrawingDocument, out Sheet);
        //    //0.1 = 1:10 or 0.2 = 1:5 1:20=0.02   X-> Y ^
        //    Inventor.DrawingView oBaseView = Sheet.DrawingViews.AddBaseView(PartDocument as Inventor._Document, oTG.CreatePoint2d(Sheet.Width / 2,( Sheet.Height / 2)-4), 0.2, Inventor.ViewOrientationTypeEnum.kTopViewOrientation, Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", null, null);

        //    //Sheet.RevisionTables.Add(oTG.CreatePoint2d(Sheet.Width, Sheet.Height));  //1mm div 10//1 row = 4

        //    Inventor.DrawingCurve SelectedCurve = null;

        //    foreach (Inventor.DrawingCurve CurveLine in oBaseView.get_DrawingCurves(null))
        //    {
        //        //Skip Circles
        //        if (CurveLine.StartPoint != null && CurveLine.EndPoint != null)
        //        {
        //            if (WithinTol(CurveLine.StartPoint.Y, CurveLine.EndPoint.Y, 0.001))
        //            {
        //                if (SelectedCurve == null)
        //                {
        //                    //This is the first horizontal curve found.
        //                    SelectedCurve = CurveLine;
        //                }
        //                else
        //                {
        //                    //Check to see if this curve is higher (smaller x value) than the current selected
        //                    if (CurveLine.MidPoint.Y < SelectedCurve.MidPoint.X)
        //                    {
        //                        SelectedCurve = CurveLine;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //Create geometry intents point for the curve.
        //    Inventor.GeometryIntent oGeomIntent1 = Sheet.CreateGeometryIntent(SelectedCurve, Inventor.PointIntentEnum.kStartPointIntent);
        //    Inventor.GeometryIntent oGeomIntent2 = Sheet.CreateGeometryIntent(SelectedCurve, Inventor.PointIntentEnum.kEndPointIntent);
        //    Inventor.Point2d oDimPos = oTG.CreatePoint2d(SelectedCurve.MidPoint.X - 2, SelectedCurve.MidPoint.Y);

        //    Inventor.GeneralDimensions oGeneralDimensions = Sheet.DrawingDimensions.GeneralDimensions;
        //    //Inventor.DimensionStyle dimstyle = DrawingDocument.StylesManager.DimensionStyles[cmbDimStyles.Text];
        //    //Inventor.Layer layer = DrawingDocument.StylesManager.Layers[cmbLayers.Text];
        //    Inventor.LinearGeneralDimension oLinearDim;
        //    oLinearDim = oGeneralDimensions.AddLinear(oDimPos, oGeomIntent1, oGeomIntent2, Inventor.DimensionTypeEnum.kAlignedDimensionType, true);

        //    mInvApplication.SilentOperation = true;
        //    string partURL = PartDocument.FullFileName;
        //    int NameLength = PartDocument.FullFileName.Length;
        //    string partURLTrimed = partURL.Remove(NameLength-4); ;

        //    //DrawingDocument.Save();
        //    DrawingDocument.SaveAs(partURLTrimed + ".idw", false);
        //    DrawingDocument.Close(true);
        //    PartDocument.Close(false);
        //    mInvApplication.SilentOperation = false;
        //}
        //private void CreateDrawingDocument(out Inventor.DrawingDocument DrawingDocument, out Inventor.Sheet Sheet)
        //{
        //    string SelectedTemplate = "";

        //    SelectedTemplate = cmbFilePaths.Items[cmbFilePaths.SelectedIndex].ToString() ;
        //    DrawingDocument = mInvApplication.Documents.Add(Inventor.DocumentTypeEnum.kDrawingDocumentObject, SelectedTemplate, true) as Inventor.DrawingDocument;
        //    Sheet = DrawingDocument.Sheets[1];
        //}
        //private bool WithinTol(double Value1, double Value2, double tol)
        //{
        //    return (Math.Abs(Value1 - Value2) < tol);
        //}





        //private void GetAllOccurrancesByCount(ref Inventor.AssemblyDocument AssemblyDocument)
        //{
        //    int length = AssemblyDocument.ComponentDefinition.Occurrences.Count;

        //    for (int i = 1; i <= length; i++)
        //    {
        //        Inventor.ComponentOccurrence Occurrence = AssemblyDocument.ComponentDefinition.Occurrences[i];
        //        if (Occurrence.SubOccurrences.Count == 0)
        //        {

        //            ListComponents.Items.Add(" Top Level Part + " + AssemblyDocument.ComponentDefinition.Occurrences[i].Name + " + ");

        //        }
        //        else
        //        {
        //            ListComponents.Items.Add(" Sub Assembly *** " + Occurrence.Name + " ***** " + Occurrence._DisplayName);
        //            GetSubAssemblyOccurrencesCount(ref Occurrence);
        //        }
        //    }
        //}

        //private void GetSubAssemblyOccurrencesCount(ref Inventor.ComponentOccurrence Occurrence)
        //{
        //    int length = Occurrence.SubOccurrences.Count;

        //    for (int i = 1; i <= length; i++)
        //    {
        //        Inventor.ComponentOccurrence SubOccurrence = Occurrence.SubOccurrences[i];
        //        if (SubOccurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
        //        {
        //            ListComponents.Items.Add(" Sub Part * " + SubOccurrence.Name + " * ");
        //        }
        //        else
        //        {
        //            GetSubAssemblyOccurrencesCount(ref SubOccurrence);
        //        }
        //    }
        //}
        private void BtnFolderBrowser_Click(object sender, EventArgs e)
        {

            DialogResult DiagResult = FolderBrowserDiag.ShowDialog();
            if (DiagResult == DialogResult.OK)
            {
                UserSaveFolderName = FolderBrowserDiag.SelectedPath;
            }
        }

        private void CmbDrgTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox TemplateComboBox = (ComboBox)sender;
            UserDrawingTemplatePath = (string)TemplateComboBox.SelectedItem; 
        }

        private void CmbView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ViewOrientatioComboBox = (ComboBox)sender;
            UserViewOrientationAngel = (string)ViewOrientatioComboBox.SelectedItem;
        }
    }
}
