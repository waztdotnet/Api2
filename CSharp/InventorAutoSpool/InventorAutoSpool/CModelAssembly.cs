using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventorAutoSpool
{
    public class CModelAssembly
    {
        private Inventor.Application InvApp;
        private string szAssemblyFileNamePath;
        private IDictionary <string,string > ComponantFileNamePaths;
        private CModelFlange ModelPartFlange;
        private CModelPipe ModelPartPipe;

        public CModelAssembly()
        {

        }

        public void ModelMake(ref Inventor.Application InvInterface)
        {
            InvApp = InvInterface;
            Inventor.PartDocument PartDocument;
            Inventor.AssemblyDocument AssemblyDocument;
            AssemblyDocument = CreateNewAssemblyDocument();

            foreach (KeyValuePair <string ,string > name in ComponantFileNamePaths )
            {
                string fp = "";
                fp = name.Value; 

            }



        }
        private void AddModelPart() { }
        public void SetAssemblyComponantPaths(IDictionary<string, string> ComponantFilePaths)
        {   
            ComponantFileNamePaths = ComponantFilePaths;
        }

        private void SaveAssemblyFile(ref Inventor.AssemblyDocument AssemblyDocument)
        {
            InvApp.SilentOperation = true;
            if (System.IO.File.Exists(szAssemblyFileNamePath))
            {
                System.IO.File.Delete(szAssemblyFileNamePath);
            }
            AssemblyDocument.SaveAs(szAssemblyFileNamePath, false);
            AssemblyDocument.Close(true);
            InvApp.SilentOperation = false;

        }
        private void CreateConstraints(ref Inventor.AssemblyDocument AssemblyDocument, ref Inventor.ComponentOccurrence BaseComponentOccurrence, Inventor.ComponentOccurrence PositionComponentOccurrence, string BaseWorkPlaneName, string PositionMateWorkPlaneName, string Affix , string Seperator, string Suffix)
        {
            Inventor.WorkPlane BaseWorkPlane = null;
            Inventor.WorkPlane PositionWorkPlane = null;

            BaseWorkPlane = GetAssemblyDocumentWorkPlane(ref AssemblyDocument, "", "");
            PositionWorkPlane = GetAssemblyDocumentWorkPlane(ref AssemblyDocument, "", "");
            if(BaseWorkPlane.Type == Inventor.ObjectTypeEnum .kWorkPlaneObject && PositionWorkPlane.Type == Inventor.ObjectTypeEnum.kWorkPlaneObject)
            {

            }
        }
        private Inventor.WorkPlane GetWorkPlaneOccurrance(ref Inventor.ComponentOccurrence   ComponentOccurrence, string WorkPlaneAffix, string WorkPlaneSuffix)
        {
            Inventor.WorkPlane DefaultWorkPlane = null;
            if (WorkPlaneSuffix != "")
            {
                foreach (Inventor.WorkPlane WorkPlane in ComponentOccurrence.Definition.Document  .WorkPlanes)
                {
                    if (WorkPlane.Name == WorkPlaneAffix + " Plane")
                    {
                        return WorkPlane;
                    }
                }
                System.Windows.Forms.MessageBox.Show("Work Plane Not Fiound Affix: " + WorkPlaneAffix);
                return DefaultWorkPlane;
            }
            else
            {
                foreach (Inventor.WorkPlane WorkPlane in ComponentOccurrence.Definition.Document.WorkPlanes)
                {
                    if (WorkPlane.Name == WorkPlaneAffix + " " + WorkPlaneSuffix)
                    {
                        return WorkPlane;
                    }
                }
                System.Windows.Forms.MessageBox.Show("Work Plane Not Fiound: Affix " + WorkPlaneAffix + " Suffix " + WorkPlaneSuffix);
                return DefaultWorkPlane;
            }

        }
        private Inventor.WorkPlane GetAssemblyDocumentWorkPlane(ref Inventor.AssemblyDocument AssemblyDocument, string WorkPlaneAffix, string WorkPlaneSuffix)
        {
            Inventor.WorkPlane DefaultWorkPlane = null;
            if(WorkPlaneSuffix !="")
            { 
            foreach (Inventor.WorkPlane WorkPlane in AssemblyDocument.ComponentDefinition.WorkPlanes)
            {
                if (WorkPlane.Name == WorkPlaneAffix + " Plane")
                {
                    return WorkPlane;
                }
            }
                System.Windows.Forms.MessageBox.Show("Work Plane Not Fiound Affix: "+WorkPlaneAffix ); 
            return DefaultWorkPlane;
            } 
            else
            {
                    foreach (Inventor.WorkPlane WorkPlane in AssemblyDocument.ComponentDefinition.WorkPlanes)
                    {
                        if (WorkPlane.Name == WorkPlaneAffix + " " + WorkPlaneSuffix)
                        {
                            return WorkPlane;
                        }
                    }
                System.Windows.Forms.MessageBox.Show("Work Plane Not Fiound: Affix " + WorkPlaneAffix +" Suffix " + WorkPlaneSuffix );
                return DefaultWorkPlane;
            }

        }
        private void CreateAssemblyiMateConstraint(ref Inventor.AssemblyComponentDefinition AssemblyComponentDefinition, ref Inventor.ComponentOccurrence BaseComponentOccurrence, Inventor.ComponentOccurrence PositionComponentOccurrence,string BaseMateName,string PositionMateName,string affix)
        {
            bool BaseMateFound = false;
            bool PositionMateFound = false;
            Inventor.iMateDefinition BaseMateDefinition = null;
            Inventor.iMateDefinition PositionMateDefinition = null;
            Inventor.iMateResult iMateResult = null;

            foreach (Inventor.iMateDefinition MateDefinition in BaseComponentOccurrence.iMateDefinitions)
            {
                if(MateDefinition.Name == BaseMateName +":"+ affix )
                {
                    BaseMateDefinition = MateDefinition;
                    BaseMateFound = true;
                }

            }

            foreach (Inventor.iMateDefinition MateDefinition in PositionComponentOccurrence.iMateDefinitions)
            {
                if (MateDefinition.Name == BaseMateName + ":" + affix)
                {
                    PositionMateDefinition = MateDefinition;
                    PositionMateFound = true;
                }

            }

            if (BaseMateFound && PositionMateFound )
            {
                iMateResult= AssemblyComponentDefinition.iMateResults.AddByTwoiMates(BaseMateDefinition, PositionMateDefinition);  
            }


        }
        private Inventor.WorkPlane GetAssemblyDocumentWorkPlane(ref Inventor.AssemblyDocument AssemblyDocument, string WorkPlaneSuffix)
        {
            Inventor.WorkPlane DefaultWorkPlane = AssemblyDocument.ComponentDefinition.WorkPlanes[3];

            foreach (Inventor.WorkPlane WorkPlane in AssemblyDocument.ComponentDefinition.WorkPlanes)
            {
                if (WorkPlane.Name == WorkPlaneSuffix + " Plane")
                {
                    return WorkPlane;
                }
            }
            return DefaultWorkPlane;
        }
        public string GetFileSavePath()
        {
            return szAssemblyFileNamePath;
        }
        public void SetFileSavePath(string FilePath, string FileName)
        {
            szAssemblyFileNamePath = FilePath + "\\" + FileName + ".iam";
        }
        private Inventor.AssemblyDocument CreateNewAssemblyDocument()
        {
            Inventor.AssemblyDocument AssemblyDocument = null;
            AssemblyDocument = (Inventor.AssemblyDocument)InvApp.Documents.Add(Inventor.DocumentTypeEnum.kAssemblyDocumentObject, InvApp.FileManager.GetTemplateFile(Inventor.DocumentTypeEnum.kPartDocumentObject, Inventor.SystemOfMeasureEnum.kMetricSystemOfMeasure), true);
            return AssemblyDocument;
        }

    }
}