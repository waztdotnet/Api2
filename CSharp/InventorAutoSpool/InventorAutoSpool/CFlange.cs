using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorAutoSpool
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
		public CFlange(string szDiaOD, string szThickness, string szLength)
		{



		}

		public CFlange(ref Inventor.Application InvApplication,ref Inventor.PartDocument invPartDocument,ref Inventor.UnitsTypeEnum UnitsType,string szworkplanename,string szDiaOD, string szDiaID, string szThickness, string szPCD, string szBoltHoleDia, string szNumberBoltHoles)
		{
			szWorkPlaneName=szworkplanename;
			InvApp = InvApplication;
			PartDocument = invPartDocument;
			CToModelValue.ConvertFlatFacedFlangeDimsToModelWorldUnits(ref PartDocument,ref UnitsType,szDiaOD,szDiaID,szThickness,szPCD,szBoltHoleDia,szNumberBoltHoles,out FlangeDiaOD,out FlangeDiaID,out FlangeThickness,out FlangePlacedCenterDiameter,out FlangeBoltHoleDia,out FlangeNumberBoltHoles);  
		}

		private Inventor.ExtrudeFeature Extrude_FlangeBody(ref Inventor.PartDocument PartDocument) {
			Inventor.TransientGeometry TransientGeometry;
			Inventor.SketchCircle SketchCircle;
			Inventor.RadiusDimConstraint RadiusDimConstraint = null;
			Inventor.Profile SketchProfile;
			Inventor.ExtrudeDefinition ExtrudeDefinition;
			Inventor.ExtrudeFeature ExtrudeFeature = null;

			Inventor.PlanarSketch Sketch;
			Inventor.SketchEntity SketchEntity;
			PartDocument=AddSketchProjectCenterPoint(PartDocument,out Sketch,out SketchEntity);

			TransientGeometry=InvApp.TransientGeometry;
			SketchCircle=Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0,0),FlangeDiaOD/2);
			RadiusDimConstraint=Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle,TransientGeometry.CreatePoint2d(0,0),false);
			Sketch.GeometricConstraints.AddCoincident(SketchEntity,(Inventor.SketchEntity)SketchCircle.CenterSketchPoint);

			RadiusDimConstraint=null;
			SketchCircle=null;
			Inventor.ObjectCollection SketchObjectCollection;
			SketchObjectCollection=InvApp.TransientObjects.CreateObjectCollection();

			SketchCircle=Sketch.SketchCircles.AddByCenterRadius(TransientGeometry.CreatePoint2d(0,0),FlangeDiaID/2);

			RadiusDimConstraint=Sketch.DimensionConstraints.AddRadius((Inventor.SketchEntity)SketchCircle,TransientGeometry.CreatePoint2d(0,0),false);
			Sketch.GeometricConstraints.AddCoincident(SketchEntity,(Inventor.SketchEntity)SketchCircle.CenterSketchPoint);
			SketchObjectCollection.Add(SketchCircle);

			SketchProfile=Sketch.Profiles.AddForSolid(true,SketchObjectCollection);
			ExtrudeDefinition=PartDocument.ComponentDefinition.Features.ExtrudeFeatures.CreateExtrudeDefinition(SketchProfile,Inventor.PartFeatureOperationEnum.kNewBodyOperation);
			ExtrudeDefinition.SetDistanceExtent(FlangeThickness,Inventor.PartFeatureExtentDirectionEnum.kPositiveExtentDirection);

			//Extrude The Sketch Profile
			ExtrudeFeature=PartDocument.ComponentDefinition.Features.ExtrudeFeatures.Add(ExtrudeDefinition);
			ExtrudeFeature.Name="FlangeBody";

			return ExtrudeFeature;
		}

		private Inventor.PartDocument AddSketchProjectCenterPoint(Inventor.PartDocument PartDocument,out Inventor.PlanarSketch Sketch,out Inventor.SketchEntity SketchEntity) {
			Inventor.WorkPlane BaseWorkPlane;
			Inventor.WorkPoint WorkPoint;
			
			WorkPoint=PartDocument.ComponentDefinition.WorkPoints[1];
			BaseWorkPlane=GetPartDocumentWorkPlane(ref PartDocument);

			Sketch=PartDocument.ComponentDefinition.Sketches.Add(BaseWorkPlane,false);
			SketchEntity=Sketch.AddByProjectingEntity(WorkPoint);
			return PartDocument;
		}
		private Inventor.WorkPlane GetPartDocumentWorkPlane(ref Inventor.PartDocument PartDocument) {
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