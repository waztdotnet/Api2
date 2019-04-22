using System;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using InvAddIn;
using Inventor;
using Microsoft.Win32;
using System.Drawing;






namespace InventorViewsServer
{

    //User Defined Interface Exposed through the Add-In Automation property 
    public interface IAutomationInterface
    {
        void Execute(String param);
        
        string GetParam();
    }
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("55488e4a-45de-4630-a00c-b31e0eb17ba8")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {
        private const string DescriptionText = "Ribbon Demo";
        private const string ToolTipText = "Ribbon Demo Description";
        private const string InternalName = "Autodesk:RibbonDemoC#:ButtonDef1";
        private const string DisplayName = "Ribbon Demo1";
        private const string ControlDefinitions = "Autodesk:BrowserDemo:ButtonDef1";
       // private const string FileNameIcon2 = "C:\\Users\\rwarr\\source\\repos\\InventorViews\\InventorViews\\resources\\Icon1.ico";
       // private const string FileNameIcon1 = "C:\\Users\\rwarr\\source\\repos\\InventorViews\\InventorViews\\resources\\Icon2.ico";
        private const string ModelType = "Part";
        private const string ID_Tab = "id_TabModel";
        private const string RibbonInternalName = "Autodesk:RibbonDemoC#:Panel1";
        private const string CommandBarsName = "PMxPartFeatureCmdBar";

        // Inventor application object.
        private Inventor.Application m_inventorApplication;
        Inventor.ButtonDefinition _buttonDef1;
        static string addInGuid = "be3ab243-6c43-4dd7-9272-06685bda91af";

        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.
            // Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application;

            // TODO: Add ApplicationAddInServer.Activate implementation.
            // e.g. event initialization, command creation etc.

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            string[] resources = assembly.GetManifestResourceNames();


           // Icon oIcon32 = new Icon(FileNameIcon2);
            Icon oIcon32 = new Icon(Properties.Resources.Icon2, Properties.Resources.Icon2.Width, Properties.Resources.Icon2.Height);
            Icon oIcon16 = new Icon(Properties.Resources.Icon1, Properties.Resources.Icon1.Width,Properties.Resources.Icon1.Height);
            //Icon oIcon16 = new Icon("Icon1.ico", Properties.Resources.Icon1.Width, Properties.Resources.Icon1.Height);
            object oIPictureDisp32 = AxHostConverter.ImageToPictureDisp(oIcon32.ToBitmap());
            object oIPictureDisp16 = AxHostConverter.ImageToPictureDisp(oIcon16.ToBitmap());

            try
            {
                _buttonDef1 = m_inventorApplication.CommandManager.ControlDefinitions[ControlDefinitions] as ButtonDefinition;
            }
            catch (Exception)
            {
                _buttonDef1 = m_inventorApplication.CommandManager.ControlDefinitions.AddButtonDefinition(DisplayName,
                                                          InternalName,
                                                          CommandTypesEnum.kEditMaskCmdType,
                                                          addInGuid,
                                                          DescriptionText,
                                                          ToolTipText,
                                                          oIPictureDisp16,
                                                          oIPictureDisp32,
                                                          ButtonDisplayEnum.kDisplayTextInLearningMode);

                CommandCategory cmdCat = m_inventorApplication.CommandManager.CommandCategories.Add(DescriptionText, InternalName, addInGuid);

                cmdCat.Add(_buttonDef1);
            }
            
            if (firstTime)
            {
                try
                {
                    if (m_inventorApplication.UserInterfaceManager.InterfaceStyle == InterfaceStyleEnum.kRibbonInterface)
                    {
                        Ribbon ribbon = m_inventorApplication.UserInterfaceManager.Ribbons[ModelType];

                        RibbonTab tab = ribbon.RibbonTabs[ID_Tab];

                        try
                        {
                            RibbonPanel panel = tab.RibbonPanels.Add(DescriptionText, RibbonInternalName, addInGuid, "", false);

                            CommandControl control1 = panel.CommandControls.AddButton(_buttonDef1, true, true, "", false);
                        }
                        catch //(Exception ex)
                        {

                        }
                    }
                    else
                    {
                        CommandBar oCommandBar = m_inventorApplication.UserInterfaceManager.CommandBars[CommandBarsName];
                        oCommandBar.Controls.AddButton(_buttonDef1, 0);
                    }
                }
                catch
                {
                    CommandBar oCommandBar = m_inventorApplication.UserInterfaceManager.CommandBars[CommandBarsName];
                    oCommandBar.Controls.AddButton(_buttonDef1, 0);
                }
            }

            _buttonDef1.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(_buttonDef1_OnExecute);

            System.Windows.Forms.MessageBox.Show("Registry - Free AddIn C# is loaded!");
            
            
        }

        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            m_inventorApplication = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public void Execute(String param)
        {
            System.Windows.Forms.MessageBox.Show("------------ Execute Command with param: " + param + " ------------", "Hello World AddIn");

            
        }
        public string GetParam()
        {
            string ret = "String Param from AutomationAddIn";

            return ret;
        }

        public void _buttonDef1_OnExecute(NameValueMap Context)
        {
            Inventor.Document Document = m_inventorApplication.ActiveDocument;
            if (Document.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                Inventor.PartDocument PartDocument = (Inventor.PartDocument)Document;
               // System.Windows.Forms.MessageBox.Show("Button clicked!!", "Ribbon Demo");
                SetReps mSetting = new SetReps();
                mSetting.Show();

                mSetting.SetSettings(ref PartDocument); 

    }
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

        internal class AxHostConverter : AxHost
        {
            private AxHostConverter()
                : base("")
            {
            }


            public static stdole.IPictureDisp ImageToPictureDisp(Image image)
            {
                return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
            }


            public static Image PictureDispToImage(stdole.IPictureDisp pictureDisp)
            {
                return GetPictureFromIPicture(pictureDisp);
            }
        }

    }
}
