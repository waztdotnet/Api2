using System.Collections;
using System.Collections.Generic;

namespace WindowsDrawApp
{
    internal class CModelViews
    {
        private Inventor.Application mInvApplication = null;

        public CModelViews(ref Inventor.Application InvApplication)
        {
            mInvApplication = InvApplication;
        }

        public void ProcessFileRefs(Inventor.File File)
        {
            foreach (Inventor.FileDescriptor DescriptedFile in File.ReferencedFileDescriptors)
            {
                if (!DescriptedFile.ReferenceMissing)
                {
                    if (DescriptedFile.ReferencedFileType != Inventor.FileTypeEnum.kForeignFileType)
                    {
                        if (DescriptedFile.ReferencedFileType != Inventor.FileTypeEnum.kUnknownFileType)
                        {
                            Inventor.Document Document = mInvApplication.Documents.Open(DescriptedFile.FullFileName, true);
                            SetModelView(Document);

                            Document.Close(false);
                            //  ProcessFileRefs(DescriptedFile.ReferencedFile);
                        }
                    }
                }
            }
        }

        public void SetSingelView(ref Inventor.Document mDocument)
        {
            SetModelView(mDocument);
        }

        private void SetModelView(Inventor.Document mDocument)
        {
            Inventor.SurfaceBody mSurfaceBody;
            if (mDocument.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                Inventor.PartDocument mPartDocument = (Inventor.PartDocument)mDocument;
                if (mPartDocument.DocumentSubType.DocumentSubTypeID != "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                {
                    Inventor.ComponentDefinition mComponentDefinition;
                    mComponentDefinition = (Inventor.ComponentDefinition)mPartDocument.ComponentDefinition;
                    mSurfaceBody = mComponentDefinition.SurfaceBodies[1];
                    GetFaceToSetView(mSurfaceBody, mPartDocument);
                    mPartDocument.Save2(false);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void GetFaceToSetView(Inventor.SurfaceBody mSurfaceBody, Inventor.PartDocument PartDocument)
        {
            Inventor.Face WorkFace = null;

            System.Collections.ArrayList Areas = new ArrayList();
            Dictionary<double, object> AreaValue = new Dictionary<double, object>();
            double Max = mSurfaceBody.Faces[1].Evaluator.Area * 100;
            double currentValue = 0;
            WorkFace = mSurfaceBody.Faces[1];

            Inventor.UnitVector zVector = mInvApplication.TransientGeometry.CreateUnitVector(0, 0, 1);
            ArrayList SortArray = new ArrayList();
            ArrayList SortArray2 = new ArrayList();
            Inventor.Plane Plane;

            for (int i = 1; i < mSurfaceBody.Faces.Count + 1; i++)
            {
                currentValue = mSurfaceBody.Faces[i].Evaluator.Area * 100;

                //SortArray.Add(currentValue);
                if (!mSurfaceBody.Faces[i].IsParamReversed)
                {
                    if (currentValue >= Max)
                    {
                        Inventor.Face fc = mSurfaceBody.Faces[i];
                        Inventor.UnitVector vect = PartDocument.ComponentDefinition.WorkPlanes["XY Plane"].Plane.Normal;
                        if (fc.Geometry is Inventor.Plane)
                        {
                            Plane = fc.Geometry;
                            Inventor.UnitVector pVect = GetMidPointAtFaceNormal(ref fc);
                            Max = mSurfaceBody.Faces[i].Evaluator.Area * 100;

                            if (pVect.IsParallelTo(vect, 0.00001))
                            {
                                if (Plane.Normal.DotProduct(vect) > 0)
                                {
                                    SortArray2.Add(currentValue);
                                    WorkFace = mSurfaceBody.Faces[i];
                                }
                            }
                        }
                    }
                }
            }

            if (WorkFace != null)
            {
                SetViewDirection(WorkFace);
            }
            else
            { return; }
        }

        private void SetViewDirectionByRange(Inventor.Face FaceView)
        {
            Inventor.SurfaceBody mSurfaceBody;
            mSurfaceBody = FaceView.SurfaceBody;
            Inventor.Box mRangeBox;
            double Length;
            double Width;
            double Thickness;
            double[] SortArray = new double[3];
            mRangeBox = mSurfaceBody.RangeBox;
            mRangeBox.Extend(mSurfaceBody.RangeBox.MinPoint);
            mRangeBox.Extend(mSurfaceBody.RangeBox.MaxPoint);
            GetFaceSize(mSurfaceBody);
            //  works only if alained to ucs
            if (mRangeBox != null)
            {
                Length = mRangeBox.MaxPoint.X - mRangeBox.MinPoint.X;
                SortArray[0] = Length;
                Width = mRangeBox.MaxPoint.Y - mRangeBox.MinPoint.Y;
                SortArray[1] = Width;
                Thickness = mRangeBox.MaxPoint.Z - mRangeBox.MinPoint.Z;
                SortArray[2] = Thickness;
            }
        }

        private void SetViewDirection(Inventor.Face FaceView)
        {
            Inventor.Camera Camera = mInvApplication.ActiveView.Camera;
            Inventor.Point Pnt;
            Inventor.View PlaneView = mInvApplication.ActiveView;
            //  ' Set Eye or Target
            if (FaceView.IsParamReversed)
            {
                Pnt = Camera.Eye.Copy();
                Pnt.TranslateBy(GetMidPointAtFaceNormal(ref FaceView).AsVector());
                Camera.Target = Pnt;
            }
            else
            {
                Pnt = Camera.Target.Copy();
                Pnt.TranslateBy(GetMidPointAtFaceNormal(ref FaceView).AsVector());
                Camera.Eye = Pnt;
            }

            Camera.UpVector = GetLargestEdgeUnitVector(ref FaceView);
            Camera.Fit();
            Camera.Apply();
            PlaneView.SetCurrentAsTop();
            PlaneView.SetCurrentAsHome(true);
            PlaneView.Update();
        }

        private Inventor.UnitVector GetMidPointAtFaceNormal(ref Inventor.Face ViewFace)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = ViewFace.Evaluator;
            double[] CenterPoint = new double[2];
            CenterPoint[0] = (SurfaceEvaluator.ParamRangeRect.MinPoint.X + SurfaceEvaluator.ParamRangeRect.MaxPoint.X) / 2;
            CenterPoint[1] = (SurfaceEvaluator.ParamRangeRect.MinPoint.Y + SurfaceEvaluator.ParamRangeRect.MaxPoint.Y) / 2;

            double[] normal = new double[2];
            SurfaceEvaluator.GetNormal(ref CenterPoint, ref normal);

            return mInvApplication.TransientGeometry.CreateUnitVector(normal[0], normal[1], normal[2]);
        }

        private Inventor.UnitVector GetLargestEdgeUnitVector(ref Inventor.Face FaceView)
        {
            int length = FaceView.Edges.Count;
            double LengthParam = 0;
            Inventor.LineSegment Segment = null;

            Inventor.Edge Edge = FaceView.Edges[1];
            Edge.Evaluator.GetParamExtents(out double MinParam, out double MaxParam);
            Edge.Evaluator.GetLengthAtParam(MinParam, MaxParam, out double Max);
            Inventor.UnitVector UnitVector = mInvApplication.TransientGeometry.CreateUnitVector(0, 0, 0);
            Inventor.UnitVector UnitVectorY = mInvApplication.TransientGeometry.CreateUnitVector(0, 1, 0);
            Inventor.UnitVector UnitVectorX = mInvApplication.TransientGeometry.CreateUnitVector(1, 0, 0);

            for (int i = 1; i <= length; i++)
            {
                Edge = FaceView.Edges[i];
                if (Edge.GeometryType == Inventor.CurveTypeEnum.kLineSegmentCurve)
                {
                    Segment = Edge.Geometry;
                }
                if (Edge.GeometryType == Inventor.CurveTypeEnum.kLineSegmentCurve)
                {
                    Edge.Evaluator.GetParamExtents(out MinParam, out MaxParam);
                    Edge.Evaluator.GetLengthAtParam(MinParam, MaxParam, out LengthParam);

                    if (LengthParam > Max)
                    {
                        Segment = Edge.Geometry;
                        Max = LengthParam;
                    }
                }
            }

            if (Segment != null)
            {
                if (Edge.GeometryType == Inventor.CurveTypeEnum.kLineSegmentCurve)
                {
                    if (Segment.Direction.IsParallelTo(UnitVectorY))
                    {
                        UnitVector = UnitVectorX;
                    }
                    else if (Segment.Direction.IsParallelTo(UnitVectorX))
                    {
                        UnitVector = UnitVectorY;
                    }
                    return UnitVector;
                }
            }
            return UnitVector;
        }

        //Used By DXF
        public Inventor.Face GetFaceLargestArea(Inventor.SurfaceBody mSurfaceBody)
        {
            Inventor.Face WorkFace = null;
            System.Collections.ArrayList Areas = new ArrayList();
            Dictionary<double, object> AreaValue = new Dictionary<double, object>();
            double Max = mSurfaceBody.Faces[1].Evaluator.Area * 100;
            double currentValue = 0;

            WorkFace = mSurfaceBody.Faces[1];

            Inventor.UnitVector zVector = mInvApplication.TransientGeometry.CreateUnitVector(0, 0, 1);
            for (int i = 1; i < mSurfaceBody.Faces.Count + 1; i++)
            {
                currentValue = mSurfaceBody.Faces[i].Evaluator.Area * 100;

                AreaValue.Add(currentValue, mSurfaceBody.Faces[i]);
                if (currentValue > Max)
                {
                    Max = mSurfaceBody.Faces[i].Evaluator.Area * 100;
                    WorkFace = mSurfaceBody.Faces[i];
                }
            }

            if (WorkFace != null)
            {
                return WorkFace;
            }
            else
            { return null; }
        }

        private void GetBodyExtents(ref Inventor.PartDocument PartDocument)
        {
            Inventor.ComponentDefinition mComponentDefinition;
            Inventor.Box mRangeBox;
            double Length;
            double Width;
            double Thickness;
            double[] SortArray = new double[3];

            mComponentDefinition = (Inventor.ComponentDefinition)PartDocument.ComponentDefinition;

            foreach (Inventor.SurfaceBody mSurfaceBody in mComponentDefinition.SurfaceBodies)
            {
                mRangeBox = mSurfaceBody.RangeBox;
                mRangeBox.Extend(mSurfaceBody.RangeBox.MinPoint);
                mRangeBox.Extend(mSurfaceBody.RangeBox.MaxPoint);
                GetFaceSize(mSurfaceBody);
                //works only if alained to ucs
                if (mRangeBox != null)
                {
                    Length = mRangeBox.MaxPoint.X - mRangeBox.MinPoint.X;
                    SortArray[0] = Length;
                    Width = mRangeBox.MaxPoint.Y - mRangeBox.MinPoint.Y;
                    SortArray[1] = Width;
                    Thickness = mRangeBox.MaxPoint.Z - mRangeBox.MinPoint.Z;
                    SortArray[2] = Thickness;
                }
            }

            //            Sub GetExtents()
            //    Dim doc As Document
            //    Set doc = ThisApplication.ActiveDocument

            //    Dim cd As ComponentDefinition
            //    Set cd = doc.ComponentDefinition

            //    Dim ext As Box

            //    Dim sb As SurfaceBody
            //    For Each sb In cd.SurfaceBodies
            //        If ext Is Nothing Then
            //            Set ext = sb.RangeBox.Copy
            //        Else
            //            ext.Extend sb.RangeBox.MinPoint
            //            ext.Extend sb.RangeBox.MaxPoint
            //        End If
            //    Next

            //    MsgBox "Extensions are: " + vbCr + _
            //        "X " + CStr(ext.MaxPoint.x - ext.MinPoint.x) + vbCr + _
            //        "Y " + CStr(ext.MaxPoint.y - ext.MinPoint.y) + vbCr + _
            //        "Z " + CStr(ext.MaxPoint.Z - ext.MinPoint.Z)
            //End Sub
        }

        private void GetFaceSize(Inventor.SurfaceBody mSurfaceBody)
        {
            ArrayList Area = new ArrayList();

            foreach (Inventor.Face Face in mSurfaceBody.Faces)
            {
                Area.Add(Face.Evaluator.Area * 100);
            }

            Area.Reverse();

            foreach (double x in Area)
            {
                //lstNames.Items.Add(x.ToString() + " ...Area");
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////////////
        // Developing `

        private Inventor.UnitVector GetFaceNormalAtPoint(ref Inventor.Face ViewFace, Inventor.Point point)
        {
            Inventor.TransientGeometry TransientGeometry = mInvApplication.TransientGeometry;
            double[] Pnt = new double[0];
            double[] n = new double[3];
            Pnt[0] = point.X; Pnt[1] = point.X; Pnt[2] = point.X;
            ViewFace.Evaluator.GetNormalAtPoint(ref Pnt, ref n);
            return TransientGeometry.CreateUnitVector(n[0], n[1], n[2]);
        }

        private double GetEdgeMidParam(Inventor.CurveEvaluator Evaluate)
        {
            // double Max = 0;
            Evaluate.GetParamExtents(out double MinParam, out double MaxParam);
            return MinParam + (MaxParam - MinParam) / 2;
        }

        private Inventor.Point GetEdgePointAtParam(Inventor.EdgeUse EdgeUse, double Pt)
        {
            Inventor.TransientGeometry TransientGeometry = mInvApplication.TransientGeometry;
            Inventor.CurveEvaluator CurveEvaluator;
            double[] Param = new double[0];
            double[] Pnt = new double[3];
            Param[0] = Pt;
            CurveEvaluator = EdgeUse.Edge.Evaluator;
            CurveEvaluator.GetPointAtParam(ref Param, ref Pnt);

            return TransientGeometry.CreatePoint(Pnt[0], Pnt[1], Pnt[2]);
        }

        private Inventor.UnitVector GetTangentAtParam(Inventor.EdgeUse EdgeUse, double Pt)
        {
            Inventor.TransientGeometry TransientGeometry = mInvApplication.TransientGeometry;
            Inventor.CurveEvaluator CurveEvaluator;
            double[] Param = new double[0];
            double[] v = new double[3];
            Param[0] = Pt;
            CurveEvaluator = EdgeUse.Edge.Evaluator;
            CurveEvaluator.GetPointAtParam(ref Param, ref v);
            if (EdgeUse.IsOpposedToEdge)
            {
                return TransientGeometry.CreateUnitVector(-v[0], -v[1], -v[2]);
            }
            else
            {
                return TransientGeometry.CreateUnitVector(v[0], v[1], v[2]);
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////////////
        // Developing
        private void SetDocumentWorkPlanes(ref Inventor.ComponentOccurrence Occurrence, bool VisableState)
        {
            Inventor.Document Document = Occurrence.Definition.Document;

            if (Occurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                Inventor.PartDocument PartDocument = (Inventor.PartDocument)Document;
                foreach (Inventor.WorkPlane WP in PartDocument.ComponentDefinition.WorkPlanes)
                {
                    if (WP.Visible != VisableState)
                    {
                        WP.Visible = VisableState;
                    }
                }
            }
            else if (Occurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)Document;
                foreach (Inventor.WorkPlane WP in AssemblyDocument.ComponentDefinition.WorkPlanes)
                {
                    if (WP.Visible != VisableState)
                    {
                        WP.Visible = VisableState;
                    }
                }
            }
        }

        private Inventor.LineSegment GetLargestEdge(ref Inventor.Face FaceView)
        {
            int length = FaceView.Edges.Count;
            double LengthParam = 0;
            Inventor.LineSegment Segment = null;
            // Inventor.Curve2dEvaluator Curve2dEvaluator;

            Inventor.Edge Edge = FaceView.Edges[1];
            Edge.Evaluator.GetParamExtents(out double MinParam, out double MaxParam);
            Edge.Evaluator.GetLengthAtParam(MinParam, MaxParam, out double Max);
            Inventor.UnitVector Uy = mInvApplication.TransientGeometry.CreateUnitVector(0, 1, 0);
            for (int i = 1; i <= length; i++)
            {
                Edge = FaceView.Edges[i];
                Edge.Evaluator.GetParamExtents(out MinParam, out MaxParam);
                Edge.Evaluator.GetLengthAtParam(MinParam, MaxParam, out LengthParam);

                if (LengthParam >= Max)
                {
                    if (Edge.GeometryType == Inventor.CurveTypeEnum.kLineSegmentCurve)
                    {
                        Segment = Edge.Geometry;
                        if (Segment.Direction.IsPerpendicularTo(Uy))
                        {
                            //  MessageBox.Show("");
                        }
                    }
                }
            }
            return Segment;
        }

        private void GetEdgeData(Inventor.Edge Edge)
        {
            Inventor.Curve2dEvaluator Curve2dEvaluator;
            double MinParam = 0;
            double MaxParam = 0;
            // double LengthParam = 0;
            double[] StartParams = new double[3];
            double[] EndParams = new double[3];
            double[] mPoints = new double[3];
            foreach (Inventor.EdgeUse EdgeUse in Edge.EdgeUses)
            {
                StartParams[0] = MinParam;
                StartParams[1] = 0;
                StartParams[2] = 0;
                EndParams[0] = MinParam;
                EndParams[1] = 0;
                EndParams[2] = 0;
                Curve2dEvaluator = EdgeUse.Evaluator;
                Curve2dEvaluator.GetParamExtents(out MinParam, out MaxParam);

                Curve2dEvaluator.GetPointAtParam(ref StartParams, ref mPoints);
            }
        }

        private void GetFaceDirction(Inventor.SurfaceBody mSurfaceBody)
        {
            foreach (Inventor.Face face in mSurfaceBody.Faces)
            {
                if (face.SurfaceType == Inventor.SurfaceTypeEnum.kPlaneSurface)
                {
                    Inventor.UnitVector YDir;

                    Inventor.Point FacePoint = face.PointOnFace;
                    Inventor.UnitVector normal = GetFaceNormalPoint(face, FacePoint);
                    Inventor.UnitVector XDir = GetXDir(face, FacePoint);
                    YDir = normal.CrossProduct(XDir);
                }
            }
        }

        private Inventor.UnitVector GetXDir(Inventor.Face ViewFace, Inventor.Point Point)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = ViewFace.Evaluator;
            Inventor.UnitVector UnitVector = null;
            double[] Points = new double[3];
            double[] GuessParam = new double[2];
            double[] maxDeviations = new double[2];
            double[] Params = new double[2];
            double[] uTangents = new double[3];
            double[] vTangents = new double[2];

            Inventor.SolutionNatureEnum[] NatureEnum = new Inventor.SolutionNatureEnum[1];

            Points[0] = Point.X;
            Points[1] = Point.Y;
            Points[2] = Point.Z;

            SurfaceEvaluator.GetParamAtPoint(Points, GuessParam, maxDeviations, Params, NatureEnum);

            SurfaceEvaluator.GetTangents(Params, uTangents, vTangents);
            UnitVector = mInvApplication.TransientGeometry.CreateUnitVector(uTangents[0], uTangents[1], uTangents[2]);
            return UnitVector;
        }

        private Inventor.UnitVector GetFaceNormalPoint(Inventor.Face ViewFace, Inventor.Point Point)
        {
            Inventor.SurfaceEvaluator SurfaceEvaluator;
            SurfaceEvaluator = ViewFace.Evaluator;
            Inventor.UnitVector UnitVector = null;
            double[] Points = new double[3];
            double[] GuessParam = new double[2];
            double[] maxDeviations = new double[2];
            double[] Params = new double[2];

            Inventor.SolutionNatureEnum[] NatureEnum = new Inventor.SolutionNatureEnum[1];

            Points[0] = Point.X;
            Points[1] = Point.Y;
            Points[2] = Point.Z;

            SurfaceEvaluator.GetParamAtPoint(Points, GuessParam, maxDeviations, Params, NatureEnum);

            double[] normal = new double[3];
            SurfaceEvaluator.GetNormal(ref Params, ref normal);
            UnitVector = mInvApplication.TransientGeometry.CreateUnitVector(normal[0], normal[1], normal[2]);
            return UnitVector;
        }
    }
}