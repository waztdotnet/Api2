using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDrawApp
{
    class CDraw
    {
        private Inventor.Application mInvApplication = null;
        private string txtPartID;
        private string ViewTypeOrientationAngel;
        private string SelectedTemplateFilePath;

        public CDraw(Inventor.Application InvApplication)
        {
            if (InvApplication != null)
            {
                mInvApplication = InvApplication;
            }
            else
                return;
         ViewTypeOrientationAngel = "Default";
         SelectedTemplateFilePath = "";
        }

        public void DrawSingelPart()
        {
            Inventor._Document Document;
            if (mInvApplication.Documents.Count > 0)
            {
                if (mInvApplication.ActiveDocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
                {
                    Inventor.PartDocument PartDocument = (Inventor._PartDocument)mInvApplication.ActiveDocument;

                    string TypeOfPart = "";
                    Document = (Inventor._Document)PartDocument;

                    // normal part
                    if (PartDocument.SubType != "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                    {
                        TypeOfPart = "Plate";
                        DrawDocument(Document, TypeOfPart);
                    }
                    else // if sheet metal
                    {
                        
                        TypeOfPart = "SheetMetal";
                        DrawDocument(Document, TypeOfPart);
                    }
                }
                else
                { return; }
            }

        }

        public void DrawMultiPart()
        {
            Inventor._Document Document;
            if (mInvApplication.Documents.Count > 0)
            {
                txtPartID = "P";
                Document = mInvApplication.ActiveDocument;
                ProcessFileRefs(Document.File);
               

            }

        }
        public void SetDrawingTemplateFilePath(string DrawingTemplateFilePath)
        {
            if (DrawingTemplateFilePath != "")
            {
                SelectedTemplateFilePath = DrawingTemplateFilePath; 
            }
        }
        public void SetViewTypeOrientation(string ViewOrientation)
        {
            if (ViewOrientation != "")
            {
                ViewTypeOrientationAngel = ViewOrientation;
            }
        }

        private static Inventor.Point2d GetDrawingCenterPoint(Inventor.Sheet CurrentSheet, Inventor.TransientGeometry TransientGeometry)
        {
            Inventor.Point2d Point2d = null;
            double DrawingTitelBlockHeight=5;
            if (CurrentSheet.Size == Inventor.DrawingSheetSizeEnum.kA4DrawingSheetSize)
            {
                DrawingTitelBlockHeight = 5; 
            }
            else if(CurrentSheet.Size == Inventor.DrawingSheetSizeEnum.kA3DrawingSheetSize)
            {
                DrawingTitelBlockHeight = 5;
            }
            double[] ViewCenter = new double[2];
            
            double SheetWidth = CurrentSheet.Width;
            double SheetHeight = CurrentSheet.Height;
            ViewCenter[0] = SheetWidth / 2;
            ViewCenter[1] = (SheetHeight + DrawingTitelBlockHeight) / 2;

            Point2d =  TransientGeometry.CreatePoint2d(ViewCenter[0], ViewCenter[1]);
            return Point2d;
        }

        private static void SetViewScale(Inventor.Sheet CurrentSheet,ref Inventor.DrawingView DrawingView)
        {
            double DrawingTitelBlockHeight = 5;
            
            if (CurrentSheet.Size == Inventor.DrawingSheetSizeEnum.kA4DrawingSheetSize)
            {
                DrawingTitelBlockHeight = 5;
            }
            else if (CurrentSheet.Size == Inventor.DrawingSheetSizeEnum.kA3DrawingSheetSize)
            {
                DrawingTitelBlockHeight = 5;
            }
            double TargetViewGapWidth = 2;
            double TargetViewGapHeight = 3;

            double TargetViewWidth = CurrentSheet.Width - (2 * TargetViewGapWidth);
            double TargetViewHeight = CurrentSheet.Height - ((2 * TargetViewGapHeight) + DrawingTitelBlockHeight);

            double SheetWidth = CurrentSheet.Width;
            double SheetHeight = CurrentSheet.Height;
           
            double[] ViewCenter = new double[2];

            string[] DrawingScales;
            dynamic[] DynamScales;
            Inventor.DrawingDocument DrawingDocument = (Inventor.DrawingDocument)CurrentSheet.Parent;
            Inventor.DrawingStandardStyle DrawingStandardStyle = DrawingDocument.StylesManager.ActiveStandardStyle;

            DynamScales = DrawingStandardStyle.PresetScales;
          
            DrawingScales = new string[DynamScales.Length];

            DrawingScales = DrawingStandardStyle.PresetScales;
            DynamScales = null;

            foreach (string ScaleString in DrawingScales)
            {
                double ScalingValue = 1;
                double ScalerValue = 1;
                double Scale = 0;
                if (ScaleString != null)
                {
                    int Delimitor = ScaleString.IndexOf(":");
                    ScalingValue = Convert.ToDouble(ScaleString.Substring(0, Delimitor));
                    ScalerValue = Convert.ToDouble(ScaleString.Substring(Delimitor + 1, ScaleString.Length - (Delimitor + 1)));
                }
                Scale = ScalingValue / ScalerValue;
            }

            for (int i = 0; i < 50; i++)
            {
                DrawingView.Scale = TargetViewWidth / DrawingView.Width * DrawingView.Scale;
                if (DrawingView.Height > TargetViewHeight)
                {

                    DrawingView.Scale = TargetViewHeight / DrawingView.Height * DrawingView.Scale;

                }
            }
        }

        private static double[] GetScaleValues(Inventor.Sheet CurrentSheet)
        {
            double []ScaleValues;
            string[] DrawingScales;
            dynamic[] DynamScales;
            Inventor.DrawingDocument DrawingDocument = (Inventor.DrawingDocument)CurrentSheet.Parent;
            Inventor.DrawingStandardStyle DrawingStandardStyle = DrawingDocument.StylesManager.ActiveStandardStyle;

            DynamScales = DrawingStandardStyle.PresetScales;
            DrawingScales = new string[DynamScales.Length];
            ScaleValues = new double[DynamScales.Length];
            DrawingScales = DrawingStandardStyle.PresetScales;
            DynamScales = null;
            int i = 0;
            foreach (string ScaleString in DrawingScales)
            {
                double ScalingValue = 1;
                double ScalerValue = 1;
                if (ScaleString != null)
                {
                    int Delimitor = ScaleString.IndexOf(":");
                    ScalingValue = Convert.ToDouble(ScaleString.Substring(0, Delimitor));
                    ScalerValue = Convert.ToDouble(ScaleString.Substring(Delimitor + 1, ScaleString.Length - (Delimitor + 1)));
                }
                ScaleValues[i] = ScalingValue / ScalerValue;
                i++;
            }

            return ScaleValues;
        }

        private static Inventor.DrawingView AddDocumentBaseView(Inventor._Document Document, Inventor.Sheet Sheet, Inventor.TransientGeometry TransientGeometry,Inventor.Camera Camera)
        {
            double ViewWidth = 17;

            Inventor.DrawingView DrawingView = Sheet.DrawingViews.AddBaseView(Document as Inventor._Document, GetDrawingCenterPoint(Sheet, TransientGeometry), 0.2, Inventor.ViewOrientationTypeEnum.kArbitraryViewOrientation, Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", Camera, null);

            DrawingView.Scale = ViewWidth / DrawingView.Width * DrawingView.Scale;

           SetViewScale(Sheet, ref DrawingView);

            return DrawingView;
        }
        private static Inventor.DrawingView AddDocumentBaseView(Inventor._Document Document, Inventor.Sheet Sheet, Inventor.TransientGeometry TransientGeometry, Inventor.ViewOrientationTypeEnum Orientation)
        {

            double ViewWidth = 17;
            Inventor.DrawingView DrawingView = Sheet.DrawingViews.AddBaseView(Document as Inventor._Document, GetDrawingCenterPoint(Sheet, TransientGeometry), 0.2, Orientation, Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", null, null);

            DrawingView.Scale = ViewWidth / DrawingView.Width * DrawingView.Scale;

            return DrawingView;
        }

        private static Inventor.DrawingView AddDocumentBaseView(Inventor._Document Document, Inventor.Sheet Sheet, Inventor.TransientGeometry TransientGeometry,Inventor.NameValueMap BaseViewOptions)
        {
            double ViewWidth = 17;

            Inventor.DrawingView DrawingView = Sheet.DrawingViews.AddBaseView(Document as Inventor._Document, GetDrawingCenterPoint(Sheet, TransientGeometry), 0.2, Inventor.ViewOrientationTypeEnum.kDefaultViewOrientation, Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", null, BaseViewOptions);

            DrawingView.Scale = ViewWidth / DrawingView.Width * DrawingView.Scale;

            return DrawingView;
        }
       
        private void DrawDocument(Inventor._Document Document, string TypeOfPart)
        {
            Inventor.ViewOrientationTypeEnum Orientation = Inventor.ViewOrientationTypeEnum.kDefaultViewOrientation;
            Inventor.DrawingDocument DrawingDocument;
            Inventor.Sheet Sheet;

            Inventor.DrawingView DrawingView;
            Inventor.TransientGeometry oTG = mInvApplication.TransientGeometry;
            CreateDrawingDocument(out DrawingDocument, out Sheet);
            //Inventor.DrawingStandardStyle DrawingStandardStyle;
            //DrawingStandardStyle = DrawingDocument.StylesManager.ActiveStandardStyle;
            if (TypeOfPart == "SheetMetal")
            {
                Inventor.NameValueMap BaseViewOptions = (Inventor.NameValueMap)mInvApplication.TransientObjects.CreateNameValueMap();
                BaseViewOptions.Add("SheetMetalFoldedModel", false);
                DrawingView = AddDocumentBaseView(Document, Sheet, oTG, BaseViewOptions); 
            }
            else if (TypeOfPart == "Plate")
            {

                CDrawingView CView = new CDrawingView();
                Inventor.Camera Camera = null;
                Camera = CView.GetDocument(ref Document, ref mInvApplication);
                DrawingView = AddDocumentBaseView(Document, Sheet, oTG, Camera);
            }
            else
            {
                ViewOrientation(ref Orientation);
                DrawingView = AddDocumentBaseView(Document, Sheet, oTG, Orientation); 
            }


            try
            {
                Inventor.DrawingCurve SelectedCurve = null;

                foreach (Inventor.DrawingCurve CurveLine in DrawingView.get_DrawingCurves(null))
                {
                    //Skip Circles
                    if (CurveLine.StartPoint != null && CurveLine.EndPoint != null)
                    {
                        if (WithinTol(CurveLine.StartPoint.Y, CurveLine.EndPoint.Y, 0.001))
                        {
                            if (SelectedCurve == null)
                            {
                                //This is the first horizontal curve found.
                                SelectedCurve = CurveLine;
                            }
                            else
                            {
                                //Check to see if this curve is higher (smaller x value) than the current selected
                                if (CurveLine.MidPoint.Y < SelectedCurve.MidPoint.X)
                                {
                                    SelectedCurve = CurveLine;
                                }
                            }
                        }
                    }
                }
                //Create geometry intents point for the curve.
                Inventor.GeometryIntent oGeomIntent1 = Sheet.CreateGeometryIntent(SelectedCurve, Inventor.PointIntentEnum.kStartPointIntent);
                Inventor.GeometryIntent oGeomIntent2 = Sheet.CreateGeometryIntent(SelectedCurve, Inventor.PointIntentEnum.kEndPointIntent);
                Inventor.Point2d oDimPos = oTG.CreatePoint2d(SelectedCurve.MidPoint.X - 2, SelectedCurve.MidPoint.Y);

                Inventor.GeneralDimensions oGeneralDimensions = Sheet.DrawingDimensions.GeneralDimensions;
                Inventor.LinearGeneralDimension oLinearDim;
                oLinearDim = oGeneralDimensions.AddLinear(oDimPos, oGeomIntent1, oGeomIntent2, Inventor.DimensionTypeEnum.kAlignedDimensionType, true);
            }
            catch (Exception)
            {


            }
            mInvApplication.SilentOperation = true;
                string partURL = Document.FullFileName;
                int NameLength = Document.FullFileName.Length;
                string partURLTrimed = partURL.Remove(NameLength - 4);

                //DrawingDocument.Save();
                DrawingDocument.SaveAs(partURLTrimed + ".idw", false);
                DrawingDocument.Close(true);
            Document.Close(false);
                mInvApplication.SilentOperation = false;


            //Sheet.RevisionTables.Add(oTG.CreatePoint2d(Sheet.Width, Sheet.Height));  //1mm div 10//1 row = 4
            //Inventor.DimensionStyle dimstyle = DrawingDocument.StylesManager.DimensionStyles[cmbDimStyles.Text];
            //Inventor.Layer layer = DrawingDocument.StylesManager.Layers[cmbLayers.Text];
        }

        private void ProcessFileRefs(Inventor.File File)
        {
            foreach (Inventor.FileDescriptor DescriptedFile in File.ReferencedFileDescriptors)
            {
                if (!DescriptedFile.ReferenceMissing)
                {

                    if (DescriptedFile.ReferencedFileType != Inventor.FileTypeEnum.kForeignFileType)
                    {

                        if (DescriptedFile.ReferencedFileType == Inventor.FileTypeEnum.kPartFileType) //part or sub;
                        {


                            string TartgetPartNumber = "";
                            Inventor.PartDocument PartDocument = (Inventor.PartDocument)mInvApplication.Documents.Open(DescriptedFile.FullFileName, false);
                            TartgetPartNumber = PartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value;
                            
                            if (TartgetPartNumber.StartsWith(txtPartID) )
                            {
                                string TypeOfPart = "";
                                Inventor._Document Document = (Inventor._Document)PartDocument;
                                
                                // normal part
                                if (PartDocument.SubType != "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                                {
                                    
                                    TypeOfPart = "Plate";
                                    DrawDocument(Document, TypeOfPart);
                                }
                                else // if sheet metal
                                {
                                    TypeOfPart = "SheetMetal";
                                    DrawDocument(Document, TypeOfPart);
                                }

                               // Document.Close(false);

                            }
                            PartDocument.Close(false);
                            
                        }

                    }
                }
            }
        }

        private void ViewOrientation(ref Inventor.ViewOrientationTypeEnum ViewTypeOrientation)
        {

            ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kDefaultViewOrientation;
            
            if (ViewTypeOrientationAngel == "Bottom")
            {
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kBottomViewOrientation;
            }
            else if (ViewTypeOrientationAngel == "Left")
            {
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kLeftViewOrientation;
            }
            else if (ViewTypeOrientationAngel == "Right")
            {
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kRightViewOrientation;
            }
            else if (ViewTypeOrientationAngel == "Front")
            {
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kFrontViewOrientation;
            }
            else if (ViewTypeOrientationAngel == "Back")
            {
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kBackViewOrientation;
            }
            else if (ViewTypeOrientationAngel == "Top")
                ViewTypeOrientation = Inventor.ViewOrientationTypeEnum.kTopViewOrientation;  
        }



        private void CreateDrawingDocument(out Inventor.DrawingDocument DrawingDocument, out Inventor.Sheet Sheet)
        {
            DrawingDocument = mInvApplication.Documents.Add(Inventor.DocumentTypeEnum.kDrawingDocumentObject, SelectedTemplateFilePath, true) as Inventor.DrawingDocument;
            Sheet = DrawingDocument.Sheets[1];
        }

        private bool WithinTol(double Value1, double Value2, double tol)
        {
            return (Math.Abs(Value1 - Value2) < tol);
        }

        //    Adding Representation views API Sample 
        //Description 
        //This sample demonstrates how to create a base view by specifying various representations.
        //Before running this sample, make sure that the file
        // C:\TempReps.iam exists (or change the path in the sample). 
        //     The file must contain a level of detail representation named MyLODRep, 
        //     a positional representation named MyPositionalRep and a design view representation named MyDesignViewRep.
        public void AddBaseViewWithRepresentations()
        {
            // Set a reference to the drawing document.
            // This assumes a drawing document is active.
            Inventor.DrawingDocument oDrawDoc = (Inventor.DrawingDocument)mInvApplication.ActiveDocument;

            //Set a reference to the active sheet.
            Inventor.Sheet oSheet = (Inventor.Sheet)oDrawDoc.ActiveSheet;

            // Create a new NameValueMap object
            Inventor.NameValueMap oBaseViewOptions = (Inventor.NameValueMap)mInvApplication.TransientObjects.CreateNameValueMap();

            // Set the representations to use when creating the base view.
            oBaseViewOptions.Add("PositionalRepresentation", "MyPositionalRep");
            oBaseViewOptions.Add("DesignViewRepresentation", "MyDesignViewRep");
            oBaseViewOptions.Add("DesignViewAssociative", true);

            // Open the model document (corresponding to the "MyLODRep" representation).
            string strFullDocumentName = null;
            strFullDocumentName = mInvApplication.FileManager.GetFullDocumentName(@"C:\tempreps.iam", "MyLODRep");

            Inventor._Document oModel = (Inventor._Document)mInvApplication.Documents.Open(strFullDocumentName, false);

            // Create the placement point object.
            Inventor.Point2d oPoint = default(Inventor.Point2d);
            oPoint = mInvApplication.TransientGeometry.CreatePoint2d(25, 25);

            // Create a base view.
            Inventor.DrawingView oBaseView = (Inventor.DrawingView)oSheet.DrawingViews.
                AddBaseView(oModel, oPoint, 2, Inventor.ViewOrientationTypeEnum.kIsoTopLeftViewOrientation,
                Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", Type.Missing, oBaseViewOptions);

            // Release reference of the invisibly open model
            oModel.ReleaseReference();
        }
        //    Create flat pattern drawing view API Sample 
        //Description 
        //This sample demonstrates the creation of a flat pattern base view in a drawing.
        //Open a drawing document and run the sample.
        public void AddFlatPatternDrawingView()
        {
            // Set a reference to the drawing document.
            // This assumes a drawing document is active.
            Inventor.DrawingDocument oDrawDoc = (Inventor.DrawingDocument)mInvApplication.ActiveDocument;

            //Set a reference to the active sheet.
            Inventor.Sheet oSheet = (Inventor.Sheet)oDrawDoc.ActiveSheet;

            // Create a new NameValueMap object
            Inventor.NameValueMap oBaseViewOptions = (Inventor.NameValueMap)mInvApplication.TransientObjects.CreateNameValueMap();

            // Set the options to use when creating the base view.
            oBaseViewOptions.Add("SheetMetalFoldedModel", false);

            // Open the sheet metal document invisibly
            Inventor._Document oModel = (Inventor._Document)mInvApplication.Documents.Open(@"C:\temp\SheetMetal.ipt", false);

            // Create the placement point object.
            Inventor.Point2d oPoint = default(Inventor.Point2d);
            oPoint = mInvApplication.TransientGeometry.CreatePoint2d(25, 25);

            // Create a base view.
            Inventor.DrawingView oBaseView = (Inventor.DrawingView)oSheet.DrawingViews
                .AddBaseView(oModel, oPoint, 1, Inventor.ViewOrientationTypeEnum.kDefaultViewOrientation,
                Inventor.DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, "", Type.Missing, oBaseViewOptions);

            // Release reference of the invisibly open model
            oModel.ReleaseReference();

        }
        //    Add detail drawing view API Sample 
        //Description 
        //This sample demonstrates the creation of a detail drawing view with an attach point.
        //Before running this sample, select a drawing view in the active drawing.
        public void CreateDetailView()
        {
            // Set a reference to the drawing document.
            // This assumes a drawing document is active.
            Inventor.DrawingDocument oDrawDoc = (Inventor.DrawingDocument)mInvApplication.ActiveDocument;

            //Set a reference to the active sheet.
            Inventor.Sheet oSheet = (Inventor.Sheet)oDrawDoc.ActiveSheet;

            Inventor.DrawingView oDrawingView = default(Inventor.DrawingView);
            // Check to make sure a drawing view is selected.
            try
            {
                // Set a reference to the selected drawing. This assumes
                // that the selected view is not a draft view.
                oDrawingView = (Inventor.DrawingView)oDrawDoc.SelectSet[1];
            }
            catch
            {
                //MessageBox.Show("A drawing view must be selected.");
                return;
            }


            // Set a reference to the center of the base view.
            Inventor.Point2d oPoint = (Inventor.Point2d)oDrawingView.Center;

            // Translate point by a distance = 2 * width of the view
            // This will be the placement point of the detail view.
            oPoint.X = oPoint.X + 2 * oDrawingView.Width;

            // Set corner one of rectangular fence as
            // the left-bottom corner of the base view.
            Inventor.Point2d oCornerOne = (Inventor.Point2d)oDrawingView.Center;


            oCornerOne.X = oCornerOne.X - oDrawingView.Width / 2;
            oCornerOne.Y = oCornerOne.Y - oDrawingView.Height / 2;

            // Set corner two of rectangular fence as
            // the center of the base view.
            Inventor.Point2d oCornerTwo = (Inventor.Point2d)oDrawingView.Center;

            // Get any linear curve from the base view
            Inventor.DrawingCurve oCurveToUse = null;
            foreach (Inventor.DrawingCurve oCurve in oDrawingView.DrawingCurves)
            {
                if (oCurve.CurveType == Inventor.CurveTypeEnum.kLineSegmentCurve)
                {
                    oCurveToUse = oCurve;
                    break;
                }
            }

            // Create an intent object
            Inventor.GeometryIntent oAttachPoint = (Inventor.GeometryIntent)oSheet.
                CreateGeometryIntent(oCurveToUse, Inventor.PointIntentEnum.kStartPointIntent);

            // Create the detail view
            Inventor.DetailDrawingView oDetailView = (Inventor.DetailDrawingView)oSheet.DrawingViews.
                AddDetailView(oDrawingView, oPoint, Inventor.DrawingViewStyleEnum.kFromBaseDrawingViewStyle,
                false, oCornerOne, oCornerTwo, oAttachPoint, 2);

        }
        public void CreateOrdinateDimensions()
        {
            // Set a reference to the drawing document.
            // This assumes a drawing document is active.
            Inventor.DrawingDocument oDrawDoc = (Inventor.DrawingDocument)mInvApplication.ActiveDocument;

            // Set a reference to the active sheet.
            Inventor.Sheet oActiveSheet = (Inventor.Sheet)oDrawDoc.ActiveSheet;

            // Set a reference to the drawing curve segment.
            // This assumes that a linear drawing curve is selected.
            Inventor.DrawingCurveSegment oDrawingCurveSegment = (Inventor.DrawingCurveSegment)oDrawDoc.SelectSet[1];


            // Set a reference to the drawing curve.
            Inventor.DrawingCurve oDrawingCurve = default(Inventor.DrawingCurve);
            oDrawingCurve = oDrawingCurveSegment.Parent;

            if (!(oDrawingCurve.CurveType == Inventor.CurveTypeEnum.kLineSegmentCurve))
            {
                //MessageBox.Show("A linear curve should be selected for this sample.");
                return;
            }

            // Create point intents to anchor the dimension to.
            Inventor.GeometryIntent oDimIntent1 = default(Inventor.GeometryIntent);
            oDimIntent1 = oActiveSheet.CreateGeometryIntent(oDrawingCurve, Inventor.PointIntentEnum.kStartPointIntent);

            Inventor.GeometryIntent oDimIntent2 = default(Inventor.GeometryIntent);
            oDimIntent2 = oActiveSheet.CreateGeometryIntent(oDrawingCurve, Inventor.PointIntentEnum.kEndPointIntent);

            // Set a reference to the view to which the curve belongs.
            Inventor.DrawingView oDrawingView = default(Inventor.DrawingView);
            oDrawingView = oDrawingCurve.Parent;

            // If origin indicator has not been already created, create it first.
            if (!oDrawingView.HasOriginIndicator)
            {
                // The indicator will be located at the start point of the selected curve.
                oDrawingView.CreateOriginIndicator(oDimIntent1);
            }

            // Set a reference to the ordinate dimensions collection.
            Inventor.OrdinateDimensions oOrdinateDimensions = default(Inventor.OrdinateDimensions);
            oOrdinateDimensions = oActiveSheet.DrawingDimensions.OrdinateDimensions;

            // Create the x-axis vector
            Inventor.Vector2d oXAxis = default(Inventor.Vector2d);
            oXAxis = mInvApplication.TransientGeometry.CreateVector2d(1, 0);

            Inventor.Vector2d oCurveVector = default(Inventor.Vector2d);
            oCurveVector = oDrawingCurve.StartPoint.VectorTo(oDrawingCurve.EndPoint);

            Inventor.Point2d oTextOrigin1 = default(Inventor.Point2d);
            Inventor.Point2d oTextOrigin2 = default(Inventor.Point2d);
            Inventor.DimensionTypeEnum DimType = default(Inventor.DimensionTypeEnum);

            if (oCurveVector.IsParallelTo(oXAxis))
            {
                // Selected curve is horizontal
                DimType = Inventor.DimensionTypeEnum.kVerticalDimensionType;

                // Set the text points for the 2 dimensions.
                oTextOrigin1 = mInvApplication.TransientGeometry.CreatePoint2d(oDrawingCurve.StartPoint.X, oDrawingView.Top + 5);
                oTextOrigin2 = mInvApplication.TransientGeometry.CreatePoint2d(oDrawingCurve.EndPoint.X, oDrawingView.Top + 5);

            }
            else
            {
                // Selected curve is vertical or at an angle.
                DimType = Inventor.DimensionTypeEnum.kHorizontalDimensionType;

                // Set the text points for the 2 dimensions.
                oTextOrigin1 = mInvApplication.TransientGeometry.CreatePoint2d(oDrawingView.Left - 5, oDrawingCurve.StartPoint.Y);
                oTextOrigin2 = mInvApplication.TransientGeometry.CreatePoint2d(oDrawingView.Left - 5, oDrawingCurve.EndPoint.Y);

            }

            // Create the first ordinate dimension.
            Inventor.OrdinateDimension oOrdinateDimension1 = default(Inventor.OrdinateDimension);
            oOrdinateDimension1 = oOrdinateDimensions.Add(oDimIntent1, oTextOrigin1, DimType);

            // Create the second ordinate dimension.
            Inventor.OrdinateDimension oOrdinateDimension2 = default(Inventor.OrdinateDimension);
            oOrdinateDimension2 = oOrdinateDimensions.Add(oDimIntent2, oTextOrigin2, DimType);
        }
    }
}
