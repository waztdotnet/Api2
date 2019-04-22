using System;
using System.Linq;

namespace WindowsDrawApp
{
    public class CDrawingView
    {
        private Inventor.Application mInvApplication = null;

        public CDrawingView(ref Inventor.Application invApplication)
        {
            mInvApplication = invApplication;
        }

        public CDrawingView()
        {
        }

        public Inventor.Camera GetDocument(ref Inventor._Document Document, ref Inventor.Application invApplication)
        {
            Inventor.PartDocument PartDocument = null;
            Inventor.Camera Camera = null;
            mInvApplication = invApplication;
            if (Document.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                PartDocument = (Inventor.PartDocument)Document;

                if (PartDocument != null)
                {
                    if (PartDocument.ComponentDefinitions.Count > 0)
                    {
                        Camera = mInvApplication.ActiveView.Camera;
                        Inventor.PartComponentDefinition PartComponentDefinition = PartDocument.ComponentDefinitions[1];

                        foreach (Inventor.SurfaceBody SurfaceBody in PartComponentDefinition.SurfaceBodies)
                        {
                            GetFaceToView(SurfaceBody, ref PartDocument, ref Camera);
                        }
                        return Camera;
                    }
                }
            }
            return Camera;
        }

        private void GetFaceToView(Inventor.SurfaceBody mSurfaceBody, ref Inventor.PartDocument PartDocument, ref Inventor.Camera Camera)
        {
            double Max = mSurfaceBody.Faces[1].Evaluator.Area * 100;
            double currentValue = 0;

            GetExtents(mSurfaceBody);

            Inventor.Face TargetFace = null;
            foreach (Inventor.Face Face in mSurfaceBody.Faces)
            {
                currentValue = Face.Evaluator.Area * 100;

                if (currentValue >= Max)
                {
                    Max = Face.Evaluator.Area * 100;
                    TargetFace = Face;
                }
            }
            SetDrawingView(mSurfaceBody, ref Camera, TargetFace);
        }

        public Inventor.Face GetBiggestFace(Inventor.SurfaceBody mSurfaceBody)
        {
            double Max = mSurfaceBody.Faces[1].Evaluator.Area * 100;
            double currentValue = 0;

            Inventor.Face TargetFace = null;
            foreach (Inventor.Face Face in mSurfaceBody.Faces)
            {
                currentValue = Face.Evaluator.Area * 100;

                if (currentValue > Max)
                {
                    Max = Face.Evaluator.Area * 100;
                    TargetFace = Face;
                }
            }

            if (TargetFace != null)
            {
                return TargetFace;
            }
            else
                return null;
        }

        private void SetDrawingView(Inventor.SurfaceBody mSurfaceBody, ref Inventor.Camera Camera, Inventor.Face TargetFace)
        {
            if (TargetFace != null)
            {
                if (!TargetFace.IsParamReversed)
                {
                    if (TargetFace.Geometry is Inventor.Plane)
                    {
                        //Inventor.Face TargetFace = Face;
                        Inventor.Point FacePoint = TargetFace.PointOnFace;
                        Inventor.UnitVector Normal = GetFaceNormal(ref TargetFace, ref FacePoint);
                        Inventor.UnitVector UplDirection = mInvApplication.TransientGeometry.CreateUnitVector(0, 1, 0);
                        Inventor.UnitVector Z_PosDirection = mInvApplication.TransientGeometry.CreateUnitVector(0, 0, 1);
                        Inventor.UnitVector Z_NegDirection = mInvApplication.TransientGeometry.CreateUnitVector(0, 0, -1);
                        Inventor.UnitVector X_PosDirection = mInvApplication.TransientGeometry.CreateUnitVector(1, 0, 0);
                        Inventor.UnitVector Y_PosDirection = mInvApplication.TransientGeometry.CreateUnitVector(0, 1, 0);
                        Inventor.UnitVector X_NegDirection = mInvApplication.TransientGeometry.CreateUnitVector(-1, 0, 0);
                        Inventor.UnitVector Y_NegDirection = mInvApplication.TransientGeometry.CreateUnitVector(0, -1, 0);
                        if (Z_PosDirection.IsEqualTo(Normal))
                        {
                            if (GetExtents(mSurfaceBody) == "X")
                            {
                                UplDirection = Y_NegDirection;
                            }
                            else if (GetExtents(mSurfaceBody) == "Y")
                            {
                                UplDirection = X_NegDirection;
                            }
                            else if (GetExtents(mSurfaceBody) == "Z")
                            {
                                UplDirection = GetXDir(ref TargetFace, ref FacePoint);
                            }
                            FacePoint = Camera.Eye.Copy();
                            FacePoint.TranslateBy(GetMidPointAtFaceNormal(ref TargetFace).AsVector());
                            Camera.Target = FacePoint;

                            Camera.UpVector = UplDirection;

                            Camera.Fit();
                            Camera.Apply();
                        }
                        else if (Z_NegDirection.IsEqualTo(Normal))
                        {
                            if (GetExtents(mSurfaceBody) == "X")
                            {
                                UplDirection = Y_PosDirection;
                            }
                            else if (GetExtents(mSurfaceBody) == "Y")
                            {
                                UplDirection = X_PosDirection;
                            }
                            else if (GetExtents(mSurfaceBody) == "Z")
                            {
                                UplDirection = GetXDir(ref TargetFace, ref FacePoint);
                            }

                            FacePoint = Camera.Eye.Copy();
                            FacePoint.TranslateBy(GetMidPointAtFaceNormal(ref TargetFace).AsVector());
                            Camera.Target = FacePoint;

                            Camera.UpVector = UplDirection;

                            Camera.Fit();
                            Camera.Apply();
                        }
                    }
                }
            }
        }

        private string GetExtents(Inventor.SurfaceBody SurfaceBody)
        {
            string VectorDirection = "X";

            double[] Lengths = new double[3];

            Inventor.Box ExtentsBox = SurfaceBody.RangeBox;

            Lengths[0] = Math.Abs(ExtentsBox.MaxPoint.X) + Math.Abs(ExtentsBox.MinPoint.X);
            Lengths[1] = Math.Abs(ExtentsBox.MaxPoint.Y) + Math.Abs(ExtentsBox.MinPoint.Y);
            Lengths[2] = Math.Abs(ExtentsBox.MaxPoint.Z) + Math.Abs(ExtentsBox.MinPoint.Z);
            int Mx = Array.IndexOf(Lengths, Lengths.Max());
            int Mn = Array.IndexOf(Lengths, Lengths.Min());
            //  RangeMid = GetMedian(Lengths);
            // int Md = Array.IndexOf(Lengths, RangeMid);
            if (Mx == 0)
            {
                VectorDirection = "X";
            }
            if (Mx == 1)
            {
                VectorDirection = "Y";
            }
            if (Mx == 2)
            {
                VectorDirection = "Z";
            }
            return VectorDirection;
        }

        private void SetCameraView(Inventor.Camera Camera, Inventor.Face Face)
        {
        }

        private Inventor.UnitVector GetFaceNormal(ref Inventor.Face Face, ref Inventor.Point Point)
        {
            Inventor.UnitVector UnitVector;
            Inventor.SurfaceEvaluator SurfaceEvaluator = Face.Evaluator;
            double[] Points = new double[3];
            double[] GuessParams = new double[2];
            double[] MaxDeviations = new double[2];
            double[] Params = new double[2];
            double[] normal = new double[2];
            Inventor.SolutionNatureEnum[] NatureEnum = new Inventor.SolutionNatureEnum[2];

            Points[0] = Point.X; Points[1] = Point.Y; Points[2] = Point.Z;
            SurfaceEvaluator.GetParamAtPoint(ref Points, ref GuessParams, ref MaxDeviations, ref Params, ref NatureEnum);
            SurfaceEvaluator.GetNormal(ref Params, ref normal);
            UnitVector = mInvApplication.TransientGeometry.CreateUnitVector(normal[0], normal[1], normal[2]);
            return UnitVector;
        }

        private Inventor.UnitVector GetXDir(ref Inventor.Face Face, ref Inventor.Point Point)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = Face.Evaluator;
            Inventor.UnitVector UnitVector = null;
            double[] Points = new double[3];
            double[] GuessParam = new double[2];
            double[] maxDeviations = new double[2];
            double[] Params = new double[2];
            double[] uTangents = new double[3];
            double[] vTangents = new double[3];

            Inventor.SolutionNatureEnum[] NatureEnum = new Inventor.SolutionNatureEnum[5];

            Points[0] = Point.X;
            Points[1] = Point.Y;
            Points[2] = Point.Z;

            SurfaceEvaluator.GetParamAtPoint(ref Points, ref GuessParam, ref maxDeviations, ref Params, ref NatureEnum);

            SurfaceEvaluator.GetTangents(ref Params, ref uTangents, ref vTangents);
            UnitVector = mInvApplication.TransientGeometry.CreateUnitVector(uTangents[0], uTangents[1], uTangents[2]);
            return UnitVector;
        }

        private Inventor.UnitVector GetMidPointAtFaceNormal(ref Inventor.Face Face, ref Inventor.Point Point)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = Face.Evaluator;
            double[] CenterPoint = new double[2];
            double[] CtrPoint = new double[3];
            CenterPoint[0] = (SurfaceEvaluator.ParamRangeRect.MinPoint.X + SurfaceEvaluator.ParamRangeRect.MaxPoint.X) / 2;
            CenterPoint[1] = (SurfaceEvaluator.ParamRangeRect.MinPoint.Y + SurfaceEvaluator.ParamRangeRect.MaxPoint.Y) / 2;

            double[] normal = new double[3];
            SurfaceEvaluator.GetNormal(ref CenterPoint, ref normal);
            SurfaceEvaluator.GetPointAtParam(ref CenterPoint, ref CtrPoint);
            Point = mInvApplication.TransientGeometry.CreatePoint(CtrPoint[0], CtrPoint[1], CtrPoint[2]);
            return mInvApplication.TransientGeometry.CreateUnitVector(normal[0], normal[1], normal[2]);
        }

        private Inventor.UnitVector GetMidPointAtFaceNormal(ref Inventor.Face Face)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = Face.Evaluator;
            double[] CenterPoint = new double[2];
            CenterPoint[0] = (SurfaceEvaluator.ParamRangeRect.MinPoint.X + SurfaceEvaluator.ParamRangeRect.MaxPoint.X) / 2;
            CenterPoint[1] = (SurfaceEvaluator.ParamRangeRect.MinPoint.Y + SurfaceEvaluator.ParamRangeRect.MaxPoint.Y) / 2;

            double[] normal = new double[2];
            SurfaceEvaluator.GetNormal(ref CenterPoint, ref normal);

            return mInvApplication.TransientGeometry.CreateUnitVector(normal[0], normal[1], normal[2]);
        }
    }
}