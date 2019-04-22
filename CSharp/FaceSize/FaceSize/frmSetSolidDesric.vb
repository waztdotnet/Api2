Public Class frmSetSolidDescription

    Private InvAppInterface As Inventor.Application
    Private ComponantFileName_Path As String
    Private selectPartDocument As Inventor.PartDocument
    Private Sub btnPickPart_Click(sender As Object, e As EventArgs) Handles btnPickPart.Click
        Dim mAssemblyDocument As Inventor.AssemblyDocument
        Dim mDocument As Inventor.Document = InvAppInterface.ActiveDocument
        If mDocument.DocumentType = Inventor.DocumentTypeEnum.kAssemblyDocumentObject Then
            mAssemblyDocument = mDocument
            Dim mPartDocument As Inventor.PartDocument
            Dim mDocumentPart As Inventor.PartDocument
            For Each mComponentOccurrence As Inventor.ComponentOccurrence In mAssemblyDocument.ComponentDefinition.Occurrences
                If mComponentOccurrence.DefinitionDocumentType = Inventor.DocumentTypeEnum.kPartDocumentObject Then
                    mPartDocument = mComponentOccurrence.Definition.Document
                    mDocumentPart = InvAppInterface.Documents.Open(mPartDocument.FullFileName, True)
                    GetExtents(mDocumentPart)
                    mDocumentPart.Close(True)
                End If
            Next
        Else
            MessageBox.Show("Not an Assembly")
        End If
    End Sub

    Sub GetExtents(ByRef mDocument As Inventor.Document)
        mDocument = InvAppInterface.ActiveDocument

        Dim mComponentDefinition As Inventor.ComponentDefinition
        Dim mBox As Inventor.Box
        Dim Length As Double
        Dim Width As Double
        Dim Thickness As Double
        Dim SortArray(2) As Double
        Dim mSurfaceBody As Inventor.SurfaceBody

        mComponentDefinition = mDocument.ComponentDefinition
        For Each mSurfaceBody In mComponentDefinition.SurfaceBodies
            mBox = mSurfaceBody.RangeBox.Copy
            mBox.Extend(mSurfaceBody.RangeBox.MinPoint)
            mBox.Extend(mSurfaceBody.RangeBox.MaxPoint)
            If Not mBox Is Nothing Then
                Length = mBox.MaxPoint.X - mBox.MinPoint.X
                SortArray(0) = Length
                Width = mBox.MaxPoint.Y - mBox.MinPoint.Y
                SortArray(1) = Width
                Thickness = mBox.MaxPoint.Z - mBox.MinPoint.Z
                SortArray(2) = Thickness
            End If
        Next
        Array.Sort(SortArray)
        Length = SortArray(2)
        Width = SortArray(1)
        Thickness = SortArray(0)
        Length = Length * 10
        Width = Width * 10
        Thickness = Thickness * 10
        MessageBox.Show(Length.ToString("F0") + System.Environment.NewLine + Width.ToString("F0") + System.Environment.NewLine + Thickness.ToString("F0"))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim Document As Inventor.Document = InvAppInterface.ActiveDocument
        Dim MeasureTools As Inventor.MeasureTools
        MeasureTools = InvAppInterface.MeasureTools
        Dim Dou As Double = 0
        Dim oj As Inventor.ObjectCollection
        oj = InvAppInterface.TransientObjects.CreateObjectCollection


        If TypeOf Document.SelectSet.Item(1) Is Inventor.Edge Then
            Dim Edge As Inventor.Edge
            'Dim Curve As Inventor.EdgeLoop
            Edge = Document.SelectSet.Item(1)
            oj.Add(Document.SelectSet.Item(1))
            Dou = MeasureTools.GetLoopLength(oj)
            MessageBox.Show(Dou)

        Else
            Exit Sub
        End If



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InvAppInterface = Nothing 'Clean up if theirs been a problem

        Try

            Try
                InvAppInterface = System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application")

            Catch ex As Exception

            End Try

            If InvAppInterface Is Nothing Then
                Dim InventorAppType As Type = System.Type.GetTypeFromProgID("Inventor.Application")
                InvAppInterface = System.Activator.CreateInstance(InventorAppType)

                InvAppInterface.Visible = True

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim AssemblyDocument As Inventor.AssemblyDocument = InvAppInterface.ActiveDocument
        GetPartDocuments(AssemblyDocument)
    End Sub

    Private Sub GetPartDocuments(ByRef AssemblyDocument As Inventor.AssemblyDocument)
        Dim PartDocument As Inventor.PartDocument
        For Each ComponentOccurrence As Inventor.ComponentOccurrence In AssemblyDocument.ComponentDefinition.Occurrences

            PartDocument = ComponentOccurrence.Definition.Document
            GetEdges(PartDocument)

        Next
    End Sub

    Private Sub btnSetPartSize_Click(sender As Object, e As EventArgs) Handles btnSetPartSize.Click
        'GetEdges()
    End Sub

    Private Sub GetEdges(ByRef PartDocument As Inventor.PartDocument)
        Try


            Dim wp As Inventor.WorkPlane
            'Dim PartDocument As Inventor.PartDocument = InvAppInterface.ActiveDocument
            wp = PartDocument.ComponentDefinition.WorkPlanes.Item(cmbSetPlane.Items.Item(cmbSetPlane.SelectedIndex))
            lstSizes.Items.Add(PartDocument.DisplayName + " " + cmbSetPlane.Items.Item(cmbSetPlane.SelectedIndex))
            Dim Faces As Inventor.Faces = PartDocument.ComponentDefinition.SurfaceBodies(1).Faces
            Dim TransientPlane As Inventor.Plane 'somthing to compair to
            ' Dim TransientEdge As Inventor.Plane 'somthing to compair to
            TransientPlane = InvAppInterface.TransientGeometry.CreatePlane(InvAppInterface.TransientGeometry.CreatePoint(0.0, 0.0, 0.0), wp.Plane.Normal.AsVector)

            Dim Face As Inventor.Face

            Dim TotalLength As Double = 0
            Dim uom As Inventor.UnitsOfMeasure
            Dim Num As List(Of Double) = New List(Of Double)

            uom = InvAppInterface.UnitsOfMeasure
            For Each Face In Faces
                If (TypeOf Face.Geometry Is Inventor.Plane) Then

                    Dim plane As Inventor.Plane
                    Dim faceGeo As Object = Face.Geometry

                    plane = faceGeo
                    If plane.IsParallelTo(TransientPlane, 0.1) Then '"Hit"
                        lstSizes.Items.Add("Hit")

                        lstSizes.Items.Add(Face.Evaluator.Area.ToString("F0") + " mm^2")
                        Dim Counter = 1
                        For Each Edge As Inventor.Edge In Face.Edges
                            Dim EdgeGeo As Object = Edge.Geometry
                            If Edge.CurveType = Inventor.CurveTypeEnum.kLineCurve Then ' check we have real straight line
                                Dim dMin As Double = 0
                                Dim dMax As Double = 0
                                Dim stringLength As Double = 0
                                Dim Length As Double = 0
                                Edge.Evaluator.GetParamExtents(dMin, dMax)
                                Edge.Evaluator.GetLengthAtParam(dMin, dMax, Length)
                                stringLength = Length * 10
                                lstSizes.Items.Add(stringLength.ToString("F1"))
                                Counter += 1
                                Num.Add(stringLength)
                            End If
                        Next

                        Dim NMom As Double = 0
                        Dim NMax As Double = 0
                        For index = 0 To Num.Count - 1
                            NMom = Num.Item(index)
                            If Num.Item(index) >= NMax Then
                                NMax = Num.Item(index)
                            End If
                        Next
                        MessageBox.Show(NMax.ToString("f1"))
                        Num.Clear()
                    Else
                        'lstSizes.Items.Add("NO")
                    End If

                End If
            Next


        Catch ex As Exception

            MessageBox.Show(ex.ToString())

        End Try
    End Sub














    Function CurvesLength(ByRef PartDocument As Inventor.PartDocument) As Integer
        Return 0
    End Function
    Sub TrueSweepLength(ByRef PartDocument As Inventor.PartDocument)
        'Set a reference to the active part document
        Dim PartComponentDefinition As Inventor.PartComponentDefinition
        PartComponentDefinition = PartDocument.ComponentDefinition

        ' Check to make sure a sweep feature is selected.
        If Not TypeOf PartDocument.SelectSet.Item(1) Is Inventor.SweepFeature Then
            MessageBox.Show("A sweep feature must be selected.")
            Exit Sub
        End If

        ' Set a reference to the selected feature.
        Dim SweepFeature As Inventor.SweepFeature
        SweepFeature = PartDocument.SelectSet.Item(1)

        ' Get the centroid of the sweep profile in sketch space
        Dim Pint2d_ProfileOrigin As Inventor.Point2d
        Pint2d_ProfileOrigin = SweepFeature.Profile.RegionProperties.Centroid

        ' Transform the centroid from sketch space to model space
        Dim oProfileOrigin3D As Inventor.Point
        oProfileOrigin3D = SweepFeature.Profile.Parent.SketchToModelSpace(Pint2d_ProfileOrigin)

        ' Get the set of curves that represent the true path of the sweep
        Dim ObjectCurves As Inventor.ObjectsEnumerator
        ObjectCurves = PartComponentDefinition.Features.SweepFeatures.GetTruePath(SweepFeature.Path, oProfileOrigin3D)

        Dim TotalLength As Double
        TotalLength = 0


        For Each Curve As Object In ObjectCurves

            Dim CurveEvaluator As Inventor.CurveEvaluator
            CurveEvaluator = Curve.Evaluator

            Dim MinParam As Double
            Dim MaxParam As Double
            Dim Length As Double

            CurveEvaluator.GetParamExtents(MinParam, MaxParam)
            CurveEvaluator.GetLengthAtParam(MinParam, MaxParam, Length)

            TotalLength = TotalLength + Length
        Next

        ' Display total sweep length
        MessageBox.Show("Total sweep length = " & InvAppInterface.UnitsOfMeasure.GetStringFromValue(TotalLength, Inventor.UnitsTypeEnum.kMillimeterLengthUnits))
    End Sub
    Private Sub GetFaces(ByRef PartDocument As Inventor.PartDocument, ByVal Face As Inventor.Face)

        Select Case Face.SurfaceType
            Case Inventor.SurfaceTypeEnum.kPlaneSurface
                Dim Plane As Inventor.Plane
                Plane = Face.Geometry
                Debug.Print("Planar face")
                Debug.Print(" Root point X: " + Plane.RootPoint.X.ToString + " Root point Y: " + Plane.RootPoint.Y.ToString + " Root point Z: " + Plane.RootPoint.Z.ToString)
                Debug.Print(" Normal vector X: " + Plane.Normal.X.ToString + " Normal vector Y: " + Plane.Normal.Y.ToString + " Normal vector Z: " + Plane.Normal.Z.ToString)

            Case Inventor.SurfaceTypeEnum.kCylinderSurface
                Dim Cylinder As Inventor.Cylinder
                Cylinder = Face.Geometry
                Debug.Print("Cylindrical face")
                Debug.Print(" Base point X: " + Cylinder.BasePoint.X + " Base point X: " + Cylinder.BasePoint.Y)
                Debug.Print(" Axis vector: " & Cylinder.AxisVector.X)
                Debug.Print(" Radius: " & Format(Cylinder.Radius, "0.000000"))
            Case Inventor.SurfaceTypeEnum.kSphereSurface
                Dim Sphere As Inventor.Sphere
                Sphere = Face.Geometry
                Debug.Print("Spherical face")
                Debug.Print(" Center point: " & Sphere.CenterPoint.ToString())
                Debug.Print(" Radius: " & Format(Sphere.Radius, "0.000000"))
            Case Else
                Debug.Print("Unsupported geometry selected: " & TypeName(Face.Geometry))
        End Select
        Dim SurfEval As Inventor.SurfaceEvaluator
        SurfEval = Face.Evaluator
        ' Get the parametric range of the surface. 
        Dim oParamRange As Inventor.Box2d
        oParamRange = SurfEval.ParamRangeRect ' Calculate the u-v values at the parametric center of the surface. ' (This code is bigger than it should be to work around a VBA issue.) 
        Dim adParamCenter(1) As Double
        Dim U As Double, V As Double
        U = oParamRange.MinPoint.X
        V = oParamRange.MaxPoint.X
        adParamCenter(0) = (U + V) / 2
        U = oParamRange.MinPoint.X
        V = oParamRange.MaxPoint.X
        adParamCenter(1) = (U + V) / 2
        ' Get the normal at the u-v parameter. 
        Dim adNormal(2) As Double
        SurfEval.GetNormal(adParamCenter, adNormal)
        ' Print the normal vector. 
        Debug.Print(" Normal: " & Format(adNormal(0), "0.000000") & "," & Format(adNormal(1), "0.000000") & "," & Format(adNormal(2), "0.000000"))
        ' Get the model space coordinate of the parameter so we 
        ' know where in space the normal was calculated. 
        Dim adPoint(2) As Double
        SurfEval.GetPointAtParam(adParamCenter, adPoint)
        ' Print the coordinate. 
        Debug.Print(" Normal coordinate: " & Format(adPoint(0), "0.000000") & "," & Format(adPoint(1), "0.000000") & "," & Format(adPoint(2), "0.000000"))
    End Sub
    Private Sub ComputePlanarFacesNormals()

        Try
            Dim PartDocument As Inventor.PartDocument = InvAppInterface.ActiveDocument
            Dim Faces As Inventor.Faces = PartDocument.ComponentDefinition.SurfaceBodies(1).Faces
            Dim Params(3) As Double
            Dim Normals(3) As Double
            Params(1) = 0

            Params(2) = 0

            Dim index As Long = 1
            Dim Face As Inventor.Face

            For Each Face In Faces

                'Ensures Face is planar

                If (TypeOf Face.Geometry Is Inventor.Plane) Then
                    Face.Evaluator.GetNormal(Params, Normals)
                    Dim oUnitNormal As Inventor.UnitVector

                    oUnitNormal = InvAppInterface.TransientGeometry.CreateUnitVector(Normals(1), Normals(2), Normals(3))
                    System.Diagnostics.Debug.Write("Planar Face[" & index & "] Normal: [" & oUnitNormal.X & ", " & oUnitNormal.Y & ", " & oUnitNormal.Z & "]" & vbCrLf)
                    index = index + 1
                    GetFaces(PartDocument, Face)
                End If

            Next

        Catch ex As Exception

            MessageBox.Show(ex.ToString())

        End Try

    End Sub





    '    Function GetMidParam(oEval As CurveEvaluator) As Double
    '        Dim min As Double
    '        Dim max As Double
    '        Call oEval.GetParamExtents(min, max)

    '        ' Since we just want the middle point, we do not
    '        ' have to consider EdgeUse.IsOpposedToEdge
    '        GetMidParam = min + (max - min) / 2
    '    End Function

    '    Function GetPointAtParam(oEdgeUse As EdgeUse, p As Double) As Point
    '        ' The EdgeUse geometry is in the parametric space of the face
    '        ' whereas EdgeUse.Edge geometry is in model space
    '        Dim oEval As CurveEvaluator
    '    Set oEval = oEdgeUse.Edge.evaluator

    '    Dim param(0) As Double
    '        Dim pt(2) As Double
    '        param(0) = p
    '        Call oEval.GetPointAtParam(param, pt)

    '        Dim oTG As TransientGeometry
    '    Set oTG = ThisApplication.TransientGeometry

    '    Set GetPointAtParam = oTG.CreatePoint(pt(0), pt(1), pt(2))
    'End Function

    '    Function GetTangentAtParam(oEdgeUse As EdgeUse, p As Double) _
    '    As UnitVector
    '        ' The EdgeUse geometry is in the parametric space of the face
    '        ' whereas EdgeUse.Edge geometry is in model space
    '        Dim oEval As CurveEvaluator
    '    Set oEval = oEdgeUse.Edge.evaluator

    '    Dim param(0) As Double
    '        Dim v(2) As Double
    '        param(0) = p
    '        Call oEval.GetTangent(param, v)

    '        Dim oTG As TransientGeometry
    '    Set oTG = ThisApplication.TransientGeometry

    '    ' If the edge is not following the direction around the face
    '    ' then the direction is opposite
    '    If oEdgeUse.IsOpposedToEdge Then
    '        Set GetTangentAtParam = _
    '            oTG.CreateUnitVector(-v(0), -v(1), -v(2))
    '    Else
    '        Set GetTangentAtParam = _
    '            oTG.CreateUnitVector(v(0), v(1), v(2))
    '    End If
    '    End Function

    '    Function GetFaceNormal(oFace As Face, oPoint As Point) As UnitVector
    '        Dim pt(2) As Double
    '        Dim n(2) As Double

    '        pt(0) = oPoint.X : pt(1) = oPoint.Y : pt(2) = oPoint.z
    '        Call oFace.evaluator.GetNormalAtPoint(pt, n)

    '        Dim oTG As TransientGeometry
    '    Set oTG = ThisApplication.TransientGeometry

    '    Set GetFaceNormal = oTG.CreateUnitVector(n(0), n(1), n(2))
    'End Function

    '    Sub CreateUcs(oEdgeUse As EdgeUse, doc As PartDocument)
    '        Dim oEdge As Edge
    '    Set oEdge = oEdgeUse.Edge

    '    Dim mid As Double
    '        mid = GetMidParam(oEdgeUse.Edge.evaluator)

    '        Dim mp As Point
    '    Set mp = GetPointAtParam(oEdgeUse, mid)

    '    Dim x As UnitVector
    '    Set x = GetTangentAtParam(oEdgeUse, mid)

    '    Dim z As UnitVector
    '    Set z = GetFaceNormal(oEdgeUse.EdgeLoop.Face, mp)

    '    Dim y As UnitVector
    '    Set y = z.CrossProduct(x)

    '    Dim oUCSS As UserCoordinateSystems
    '    Set oUCSS = doc.ComponentDefinition.UserCoordinateSystems

    '    Dim oUCSD As UserCoordinateSystemDefinition
    '    Set oUCSD = oUCSS.CreateDefinition

    '    Dim mx As Matrix
    '    Set mx = ThisApplication.TransientGeometry.CreateMatrix
    '    Call mx.SetCoordinateSystem(mp,
    '        x.AsVector(), y.AsVector(), z.AsVector())

    '        oUCSD.Transformation = mx

    '        Dim oUCS As UserCoordinateSystem
    '    Set oUCS = oUCSS.Add(oUCSD)

    '    oUCS.XAxis.Visible = False
    '        oUCS.YAxis.Visible = False
    '        oUCS.ZAxis.Visible = False
    '        oUCS.XYPlane.Visible = False
    '        oUCS.XZPlane.Visible = False
    '        oUCS.YZPlane.Visible = False
    '        oUCS.Origin.Visible = False
    '    End Sub

    '    Sub ShowEdgeDirections()
    '        Dim doc As PartDocument
    '    Set doc = ThisApplication.ActiveDocument

    '    Dim oFace As Face
    '    Set oFace = doc.SelectSet(1)

    '    Dim oEdgeLoop As EdgeLoop
    '        For Each oEdgeLoop In oFace.EdgeLoops
    '            Dim oEdgeUse As EdgeUse
    '            For Each oEdgeUse In oEdgeLoop.EdgeUses
    '                Call CreateUcs(oEdgeUse, doc)
    '            Next
    '        Next
    '    End Sub
End Class
