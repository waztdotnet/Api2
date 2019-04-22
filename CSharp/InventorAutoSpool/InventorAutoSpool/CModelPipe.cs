using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorAutoSpool
{
    public class CModelPipe
    {
        private string szDiaOD;
        private string szDiaID;
        private string szWallThickness;
        private Inventor.Application InvApp;
        private string szPartFileNameSavePath;

        public CModelPipe(string  DiaOD, string WallThickness)
        {
            szDiaOD = DiaOD;
            szWallThickness = WallThickness;
        }

        public CModelPipe()
        {
            throw new System.NotImplementedException();
        }

        public void SetModelPipeType(string szType, string NominalBore)
        {
            throw new System.NotImplementedException();
        }

        public void ModelMake(ref Inventor.Application InvInterface)
        {
            InvApp = InvInterface;
            Inventor.PartDocument PipeModelPartDocument = null;
            PipeModelPartDocument = (Inventor.PartDocument)InvApp.ActiveDocument ;

            PipeModelPartDocument =  CreatePipeExtrustion(szDiaOD ,szWallThickness );
            Changeview();
            AddiMateDefinitionsToPipe(ref PipeModelPartDocument, szDiaOD,"E","0");
            SavePartFile(ref PipeModelPartDocument); 

        }

        public string GetFileSavePath()
        {
            return szPartFileNameSavePath;
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
                System.IO.File.Delete(szPartFileNameSavePath);
            }
            PartDocument.SaveAs(szPartFileNameSavePath, false);
            PartDocument.Close(true);
            InvApp.SilentOperation = false;

        }


        private Inventor.PartDocument CreatePipeExtrustion(string Dia, string WallThickness)
        {
            Inventor.PartDocument PartDocument = null;
            Inventor.UnitsOfMeasure UnitsOfMeasure;
            Inventor.PlanarSketch Sketch;
            Inventor.TransientGeometry TransientGeometry;
            Inventor.SketchCircle SketchCircle;
            Inventor.WorkPoint WorkPoint;
            Inventor.WorkPlane BaseWorkPlane;
            Inventor.RadiusDimConstraint RadiusDimConstraint = null ;
            Inventor.SketchEntity SketchEntity;
            Inventor.ObjectCollection SketchObjectCollection;
            Inventor.Profile Profile;
            Inventor.ExtrudeDefinition ExtrudeDefinition;
            

            SketchObjectCollection = InvApp.TransientObjects.CreateObjectCollection();
            PartDocument = CreateNewPartDocument();
            UnitsOfMeasure = PartDocument.UnitsOfMeasure;
            double DiaOD = 0, DiaID = 0;
           
            DiaOD = UnitsOfMeasure.GetValueFromExpression(Dia, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
            DiaID = UnitsOfMeasure.GetValueFromExpression(WallThickness, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);
            DiaID = DiaOD - (2 * DiaID);

            TransientGeometry = InvApp.TransientGeometry;
            WorkPoint = PartDocument.ComponentDefinition.WorkPoints[1];
            BaseWorkPlane = GetPartDocumentWorkPlane(ref PartDocument, "YZ");

            Sketch = PartDocument.ComponentDefinition.Sketches.Add(BaseWorkPlane, false);
            SketchEntity = Sketch.AddByProjectingEntity(WorkPoint);
            SketchCircle = Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0,0), DiaOD/2);
            RadiusDimConstraint = Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle, TransientGeometry.CreatePoint2d(0, 0), false);
            Sketch.GeometricConstraints.AddCoincident(SketchEntity, (Inventor.SketchEntity)SketchCircle.CenterSketchPoint);
            SketchObjectCollection.Add(SketchCircle);

            RadiusDimConstraint = null;
            SketchCircle = null;
            SketchCircle = Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0, 0), DiaID / 2);
            RadiusDimConstraint = Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle , TransientGeometry.CreatePoint2d(0, 0),false );
            Sketch.GeometricConstraints.AddCoincident(SketchEntity, (Inventor.SketchEntity)SketchCircle.CenterSketchPoint);
            SketchObjectCollection.Add(SketchCircle);

            Profile = Sketch.Profiles.AddForSolid(true , SketchObjectCollection);
            ExtrudeDefinition = PartDocument.ComponentDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(Profile,Inventor.PartFeatureOperationEnum.kNewBodyOperation);    
            ExtrudeDefinition.SetDistanceExtent(300, Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection );
            PartDocument.ComponentDefinition.Features.ExtrudeFeatures.Add(ExtrudeDefinition);
          
            return PartDocument;    
        }

        private void AddiMateDefinitionsToPipe(ref Inventor.PartDocument PartDocument, string Dia, string MateNamePrefix, string Offset)
        {
            Inventor.ExtrudeFeature ExtrudeFeature;
            Inventor.UnitsOfMeasure UnitsOfMeasure;
            Inventor.Edge Edge = null;
            Inventor.EdgeLoops EdgeLoops;
            double DiaOD = 0;
            int Counter = 0;
            ExtrudeFeature = PartDocument.ComponentDefinition.Features.ExtrudeFeatures[1];
            UnitsOfMeasure = PartDocument.UnitsOfMeasure;
            DiaOD = UnitsOfMeasure.GetValueFromExpression(Dia, Inventor.UnitsTypeEnum.kMillimeterLengthUnits);

            foreach (Inventor.Face Face in ExtrudeFeature.Faces)
            {
                if (Face.SurfaceType == Inventor.SurfaceTypeEnum.kCylinderSurface  )
                {
                    Inventor.Cylinder Cylinder;
                    Cylinder = Face.Geometry;
                    if (Cylinder.Radius == DiaOD / 2)
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
                                        PartDocument.ComponentDefinition.iMateDefinitions.AddInsertiMateDefinition(Edge, false, Offset, null, MateNamePrefix+":"+ Counter.ToString());
                                        Counter++;
                                    }
                                }
                            }
                        }
                    }
                }
            } 

        }

        private Inventor.WorkPlane GetPartDocumentWorkPlane(ref Inventor.PartDocument PartDocument, string WorkPlaneSuffix)
        {
            Inventor.WorkPlane DefaultWorkPlane = PartDocument.ComponentDefinition.WorkPlanes[3];

            foreach (Inventor .WorkPlane WorkPlane in PartDocument.ComponentDefinition.WorkPlanes)
            {
                if (WorkPlane.Name == WorkPlaneSuffix +" Plane" )
                {
                    return WorkPlane;
                }
            }
            return DefaultWorkPlane;
        }



        private Inventor.PartDocument CreateNewPartDocument()
        {
            Inventor.PartDocument PartDocument = null;
            PartDocument = (Inventor.PartDocument)InvApp.Documents.Add(Inventor.DocumentTypeEnum.kPartDocumentObject, InvApp.FileManager.GetTemplateFile(Inventor.DocumentTypeEnum.kPartDocumentObject,Inventor.SystemOfMeasureEnum.kMetricSystemOfMeasure ), true);
            return PartDocument;
        }

        private void Changeview()
        {
            Inventor.Camera Camera = InvApp.ActiveView.Camera;
            Camera.Fit() ;
            Camera.Apply();  
        }


        private class LookUpPipeSize
        {
            private Double dDiaOD;
            private Double dDiaID;
            private Double dWallThickness;

            public LookUpPipeSize(ref Inventor.Application InvApp)
            {
                throw new System.NotImplementedException();
            }

            public String GetRadius()
            {
                throw new System.NotImplementedException();
            }

            public void SetPipeDiaOD(string DiaOD, string WallThickness)
            {
                throw new System.NotImplementedException();
            }

            public void SetStandard()
            {
                throw new System.NotImplementedException();
            }

            public String GetStandard()
            {
                throw new System.NotImplementedException();
            }

            public void SetLength(string Length, string Units)
            {
                throw new System.NotImplementedException();
            }

        }
    }

}


//WorkPlanes = PartDocument.ComponentDefinition.WorkPlanes;
//            Inventor.WorkPlane XZWorkPlane = WorkPlanes[1];
//Inventor.WorkPlane YZWorkPlane = WorkPlanes[2];
//Inventor.WorkPlane XYWorkPlane = WorkPlanes[3];