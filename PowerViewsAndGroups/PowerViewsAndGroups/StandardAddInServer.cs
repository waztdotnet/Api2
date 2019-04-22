using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;
using System.Windows.Forms;
using PowerViewsAndGroups;
/// <summary>
/// This is the primary StandardAddInServer class that implements the ApplicationAddInServer interface
/// that all Inventor AddIns are required to implement. The communication between Inventor and
/// the AddIn is via the methods on this interface.
/// </summary>
namespace PowerViewsAndGroups
{

    [GuidAttribute("ff68a3bf-0498-45b6-9cdd-ddd9f316ce24")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {
        private const string DescriptionText = "Assembly Power Groups";
        private const string ToolTipText = "Assembly Power ViewGroups";
        private const string InternalName = "IN_AssemblyPowerGroupViews";
        private const string DisplayName = "PowerVGroups";
        private const string ControlDefinitions = "Autodesk:BrowserDemo:ButtonDef1";
        // private const string FileNameIcon2 = "C:\\Users\\rwarr\\source\\repos\\InventorViews\\InventorViews\\resources\\Icon1.ico";
        // private const string FileNameIcon1 = "C:\\Users\\rwarr\\source\\repos\\InventorViews\\InventorViews\\resources\\Icon2.ico";
        private const string ModelType = "Assembly";
        private const string ID_Tab = "id_TabAssemble";
        private const string RibbonInternalName = "id_PVG";
        private const string CommandBarsName = "Measure";

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

        Inventor.ButtonDefinition _buttonDef1;
        static string addInGuid = "ff68a3bf-0498-45b6-9cdd-ddd9f316ce24";
        public StandardAddInServer()
        {



        }

        #region ApplicationAddInServer Members

        //Loads With Inventor
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

            Icon oIcon32 = new Icon(PowerViewsAndGroups.Properties.Resources.Icon2, PowerViewsAndGroups.Properties.Resources.Icon2.Width, PowerViewsAndGroups.Properties.Resources.Icon2.Height);
            Icon oIcon16 = new Icon(PowerViewsAndGroups.Properties.Resources.Icon1, PowerViewsAndGroups.Properties.Resources.Icon1.Width, PowerViewsAndGroups.Properties.Resources.Icon1.Height);

            object oIPictureDisp32 = AxHostConverter.ImageToPictureDisp(oIcon32.ToBitmap());
            object oIPictureDisp16 = AxHostConverter.ImageToPictureDisp(oIcon16.ToBitmap());
            try
            {
                _buttonDef1 = m_inventorApplication.CommandManager.ControlDefinitions[ControlDefinitions] as ButtonDefinition;
            }
            catch (Exception)
            {
                _buttonDef1 = m_inventorApplication.CommandManager.ControlDefinitions.AddButtonDefinition(DisplayName,InternalName,CommandTypesEnum.kUpdateWithReferencesCmdType,
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
                        CommandBar m_CommandBar = m_inventorApplication.UserInterfaceManager.CommandBars[CommandBarsName];
                        m_CommandBar.Controls.AddButton(_buttonDef1, 0);
                    }
                }
                catch
                {
                    CommandBar oCommandBar = m_inventorApplication.UserInterfaceManager.CommandBars[CommandBarsName];
                    oCommandBar.Controls.AddButton(_buttonDef1, 0);
                }
            }

            _buttonDef1.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(_buttonDef1_OnExecute);
            MessageBox.Show("PGroups");
        }

        public void _buttonDef1_OnExecute(NameValueMap Context)
        {

            Inventor.Document Document = m_inventorApplication.ActiveDocument;
            if (Document.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)Document;

                IComDialog comDialog = new UIMain();
                comDialog.RunUI(m_inventorApplication);
                

            }
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

        // Note:this method is now obsolete
        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
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

        internal class AxHostConverter : AxHost
        {
            private AxHostConverter(): base("")
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
        #endregion

    }/////////////////////////////End class StandardAddInServer
}
