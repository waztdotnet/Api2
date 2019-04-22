using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsDrawApp
{
    public class CDXFExport
    {
        private  Inventor.Application mInvApplication = null;
        private CDrawingView FaceViews;
        private string FileSaveUrl = "";
        private string PartNumberID = "";
        private System.Windows.Forms.ListBox ListComponents;

        public CDXFExport(string PartNumberIdent, string FileSaveLocation, ref Inventor.Application InvApplication)
        {
            mInvApplication = InvApplication;
 
            PartNumberID = PartNumberIdent;
            
            FaceViews = new CDrawingView(ref InvApplication);
            Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)mInvApplication.ActiveDocument;
            GetAllPartsInBom(ref AssemblyDocument);
        }

        public CDXFExport(ref System.Windows.Forms.ListBox ListBoxComponents, string PartNumberIdent, string FileSaveLocation, ref Inventor.Application InvApplication)
        {
            mInvApplication = InvApplication;
            ListComponents = ListBoxComponents;
            PartNumberID = PartNumberIdent;

            FaceViews = new CDrawingView(ref InvApplication);
            Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)mInvApplication.ActiveDocument;
            GetAllPartsInBom(ref AssemblyDocument);
        }

        private void GetAllPartsInBom(ref Inventor.AssemblyDocument AssemblyDocument)
        {
            Inventor.BOM bOM = AssemblyDocument.ComponentDefinition.BOM;
            bOM.PartsOnlyViewEnabled = true;
            bOM.StructuredViewEnabled = true;

            Inventor.BOMView bOMViews = AssemblyDocument.ComponentDefinition.BOM.BOMViews[3];
            Inventor.BOMRowsEnumerator bOMRows = bOMViews.BOMRows;

            int length = bOMRows.Count;
            for (int i = 1; i <= length; i++)
            {
                Inventor.BOMRow bOMRow = bOMViews.BOMRows[i];
                if (true)
                {
                    
                    Inventor.Document Document = bOMRow.ComponentDefinitions[1].Document;

                    if (Document.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
                    {
                        string PartNumber = "";
                        PartNumber = Document.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value;

                        if (CFileNames.IsPartNumberPreFixsMatch(PartNumber, PartNumberID))
                        {
                            //Inventor.PartDocument PartDocument = (Inventor.PartDocument)Document;
                            Inventor.PartDocument PartDocument = (Inventor.PartDocument)mInvApplication.Documents.Open(Document.FullFileName, true);
                            if (PartDocument.Open)
                            {
                                FileSaveUrl = PartDocument.FullFileName.Substring(0, PartDocument.FullFileName.LastIndexOf("."));
                                

                                if ((!PartDocument.ComponentDefinition.IsContentMember) || !PartDocument.ComponentDefinition.IsiPartFactory || !PartDocument.ComponentDefinition.IsiPartMember)
                                {
                                    //check for Flat Pattern
                                    if (PartDocument.SubType == "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                                    {
                                        DXFExport(FileSaveUrl, PartDocument);
                                        ListComponents.Items.Add("SM Qty " + bOMRow.ItemQuantity.ToString() + " Part * " + PartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value + " * ");

                                    }
                                    else
                                    {
                                        //if (true)
                                        //{
                                        //    logWriter.WriteAsync(PartNumber); 
                                        //}
                                        DoCommandExportDXF(PartDocument);
                                        ListComponents.Items.Add("Cmmd Qty " + bOMRow.ItemQuantity.ToString() + " Part * " + PartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value + " * ");

                                    }
                                }
                                // ListComponents.Items.Add("Qty " + bOMRow.ItemQuantity.ToString() + " Part * " + PartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value + " * ");

                            }
                            else
                            {

                            }
                            PartDocument.Close(false);
                            PartDocument = null;
                        }

                    }
                    else if (Document.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        Inventor.AssemblyDocument AssemDocument = (Inventor.AssemblyDocument)Document;
                        //ListComponents.Items.Add("Qty " + bOMRow.ItemQuantity.ToString() + " Part * " + AssemDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value + " * ");
                        ListComponents.Items.Add("Qty " + bOMRow.ItemQuantity.ToString() + " Part * " + AssemDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value + " * ");

                    }
                    Document = null;


                }

            }



        }
        private void DXFExport(string filenamefull, Inventor.PartDocument mPartDocument)
        {
            Inventor.DataIO oData = mPartDocument.ComponentDefinition.DataIO;
            oData.WriteDataToFile("FLAT PATTERN DXF?AcadVersion=R12&OuterProfileLayer=Outer", filenamefull + ".dxf");
        }

        private void DoCommandExportDXF(Inventor.PartDocument mPartDocument)
        {
            Inventor.Face Face = null;
           
            Face = FaceViews.GetBiggestFace(mPartDocument.ComponentDefinition.SurfaceBodies[1]);

            if (Face != null)
            {

                Inventor.ControlDefinition Definition;
                Inventor.CommandManager CmdManager = mInvApplication.CommandManager;
                CmdManager.PostPrivateEvent(Inventor.PrivateEventTypeEnum.kFileNameEvent, FileSaveUrl + ".dxf");
                mPartDocument.SelectSet.Clear();
                mPartDocument.SelectSet.Select(Face);

                Definition = CmdManager.ControlDefinitions["GeomToDXFCommand"];
                Definition.Execute2(true);

                CmdManager.ClearPrivateEvents();
                mPartDocument.SelectSet.Clear();

            }
        }
    }
}