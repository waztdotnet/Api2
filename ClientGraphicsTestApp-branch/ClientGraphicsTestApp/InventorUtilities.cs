using Inventor;
using System;
using System.Collections.Generic;

namespace UtilitysLib
{
    /// <summary>
    /// General purpose Inventor API utility class
    /// </summary>
    /// <returns></returns>
    public class InventorUtilities
    {
        private static double _Tolerance = 0.0001;

        public static string AddInGuid
        {
            get;
            internal set;
        }

        public static Inventor.Application InvApplication
        {
            get;
            internal set;
        }

        public static void Initialize(Inventor.Application Application, Type addinType)
        {
            InvApplication = Application;

            AddInGuid = addinType.GUID.ToString("B");
        }

        public static double[] ToArray(Point point)
        {
            return new double[] { point.X, point.Y, point.Z };
        }

        /// <summary>
        /// Late-binded method to retrieve ObjectType property
        /// </summary>
        /// <returns>ObjectTypeEnum</returns>
        public static ObjectTypeEnum GetInventorType(Object obj)
        {
            try
            {
                System.Object objType = obj.GetType().InvokeMember(
                    "Type",
                    System.Reflection.BindingFlags.GetProperty,
                    null,
                    obj,
                    null,
                    null, null, null);

                return (ObjectTypeEnum)objType;
            }
            catch
            {
                //error...
                return ObjectTypeEnum.kGenericObject;
            }
        }

        /// <summary>
        /// Late-binded method to get object property based on name.
        /// </summary>
        /// <returns>System.Object</returns>
        public static System.Object GetProperty(System.Object obj, string property)
        {
            try
            {
                System.Object objType = obj.GetType().InvokeMember(
                    property,
                    System.Reflection.BindingFlags.GetProperty,
                    null,
                    obj,
                    null,
                    null, null, null);

                return objType;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns ComponentDefinition for a part or assembly
        /// </summary>
        /// <returns>ComponentDefinition</returns>
        public static ComponentDefinition GetCompDefinition(Document document)
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
        /// Returns Point object from input entity. Supports Vertex, WorkPoint
        /// </summary>
        /// <returns>Point</returns>
        public static Point GetPoint(object entity)
        {
            ObjectTypeEnum type = GetInventorType(entity);

            switch (type)
            {
                case ObjectTypeEnum.kVertexObject:
                case ObjectTypeEnum.kVertexProxyObject:

                    Vertex vertex = entity as Vertex;
                    return vertex.Point;

                case ObjectTypeEnum.kWorkPointObject:
                case ObjectTypeEnum.kWorkPointProxyObject:

                    WorkPoint workpoint = entity as WorkPoint;
                    return workpoint.Point;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns Plane object from input entity. Supports Face, Workplane and Faces.
        /// </summary>
        /// <returns>Plane</returns>
        public static Plane GetPlane(object planarEntity)
        {
            ObjectTypeEnum type = GetInventorType(planarEntity);

            switch (type)
            {
                case ObjectTypeEnum.kFaceObject:
                case ObjectTypeEnum.kFaceProxyObject:

                    Face face = planarEntity as Face;
                    return face.Geometry as Plane;

                case ObjectTypeEnum.kWorkPlaneObject:
                case ObjectTypeEnum.kWorkPlaneProxyObject:

                    WorkPlane workplane = planarEntity as WorkPlane;
                    return workplane.Plane;

                case ObjectTypeEnum.kFacesObject:

                    Face face1 = (planarEntity as Faces)[1];
                    return face1.Geometry as Plane;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns Normal as UnitVetor for different type of input entities.
        /// </summary>
        /// <returns>UnitVector</returns>
        public static UnitVector GetNormal(object planarEntity)
        {
            ObjectTypeEnum type = GetInventorType(planarEntity);

            switch (type)
            {
                case ObjectTypeEnum.kFaceObject:
                case ObjectTypeEnum.kFaceProxyObject:

                    Face face = planarEntity as Face;
                    return GetFaceNormal(face);

                case ObjectTypeEnum.kWorkPlaneObject:
                case ObjectTypeEnum.kWorkPlaneProxyObject:

                    WorkPlane workplane = planarEntity as WorkPlane;
                    return workplane.Plane.Normal;

                case ObjectTypeEnum.kFacesObject:

                    Face face1 = (planarEntity as Faces)[1];
                    return GetFaceNormal(face1);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns Normal as UnitVector for input Face..
        /// </summary>
        /// <returns>UnitVector</returns>
        public static UnitVector GetFaceNormal(Face face, Point point)
        {
            SurfaceEvaluator evaluator = face.Evaluator;

            double[] points = { point.X, point.Y, point.Z };

            double[] guessParams = new double[2];
            double[] maxDev = new double[2];
            double[] Params = new double[2];
            SolutionNatureEnum[] sol = new SolutionNatureEnum[2];

            evaluator.GetParamAtPoint(ref points, ref guessParams, ref maxDev, ref Params, ref sol);

            double[] normal = new double[3];

            evaluator.GetNormal(ref Params, ref normal);

            return InvApplication.TransientGeometry.CreateUnitVector(
                normal[0], normal[1], normal[2]);
        }

        /// <summary>
        /// Returns Normal as UnitVector for input Face..
        /// </summary>
        /// <returns>UnitVector</returns>
        public static UnitVector GetFaceNormal(Face face)
        {
            SurfaceEvaluator evaluator = face.Evaluator;

            double[] points = { face.PointOnFace.X, face.PointOnFace.Y, face.PointOnFace.Z };

            double[] guessParams = new double[2];
            double[] maxDev = new double[2];
            double[] Params = new double[2];
            SolutionNatureEnum[] sol = new SolutionNatureEnum[2];

            evaluator.GetParamAtPoint(ref points, ref guessParams, ref maxDev, ref Params, ref sol);

            double[] normal = new double[3];

            evaluator.GetNormal(ref Params, ref normal);

            return InvApplication.TransientGeometry.CreateUnitVector(
                normal[0], normal[1], normal[2]);
        }

        /// <summary>
        /// Returns an UnitVector orthogonal to input vector
        /// </summary>
        /// <returns>UnitVector</returns>
        public static UnitVector GetOrthoVector(UnitVector vector)
        {
            if (Math.Abs(vector.Z) < _Tolerance)
            {
                return InvApplication.TransientGeometry.CreateUnitVector(0, 0, 1);
            }
            else if (Math.Abs(vector.Y) < _Tolerance)
            {
                return InvApplication.TransientGeometry.CreateUnitVector(0, 1, 0);
            }
            else
            {
                //Expr: - xx'/y = y'
                return InvApplication.TransientGeometry.CreateUnitVector(1, -vector.X / vector.Y, 0);
            }
        }

        /// <summary>
        /// Returns two orthogonal vectors depending on the input normal
        /// </summary>
        /// <returns>Void</returns>
        public static void GetOrthoBase(UnitVector normal, out UnitVector xAxis, out UnitVector yAxis)
        {
            xAxis = GetOrthoVector(normal);

            yAxis = normal.CrossProduct(xAxis);
        }

        /// <summary>
        /// Projects input point onto input plane and returns projected point
        /// </summary>
        /// <returns>Point</returns>
        public static Point ProjectOnPlane(Point point, Plane plane)
        {
            try
            {
                MeasureTools measureTools = InvApplication.MeasureTools;

                double minDist;
                object contextObj = null;

                minDist = measureTools.GetMinimumDistance((object)point,
                    plane,
                    InferredTypeEnum.kNoInference,
                    InferredTypeEnum.kNoInference,
                    ref contextObj);

                NameValueMap context = contextObj as NameValueMap;

                Point projectedPoint = context.get_Item(context.Count < 3 ? 2 : 3) as Point;

                return projectedPoint;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Read attribute and returns its value in out parameter.
        /// Returns true if attribute exists, false otherwise
        /// </summary>
        /// <returns>bool</returns>
        public static bool ReadAttribute(object target, string setName, string attName, out object value, out ValueTypeEnum type)
        {
            value = null;
            type = ValueTypeEnum.kIntegerType;

            try
            {
                AttributeSets sets = InventorUtilities.GetProperty(target, "AttributeSets") as AttributeSets;

                if (sets == null)
                    return false;

                if (!sets.get_NameIsUsed(setName))
                    return false;

                AttributeSet set = sets[setName];

                if (!set.get_NameIsUsed(attName))
                    return false;

                Inventor.Attribute att = set[attName];

                type = att.ValueType;

                value = att.Value;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns parameter value as string
        /// </summary>
        /// <returns>String</returns>
        public static string GetStringFromValue(Parameter parameter)
        {
            try
            {
                if (parameter.Value is string || parameter.Value is bool)
                    return parameter.Value.ToString();

                return InvApplication.UnitsOfMeasure.GetStringFromValue(parameter.ModelValue, parameter.get_Units());
            }
            catch
            {
                return "*Error*";
            }
        }

        /// <summary>
        /// Return string from API value
        /// </summary>
        /// <returns>String</returns>
        public static string GetStringFromAPILength(double value)
        {
            try
            {
                UnitsOfMeasure uom = InvApplication.ActiveDocument.UnitsOfMeasure;

                string strValue = uom.GetStringFromValue(value, UnitsTypeEnum.kDefaultDisplayLengthUnits);

                return strValue;
            }
            catch
            {
                return "*Error*";
            }
        }

        /// <summary>
        /// Create a new derived PartDocument from a ComponentDefinition (asm or part)
        /// </summary>
        /// <returns>PartDocument</returns>
        public static PartDocument DeriveComponent(ComponentDefinition compDef)
        {
            Inventor.Application InvApp = InventorUtilities.InvApplication;

            PartDocument derivedDoc = InvApp.Documents.Add(
                    DocumentTypeEnum.kPartDocumentObject,
                    InvApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject,
                       SystemOfMeasureEnum.kDefaultSystemOfMeasure,
                       DraftingStandardEnum.kDefault_DraftingStandard,
                       null),
                   false) as PartDocument;

            if (compDef.Type == ObjectTypeEnum.kAssemblyComponentDefinitionObject)
            {
                DerivedAssemblyComponents derAsmComps =
                    derivedDoc.ComponentDefinition.ReferenceComponents.DerivedAssemblyComponents;

                DerivedAssemblyDefinition derAsmDef = derAsmComps.CreateDefinition(
                        (compDef.Document as Document).FullFileName);

                derAsmDef.InclusionOption = DerivedComponentOptionEnum.kDerivedIncludeAll;

                derAsmComps.Add(derAsmDef);

                return derivedDoc;
            }

            if (compDef.Type == ObjectTypeEnum.kPartComponentDefinitionObject)
            {
                DerivedPartComponents derPartComps =
                    derivedDoc.ComponentDefinition.ReferenceComponents.DerivedPartComponents;

                DerivedPartUniformScaleDef derPartDef = derPartComps.CreateDefinition(
                       (compDef.Document as Document).FullFileName);

                derPartDef.IncludeAll();

                derPartComps.Add(derPartDef as DerivedPartDefinition);

                return derivedDoc;
            }

            derivedDoc.Close(true);

            return null;
        }

        /// <summary>
        /// Returns a collection of transient bodies transformed in the context of the assembly
        /// (if compDef is an assembly CompDef).
        /// The Key of the dictionary is the original body, the value is the transformed transient body
        /// </summary>
        /// <returns>Dictionary<SurfaceBody, SurfaceBody></returns>
        public static Dictionary<SurfaceBody, SurfaceBody> GetTransientBodies(ComponentDefinition compDef)
        {
            Dictionary<SurfaceBody, SurfaceBody> bodies = new Dictionary<SurfaceBody, SurfaceBody>();

            if (compDef.Type == ObjectTypeEnum.kAssemblyComponentDefinitionObject)
            {
                foreach (ComponentOccurrence occurrence in compDef.Occurrences)
                {
                    if (occurrence.DefinitionDocumentType == DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        Dictionary<SurfaceBody, SurfaceBody> bodiesRec =
                            GetTransientBodies(occurrence.Definition);

                        foreach (SurfaceBody key in bodiesRec.Keys)
                        {
                            SurfaceBody bodyCpy = InventorUtilities.InvApplication.TransientBRep.Copy(bodiesRec[key]);

                            InventorUtilities.InvApplication.TransientBRep.Transform(
                                bodyCpy,
                                occurrence.Transformation);

                            bodies.Add(key, bodyCpy);
                        }
                    }
                    else
                    {
                        foreach (SurfaceBody body in occurrence.SurfaceBodies)
                        {
                            SurfaceBody bodyCpy = InventorUtilities.InvApplication.TransientBRep.Copy(body);

                            InventorUtilities.InvApplication.TransientBRep.Transform(
                                bodyCpy,
                                occurrence.Transformation);

                            bodies.Add(body, bodyCpy);
                        }
                    }
                }
            }
            else
            {
                foreach (SurfaceBody body in compDef.SurfaceBodies)
                {
                    SurfaceBody bodyCpy =
                        InventorUtilities.InvApplication.TransientBRep.Copy(body);

                    bodies.Add(body, bodyCpy);
                }
            }

            return bodies;
        }
    }

    public enum SupportedSoftwareVersionEnum
    {
        kSupportedSoftwareVersionLessThan,
        kSupportedSoftwareVersionGreaterThan,
        kSupportedSoftwareVersionEqualTo,
        kSupportedSoftwareVersionNotEqualTo
    }
}