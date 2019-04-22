namespace WindowsDrawApp
{
    internal class CExportDXF
    {
        private Inventor.Application mInvApplication = null;
        private CDrawingView FaceViews;

        private string PartNumberID;
      //  private System.IO.StreamWriter logWriter;
        private string FileSaveUrl;
        private bool IsFileNameQtyAdded;

        public CExportDXF(string PartNumberPreFixIdent, string FileSaveLocation, ref Inventor.Application InvApplication)
        {
            mInvApplication = InvApplication;
            FileSaveUrl = "";
            IsFileNameQtyAdded = false;
            FaceViews = new CDrawingView(ref InvApplication);
            //logWriter = new System.IO.StreamWriter("E:\\LogParts.txt");
            if (PartNumberPreFixIdent != "")
            {
                PartNumberID = PartNumberPreFixIdent;
            }
            else
            {
                PartNumberID = "P";
            }

            if (FileSaveLocation != "")
            {
                FileSaveUrl = FileSaveLocation;
            }
            else
            {
                FileSaveUrl = "";
            }

            

        }

        ~CExportDXF()
        {
           // logWriter.Close();
        }

        /// <param name="SetPartNumberPreFixIdent">Set PartNumber PreFix Ident Default To P </param>
        public void SetPartNumberPreFixIdent(string PartNumberPreFixIdent)
        {
            if (PartNumberPreFixIdent != "")
            {
                PartNumberID = PartNumberPreFixIdent;
            }
            else
            {
                PartNumberID = "P";
            }
        }

        /// <param name="FileSaveLocation">File Save Location if Null string Save In Part Folder</param>
        public void SetFileSaveLocation(string FileSaveLocation)
        {
            if (FileSaveLocation != "")
            {
                FileSaveUrl = FileSaveLocation;
            }
            else
            {
                FileSaveUrl = "";
            }
        }

        private void DXFExport(string filenamefull, Inventor.PartDocument mPartDocument)
        {
            Inventor.DataIO oData = mPartDocument.ComponentDefinition.DataIO;
            oData.WriteDataToFile("FLAT PATTERN DXF?AcadVersion=R12&OuterProfileLayer=Outer", filenamefull + ".dxf");
        }

        public void IsQtyAdded(bool IsAdded)
        {
            IsFileNameQtyAdded = IsAdded;
        }

        public void GetFileRefs()
        {
            Inventor.File pFile = (Inventor.File)mInvApplication.ActiveDocument.File;

            ProcessFileRefs(pFile);
            
        }

        private void ProcessFileRefs(Inventor.File PartFile)
        {
            FileSaveUrl = "";
            
            foreach (Inventor.FileDescriptor FileDescriptor in PartFile.ReferencedFileDescriptors)
            {
                if (!FileDescriptor.ReferenceMissing)
                {
                    if (FileDescriptor.ReferencedFileType != Inventor.FileTypeEnum.kUnknownFileType || FileDescriptor.ReferencedFileType != Inventor.FileTypeEnum.kForeignFileType)
                    {
                        Inventor.File File = FileDescriptor.ReferencedFile;
                        if (File.Type != Inventor.ObjectTypeEnum.kContentCenterObject)
                        {
                            if (FileDescriptor.ReferencedFileType == Inventor.FileTypeEnum.kPartFileType)
                            {
                                string PartNumber = "";
                                string PartQty = "-Qty";
                                // FileDescriptor.
                                Inventor.PartDocument mPartDocument = (Inventor.PartDocument)mInvApplication.Documents.Open(FileDescriptor.FullFileName, true);
                                PartNumber = mPartDocument.PropertySets["{32853F0F-3444-11d1-9E93-0060B03C1CA6}"].get_ItemByPropId((int)Inventor.PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties).Value;
                                PartQty += mPartDocument.PropertySets["{F29F85E0-4FF9-1068-AB91-08002B27B3D9}"].get_ItemByPropId((int)Inventor.PropertiesForSummaryInformationEnum.kKeywordsSummaryInformation).Value;

                                if (CFileNames.IsPartNumberPreFixsMatch(PartNumber, PartNumberID))
                                {
                                    if (FileSaveUrl == "")
                                    {
                                        if (!IsFileNameQtyAdded)
                                        {
                                            FileSaveUrl = FileDescriptor.FullFileName.Substring(0, FileDescriptor.FullFileName.LastIndexOf("."));
                                        }
                                        else
                                        {
                                            FileSaveUrl = FileDescriptor.FullFileName.Substring(0, FileDescriptor.FullFileName.LastIndexOf("."));
                                            FileSaveUrl = CFileNames.AppendAffixToFileNameInFullFileName(FileSaveUrl, "-", PartQty);
                                        }

                                        //check for Flat Pattern
                                        if (mPartDocument.SubType == "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                                        {
                                            DXFExport(FileSaveUrl, mPartDocument);
                                        }
                                        else
                                        {
                                            //if (true)
                                            //{
                                            //    logWriter.WriteAsync(PartNumber); 
                                            //}
                                            DoCommandExportDXF(mPartDocument);
                                        }
                                    }
                                    else //with user folder
                                    {
                                        CFileNames.GetFolderFromFullFileName(FileDescriptor.FullFileName, ref FileSaveUrl, false);
                                        FileSaveUrl += PartNumber;
                                        //check for Flat Pattern
                                        if (mPartDocument.SubType == "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                                        {
                                            DXFExport(FileSaveUrl, mPartDocument);
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                mPartDocument.Close(false);
                                mPartDocument = null;


                            } 
                        }
                        ProcessFileRefs(FileDescriptor.ReferencedFile);
                    }
                }
            }
            
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