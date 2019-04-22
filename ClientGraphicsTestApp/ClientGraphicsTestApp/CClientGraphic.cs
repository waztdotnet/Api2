using EventsInventLib;
using System.Reflection;

namespace ClientGraphicsLib
{
    public class CClientGraphic
    {
        private CInvInputEvents ev;

        public CClientGraphic(ref Inventor.Application _InvApplication)
        {
        }

        public static void Run(ref Inventor.Application _InvApplication, string Type)
        {
            Inventor.AssemblyDocument assemblyDocument = (Inventor.AssemblyDocument)_InvApplication.ActiveDocument;
            CInvInputEvents evt = new CInvInputEvents(_InvApplication);
            evt.OnSelectionChange += Evt_OnSelectionChange;
        }

        private static void Evt_OnSelectionChange(ref Inventor.AssemblyDocument AssemblyDocument, ref Inventor.ObjectCollection ObjectCollection)
        {
            //Inventor.ObjectCollection objectCollection = JustSelected.GetEnumerator()
            SetHighLightSet(ref AssemblyDocument, ref ObjectCollection);
        }

        public static void Draw(ref Inventor.Application _InvApplication, string Type)
        {
            string GraphicsName = "CG_Test";
            Inventor.AssemblyDocument assemblyDocument = (Inventor.AssemblyDocument)_InvApplication.ActiveDocument;
            Inventor.ComponentOccurrences Occurrences = assemblyDocument.ComponentDefinition.Occurrences;
            if (Type == "Run")
            {
                // Test(ref _InvApplication,ref assemblyDocument, GraphicsName);
                //TestDrawRangeBoxs(ref _InvApplication, ref assemblyDocument, ref Occurrences, GraphicsName);

                Inventor.ClientGraphics ClientGraphics = null;
                DeleteNamedGraphics(ref assemblyDocument, GraphicsName);
                ClientGraphics = assemblyDocument.ComponentDefinition.ClientGraphicsCollection.Add(GraphicsName);
                DrawClientGraphicSolid(ref _InvApplication, ref ClientGraphics, ref assemblyDocument, "Clear - Green 1");
                SetHighLightSet(ref _InvApplication, ref assemblyDocument, ref Occurrences, "");
            }
            else
            {
                DeleteNamedGraphics(ref assemblyDocument, GraphicsName);
                _InvApplication.ActiveView.Update();
            }
            // Inventor.AssemblyComponentDefinition ComponentDefinition = null;
            // Inventor.ClientGraphics ClientGraphics = null;
        }

        private static void SetHighLightSet(ref Inventor.AssemblyDocument assemblyDocument, ref Inventor.ObjectCollection objectCollection)
        {
            if (objectCollection.Count >= 1)
            {
                Inventor.HighlightSet highlightSet = assemblyDocument.HighlightSets.Add();
                //highlightSet.Color = _InvApplication.TransientObjects.CreateColor(0, 0, 255, 1);
                //highlightSet.Color = _InvApplication.TransientObjects.CreateColor(255, 0, 0, 1);
                highlightSet.AddMultipleItems(objectCollection);
            }
            else return;
        }

        private static void SetHighLightSet(ref Inventor.Application _InvApplication, ref Inventor.AssemblyDocument assemblyDocument, ref Inventor.ComponentOccurrences Occurrences, string GraphicsName)
        {
            if (Occurrences.Count >= 1)
            {
                //Inventor.ObjectCollection objectCollection = _InvApplication.
                Inventor.HighlightSet highlightSet = assemblyDocument.HighlightSets.Add();
                highlightSet.Color = _InvApplication.TransientObjects.CreateColor(255, 0, 0, 1);
                //highlightSet.AddMultipleItems(Occurrences);
                int length = Occurrences.Count;
                for (int i = length; i >= 1; i--)
                {
                    if (GetInventorObjType(Occurrences[i]) == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                    {
                        System.Diagnostics.Debug.WriteLine("Just Selected");
                        Inventor.ComponentOccurrence componentOccurrence = Occurrences[i];
                        //highlightSet.AddItem(componentOccurrence);
                    }
                }
            }
            else return;
        }

        public static void TestDrawRangeBoxs(ref Inventor.Application _InvApplication, ref Inventor.AssemblyDocument assemblyDocument, ref Inventor.ComponentOccurrences Occurrences, string GraphicsName)
        {
            if (Occurrences.Count >= 1)
            {
                Inventor.ClientGraphics ClientGraphics = null;
                Inventor.GraphicsCoordinateSet CoordSet = null;
                Inventor.GraphicsDataSets GraphDataSets = null;
                Inventor.Box box = null;
                DeleteNamedGraphics(ref assemblyDocument, GraphicsName);
                CoordSet = SetClientGraphics(ref assemblyDocument, ref ClientGraphics, ref GraphDataSets, GraphicsName);
                int length = Occurrences.Count;
                for (int i = length; i >= 1; i--)
                {
                    if (GetInventorObjType(Occurrences[i]) == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                    {
                        System.Diagnostics.Debug.WriteLine("Just Selected");
                        Inventor.ComponentOccurrence componentOccurrence = Occurrences[i];
                        box = componentOccurrence.RangeBox;

                        DrawBox(ref _InvApplication, ref ClientGraphics, ref CoordSet, ref GraphDataSets, ref box);
                    }
                    //else if (GetInventorObjType(justSelectedEntities[i]) == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                    //{
                    //    Inventor.OccurrencePattern OccurrencePattern = justSelectedEntities[i];
                    //    int PatternLength = 10;
                    //    for (int x = PatternLength - 1; x >= 1; x--)
                    //    {
                    //    }
                    //} use after idea tested
                }
            }
            else return;
        }

        private static void DrawBox(ref Inventor.Application _InvApplication, ref Inventor.ClientGraphics ClientGraphics, ref Inventor.GraphicsCoordinateSet oCoordSet, ref Inventor.GraphicsDataSets GraphDataSets, ref Inventor.Box box)
        {
            Inventor.GraphicsNode LineNode = ClientGraphics.AddNode(1);
            // ClientGraphics.Selectable = Inventor.GraphicsSelectabilityEnum.kAllGraphicsSelectable;
            // add LineGraphics
            Inventor.LineGraphics LineSet = LineNode.AddLineGraphics();
            LineSet.CoordinateSet = oCoordSet;

            Inventor.TransientGeometry oTG = _InvApplication.TransientGeometry;
            //Line 1 Start Point End Point
            oCoordSet.Add(1, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(2, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            //Line 2 Start Point End Point
            oCoordSet.Add(3, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(4, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            //Line 3 Start Point End Point
            oCoordSet.Add(5, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(6, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            //Line 4 Start Point End Point
            oCoordSet.Add(7, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(8, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            //Upper Line 1 Start Point End Point
            oCoordSet.Add(9, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            oCoordSet.Add(10, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            //Upper Line 2 Start Point End Point
            oCoordSet.Add(11, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            oCoordSet.Add(12, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            //Upper Line 3 Start Point End Point
            oCoordSet.Add(13, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            oCoordSet.Add(14, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            //Upper Line 4 Start Point End Point
            oCoordSet.Add(15, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            oCoordSet.Add(16, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            //Line Vert 1 Start Point End Point
            oCoordSet.Add(17, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(18, oTG.CreatePoint(box.MinPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            //Line Vert 2 Start Point End Point
            oCoordSet.Add(19, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(20, oTG.CreatePoint(box.MaxPoint.X, box.MinPoint.Y, box.MaxPoint.Z));
            //Line Vert 3 Start Point End Point
            oCoordSet.Add(21, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(22, oTG.CreatePoint(box.MaxPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            //Line Vert 4 Start Point End Point
            oCoordSet.Add(23, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MinPoint.Z));
            oCoordSet.Add(24, oTG.CreatePoint(box.MinPoint.X, box.MaxPoint.Y, box.MaxPoint.Z));
            // set LineDefinitionSpace as screen space
            LineSet.LineDefinitionSpace = Inventor.LineDefinitionSpaceEnum.kScreenSpace;
            // set Colour
            Inventor.GraphicsColorSet graphicsColorSet = LineSet.ColorSet;
            graphicsColorSet = GraphDataSets.CreateColorSet(1);
            LineSet.ColorSet = graphicsColorSet;
            LineSet.ColorSet.Add(1, 255, 1, 0);
            // set lineweight
            LineSet.LineWeight = 2;
            // set LineType
            LineSet.LineType = Inventor.LineTypeEnum.kChainLineType;
            // set LineScale
            LineSet.LineScale = 2;

            _InvApplication.ActiveView.Update();
        }

        private static Inventor.GraphicsCoordinateSet SetClientGraphics(ref Inventor.AssemblyDocument assemblyDocument, ref Inventor.ClientGraphics ClientGraphics, ref Inventor.GraphicsDataSets GraphDataSets, string GraphicsName)
        {
            Inventor.AssemblyComponentDefinition ComponentDefinition = assemblyDocument.ComponentDefinition;

            // add ClientGraphics
            ClientGraphics = ComponentDefinition.ClientGraphicsCollection.Add(GraphicsName);

            //add GraphicsDataSets

            try
            {
                GraphDataSets = assemblyDocument.GraphicsDataSetsCollection[GraphicsName];
                if (GraphDataSets != null)
                {
                    GraphDataSets.Delete();
                }
            }
            catch
            { }

            GraphDataSets = assemblyDocument.GraphicsDataSetsCollection.Add(GraphicsName);
            return GraphDataSets.CreateCoordinateSet(1);
            //Inventor.GraphicsCoordinateSet oCoordSet = GraphDataSets.CreateCoordinateSet(1);

            //return oCoordSet;
        }

        public static Inventor.ClientGraphics DeleteNamedGraphics(ref Inventor.AssemblyDocument assemblyDocument, string Name)//, Inventor.ClientGraphics oClientGraphics)
        {
            Inventor.ClientGraphics ClientGraphics = null;
            try
            {
                ClientGraphics = assemblyDocument.ComponentDefinition.ClientGraphicsCollection[Name];
                if (ClientGraphics != null)
                {
                    ClientGraphics.Delete();
                }
            }
            catch
            { }

            return ClientGraphics;
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void DrawClientGraphicSolid(ref Inventor.Application _InvApplication, ref Inventor.ClientGraphics ClientGraphics, ref Inventor.AssemblyDocument assemblyDocument, string AssetName)
        {//"Clear - Green 1" "Clear"
            Inventor.Asset localAsset;
            localAsset = assemblyDocument.Assets[AssetName];
            Inventor.GraphicsNode SurfacesNode = ClientGraphics.AddNode(1);
            SurfacesNode.Appearance = localAsset;
            Inventor.Box box = null;
            Inventor.SurfaceBody SurfaceBody;
            Inventor.TransientBRep TransientBRep;
            TransientBRep = _InvApplication.TransientBRep;
            Inventor.SurfaceGraphics SurfaceGraphics;
            int length = assemblyDocument.ComponentDefinition.Occurrences.Count;

            for (int i = length; i >= 1; i--)
            {
                if (GetInventorObjType(assemblyDocument.ComponentDefinition.Occurrences[i]) == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                {
                    Inventor.ComponentOccurrence componentOccurrence = assemblyDocument.ComponentDefinition.Occurrences[i];
                    box = componentOccurrence.RangeBox;
                    SurfaceBody = TransientBRep.CreateSolidBlock(box.Copy());
                    SurfaceGraphics = SurfacesNode.AddSurfaceGraphics(SurfaceBody);
                    SurfacesNode.Appearance = localAsset;
                }
            }
            _InvApplication.ActiveView.Update();
        }

        private static void Test(ref Inventor.Application _InvApplication, ref Inventor.AssemblyDocument assemblyDocument, string ClientGraphicsName)
        {
            Inventor.AssemblyComponentDefinition ComponentDefinition = assemblyDocument.ComponentDefinition;

            // add ClientGraphics
            Inventor.ClientGraphics ClientGraphics = null;

            ClientGraphics = DeleteNamedGraphics(ref assemblyDocument, ClientGraphicsName);

            ClientGraphics = ComponentDefinition.ClientGraphicsCollection.Add(ClientGraphicsName);

            //add GraphicsDataSets
            Inventor.GraphicsDataSets GraphDataSets;
            try
            {
                GraphDataSets = assemblyDocument.GraphicsDataSetsCollection[ClientGraphicsName];
                if (GraphDataSets != null)
                {
                    GraphDataSets.Delete();
                }
            }
            catch
            { }

            GraphDataSets = assemblyDocument.GraphicsDataSetsCollection.Add(ClientGraphicsName);
            Inventor.GraphicsCoordinateSet oCoordSet = GraphDataSets.CreateCoordinateSet(1);

            // add GraphicsNode
            Inventor.GraphicsNode LineNode = ClientGraphics.AddNode(1);
            // add LineGraphics
            Inventor.LineGraphics LineSet = LineNode.AddLineGraphics();
            LineSet.CoordinateSet = oCoordSet;

            Inventor.TransientGeometry oTG = _InvApplication.TransientGeometry;

            oCoordSet.Add(1, oTG.CreatePoint(0, 0, 0));
            oCoordSet.Add(2, oTG.CreatePoint(20, 30, 0));

            oCoordSet.Add(3, oTG.CreatePoint(40, 20, 0));
            oCoordSet.Add(4, oTG.CreatePoint(60, 30, 100));

            // set LineDefinitionSpace as screen space
            LineSet.LineDefinitionSpace = Inventor.LineDefinitionSpaceEnum.kScreenSpace;
            // set Colour
            Inventor.GraphicsColorSet graphicsColorSet = LineSet.ColorSet;
            graphicsColorSet = GraphDataSets.CreateColorSet(1);
            LineSet.ColorSet = graphicsColorSet;
            LineSet.ColorSet.Add(1, 255, 1, 0);
            // set lineweight
            LineSet.LineWeight = 2;
            // set LineType
            LineSet.LineType = Inventor.LineTypeEnum.kChainLineType;
            // set LineScale
            LineSet.LineScale = 2;

            _InvApplication.ActiveView.Update();
        }

        private static Inventor.ClientGraphics DeleteClientGraphics(ref Inventor.AssemblyDocument assemblyDocument, ref Inventor.AssemblyComponentDefinition CompDef)//, Inventor.ClientGraphics oClientGraphics)
        {
            CompDef = assemblyDocument.ComponentDefinition;
            Inventor.ClientGraphics ClientGraphics = null;
            try
            {
                ClientGraphics = CompDef.ClientGraphicsCollection["CG_Test"];
                if (ClientGraphics != null)
                {
                    ClientGraphics.Delete();
                }
            }
            catch
            { }

            return ClientGraphics;
        }

        public static void DrawRangeBoxs(ref Inventor.Application Inv_Application, ref Inventor.AssemblyDocument assemblyDocument, Inventor.ObjectsEnumerator justSelectedEntities)
        {
            //Inventor.ComponentDefinition componentDefinition = (Inventor.ComponentDefinition)assemblyDocument.ComponentDefinition;

            if (justSelectedEntities.Count >= 1)
            {
                if (assemblyDocument.GraphicsDataSetsCollection.Count >= 1)
                {
                    int length = justSelectedEntities.Count;
                    for (int i = length - 1; i >= 0; i--)
                    {
                        if (GetInventorObjType(justSelectedEntities[i]) == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                        {
                            Inventor.Box box = null;
                            System.Diagnostics.Debug.WriteLine("Just Selected");
                            Inventor.ComponentOccurrence componentOccurrence = justSelectedEntities[i];
                            box = componentOccurrence.RangeBox;
                        }
                        //else if (GetInventorObjType(justSelectedEntities[i]) == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                        //{
                        //    Inventor.OccurrencePattern OccurrencePattern = justSelectedEntities[i];
                        //    int PatternLength = 10;
                        //    for (int x = PatternLength - 1; x >= 1; x--)
                        //    {
                        //    }
                        //} use after idea tested
                    }
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

        private static Inventor.ObjectTypeEnum GetInventorObjType(object objUnKnown)
        {
            System.Type invokeType = objUnKnown.GetType();

            object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);

            Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;

            return objTypeEum;
        }

        ~CClientGraphic()
        {
        }
    }
}