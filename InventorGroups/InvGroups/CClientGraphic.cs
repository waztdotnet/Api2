using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvGroups
{
    class CClientGraphic
    {
        private Inventor.ClientGraphics ClientGraphics;

        public CClientGraphic(ref Inventor.Application Application)
        {
            
            
        }
        //public CClientGraphic(ref Inventor.AssemblyDocument assemblyDocument)
        //{
        //   // Inventor.ClientGraphicsCollection clientGraphicsCollection;
        //    //ClientGraphics

        //}

        public static void DrawRangBox(ref Inventor.Application Inv_Application, ref Inventor.AssemblyDocument assemblyDocument, Inventor.ObjectsEnumerator justSelectedEntities)
        {
            //Inventor.ComponentDefinition componentDefinition = (Inventor.ComponentDefinition)assemblyDocument.ComponentDefinition;
            if (justSelectedEntities.Count >= 1)
            {
                if (assemblyDocument.GraphicsDataSetsCollection.Count >= 1)
                {

                }
                else
                {
                    Inventor.ClientGraphics clientGraphics = assemblyDocument.ComponentDefinition.ClientGraphicsCollection["RangeID"];
                    assemblyDocument.GraphicsDataSetsCollection["RangeID"].Delete();
                    clientGraphics.Delete();
                }
            }
            else return;
        }


        ~CClientGraphic()
        {
            throw new System.NotImplementedException();
        }
    }
}

//Public Sub DrawRangeBox()
//    Dim oDoc As Document
//    Set oDoc = ThisApplication.ActiveDocument

//    ' Set a reference to component definition of the active document.
//    ' This assumes that a part or assembly document is active.
//    Dim oCompDef As ComponentDefinition
//    Set oCompDef = ThisApplication.ActiveDocument.ComponentDefinition

//    ' Make sure something is selected.
//    If oDoc.SelectSet.Count = 0 Then
//        MsgBox "You must select the object(s) to display the range box."

//        ' Delete any graphics, if they exist.
//        On Error Resume Next
//        Dim oExistingGraphicsData As GraphicsDataSets
//        Set oExistingGraphicsData = oDoc.GraphicsDataSetsCollection.Item("RangeBoxGraphics")
//        If Err.Number = 0 Then
//            On Error Goto 0
//            Dim oExistingGraphics As ClientGraphics
//            Set oExistingGraphics = oCompDef.ClientGraphicsCollection.Item("RangeBoxGraphics")
//            oExistingGraphics.Delete
//            oExistingGraphicsData.Delete
//            ThisApplication.ActiveView.Update
//        End If

//        Exit Sub
//    End If

//    Redim aoRanges(1 To oDoc.SelectSet.Count) As Box
//    Dim iRangeCount As Long
//    Dim i As Long
//    On Error Resume Next
//    For i = 1 To oDoc.SelectSet.Count
//       Dim oBox As Box
//        Set oBox = oDoc.SelectSet.Item(i).RangeBox
//        If Err Then
//            Err.Clear
//            ' Special case for B-Rep entities.
//            If oDoc.SelectSet.Item(i).Type = kFaceObject Or _
//               oDoc.SelectSet.Item(i).Type = kFaceProxyObject Or _
//               oDoc.SelectSet.Item(i).Type = kEdgeObject Or _
//               oDoc.SelectSet.Item(i).Type = kEdgeProxyObject Then
//                ' Get the range from evaluator of the BRep object.
//                Set oBox = oDoc.SelectSet.Item(i).Evaluator.RangeBox
//                iRangeCount = iRangeCount + 1
//                Set aoRanges(iRangeCount) = oBox
//            End If
//        Else
//            iRangeCount = iRangeCount + 1
//            Set aoRanges(iRangeCount) = oBox
//        End If
//    Next
//    On Error Goto 0

//    If iRangeCount = 0 Then
//        MsgBox "You must pick object(s) that support a 3D RangeBox property."
//        Exit Sub
//    End If

//    ' Check to see if range box graphics information already exists.
//    On Error Resume Next
//    Dim oClientGraphics As ClientGraphics
//    Dim oLineGraphics As LineGraphics
//    Dim oBoxNode As GraphicsNode
//    Dim oGraphicsData As GraphicsDataSets
//    Set oGraphicsData = oDoc.GraphicsDataSetsCollection.Item("RangeBoxGraphics")
//    If Err Then
//        Err.Clear
//        On Error Goto 0

//        ' Set a reference to the transient geometry object for user later.
//        Dim oTransGeom As TransientGeometry
//        Set oTransGeom = ThisApplication.TransientGeometry

//        ' Create a graphics data set object. This object contains all of the
//        ' information used to define the graphics.
//        Dim oDataSets As GraphicsDataSets
//        Set oDataSets = oDoc.GraphicsDataSetsCollection.Add("RangeBoxGraphics")

//        ' Create a coordinate set.
//        Dim oCoordSet As GraphicsCoordinateSet
//        Set oCoordSet = oDataSets.CreateCoordinateSet(1)

//        ' Create the client graphics for this compdef.
//        Set oClientGraphics = oCompDef.ClientGraphicsCollection.Add("RangeBoxGraphics")

//        ' Create a graphics node.
//        Set oBoxNode = oClientGraphics.AddNode(1)
//        oBoxNode.Selectable = False

//        ' Create line graphics.
//        Set oLineGraphics = oBoxNode.AddLineGraphics
//        oLineGraphics.CoordinateSet = oCoordSet
//    Else
//        Set oCoordSet = oGraphicsData.ItemById(1)
//        Set oBoxNode = oCompDef.ClientGraphicsCollection.Item("RangeBoxGraphics").ItemById(1)
//    End If

//    Dim dBoxLines() As Double
//    Redim dBoxLines(1 To 12 * 6 * iRangeCount) As Double
//    For i = 0 To iRangeCount - 1
//        Dim MinPoint(1 To 3) As Double
//        Dim MaxPoint(1 To 3) As Double
//        Call aoRanges(i + 1).GetBoxData(MinPoint, MaxPoint)

//        ' Line 1
//        dBoxLines(i* 72 + 1) = MinPoint(1)
//        dBoxLines(i* 72 + 2) = MinPoint(2)
//        dBoxLines(i* 72 + 3) = MinPoint(3)
//        dBoxLines(i* 72 + 4) = MaxPoint(1)
//        dBoxLines(i* 72 + 5) = MinPoint(2)
//        dBoxLines(i* 72 + 6) = MinPoint(3)

//        ' Line 2
//        dBoxLines(i* 72 + 7) = MinPoint(1)
//        dBoxLines(i* 72 + 8) = MinPoint(2)
//        dBoxLines(i* 72 + 9) = MinPoint(3)
//        dBoxLines(i* 72 + 10) = MinPoint(1)
//        dBoxLines(i* 72 + 11) = MaxPoint(2)
//        dBoxLines(i* 72 + 12) = MinPoint(3)

//        ' Line 3
//        dBoxLines(i* 72 + 13) = MinPoint(1)
//        dBoxLines(i* 72 + 14) = MinPoint(2)
//        dBoxLines(i* 72 + 15) = MinPoint(3)
//        dBoxLines(i* 72 + 16) = MinPoint(1)
//        dBoxLines(i* 72 + 17) = MinPoint(2)
//        dBoxLines(i* 72 + 18) = MaxPoint(3)

//        ' Line 4
//        dBoxLines(i* 72 + 19) = MaxPoint(1)
//        dBoxLines(i* 72 + 20) = MaxPoint(2)
//        dBoxLines(i* 72 + 21) = MaxPoint(3)
//        dBoxLines(i* 72 + 22) = MinPoint(1)
//        dBoxLines(i* 72 + 23) = MaxPoint(2)
//        dBoxLines(i* 72 + 24) = MaxPoint(3)

//        ' Line 5
//        dBoxLines(i* 72 + 25) = MaxPoint(1)
//        dBoxLines(i* 72 + 26) = MaxPoint(2)
//        dBoxLines(i* 72 + 27) = MaxPoint(3)
//        dBoxLines(i* 72 + 28) = MaxPoint(1)
//        dBoxLines(i* 72 + 29) = MinPoint(2)
//        dBoxLines(i* 72 + 30) = MaxPoint(3)

//        ' Line 6
//        dBoxLines(i* 72 + 31) = MaxPoint(1)
//        dBoxLines(i* 72 + 32) = MaxPoint(2)
//        dBoxLines(i* 72 + 33) = MaxPoint(3)
//        dBoxLines(i* 72 + 34) = MaxPoint(1)
//        dBoxLines(i* 72 + 35) = MaxPoint(2)
//        dBoxLines(i* 72 + 36) = MinPoint(3)

//        ' Line 7
//        dBoxLines(i* 72 + 37) = MinPoint(1)
//        dBoxLines(i* 72 + 38) = MaxPoint(2)
//        dBoxLines(i* 72 + 39) = MinPoint(3)
//        dBoxLines(i* 72 + 40) = MaxPoint(1)
//        dBoxLines(i* 72 + 41) = MaxPoint(2)
//        dBoxLines(i* 72 + 42) = MinPoint(3)

//        ' Line 8
//        dBoxLines(i* 72 + 43) = MinPoint(1)
//        dBoxLines(i* 72 + 44) = MaxPoint(2)
//        dBoxLines(i* 72 + 45) = MinPoint(3)
//        dBoxLines(i* 72 + 46) = MinPoint(1)
//        dBoxLines(i* 72 + 47) = MaxPoint(2)
//        dBoxLines(i* 72 + 48) = MaxPoint(3)

//        ' Line 9
//        dBoxLines(i* 72 + 49) = MaxPoint(1)
//        dBoxLines(i* 72 + 50) = MinPoint(2)
//        dBoxLines(i* 72 + 51) = MaxPoint(3)
//        dBoxLines(i* 72 + 52) = MaxPoint(1)
//        dBoxLines(i* 72 + 53) = MinPoint(2)
//        dBoxLines(i* 72 + 54) = MinPoint(3)

//        ' Line 10
//        dBoxLines(i* 72 + 55) = MaxPoint(1)
//        dBoxLines(i* 72 + 56) = MinPoint(2)
//        dBoxLines(i* 72 + 57) = MaxPoint(3)
//        dBoxLines(i* 72 + 58) = MinPoint(1)
//        dBoxLines(i* 72 + 59) = MinPoint(2)
//        dBoxLines(i* 72 + 60) = MaxPoint(3)

//        ' Line 11
//        dBoxLines(i* 72 + 61) = MinPoint(1)
//        dBoxLines(i* 72 + 62) = MinPoint(2)
//        dBoxLines(i* 72 + 63) = MaxPoint(3)
//        dBoxLines(i* 72 + 64) = MinPoint(1)
//        dBoxLines(i* 72 + 65) = MaxPoint(2)
//        dBoxLines(i* 72 + 66) = MaxPoint(3)

//        ' Line 12
//        dBoxLines(i* 72 + 67) = MaxPoint(1)
//        dBoxLines(i* 72 + 68) = MinPoint(2)
//        dBoxLines(i* 72 + 69) = MinPoint(3)
//        dBoxLines(i* 72 + 70) = MaxPoint(1)
//        dBoxLines(i* 72 + 71) = MaxPoint(2)
//        dBoxLines(i* 72 + 72) = MinPoint(3)
//    Next

//    ' Assign the points into the coordinate set.
//    Call oCoordSet.PutCoordinates(dBoxLines)

//    ' Update the display.
//    ThisApplication.ActiveView.Update
//End Sub
