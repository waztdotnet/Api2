using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorAutoSpool
{
    public class CModelFlange
    {
        private Inventor.Application InvApp;
        private string szFlangeDiaOD;
        private string szFlangeDiaID;
        private string szThickness;
        //private string szFlangeFaceDiaOD;
        //private string szFlangeBoltHoleDia;
        //private string szFlangeNumberOfBoltsHoles;
        //private string szFlangePCD;
        private string szPartFileNameSavePath;

        public CModelFlange(string DiaOD, string DiaID, string WallThickness)
            {
                szFlangeDiaOD = DiaOD;
            szFlangeDiaID = DiaID;
            szThickness = WallThickness;
            }

        public CModelFlange()
            {
                throw new System.NotImplementedException();
            }

        public void SetFlangeType(string szType, string NominalBore)
            {
                throw new System.NotImplementedException();
            }

        public void ModelMake(ref Inventor.Application InvInterface)
            {
                InvApp = InvInterface;
             Inventor.PartDocument FlangePartDocument = null;
            Inventor.ExtrudeFeature ExtrudeFeature = null;
            Inventor.ObjectCollection ObjectCollection = null;
             FlangePartDocument = (Inventor.PartDocument)InvApp.ActiveDocument;
            FlangePartDocument = CreateNewPartDocument();
            ExtrudeFeature = CreateFlangeExtrustion(ref FlangePartDocument, szFlangeDiaOD  ,szFlangeDiaID ,szThickness );
            Changeview();
            ObjectCollection = CreateHoleSketch(ref FlangePartDocument, ref ExtrudeFeature, "160", "30");
             CreateHoleFeatureFromSketch(ref FlangePartDocument,ref ExtrudeFeature, ObjectCollection,"18","8");
         AddFlangeiMateDefinitions (ref FlangePartDocument, szFlangeDiaID, "E", "0");
               SavePartFile(ref FlangePartDocument); 
            

        }

        public string GetFileSavePath()
        {
            return szPartFileNameSavePath;
        }

        private void AddFlangeiMateDefinitions(ref Inventor.PartDocument PartDocument, string Dia, string MateNamePrefix, string Offset)
        {
            Inventor.ExtrudeFeature ExtrudeFeature;
            Inventor.UnitsOfMeasure UnitsOfMeasure;
            Inventor.Edge Edge = null;
            Inventor.EdgeLoops EdgeLoops;
            double FlangeHoleDiaOD = 0;
            int Counter = 0;
            ExtrudeFeature = PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1];
            UnitsOfMeasure = PartDocument.UnitsOfMeasure;
            FlangeHoleDiaOD = UnitsOfMeasure.GetValueFromExpression(Dia, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);

            foreach (Inventor.Face Face in ExtrudeFeature.Faces)
            {
                if (Face.SurfaceType == Inventor.SurfaceTypeEnum.kCylinderSurface)
                {
                    Inventor.Cylinder Cylinder;
                    Cylinder = Face.Geometry;
                    if (Cylinder.Radius == FlangeHoleDiaOD / 2)
                    {
                        EdgeLoops = Face.EdgeLoops;
                        foreach (Inventor.EdgeLoop EdgeLoop in EdgeLoops)
                        {
                            if (EdgeLoop.IsOuterEdgeLoop)
                            {
                                foreach (Inventor.Edge mEdge in EdgeLoop.Edges)
                                {
                                    if (mEdge.CurveType == Inventor.CurveTypeEnum.kCircleCurve)
                                    {
                                        Edge = mEdge;
                                        PartDocument.ComponentDefinition.iMateDefinitions.AddInsertiMateDefinition(Edge, false, Offset, null, MateNamePrefix + ":" + Counter.ToString());
                                        Counter++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public void SetFileSavePath(string FilePath, string FileName)
        {
            szPartFileNameSavePath = FilePath + "\\" + FileName + ".ipt";
              
        }

        private void SavePartFile(ref Inventor.PartDocument PartDocument)
        {
            InvApp.SilentOperation = true;
            if (System.IO.File.Exists(szPartFileNameSavePath))
            {
                System.IO.File.Delete(szPartFileNameSavePath ); 
            }
            PartDocument.SaveAs(szPartFileNameSavePath, false);


            PartDocument.Close(true);
            InvApp.SilentOperation = false;
              
        }

        private Inventor.ExtrudeFeature CreateFlangeExtrustion(ref Inventor.PartDocument PartDocument, string FlangeDiaOD, string FlangeDiaID, string FlangeThickness)
            {
                
                Inventor.UnitsOfMeasure UnitsOfMeasure;
                Inventor.PlanarSketch Sketch;
                Inventor.TransientGeometry TransientGeometry;
                Inventor.SketchCircle SketchCircle;
                Inventor.WorkPoint WorkPoint;
                Inventor.WorkPlane BaseWorkPlane;
                Inventor.RadiusDimConstraint RadiusDimConstraint = null;
                Inventor.SketchEntity SketchEntity;
                Inventor.ObjectCollection SketchObjectCollection;
                Inventor.Profile Profile;
                Inventor.ExtrudeDefinition ExtrudeDefinition;
                Inventor.ExtrudeFeature ExtrudeFeature = null;

                SketchObjectCollection = InvApp.TransientObjects.CreateObjectCollection();
                
                UnitsOfMeasure = PartDocument.UnitsOfMeasure;
                double DiaOD = 0, DiaID = 0, Thickness = 0;

                DiaOD = UnitsOfMeasure.GetValueFromExpression(FlangeDiaOD, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
                DiaID = UnitsOfMeasure.GetValueFromExpression(FlangeDiaID, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
                Thickness = UnitsOfMeasure.GetValueFromExpression(FlangeThickness , Inventor.UnitsTypeEnum.kMillimeterLengthUnits);

                TransientGeometry = InvApp.TransientGeometry;
                WorkPoint = PartDocument.ComponentDefinition.WorkPoints[1];
                BaseWorkPlane = GetPartDocumentWorkPlane(ref PartDocument, "XY");

                Sketch = PartDocument.ComponentDefinition.Sketches.Add(BaseWorkPlane, false);
                SketchEntity = Sketch.AddByProjectingEntity(WorkPoint);
                SketchCircle = Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0, 0), DiaOD / 2);
                RadiusDimConstraint = Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle, TransientGeometry.CreatePoint2d(0, 0), false);
                Sketch.GeometricConstraints.AddCoincident(SketchEntity, (Inventor.SketchEntity)SketchCircle.CenterSketchPoint);

                RadiusDimConstraint = null;
                SketchCircle = null;
                SketchCircle = Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0, 0), DiaID / 2);
                RadiusDimConstraint = Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle, TransientGeometry.CreatePoint2d(0, 0), false);
                Sketch.GeometricConstraints.AddCoincident(SketchEntity, (Inventor.SketchEntity)SketchCircle.CenterSketchPoint);
                SketchObjectCollection.Add(SketchCircle);

                Profile = Sketch.Profiles.AddForSolid(true, SketchObjectCollection);
                ExtrudeDefinition = PartDocument.ComponentDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(Profile, Inventor.PartFeatureOperationEnum.kNewBodyOperation);
                ExtrudeDefinition.SetDistanceExtent(Thickness, Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection);

            ExtrudeFeature = PartDocument.ComponentDefinition.Features.ExtrudeFeatures.Add(ExtrudeDefinition);
            ExtrudeFeature.Name = "FlangeBase";
            
                return ExtrudeFeature;
            }
        private Inventor.ObjectCollection CreateHoleSketch(ref Inventor.PartDocument PartDocument,ref Inventor.ExtrudeFeature ExtrudeFeature,string PCD, string PCDAngle)
        {
            Inventor.UnitsOfMeasure UnitsOfMeasure;
            Inventor.PlanarSketch Sketch = null ;
            Inventor.WorkPoint WorkPoint = null;
            Inventor.SketchEntity CenterPointSketchEntity = null;
            Inventor.SketchArc pcdSketchArc = null;
            Inventor.SketchLine AngleSketchLine = null;
            Inventor.SketchLine CenterSketchLine = null;
            Inventor.SketchPoint SketchPoint = null;
            Inventor.TransientGeometry TransientGeometry;
            Inventor.RadiusDimConstraint RadiusDimConstraint = null;
            Inventor.TwoLineAngleDimConstraint TwoLineAngleDimConstraint = null;
            Inventor.ObjectCollection ObjectCollection = null ;
            Inventor.ModelParameter ModelParameter = null;
            ObjectCollection = InvApp.TransientObjects.CreateObjectCollection(); 
            
            double pcdDia = 0, pcdAngle = 0.0;
            UnitsOfMeasure = PartDocument.UnitsOfMeasure;
            pcdDia = UnitsOfMeasure.GetValueFromExpression(PCD, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);

            pcdAngle = UnitsOfMeasure.GetValueFromExpression(PCDAngle, Inventor.UnitsTypeEnum.kDegreeAngleUnits);
            WorkPoint = PartDocument.ComponentDefinition.WorkPoints[1];

            TransientGeometry = InvApp.TransientGeometry;

            foreach (Inventor.Face Face in ExtrudeFeature.EndFaces)
            {
                if (Face.SurfaceType == Inventor.SurfaceTypeEnum.kPlaneSurface )
                {
                    Sketch = PartDocument.ComponentDefinition.Sketches.Add(Face, false    );
                    CenterPointSketchEntity = Sketch.AddByProjectingEntity(WorkPoint);                    
                    CenterSketchLine = Sketch.SketchLines.AddByTwoPoints(TransientGeometry.CreatePoint2d(0, 0), TransientGeometry.CreatePoint2d(0, pcdDia/2));
                    CenterSketchLine.Construction =true ;  
                    AngleSketchLine = Sketch.SketchLines.AddByTwoPoints(CenterSketchLine.StartSketchPoint , TransientGeometry.CreatePoint2d(20, pcdDia / 2));
                    AngleSketchLine .Construction = true;
                    Sketch.GeometricConstraints.AddCoincident(CenterPointSketchEntity, (Inventor.SketchEntity)CenterSketchLine.StartSketchPoint );
                    Sketch.GeometricConstraints.AddEqualLength(CenterSketchLine, AngleSketchLine);
                    pcdSketchArc = Sketch.SketchArcs.AddByCenterStartEndPoint(CenterSketchLine.StartSketchPoint,CenterSketchLine.EndSketchPoint, AngleSketchLine.EndSketchPoint, false );
                    pcdSketchArc.Construction = true;
                    Sketch.GeometricConstraints.AddVertical ((Inventor.SketchEntity)CenterSketchLine,false );
                    Sketch.GeometricConstraints.AddCoincident(CenterPointSketchEntity ,(Inventor.SketchEntity)pcdSketchArc.CenterSketchPoint);
                    SketchPoint = Sketch.SketchPoints.Add(TransientGeometry.CreatePoint2d(0, 0), true);
                    
                    Sketch.GeometricConstraints.AddCoincident((Inventor.SketchEntity)SketchPoint , (Inventor.SketchEntity)AngleSketchLine.EndSketchPoint);  
                    RadiusDimConstraint = Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)pcdSketchArc, pcdSketchArc.Geometry.Center , false);
                    if (RadiusDimConstraint.Parameter.ParameterType ==  Inventor.ParameterTypeEnum.kModelParameter )
                    {
                        
                         
                        ModelParameter =(Inventor.ModelParameter) RadiusDimConstraint.Parameter ;
                        if (ModelParameter.BuiltIn)
                        {

                            ModelParameter.Name = "PCD";
                            ModelParameter.Value = pcdDia / 2;

                        }


                    }


                    TwoLineAngleDimConstraint = Sketch.DimensionConstraints.AddTwoLineAngle(CenterSketchLine, AngleSketchLine, TransientGeometry.CreatePoint2d(1,1), false);
                    if (TwoLineAngleDimConstraint.Parameter.ParameterType == Inventor.ParameterTypeEnum.kModelParameter)
                    {
                        ModelParameter = null;
                        ModelParameter = (Inventor.ModelParameter)TwoLineAngleDimConstraint.Parameter;
                        if (ModelParameter.BuiltIn)
                        {

                            ModelParameter.Name = "Angle";
                            ModelParameter.Value = pcdAngle / 2;

                        }
                    }

                    ObjectCollection.Add(SketchPoint);

                }
                PartDocument.Update();  
            }
            return ObjectCollection;
        }

        private void CreateHoleFeatureFromSketch(ref Inventor.PartDocument PartDocument, ref Inventor.ExtrudeFeature ExtrudeFeature, Inventor.ObjectCollection ObjectCollection, string HoleDia, string HoleNumber)
        {
            Inventor.ObjectCollection HoleObjectCollection = null;
            Inventor.HoleFeature HoleFeature;
            Inventor.SketchHolePlacementDefinition SketchHolePlacementDefinition;
             
            HoleObjectCollection = InvApp.TransientObjects.CreateObjectCollection();
            //HoleFeature.PlacementDefinition  
            SketchHolePlacementDefinition = PartDocument.ComponentDefinition.Features.HoleFeatures.CreateSketchPlacementDefinition(ObjectCollection);
            HoleFeature = PartDocument.ComponentDefinition.Features.HoleFeatures.AddDrilledByThroughAllExtent(SketchHolePlacementDefinition, "18 mm", Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
            HoleObjectCollection.Add(HoleFeature);
            PartDocument.ComponentDefinition.Features.CircularPatternFeatures.Add(HoleObjectCollection, GetPartDocumentAxis(ref PartDocument, "Z"), false, HoleNumber, "360", true, Inventor.PatternComputeTypeEnum.kOptimizedCompute );    

        }
       

        private Inventor.WorkPlane GetPartDocumentWorkPlane(ref Inventor.PartDocument PartDocument, string WorkPlaneAffix)
            {
                Inventor.WorkPlane DefaultWorkPlane = PartDocument.ComponentDefinition.WorkPlanes[3];

                foreach (Inventor.WorkPlane WorkPlane in PartDocument.ComponentDefinition.WorkPlanes)
                {
                    if (WorkPlane.Name == WorkPlaneAffix + " Plane")
                    {
                        return WorkPlane;
                    }
                }
                return DefaultWorkPlane;
            }

        private Inventor.WorkAxis GetPartDocumentAxis(ref Inventor.PartDocument PartDocument, string WorkAxisAffix)
        {
            Inventor.WorkAxis DefaultWorkAxis = PartDocument.ComponentDefinition.WorkAxes[3];

            foreach (Inventor.WorkAxis WorkAxis in PartDocument.ComponentDefinition.WorkAxes )
            {
                if (WorkAxis.Name == WorkAxisAffix + " Axis")
                {
                    return WorkAxis;
                }
            }
            return DefaultWorkAxis;
        }
        private void Changeview()
        {
            Inventor.Camera Camera = InvApp.ActiveView.Camera;
            Camera.Fit();
            Camera.Apply();
        }

        private Inventor.PartDocument CreateNewPartDocument()
            {
                Inventor.PartDocument PartDocument = null;
                PartDocument = (Inventor.PartDocument)InvApp.Documents.Add(Inventor.DocumentTypeEnum.kPartDocumentObject, InvApp.FileManager.GetTemplateFile(Inventor.DocumentTypeEnum.kPartDocumentObject, Inventor.SystemOfMeasureEnum.kMetricSystemOfMeasure), true);
                return PartDocument;
            }


    }
}