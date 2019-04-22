using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvMirrorWFApplication
{
    public partial class Form1 : Form
    {
        private Inventor.Application InvApp = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                InvApp = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Starting a New Inventor Session");
            }
            if (InvApp == null)
            {
                Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                InvApp = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                InvApp.Visible = true;
            }
        }

        public void MirrorPart()
        {
            Inventor.AssemblyDocument AssemblyDocument;
            Inventor.ComponentDefinition ComponentDefinition;
            Inventor.ComponentOccurrence ComponentOccurrence;
            Inventor.Document Document = InvApp.ActiveDocument;
            Inventor.PartDocument PartDocument;


            if (Document.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                AssemblyDocument = (Inventor.AssemblyDocument)Document;
            Inventor.WorkPlane WorkPlane;

            WorkPlane = AssemblyDocument.ComponentDefinition.WorkPlanes["XZ Plane"];

            double XNormal;
            double YNormal;
            double ZNormal;

            Inventor.Plane Plane;

            Plane = WorkPlane.Plane;
                //Get the Normals
                XNormal = Plane.Normal.X;
                YNormal = Plane.Normal.Y;
                ZNormal = Plane.Normal.Z;


                //Mirroring matrix 
                Inventor.Matrix MirroringMatrix = InvApp.TransientGeometry.CreateMatrix();
                Inventor.Matrix MMatrix = InvApp.TransientGeometry.CreateMatrix();

                double[] MirroringMatrixData = new double[16];
                MirroringMatrixData[0] = 1 - 2 * XNormal * XNormal;
                MirroringMatrixData[1] = - 2 * XNormal * YNormal;
                MirroringMatrixData[2] = - 2 * XNormal * ZNormal;
                MirroringMatrixData[3] = 0;

                MirroringMatrixData[4] = - 2 * XNormal * YNormal;
                MirroringMatrixData[5] = 1 - 2 * YNormal * YNormal;
                MirroringMatrixData[6] = - 2 * ZNormal * YNormal;
                MirroringMatrixData[7] = 0;

                MirroringMatrixData[8] = - 2 * XNormal * ZNormal;
                MirroringMatrixData[9] = - 2 * ZNormal * YNormal;
                MirroringMatrixData[10] = 1 - 2 * ZNormal * ZNormal;
                MirroringMatrixData[11] = 0;

                MirroringMatrixData[12] = 0;
                MirroringMatrixData[13] = 0;
                MirroringMatrixData[14] = 0;
                MirroringMatrixData[15] = 1;


                //Hand over the marix Data
                MirroringMatrix.PutMatrixData(ref MirroringMatrixData);

                ComponentOccurrence = AssemblyDocument.ComponentDefinition.Occurrences[1];
                ComponentDefinition = ComponentOccurrence.Definition;

                MirroringMatrix.PostMultiplyBy(ComponentOccurrence.Transformation);

                AssemblyDocument.ComponentDefinition.Occurrences.AddByComponentDefinition(ComponentDefinition, MirroringMatrix);


            }
            else if (Document.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                PartDocument = (Inventor.PartDocument)Document;
            }

        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
           MirrorPart();
        }
    }
}

//            Sub MirrorPartInAss()

//    Dim oAssDoc As AssemblyDocument
//    Set oAssDoc = ThisApplication.ActiveDocument


//    'mirror plane 
//    Dim oMirrorWP As WorkPlane
//    Set oMirrorWP = oAssDoc.SelectSet(1)
//    Dim oPlane As Plane
//    Set oPlane = oMirrorWP.Plane

//    'get normal of the plane 
//    Dim oNormalX As Double
//    oNormalX = oPlane.Normal.X
//    Dim oNormalY As Double
//    oNormalY = oPlane.Normal.Y
//    Dim oNormalZ As Double
//    oNormalZ = oPlane.Normal.Z
//    Dim oMirrorMatrix As Matrix
//    Set oMirrorMatrix = ThisApplication.TransientGeometry.CreateMatrix()
//    Dim oMatrixData(15) As Double
//    oMatrixData(0) = 1 - 2 * oNormalX * oNormalX
//    oMatrixData(1) = -2 * oNormalX * oNormalY
//    oMatrixData(2) = -2 * oNormalX * oNormalZ
//    oMatrixData(3) = 0

//    oMatrixData(4) = -2 * oNormalX * oNormalY
//    oMatrixData(5) = 1 - 2 * oNormalY * oNormalY
//    oMatrixData(6) = -2 * oNormalZ * oNormalY
//    oMatrixData(7) = 0

//    oMatrixData(8) = -2 * oNormalX * oNormalZ
//    oMatrixData(9) = -2 * oNormalZ * oNormalY
//    oMatrixData(10) = 1 - 2 * oNormalZ * oNormalZ
//    oMatrixData(11) = 0

//    oMatrixData(12) = 0
//    oMatrixData(13) = 0
//    oMatrixData(14) = 0
//    oMatrixData(15) = 1

//    Call oMirrorMatrix.PutMatrixData(oMatrixData)

//    'get the first component 
//    Dim oOcc As ComponentOccurrence
//    Set oOcc = oAssDoc.ComponentDefinition.Occurrences(1)
//    'multiply with the transformation of the parent component 
//     oMirrorMatrix.PostMultiplyBy oOcc.Transformation
//    Dim oParentPartPath As String
//    oParentPartPath = oOcc.Definition.Document.FullFileName


//     ' Create a new part file to derive the  part in. 
//    Dim oPartDoc As PartDocument
//    Set oPartDoc = ThisApplication.Documents.Add(kPartDocumentObject, _
//                 ThisApplication.FileManager.GetTemplateFile(kPartDocumentObject))

//     ' Create a derived definition for the  part. 
//    Dim oDerivedPartDef As DerivedPartTransformDef
//    Set oDerivedPartDef = oPartDoc.ComponentDefinition.ReferenceComponents.DerivedPartComponents.CreateTransformDef(oParentPartPath)


//    ' Create the derived part. 
//    Call oPartDoc.ComponentDefinition.ReferenceComponents.DerivedPartComponents.Add(oDerivedPartDef)
//    'save the derived part. 
//    Call oPartDoc.SaveAs("c:\temp\mirrorPart.ipt", False)
//   'add the derived part as a component 
//    Call oAssDoc.ComponentDefinition.Occurrences.Add(oPartDoc.FullFileName, oMirrorMatrix)
//     oPartDoc.Close
//    'activate the assembly document 
//    oAssDoc.Activate
//End Sub