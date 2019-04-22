using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Inventor;

namespace Drawing_Creation_Forms_Application
{
    public partial class DrawingCreationForm : Form
    {

        Inventor.Application _invProgram;
        bool _started = false;
        private Inventor.Application mApp = null; //The Application Instance
        //***********************************************************************************************
       
        public DrawingCreationForm(Inventor.Application oApp)//Constructor
        {
            InitializeComponent();
            mApp = oApp;
            try
            {
                _invProgram = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");
                DrawingDocument oDoc;
         
               // GetNewDrawingDocument(out oDoc, out oSheet);
                GetNewDrawingDocument(out oDoc);
                
                Styles sty = oDoc.StylesManager.Styles;
                foreach (DimensionStyle dims in oDoc.StylesManager.DimensionStyles)
                {
                    cmbDimStyles.Items.Add(dims.Name);   
                }
                foreach (Layer lay in oDoc.StylesManager.Layers)
                {
                    cmbLayers.Items.Add(lay.Name);   
                }
                foreach (BorderDefinition mBorder in oDoc.BorderDefinitions)
                {
                    cmbBorder.Items.Add(mBorder.Name); 
                }
                foreach (TitleBlockDefinition oTitleBlock in oDoc.TitleBlockDefinitions)
                {
                    cmbTitle_Block.Items.Add(oTitleBlock.Name);
                }
                cmbBorder.SelectedIndex = 0;
                cmbTitle_Block.SelectedIndex = 0;
                cmbLayers.SelectedIndex = 0; 
                cmbDimStyles.SelectedIndex = 0;  
               oDoc.Close(false);   
            }
            catch (Exception ex)
            {
                try
                {
                    Type invAppType = Type.GetTypeFromProgID("Inventor.Application");

                    _invProgram = (Inventor.Application)System.Activator.CreateInstance(invAppType);
                    _invProgram.Visible = true;

                    //Note: if the Inventor session is left running after this
                    //form is closed, there will still an be and Inventor.exe 
                    //running. We will use this Boolean to test in Form1.Designer.cs 
                    //in the dispose method whether or not the Inventor App should
                    //be shut down when the form is closed.
                    _started = true;

                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.ToString());
                    MessageBox.Show("Unable to get or start Inventor");
                }
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {


            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                    Inventor.AssemblyDocument mAssemblyDocument;
                    Inventor.AssemblyComponentDefinition mAssemblyComponentDefinition;
                    mAssemblyDocument = (AssemblyDocument)mApp.Documents.Open(dlg.FileName, false);
                    mAssemblyComponentDefinition = mAssemblyDocument.ComponentDefinition;

                        MessageBox.Show("Assembly File");
                        BuildTree(0, mAssemblyDocument);
                        txtOpenPath.Text = dlg.FileName;
            }
        }

        private void BuildTree(int typeImage, AssemblyDocument tAssemblyDocument) //Uses GetComponents() for tree
        {
            try
            {
                //suppress re-painting the tree view until all nodes have been created
                treList.BeginUpdate();
                //clear the tree contents
                treList.Nodes.Clear();
                //build a node for the top level assembly
                TreeNode objTopNode = new TreeNode(tAssemblyDocument.DisplayName, typeImage, typeImage);
                treList.Nodes.Add(objTopNode);
                treList.Nodes[0].Tag = tAssemblyDocument.FullFileName;
                //call the recursive function to iterate through the assembly tree
                GetTreeComponents(tAssemblyDocument.ComponentDefinition.Occurrences, objTopNode);
                //expand the node representing the top level assembly
                objTopNode.Expand();
                //re-paint the tree view 
                treList.EndUpdate();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open and get the File heirarchy of the specified file", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GetTreeComponents(Inventor.ComponentOccurrences inCollection, TreeNode objparentNode) //uses BuildTree iterate components TO FINISH
        {
            //iterate through the components in the current collection
            IEnumerator assemblyEnumerator = inCollection.GetEnumerator();
            Inventor.ComponentOccurrence invCompOccurrence;

            while (assemblyEnumerator.MoveNext() == true)
            {
                invCompOccurrence = (Inventor.ComponentOccurrence)assemblyEnumerator.Current;
                int ImageType = 10;

                //determine if the current component is an assembly or part or iparts/iAssembly
                if (invCompOccurrence.IsiAssemblyMember || invCompOccurrence.IsiPartMember)
                {
                    if (invCompOccurrence.IsiAssemblyMember)
                    {
                        ImageType = 1;

                    }
                    else
                    {
                        ImageType = 3;
                    }
                }
                else
                {
                    if (invCompOccurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        ImageType = 0;
                    }
                    else
                    {
                        ImageType = 2;
                    }
                }
                TreeNode invCurrentNode = new TreeNode(invCompOccurrence.Name, ImageType, ImageType);
                invCurrentNode.Tag = invCompOccurrence.ReferencedDocumentDescriptor.ReferencedFileDescriptor.ReferencedFile.FullFileName;
                objparentNode.Nodes.Add(invCurrentNode);

                //recursively call this function for the suboccurrences of the current component
                GetTreeComponents((Inventor.ComponentOccurrences)invCompOccurrence.SubOccurrences, invCurrentNode);
                invCurrentNode.Expand();
            }
        }

        public void DrawGroups(bool add)
        {

            if (mApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open an Assembly document");
                return;
            }

            if (mApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("Need to have an Assembly document active");
                return;
            }

            AssemblyDocument assmbleyActiveDoc = default(AssemblyDocument);
            assmbleyActiveDoc = (AssemblyDocument)mApp.ActiveDocument; //GET ACTIVE INVENTOR WINDOW
                   
            if (assmbleyActiveDoc.SelectSet.Count == 0)
            {
                MessageBox.Show("Nothing Selected Part or Assembly");
                return;
            }

            SelectSet selActiveSet = default(SelectSet);
            selActiveSet = assmbleyActiveDoc.SelectSet;
           
            try
            {
                ComponentOccurrence compOcc = default(ComponentOccurrence);
                PartDocument mPartDoc;
                AssemblyDocument mAssmbleyDoc;

                object obj = null;
                foreach (object obj_loopVariable in selActiveSet)
                {
                    obj = obj_loopVariable;
                    
                    compOcc = (ComponentOccurrence)obj;
                    //System.Diagnostics.Debug.Print(compOcc.Name);

                    AttributeSets attbSets = compOcc.AttributeSets;
                    if (compOcc.DefinitionDocumentType == DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        mAssmbleyDoc = (AssemblyDocument)compOcc.Definition.Document;
             
                        DrawAssemDoc(mAssmbleyDoc.FullDocumentName);
                    }
                    else
                    {
                        mPartDoc = (PartDocument)compOcc.Definition.Document;

                        DrawPartDoc(mPartDoc.FullDocumentName);


                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Is the selected item a Component?");
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void DrawAssemDoc(string filename)
        {
            DrawingDocument oDoc;
            Sheet oSheet;
            SetupNewDrawingDocument(out oDoc, out oSheet);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Open the Ass document, invisibly.
            AssemblyDocument oBlockAssem = mApp.Documents.Open(filename, false) as AssemblyDocument;
            //AssemblyDocument oBlockAssem = mApp.Documents.Open(@"E:\Projects\TST PRJ\Assembly1.iam", false) as AssemblyDocument;
            TransientGeometry oTG = mApp.TransientGeometry;

            //Create base drawing view//0.1 = 1:10 or 0.2 = 1:5 1:20=0.02   X-> Y ^
            DrawingView oBaseView = oSheet.DrawingViews.AddBaseView(oBlockAssem as _Document,
                                                                    oTG.CreatePoint2d(28.7, 21), 0.1,
                                                                    ViewOrientationTypeEnum.kFrontViewOrientation,
                                                                    DrawingViewStyleEnum.kHiddenLineDrawingViewStyle, "", null, null);
            //59.4 x 42.0   29.7 X 21.0
            DrawingView oTopView = oSheet.DrawingViews.AddProjectedView(oBaseView,
                                                oTG.CreatePoint2d(28.7, 29),
                                                DrawingViewStyleEnum.kFromBaseDrawingViewStyle, null);

            //Create Projected views
            DrawingView oRightView = oSheet.DrawingViews.AddProjectedView(oBaseView,
                                                                          oTG.CreatePoint2d(45, 21),
                                                                          DrawingViewStyleEnum.kFromBaseDrawingViewStyle, null);
            //Find an edge in the part to dimension.  Any method can be used, (attributes, B-Rep query, selection, etc.).  This
            //looks through the curves in the drawing view and finds the top horizontal curve.
            oSheet.RevisionTables.Add(oTG.CreatePoint2d(48.4, 23.5));  //1mm div 10//1 row = 4

            DrawingCurve oSelectedCurve = null;

            foreach (DrawingCurve oCurve in oBaseView.get_DrawingCurves(null))
            {
                //Skip Circles
                if (oCurve.StartPoint != null && oCurve.EndPoint != null)
                {
                    if (WithinTol(oCurve.StartPoint.Y, oCurve.EndPoint.Y, 0.001))
                    {
                        if (oSelectedCurve == null)
                        {
                            //This is the first horizontal curve found.
                            oSelectedCurve = oCurve;
                        }
                        else
                        {
                            //Check to see if this curve is higher (smaller x value) than the current selected
                            if (oCurve.MidPoint.Y < oSelectedCurve.MidPoint.X)
                            {
                                oSelectedCurve = oCurve;
                            }
                        }
                    }
                }
            }
            //Create geometry intents point for the curve.
            GeometryIntent oGeomIntent1 = oSheet.CreateGeometryIntent(oSelectedCurve, PointIntentEnum.kStartPointIntent);
            GeometryIntent oGeomIntent2 = oSheet.CreateGeometryIntent(oSelectedCurve, PointIntentEnum.kEndPointIntent);
            Point2d oDimPos = oTG.CreatePoint2d(oSelectedCurve.MidPoint.X - 2, oSelectedCurve.MidPoint.Y);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //set up Dim
            GeneralDimensions oGeneralDimensions = oSheet.DrawingDimensions.GeneralDimensions;

            DimensionStyle dimstyle = oDoc.StylesManager.DimensionStyles[cmbDimStyles.Text];

            //Set Layer
            Layer layer = oDoc.StylesManager.Layers[cmbLayers.Text];

            //Create the dimension.
            LinearGeneralDimension oLinearDim;
            oLinearDim = oGeneralDimensions.AddLinear(oDimPos, oGeomIntent1, oGeomIntent2,
                                                      DimensionTypeEnum.kAlignedDimensionType, true,
                                                      dimstyle,
                                                      layer);

            string newfilename;
            string swapfilename;
            newfilename = "";
            swapfilename = "";
            //Build New Filname
            Inventor.PropertySet InvPropertySet = oBlockAssem.PropertySets["{D5CDD505-2E9C-101B-9397-08002B2CF9AE}"];
            swapfilename = oBlockAssem.FullFileName.Substring(0, oBlockAssem.FullFileName.LastIndexOf("\\") + 1);
            //MessageBox.Show(swapfilename);
            newfilename = swapfilename + InvPropertySet["FULLFILENAME"].Value + ".idw";
            oBlockAssem.Close(true);
            oDoc.SaveAs(newfilename, true);
            oDoc.Close(true);  
        }

        private void SetupNewDrawingDocument(out DrawingDocument oDoc, out Sheet oSheet)
        {
            //new drawing document.
            oDoc = mApp.Documents.Add(DocumentTypeEnum.kDrawingDocumentObject,
                       mApp.FileManager.GetTemplateFile(DocumentTypeEnum.kDrawingDocumentObject,SystemOfMeasureEnum.kDefaultSystemOfMeasure,
                       DraftingStandardEnum.kDefault_DraftingStandard,null),true) as DrawingDocument;

            //Create a new sheet.
            oSheet = oDoc.Sheets.Add(DrawingSheetSizeEnum.kA2DrawingSheetSize,PageOrientationTypeEnum.kDefaultPageOrientation,"A2", 0, 0);

            //Add the border.
            oSheet.AddBorder(oDoc.BorderDefinitions[cmbBorder.Text ]);

            //Add  TitleBlock
            //MessageBox.Show(cmbTitle_Block.Text);
            TitleBlock oTitleBlock = oSheet.AddTitleBlock(oDoc.TitleBlockDefinitions[cmbTitle_Block.Text], null, null);
            //TitleBlock oTitleBlock = oSheet.AddTitleBlock(oDoc.TitleBlockDefinitions["Portasilo Drax Title Block (rev 2)"], null, null);
        }
       
        // private void GetNewDrawingDocument(out DrawingDocument oDoc, out Sheet oSheet)
        private void GetNewDrawingDocument(out DrawingDocument oDoc)
        {
            //new drawing document.
            oDoc = mApp.Documents.Add(DocumentTypeEnum.kDrawingDocumentObject,
                       mApp.FileManager.GetTemplateFile(DocumentTypeEnum.kDrawingDocumentObject, SystemOfMeasureEnum.kDefaultSystemOfMeasure,
                       DraftingStandardEnum.kDefault_DraftingStandard, null), false) as DrawingDocument;
            
        }

        private void DrawPartDoc(string filename)
        {
            DrawingDocument oDoc;
            Sheet oSheet;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            SetupNewDrawingDocument(out oDoc, out oSheet);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //Open the part document, invisibly.
                PartDocument oBlockPart = mApp.Documents.Open(filename, false) as PartDocument;
                TransientGeometry oTG = mApp.TransientGeometry;

               //0.1 = 1:10 or 0.2 = 1:5 1:20=0.02   X-> Y ^
                DrawingView oBaseView = oSheet.DrawingViews.AddBaseView(oBlockPart as _Document,
                                                                        oTG.CreatePoint2d(28.7, 21), 0.1,
                                                                        ViewOrientationTypeEnum.kFrontViewOrientation,
                                                                        DrawingViewStyleEnum.kHiddenLineDrawingViewStyle, "", null, null);
                //59.4 x 42.0   29.7 X 21.0
                DrawingView oTopView = oSheet.DrawingViews.AddProjectedView(oBaseView,
                                                    oTG.CreatePoint2d(28.7, 29),
                                                    DrawingViewStyleEnum.kFromBaseDrawingViewStyle, null);

                //Projected views
                DrawingView oRightView = oSheet.DrawingViews.AddProjectedView(oBaseView,
                                                                              oTG.CreatePoint2d(45, 21),
                                                                              DrawingViewStyleEnum.kFromBaseDrawingViewStyle, null);

                //look through the curves in view finds top horiz curve. Find an edge
                oSheet.RevisionTables.Add(oTG.CreatePoint2d(48.4, 23.5));  //1mm div 10//1 row = 4

                DrawingCurve oSelectedCurve = null;

                foreach (DrawingCurve oCurve in oBaseView.get_DrawingCurves(null))
                {
                    //Skip Circles
                    if (oCurve.StartPoint != null && oCurve.EndPoint != null)
                    {
                        if (WithinTol(oCurve.StartPoint.Y, oCurve.EndPoint.Y, 0.001))
                        {
                            if (oSelectedCurve == null)
                            {
                                //This is the first horizontal curve found.
                                oSelectedCurve = oCurve;
                            }
                            else
                            {
                                //Check to see if this curve is higher (smaller x value) than the current selected
                                if (oCurve.MidPoint.Y < oSelectedCurve.MidPoint.X)
                                {
                                    oSelectedCurve = oCurve;
                                }
                            }
                        }
                    }
                }
                //Create geometry intents point for the curve.
                GeometryIntent oGeomIntent1 = oSheet.CreateGeometryIntent(oSelectedCurve, PointIntentEnum.kStartPointIntent);
                GeometryIntent oGeomIntent2 = oSheet.CreateGeometryIntent(oSelectedCurve, PointIntentEnum.kEndPointIntent);
                Point2d oDimPos = oTG.CreatePoint2d(oSelectedCurve.MidPoint.X - 2, oSelectedCurve.MidPoint.Y);

                //set up Dim
                GeneralDimensions oGeneralDimensions = oSheet.DrawingDimensions.GeneralDimensions;
               
                 //Styles sty = oDoc.StylesManager.Styles   ; 
               // DimensionStyle dimstyle = oDoc.StylesManager.DimensionStyles["Drax Dim Above"];
                //MessageBox.Show(cmbDimStyles.Text);
                DimensionStyle dimstyle = oDoc.StylesManager.DimensionStyles[cmbDimStyles.Text];

                //Set Layer
                //Layer layer = oDoc.StylesManager.Layers["D"];
                //MessageBox.Show(cmbLayers.Text);
                Layer layer = oDoc.StylesManager.Layers[cmbLayers.Text];

                //Create the dimension.
                LinearGeneralDimension oLinearDim;
                oLinearDim = oGeneralDimensions.AddLinear(oDimPos, oGeomIntent1, oGeomIntent2,
                                                          DimensionTypeEnum.kAlignedDimensionType, true,
                                                          dimstyle,
                                                          layer);


                string newfilename;
                string swapfilename;
                newfilename = "";
                swapfilename = "";
                                             //Build New Filname
                Inventor.PropertySet InvPropertySet = oBlockPart.PropertySets["{D5CDD505-2E9C-101B-9397-08002B2CF9AE}"];
                swapfilename = oBlockPart.FullFileName.Substring(0, oBlockPart.FullFileName.LastIndexOf("\\") + 1);
                //MessageBox.Show(swapfilename);
                newfilename = swapfilename + InvPropertySet["FULLFILENAME"].Value + ".idw";
                oBlockPart.Close(true);
                oDoc.SaveAs(newfilename, true);
                oDoc.Close(true);   
        }

        private bool WithinTol(double Value1, double Value2, double tol)
        {
            return (Math.Abs(Value1 - Value2) < tol);
        }
               
        private void btnCreatDrawings_Click(object sender, EventArgs e)
        {
            DrawGroups(true);
             
        }
    }
}

//if (add)
//{
//    // Add the attributes to the ComponentOccurrence

//    if (!attbSets.NameIsUsed["DrawMeGroup"])
//    {
//        AttributeSet attbSet = attbSets.Add("DrawMeGroup");

//        Inventor.Attribute attb = attbSet.Add("DrawGroupOne", ValueTypeEnum.kStringType, "GroupOne");

//    }
//}
//else
//{
//    // Delete the attributes to the ComponentOccurrence
//    if (attbSets.NameIsUsed["DrawMeGroup"])
//    {
//        attbSets["DrawMeGroup"].Delete();
//    }

//}

//compOcc.Visible = False

//public void HideOrShowDrawGroup(bool hide)
//{
//    if (mApp.Documents.Count == 0)
//    {
//        MessageBox.Show("Need to open an Assembly document");
//        return;
//    }

//    if (mApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
//    {
//        MessageBox.Show("Need to have an Assembly document active");
//        return;
//    }

//    AssemblyDocument asmDoc = default(AssemblyDocument);
//    asmDoc = (AssemblyDocument)mApp.ActiveDocument;


//    try
//    {
//        AttributeManager attbMan = asmDoc.AttributeManager;

//        ObjectCollection objsCol = default(ObjectCollection);
//        objsCol = attbMan.FindObjects("DrawMeGroup", "DrawGroupOne", "GroupOne");

//        ComponentOccurrence compOcc = default(ComponentOccurrence);
//        AssemblyComponentDefinitions CDef = default(AssemblyComponentDefinitions);

//        foreach (ComponentDefinition cd in CDef)
//        {

//        }

//        foreach (object obj in objsCol)
//        {
//            compOcc = (ComponentOccurrence)obj;
//            compOcc.Visible = hide;

//        }

//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show("Problem hiding component");
//    }
//}