using Inventor;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Graphics
{
    /// <summary>
    /// Enums used by ClientGraphicsManager
    /// </summary>
    /// <returns></returns>
    public enum GraphicModeEnum
    {
        kDocumentGraphics,
        kInteractionGraphics,
        kClientFeatureGraphics,
        kDrawingViewGraphics,
        kDrawingSheetGraphics,
        kFlatPatternGraphics
    }

    /// <summary>
    /// Enums used by ClientGraphicsManager
    /// </summary>
    /// <returns></returns>
    public enum IntActionGraphicsModeEnum
    {
        kPreviewGraphics,
        kOverlayGraphics
    }

    /// <summary>
    /// class to manipulate ClientGraphics
    /// </summary>
    /// <returns></returns>
    public class CGraphicsManager
    {
        private Inventor.Application _Application;
        private string _clientId;

        private InteractionEvents _workingInteraction;
        private ClientFeature _workingFeature;
        private DrawingView _workingView;
        private FlatPattern _workingFlat;
        private Document _workingDocument;
        private Sheet _workingSheet;

        private GraphicModeEnum _mode;

        /// <summary>
        /// Set whether or not ClientGraphics and GraphicsData are transacted or not
        /// </summary>
        /// <returns>bool</returns>
        public bool Transacting
        {
            get;
            set;
        }

        /// <summary>
        /// Set InteractionGraphics to Preview or Overlay
        /// </summary>
        /// <returns>IntActionGraphicsModeEnum</returns>
        public IntActionGraphicsModeEnum InteractionGraphicsMode
        {
            get;
            set;
        }

        /// <summary>
        ///Returns the current GraphicsData depending of the graphic source
        ///(Document, InteractionEvents, ClientFeature, ...)
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics WorkingGraphics
        {
            get
            {
                switch (_mode)
                {
                    case GraphicModeEnum.kDocumentGraphics:
                        return new CGraphics(_workingDocument, _clientId, Transacting);

                    case GraphicModeEnum.kInteractionGraphics:
                        return new CGraphics(_workingInteraction, InteractionGraphicsMode);

                    case GraphicModeEnum.kClientFeatureGraphics:
                        return new CGraphics(_workingFeature, _clientId, true);

                    case GraphicModeEnum.kDrawingViewGraphics:
                        return new CGraphics(_workingView, _clientId, Transacting);

                    case GraphicModeEnum.kDrawingSheetGraphics:
                        return new CGraphics(_workingSheet, _clientId, Transacting);

                    case GraphicModeEnum.kFlatPatternGraphics:
                        return new CGraphics(_workingFlat, _clientId, Transacting);

                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// ClientGraphicsManager Constructor
        /// </summary>
        /// <returns>CGraphicsManager</returns>
        public CGraphicsManager(Inventor.Application Application, string clientId)
        {
            _Application = Application;

            _clientId = clientId;

            _workingDocument = _Application.ActiveDocument;

            Transacting = true;

            _mode = GraphicModeEnum.kDocumentGraphics;

            InteractionGraphicsMode = IntActionGraphicsModeEnum.kPreviewGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(Document document)
        {
            _workingDocument = document;

            _mode = GraphicModeEnum.kDocumentGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(DrawingView drawingView)
        {
            _workingView = drawingView;

            _mode = GraphicModeEnum.kDrawingViewGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(Sheet sheet)
        {
            _workingSheet = sheet;

            _mode = GraphicModeEnum.kDrawingSheetGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(FlatPattern flatPattern)
        {
            _workingFlat = flatPattern;

            _mode = GraphicModeEnum.kFlatPatternGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(InteractionEvents interactionEvents)
        {
            _workingInteraction = interactionEvents;

            _mode = GraphicModeEnum.kInteractionGraphics;
        }

        /// <summary>
        /// Overloaded methods to define the graphic source
        /// </summary>
        /// <returns>Void</returns>
        public void SetGraphicsSource(ClientFeature feature)
        {
            _workingFeature = feature;

            _mode = GraphicModeEnum.kClientFeatureGraphics;
        }

        /// <summary>
        /// Retrieve current graphic source
        /// </summary>
        /// <returns>object</returns>
        public object GetGraphicsSource(out GraphicModeEnum mode)
        {
            mode = _mode;

            switch (_mode)
            {
                case GraphicModeEnum.kDocumentGraphics:
                    return _workingDocument;

                case GraphicModeEnum.kInteractionGraphics:
                    return _workingInteraction;

                case GraphicModeEnum.kClientFeatureGraphics:
                    return _workingFeature;

                case GraphicModeEnum.kDrawingViewGraphics:
                    return _workingView;

                case GraphicModeEnum.kDrawingSheetGraphics:
                    return _workingSheet;

                case GraphicModeEnum.kFlatPatternGraphics:
                    return _workingFlat;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Use: Update view to display non-interaction or interaction drawn graphics
        /// </summary>
        /// <returns>Void</returns>
        public void UpdateView()
        {
            View activeView = _Application.ActiveView;

            if (_mode == GraphicModeEnum.kInteractionGraphics &&
                InteractionGraphicsMode == IntActionGraphicsModeEnum.kOverlayGraphics)

                _workingInteraction.InteractionGraphics.UpdateOverlayGraphics(activeView);
            else

                activeView.Update();
        }

        /// <summary>
        /// Use: Delete all graphics for input source created by the AdnClientGraphicsManager
        /// </summary>
        /// <returns>Void</returns>
        public void DeleteGraphics(Document document, bool deleteData)
        {
            CGraphics dataTx = new CGraphics(document, _clientId, true, false);
            dataTx.Delete(deleteData);

            CGraphics dataNonTx = new CGraphics(document, _clientId, false, false);
            dataNonTx.Delete(deleteData);
        }

        /// <summary>
        /// Use: Delete all graphics for input source created by the AdnClientGraphicsManager
        /// </summary>
        /// <returns>Void</returns>
        public void DeleteGraphics(DrawingView drawingView, bool deleteData)
        {
            CGraphics dataTx = new CGraphics(drawingView, _clientId, true);
            dataTx.Delete(deleteData);

            CGraphics dataNonTx = new CGraphics(drawingView, _clientId, false);
            dataNonTx.Delete(deleteData);
        }

        /// <summary>
        /// Use: Delete all graphics for input source created by the AdnClientGraphicsManager
        /// </summary>
        /// <returns>Void</returns>
        public void DeleteGraphics(Sheet sheet, bool deleteData)
        {
            CGraphics dataTx = new CGraphics(sheet, _clientId, true);
            dataTx.Delete(deleteData);

            CGraphics dataNonTx = new CGraphics(sheet, _clientId, false);
            dataNonTx.Delete(deleteData);
        }

        /// <summary>
        /// Use: Delete all graphics for input source created by the AdnClientGraphicsManager
        /// </summary>
        /// <returns>Void</returns>
        public void DeleteGraphics(InteractionEvents interactionEvents, bool deleteData)
        {
            CGraphics data = new CGraphics(interactionEvents, InteractionGraphicsMode);
            data.Delete(deleteData);
        }

        /// <summary>
        /// Use: Delete all graphics for input source created by the AdnClientGraphicsManager
        /// </summary>
        /// <returns>Void</returns>
        public void DeleteGraphics(ClientFeature feature, bool deleteData)
        {
            CGraphics data = new CGraphics(feature, _clientId, true);
            data.Delete(deleteData);
        }

        /// <summary>
        /// Returns a new GraphicsNode
        /// </summary>
        /// <returns>GraphicsNode</returns>
        public GraphicsNode CreateNewGraphicsNode()
        {
            CGraphics graphicsData = WorkingGraphics;

            GraphicsNode node = graphicsData.ClientGraphics.AddNode(
                    graphicsData.GetGraphicNodeFreeId());

            return node;
        }

        /// <summary>
        /// Returns a new GraphicsNode
        /// </summary>
        /// <returns>GraphicsNode</returns>
        public GraphicsNode CreateNewGraphicsNode(int customId)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                GraphicsNode node = graphicsData.ClientGraphics.AddNode(customId);

                return node;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a LineGraphics primitive
        /// </summary>
        /// <returns>LineGraphics</returns>
        public LineGraphics DrawLine(
                    double[] startPoint,
                    double[] endPoint)
        {
            return DrawLine(startPoint, endPoint, null);
        }

        /// <summary>
        /// Draws a LineGraphics primitive
        /// </summary>
        /// <returns>LineGraphics</returns>
        public LineGraphics DrawLine(
                    double[] startPoint,
                    double[] endPoint,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                LineGraphics graphic = node.AddLineGraphics();

                if ((startPoint != null) && (endPoint != null))
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    double[] coordsArray = startPoint.Concat(endPoint).ToArray();

                    coordSet.PutCoordinates(ref coordsArray);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a LineStripGraphics primitive
        /// </summary>
        /// <returns>LineStripGraphics</returns>
        public LineStripGraphics DrawLineStrip(
                    double[] coordinates)
        {
            return DrawLineStrip(coordinates, null);
        }

        /// <summary>
        /// Draws a LineStripGraphics primitive
        /// </summary>
        /// <returns>LineStripGraphics</returns>
        public LineStripGraphics DrawLineStrip(
                    double[] coordinates,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                LineStripGraphics graphic = node.AddLineStripGraphics();

                if (coordinates != null)
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    coordSet.PutCoordinates(ref coordinates);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a TriangleGraphics primitive
        /// </summary>
        /// <returns>TriangleGraphics</returns>
        public TriangleGraphics DrawTriangle(
                    double[] v1,
                    double[] v2,
                    double[] v3)
        {
            return DrawTriangle(v1, v2, v3, null);
        }

        /// <summary>
        /// Draws a TriangleGraphics primitive
        /// </summary>
        /// <returns>TriangleGraphics</returns>
        public TriangleGraphics DrawTriangle(
                    double[] v1,
                    double[] v2,
                    double[] v3,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                TriangleGraphics graphic = node.AddTriangleGraphics();

                if ((v1 != null) && (v2 != null) && (v3 != null))
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    List<double> coordinates = new List<double>();

                    coordinates.AddRange(v1);
                    coordinates.AddRange(v2);
                    coordinates.AddRange(v3);

                    double[] coordsArray = coordinates.ToArray();

                    coordSet.PutCoordinates(ref coordsArray);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a TriangleFanGraphics primitive
        /// </summary>
        /// <returns>TriangleFanGraphics</returns>
        public TriangleFanGraphics DrawTriangleFan(
                   double[] coordinates)
        {
            return DrawTriangleFan(coordinates, null);
        }

        /// <summary>
        /// Draws a TriangleFanGraphics primitive
        /// </summary>
        /// <returns>TriangleFanGraphics</returns>
        public TriangleFanGraphics DrawTriangleFan(
                   double[] coordinates,
                   GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                TriangleFanGraphics graphic = node.AddTriangleFanGraphics();

                if (coordinates != null)
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    coordSet.PutCoordinates(ref coordinates);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a TriangleStripGraphics primitive
        /// </summary>
        /// <returns>TriangleStripGraphics</returns>
        public TriangleStripGraphics DrawTriangleStrip(
                  double[] coordinates)
        {
            return DrawTriangleStrip(coordinates, null);
        }

        /// <summary>
        /// Draws a TriangleStripGraphics primitive
        /// </summary>
        /// <returns>TriangleStripGraphics</returns>
        public TriangleStripGraphics DrawTriangleStrip(
                   double[] coordinates,
                   GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                TriangleStripGraphics graphic = node.AddTriangleStripGraphics();

                if (coordinates != null)
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    coordSet.PutCoordinates(ref coordinates);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // Use: Draws a CurveGraphics primitive
        //
        //////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws a CurveGraphics primitive
        /// </summary>
        public CurveGraphics DrawCurve(
                  object curve)
        {
            return DrawCurve(curve, null);
        }

        /// <summary>
        /// Draws a CurveGraphics primitive
        /// </summary>
        public CurveGraphics DrawCurve(
                    object curve,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                CurveGraphics graphic = node.AddCurveGraphics(curve);

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a SurfaceGraphics primitive
        /// </summary>
        public SurfaceGraphics DrawSurface(
                    object surface)
        {
            return DrawSurface(surface, null);
        }

        /// <summary>
        /// Draws a SurfaceGraphics primitive
        /// </summary>
        public SurfaceGraphics DrawSurface(
                    object surface,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                SurfaceGraphics graphic = node.AddSurfaceGraphics(surface);

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a ComponentGraphics primitive
        /// </summary>
        public ComponentGraphics DrawComponent(
           ComponentDefinition compDef)
        {
            return DrawComponent(compDef, null);
        }

        /// <summary>
        /// Draws a ComponentGraphics primitive
        /// </summary>
        public ComponentGraphics DrawComponent(
            ComponentDefinition compDef,
            GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                ComponentGraphics graphic = node.AddComponentGraphics(compDef);

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Draws a TextGraphics primitive
        /// </summary>
        public TextGraphics DrawText(
                    string text,
                    bool scalable)
        {
            return DrawText(text, scalable, null);
        }

        /// <summary>
        /// Draws a TextGraphics primitive
        /// </summary>
        public TextGraphics DrawText(
                    string text,
                    bool scalable,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                TextGraphics graphic = (scalable ? node.AddScalableTextGraphics() : node.AddTextGraphics());

                graphic.Text = text;

                return graphic;
            }
            catch
            {
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // Use: Draws a PointGraphics  primitive
        //
        //////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws a PointGraphics  primitive
        /// </summary>
        public PointGraphics DrawPoint(
                   double[] position)
        {
            return DrawPoint(position, null);
        }

        /// <summary>
        /// Draws a PointGraphics  primitive
        /// </summary>
        public PointGraphics DrawPoint(
                    double[] position,
                    GraphicsNode node)
        {
            try
            {
                CGraphics graphicsData = WorkingGraphics;

                if (node == null)
                {
                    node = graphicsData.ClientGraphics.AddNode(
                        graphicsData.GetGraphicNodeFreeId());
                }

                PointGraphics graphic = node.AddPointGraphics();

                if (position != null)
                {
                    GraphicsCoordinateSet coordSet =
                        graphicsData.GraphicsDataSets.CreateCoordinateSet(
                            graphicsData.GetDataSetFreeId());

                    coordSet.PutCoordinates(ref position);

                    graphic.CoordinateSet = coordSet;
                }

                return graphic;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// utility class to handle GraphicsDataSets and ClientGraphics
    /// </summary>
    /// <returns></returns>
    public class CGraphics
    {
        private string _clientId;

        private GraphicsDataSets _graphicsData;
        private ClientGraphics _graphics;

        /// <summary>
        /// Returns GraphicsDataSets object
        /// </summary>
        /// <returns>GraphicsDataSets</returns>
        public GraphicsDataSets GraphicsDataSets
        {
            get
            {
                return _graphicsData;
            }
        }

        /// <summary>
        /// Returns ClientGraphics object
        /// </summary>
        /// <returns>ClientGraphics</returns>
        public ClientGraphics ClientGraphics
        {
            get
            {
                return _graphics;
            }
        }

        /// <summary>
        /// GraphicsData constructor for ComponentDefinition graphics
        /// </summary>
        /// <returns></returns>
        public CGraphics(
            Document document,
            string clientId,
            bool transacting,
            bool createIfNotExist)
        {
            _clientId = clientId + (transacting ? "-Tx" : "-NonTx");

            _graphicsData = null;
            _graphics = null;

            try
            {
                _graphicsData = document.GraphicsDataSetsCollection[_clientId];
            }
            catch
            {
                if (createIfNotExist)
                {
                    if (transacting)
                    {
                        _graphicsData = document.GraphicsDataSetsCollection.Add(_clientId);
                    }
                    else
                    {
                        _graphicsData = document.GraphicsDataSetsCollection.AddNonTransacting(_clientId);
                    }
                }
            }

            ComponentDefinition compDef = GetCompDefinition(document);

            try
            {
                _graphics = compDef.ClientGraphicsCollection[_clientId];
            }
            catch
            {
                if (createIfNotExist)
                {
                    if (transacting)
                    {
                        _graphics = compDef.ClientGraphicsCollection.Add(_clientId);
                    }
                    else
                    {
                        _graphics = compDef.ClientGraphicsCollection.AddNonTransacting(_clientId);
                    }
                }
            }
        }

        public CGraphics(Document document, string clientId, bool transacting) :
            this(document, clientId, transacting, true)
        {
        }

        private static ComponentDefinition GetCompDefinition(Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentTypeEnum.kAssemblyDocumentObject:
                    AssemblyDocument asm = document as AssemblyDocument;
                    return asm.ComponentDefinition as ComponentDefinition;

                case DocumentTypeEnum.kPartDocumentObject:
                    PartDocument part = document as PartDocument;
                    return part.ComponentDefinition as ComponentDefinition;

                default:
                    return null;
            }
        }

        /// <summary>
        /// GraphicsData constructor for DrawingView graphics
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics(DrawingView view, string clientId, bool transacting)
        {
            _clientId = clientId + (transacting ? "-Tx" : "-NonTx");

            _graphicsData = null;
            _graphics = null;

            try
            {
                _graphicsData = view.GraphicsDataSetsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphicsData = view.GraphicsDataSetsCollection.Add(_clientId);
                }
                else
                {
                    _graphicsData = view.GraphicsDataSetsCollection.AddNonTransacting(_clientId);
                }
            }

            try
            {
                _graphics = view.ClientGraphicsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphics = view.ClientGraphicsCollection.Add(_clientId);
                }
                else
                {
                    _graphics = view.ClientGraphicsCollection.AddNonTransacting(_clientId);
                }
            }
        }

        /// <summary>
        /// GraphicsData constructor for Sheet graphics
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics(Sheet sheet, string clientId, bool transacting)
        {
            _clientId = clientId + (transacting ? "-Tx" : "-NonTx");

            _graphicsData = null;
            _graphics = null;

            try
            {
                _graphicsData = sheet.GraphicsDataSetsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphicsData = sheet.GraphicsDataSetsCollection.Add(_clientId);
                }
                else
                {
                    _graphicsData = sheet.GraphicsDataSetsCollection.AddNonTransacting(_clientId);
                }
            }

            try
            {
                _graphics = sheet.ClientGraphicsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphics = sheet.ClientGraphicsCollection.Add(_clientId);
                }
                else
                {
                    _graphics = sheet.ClientGraphicsCollection.AddNonTransacting(_clientId);
                }
            }
        }

        /// <summary>
        /// GraphicsData constructor for FlatPattern graphics
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics(FlatPattern flat, string clientId, bool transacting)
        {
            _clientId = clientId + (transacting ? "-Tx" : "-NonTx");

            _graphicsData = null;
            _graphics = null;

            try
            {
                _graphicsData = flat.GraphicsDataSetsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphicsData = flat.GraphicsDataSetsCollection.Add(_clientId);
                }
                else
                {
                    _graphicsData = flat.GraphicsDataSetsCollection.AddNonTransacting(_clientId);
                }
            }

            try
            {
                _graphics = flat.ClientGraphicsCollection[_clientId];
            }
            catch
            {
                if (transacting)
                {
                    _graphics = flat.ClientGraphicsCollection.Add(_clientId);
                }
                else
                {
                    _graphics = flat.ClientGraphicsCollection.AddNonTransacting(_clientId);
                }
            }
        }

        /// <summary>
        /// GraphicsData constructor for Interaction graphics
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics(InteractionEvents InteractionEvents, IntActionGraphicsModeEnum mode)
        {
            _graphicsData = InteractionEvents.InteractionGraphics.GraphicsDataSets;

            switch (mode)
            {
                case IntActionGraphicsModeEnum.kOverlayGraphics:
                    _graphics = InteractionEvents.InteractionGraphics.OverlayClientGraphics;
                    break;

                case IntActionGraphicsModeEnum.kPreviewGraphics:
                    _graphics = InteractionEvents.InteractionGraphics.PreviewClientGraphics;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// GraphicsData constructor for ClientFeature graphics
        /// </summary>
        /// <returns>CGraphics</returns>
        public CGraphics(ClientFeature feature, string clientId, bool saveWithDoc)
        {
            _clientId = clientId;

            ClientFeatureDefinition cfDef = feature.Definition;

            try
            {
                _graphicsData = cfDef.GraphicsDataSetsCollection[_clientId];
            }
            catch
            {
                _graphicsData = cfDef.GraphicsDataSetsCollection.Add(_clientId);
            }

            try
            {
                _graphics = cfDef.ClientGraphicsCollection[_clientId];
            }
            catch
            {
                _graphics = cfDef.ClientGraphicsCollection.Add(_clientId);
            }
        }

        /// <summary>
        /// Returns first free Id for new GraphicsDataSet to be created
        /// </summary>
        /// <returns>int</returns>
        public int GetDataSetFreeId()
        {
            List<int> ids = new List<int>();

            foreach (GraphicsDataSet data in _graphicsData)
            {
                ids.Add(data.Id);
            }

            int freeId = 1;

            while (ids.Contains(freeId))
                ++freeId;

            return freeId;
        }

        /// <summary>
        /// Returns first free Id for new GraphicsNode to be created
        /// </summary>
        /// <returns>int</returns>
        public int GetGraphicNodeFreeId()
        {
            List<int> ids = new List<int>();

            foreach (GraphicsNode node in _graphics)
            {
                ids.Add(node.Id);
            }

            int freeId = 1;

            while (ids.Contains(freeId))
                ++freeId;

            return freeId;
        }

        /// <summary>
        /// Delete graphics collections own by that object
        /// </summary>
        /// <returns>void</returns>
        public void Delete(bool deleteData)
        {
            if (deleteData)
                if (_graphicsData != null)
                    _graphicsData.Delete();

            if (_graphics != null)
                _graphics.Delete();
        }
    }
}