using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorTestWinApp
{
	public class CFlange
	{
		private Inventor.Application InvApp;
		private Inventor.PartDocument PartDocument;
		private string szPartFileNameSavePath;
		private double FlangeDiaOD;
		private double FlangeDiaID;
		private double FlangeThickness;
		private double FlangePlacedCenterDiameter;
		private double FlangeBoltHoleDia;
		private double FlangeNumberBoltHoles;
		private string szWorkPlaneName;
		private string szWorkAxisName;
		public CFlange(string szDiaOD,string szThickness,string szLength) {



		}

		public CFlange(ref Inventor.Application InvApplication,ref Inventor.PartDocument invPartDocument,ref Inventor.UnitsTypeEnum UnitsType,string szworkplanename,string szDiaOD,string szDiaID,string szThickness,string szPCD,string szBoltHoleDia,string szNumberBoltHoles) 
		{
			szWorkPlaneName=szworkplanename;
			InvApp=InvApplication;
			PartDocument=invPartDocument;
			CToModelValue.ConvertFlatFacedFlangeDimsToModelWorldUnits(ref PartDocument,ref UnitsType,szDiaOD,szDiaID,szThickness,szPCD,szBoltHoleDia,szNumberBoltHoles,out FlangeDiaOD,out FlangeDiaID,out FlangeThickness,out FlangePlacedCenterDiameter,out FlangeBoltHoleDia,out FlangeNumberBoltHoles);
		}

		private void Bolt_HolePatternFeature(ref Inventor.PartDocument PartDocument,ref Inventor.ExtrudeFeature ExtrudeFeature,Inventor.ObjectCollection ObjectCollection,string HoleDia,string HoleNumber) {
			Inventor.ObjectCollection HoleObjectCollection = null;
			Inventor.HoleFeature HoleFeature;
			Inventor.SketchHolePlacementDefinition SketchHolePlacementDefinition;

			HoleObjectCollection=InvApp.TransientObjects.CreateObjectCollection();
			SketchHolePlacementDefinition=PartDocument.ComponentDefinition.Features.HoleFeatures.CreateSketchPlacementDefinition(ObjectCollection);
			HoleFeature=PartDocument.ComponentDefinition.Features.HoleFeatures.AddDrilledByThroughAllExtent(SketchHolePlacementDefinition,FlangeBoltHoleDia,Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
			HoleObjectCollection.Add(HoleFeature);
			PartDocument.ComponentDefinition.Features.CircularPatternFeatures.Add(HoleObjectCollection,GetWorkAxis(ref PartDocument,"Z"),false,FlangeNumberBoltHoles,"360",true,Inventor.PatternComputeTypeEnum.kOptimizedCompute);

		}

		private Inventor.WorkAxis GetWorkAxis(ref Inventor.PartDocument PartDocument,string WorkAxisAffix) {
			Inventor.WorkAxis DefaultWorkAxis = PartDocument.ComponentDefinition.WorkAxes[3];

			foreach(Inventor.WorkAxis WorkAxis in PartDocument.ComponentDefinition.WorkAxes) {
				if(WorkAxis.Name==WorkAxisAffix+" Axis") {
					return WorkAxis;
				}
			}
			return DefaultWorkAxis;
		}

		public Inventor.ExtrudeFeature Extrude_FlangeBody() {
			Inventor.TransientGeometry TransientGeometry;
			Inventor.SketchCircle SketchCircle;
			Inventor.RadiusDimConstraint RadiusDiamension = null;
			Inventor.Profile SketchProfile;
			Inventor.ExtrudeDefinition ExtrudeDefinition;
			Inventor.ExtrudeFeature ExtrudeFeature = null;
			Inventor.PlanarSketch Sketch;
			Inventor.SketchEntity SketchEntity;
			//Create Sketch on wp project 0,0,0 work point Geometry   
			PartDocument=AddSketchProjectCenterPoint(PartDocument,out Sketch,out SketchEntity);
			//Get the Transient Geometry Object from Inventor
			TransientGeometry=InvApp.TransientGeometry;
			//Draw Outside Hole 
			DrawCircle_Constrain_Diamension(TransientGeometry.CreatePoint2d(0,0),out SketchCircle,out RadiusDiamension,Sketch,SketchEntity,FlangeDiaOD/2);

			RadiusDiamension=null; // reset for next use
			SketchCircle=null;
			Inventor.ObjectCollection SketchObjectCollection;
			SketchObjectCollection=InvApp.TransientObjects.CreateObjectCollection();
			//Draw Inside Hole Add to sketch objects 
			DrawCircle_Constrain_Diamension(TransientGeometry.CreatePoint2d(0,0),out SketchCircle,out RadiusDiamension,Sketch,SketchEntity,FlangeDiaID/2);
			//Add to sketch Collection ready for Extrude  
			SketchObjectCollection.Add(SketchCircle);
			//Convert to profileing
			SketchProfile=Sketch.Profiles.AddForSolid(true,SketchObjectCollection);
			ExtrudeDefinition=PartDocument.ComponentDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(SketchProfile,Inventor.PartFeatureOperationEnum.kNewBodyOperation);
			ExtrudeDefinition.SetDistanceExtent(FlangeThickness,Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection);

			//Extrude The Sketch Profile
			ExtrudeFeature=PartDocument.ComponentDefinition.Features.ExtrudeFeatures.Add(ExtrudeDefinition);
			ExtrudeFeature.Name="FlangeBody";

			return ExtrudeFeature;
		}

		private void DrawCircle_Constrain_Diamension(Inventor.Point2d CenterPoint,out Inventor.SketchCircle SketchCircle,out Inventor.RadiusDimConstraint RadiusDimConstraint,Inventor.PlanarSketch Sketch,Inventor.SketchEntity SketchEntity,double Radius) {
			//Draw
			SketchCircle=Sketch.SketchCircles.AddByCenterRadius(CenterPoint ,Radius);
			//Dim
			RadiusDimConstraint=Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle,CenterPoint ,false);
			//Anchor 
			Sketch.GeometricConstraints.AddCoincident(SketchEntity,(Inventor.SketchEntity)SketchCircle.CenterSketchPoint);
		}

		private Inventor.PartDocument AddSketchProjectCenterPoint(Inventor.PartDocument PartDocument,out Inventor.PlanarSketch Sketch,out Inventor.SketchEntity SketchEntity) {
			Inventor.WorkPlane BaseWorkPlane;
			Inventor.WorkPoint WorkPoint;

			WorkPoint=PartDocument.ComponentDefinition.WorkPoints[1];
			BaseWorkPlane=GetWorkPlane(ref PartDocument);

			Sketch=PartDocument.ComponentDefinition.Sketches.Add(BaseWorkPlane,false);
			SketchEntity=Sketch.AddByProjectingEntity(WorkPoint);
			return PartDocument;
		}

		private Inventor.WorkPlane GetWorkPlane(ref Inventor.PartDocument PartDocument) {
			Inventor.WorkPlane DefaultWorkPlane = PartDocument.ComponentDefinition.WorkPlanes[3];

			foreach(Inventor.WorkPlane WorkPlane in PartDocument.ComponentDefinition.WorkPlanes) {
				if(WorkPlane.Name==szWorkPlaneName+" Plane") {
					return WorkPlane;
				}
			}
			return DefaultWorkPlane;
		}

	}
}