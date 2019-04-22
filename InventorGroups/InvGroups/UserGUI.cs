using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvGroups
{
    public partial class UserGUI : Form
    {
        Inventor.Application m_InvApplication;
        //bool _started = false;
        bool bInventorVisable = true;
        private CInventorInputEvents InputEvents;
        private Inventor.ObjectsEnumerator UIJustSelectedObjects;
        private Inventor.AssemblyDocument UIAssemblyDocument;
        //CGroup Group;
        //String NamedGroup = "GroupObject";
        //String Number = "01";
        public UserGUI()
        {
            InitializeComponent();
            try
            {
                m_InvApplication = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");

            }
            catch
            {
                if (System.Windows.Forms.MessageBox.Show("Trying to Open a new instance of Autodesk Inventor", "Inventor Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return; //To Caller
                }
            }
            if (m_InvApplication == null)
            {
                try
                {
                    Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                    m_InvApplication = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                    m_InvApplication.Visible = bInventorVisable;
                   // _started = true;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Can Not Open Autodesk Inventor " + Environment.NewLine + "See Your Administrator", "Instalation Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Question);
                  //  _started = false;
                    return;
                }

            }
            //_started = true;
           // Group = new CGroup();
            InputEvents = new CInventorInputEvents(m_InvApplication);

            InputEvents.OnSelectionChange += new CInventorInputEvents.CInventorEvents_OnSelect_DelegateHandler(InputEvents_OnSelectionChange);
            UIJustSelectedObjects = null;
            UIAssemblyDocument = null;
            GetApplicationHighLightColour();
        }

        private void GetApplicationHighLightColour()
        {
            object obj = "314DE259-5443-4621-BFBD-1730C6CC9AE9";
            Inventor.AssetLibrary assetLibrary = m_InvApplication.AssetLibraries[obj];
            Inventor.AssetCategories assetCategories = assetLibrary.AppearanceAssetCategories;
            int AssetCatlength = assetCategories.Count;
            for (int i = 1; i <= AssetCatlength; i++)
            {
                CmbAssetCat.Items.Add(assetCategories[i].DisplayName);
                
            }
            CmbAssetCat.SelectedIndex = 1;

        }


        private void InputEvents_OnSelectionChange(ref Inventor.AssemblyDocument AssemblyDocument, ref Inventor.ObjectsEnumerator JustSelected)
        {
            if (AssemblyDocument != null || JustSelected != null)
            {
                UIJustSelectedObjects = JustSelected;
                UIAssemblyDocument = AssemblyDocument; 
            }
        }

        private static void GetArrays()
        {


        }

        private static void AddAttributes(ref Inventor.ObjectsEnumerator JustSelected,ref Inventor.AssemblyDocument assemblyDocument)
        {
            int length = JustSelected.Count;
            for (int i = length - 1; i >= 1; i--)
            {
                if(GetInventorObjType(JustSelected[i])== Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                {
                    System.Diagnostics.Debug.WriteLine("Just Selected");
                    Inventor.ComponentOccurrence componentOccurrence = JustSelected[i];
                    ColourOccurance(ref assemblyDocument,ref componentOccurrence, "Red");

                }
                else if(GetInventorObjType(JustSelected[i]) == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                {
                    Inventor.OccurrencePattern OccurrencePattern = JustSelected[i];
                    int PatternLength = 10;
                    //OccurrencePattern.OccurrencePatternElements.
                    for (int x = PatternLength - 1; x >= 1; x--)
                    {

                    }
                }
            }
        }



        private static Inventor.ObjectTypeEnum GetInventorObjType(object objUnKnown)
        {
            System.Type invokeType = objUnKnown.GetType();

            object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);

            Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;

            return objTypeEum;
        }
        //switch (GetInventorObjType(JustSelected[i]))
        //{
        //    case Inventor.ObjectTypeEnum.kComponentOccurrenceObject:

        //        //Inventor.ComponentOccurrence ComponentOccurrence = (Inventor.ComponentOccurrence)objUnKnown;
        //        //ColourIt(AssemblyDocument, ComponentOccurrence);
        //        //System.Diagnostics.Debug.WriteLine(ComponentOccurrence.Name);
        //        return objTypeEum;
        //        break;
        //    case Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject:

        //        //Inventor.RectangularOccurrencePattern RectangularOccurrencePattern = (Inventor.RectangularOccurrencePattern)objUnKnown;

        //        //foreach (var Ritem in RectangularOccurrencePattern.OccurrencePatternElements)
        //        //{
        //        //    Inventor.OccurrencePatternElement occurrencePatternElement = (Inventor.OccurrencePatternElement)Ritem;
        //        //    foreach (var Occitem in occurrencePatternElement.Occurrences)
        //        //    {

        //        //        Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)Occitem;
        //        //        ColourIt(AssemblyDocument, componentOccurrence);

        //        //    }
        //        //}
        //        break;


        //    default:
        //        break;
        //}
        //foreach (object item in JustSelectedEntities)
        //{
        //    ObjType(item);
        //    System.Diagnostics.Debug.WriteLine("Just Selected");
        //}

        //if (MoreSelectedEntities != null)
        //{
        //    foreach (object item in MoreSelectedEntities)
        //    {
        //        ObjType(item);
        //        System.Diagnostics.Debug.WriteLine("More Selected");
        //    }
        //}
        //Inventor.AssemblyDocument AssemblyDocument,
        //string Colour = "Red";
        //ColourOccurance(AssemblyDocument, componentOccurrence, Colour);
        //,Inventor.AssemblyDocument AssemblyDocument
        private static void AddAttributeSets(ref Inventor.ComponentOccurrence componentOccurrence, string AttributeSets_Name, string Attribute_Name, string Attribute_Value)
        {
            if (AttributeSets_Name != null || Attribute_Name != null)
            {
                Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
                if (!m_AttributeSets.NameIsUsed[AttributeSets_Name])
                {
                    Inventor.AttributeSet m_AttributeSet = m_AttributeSets.Add(AttributeSets_Name);
                    Inventor.Attribute m_Attribute = m_AttributeSet.Add(Attribute_Name, Inventor.ValueTypeEnum.kStringType, Attribute_Value);
                }
            }
        }

        private static void ColourOccurance(ref Inventor.AssemblyDocument AssemblyDocument,ref Inventor.ComponentOccurrence componentOccurrence, string Colour)
        {
            Inventor.Asset asset = AssemblyDocument.Assets[Colour]; //this to sort out
            componentOccurrence.Appearance = asset;
        }

        private void ButtonAddGroup_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (UIJustSelectedObjects != null || UIAssemblyDocument != null)
            {
                AddAttributes(ref UIJustSelectedObjects,ref UIAssemblyDocument); 
            }
            else { return; }
            watch.Stop();

            txtTimerBox.Text = watch.ElapsedMilliseconds.ToString();
        }

        private void ButtonHideShowGroup_Click(object sender, EventArgs e)
        {
            //if (m_InvApplication.Documents.Count >= 1 && m_InvApplication.ActiveDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            //{
            //    Inventor.AssemblyDocument m_AssemblyDocument = (Inventor.AssemblyDocument)m_InvApplication.ActiveDocument;
            //    Group.HideOrShowGroup(ref m_AssemblyDocument, false, NamedGroup, Number);
            //}
            //else
            //{
            //    MessageBox.Show("Open Assembly document");
            //    return;
            //}
        }

        private void UserGUI_Load(object sender, EventArgs e)
        {

        }

        private void ButtonRemoveGroup_Click(object sender, EventArgs e)
        {
            //if (m_InvApplication.Documents.Count >= 1 && m_InvApplication.ActiveDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)

            //}
        }

        private void AddFor_Click(object sender, EventArgs e)
        {

        }

        public void AddForAttributes(Inventor.AssemblyDocument m_AssemblyDocument, Inventor.SelectSet selectSet, string GroupName, int cnt)
        {
            // int cnt = m_AssemblyDocument.SelectSet.Count;
            // Inventor.SelectSet selectSet = m_AssemblyDocument.SelectSet;
            // m_AssemblyDocument.SelectSet.GetEnumerator().Current.GetType();
            for (int i = cnt; i >= 1; i--)
            {
                // object itm = selectSet[i];
                try
                {
                    object objUnKnown = selectSet[i];

                    System.Type invokeType = objUnKnown.GetType();
                    object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);
                    Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;


                    if (objTypeEum == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                    {
                        Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)objUnKnown;
                        Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;

                        if (!(m_AttributeSets is null))
                        {

                            // Add the attributes to the ComponentOccurrence Name = "GroupObject";
                            if (!m_AttributeSets.NameIsUsed[GroupName])
                            {
                                Inventor.AttributeSet m_AttributeSet = m_AttributeSets.Add(GroupName);
                                Inventor.Attribute m_Attribute = m_AttributeSet.Add(GroupName + "A", Inventor.ValueTypeEnum.kStringType, "G");

                                Inventor.Asset asset = m_AssemblyDocument.Assets["Red"]; //this to sort out
                                componentOccurrence.Appearance = asset;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Exception");

                }
            }
        }

        private void CmbColour_DropDown(object sender, EventArgs e)
        {
            object obj = "314DE259-5443-4621-BFBD-1730C6CC9AE9";
            Inventor.AssetLibrary assetLibrary = m_InvApplication.AssetLibraries[obj];
            Inventor.AssetCategories assetCategories = assetLibrary.AppearanceAssetCategories;
            object Objct = CmbAssetCat.SelectedIndex;
            int length = assetLibrary.AppearanceAssetCategories[Objct].Assets.Count;
            for (int x = 1; x <= length; x++)
            {
                CmbColour.Items.Add(assetLibrary.AppearanceAssetCategories[Objct].Assets[x].DisplayName);
            }
        }

        private void CmbColour_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void CmbColour_MouseLeave(object sender, EventArgs e)
        {

        }

        //if (m_InvApplication.Documents.Count >= 1 && m_InvApplication.ActiveDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
        //{
        //    Inventor.AssemblyDocument m_AssemblyDocument = (Inventor.AssemblyDocument)m_InvApplication.ActiveDocument;
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    Group.AddEach(ref m_AssemblyDocument,NamedGroup);

        //    watch.Stop();

        //    textBox1.Text = watch.ElapsedMilliseconds.ToString();
        //    System.Diagnostics.Debug.Print(watch.ElapsedMilliseconds.ToString());
        //}
        //else
        //{
        //    MessageBox.Show("Open Assembly document");
        //    return;
        //}

    }
}

