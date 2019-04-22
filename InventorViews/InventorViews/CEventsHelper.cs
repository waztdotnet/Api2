using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvGroups
{
    class CEventsHelper
    {
        private Inventor.Application mApplication = null;
        private Inventor.AssemblyEvents m_AssemblyEvents;
        //private Inventor.ApplicationEvents mApplicationEvents;

        private Inventor.UserInputEvents mUserInputEvents;

        public delegate void CEventsHelperDelegateHandler(string args, int count);
        public event CEventsHelperDelegateHandler OnChange;


        public CEventsHelper(Inventor.Application The_Application)
        {
            mApplication = The_Application;
            InitiateEvents();
        }

        private void InitiateEvents()
        {
            mUserInputEvents = mApplication.CommandManager.UserInputEvents;
            mUserInputEvents.OnSelect += new Inventor.UserInputEventsSink_OnSelectEventHandler(UserInputEvents_OnSelect);
            mUserInputEvents.OnPreSelect += new Inventor.UserInputEventsSink_OnPreSelectEventHandler(UserInputEvents_OnPreSelect);
            mUserInputEvents.OnUnSelect += new Inventor.UserInputEventsSink_OnUnSelectEventHandler(UserInputEvents_OnUnSelect);
            mUserInputEvents.OnStopPreSelect += new Inventor.UserInputEventsSink_OnStopPreSelectEventHandler(UserInputEvents_OnStopPreSelect);
            mUserInputEvents.OnDoubleClick += new Inventor.UserInputEventsSink_OnDoubleClickEventHandler(UserInputEvents_OnDoubleClick);
        }
        private void UserInputEvents_OnSelect(Inventor.ObjectsEnumerator JustSelectedEntities, ref Inventor.ObjectCollection MoreSelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
           
            if (View.Parent != null)
            {
                Inventor.Document Document = (Inventor.Document)View.Parent;
                Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)Document;

                if (JustSelectedEntities.Count > 0)
                {
                    for (int i = JustSelectedEntities.Count; i >= 1; i--)
                    {
                        GetInventorObjType(JustSelectedEntities[i]);
                    }
                }
            }

        }

        private Inventor.ObjectTypeEnum GetInventorObjType(object objUnKnown)
        {
            System.Type invokeType = objUnKnown.GetType();

            object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);

            Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;
         
            return objTypeEum;
        }
        //switch (objTypeEum)
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
        private static void AddAttributeSets( Inventor.ComponentOccurrence componentOccurrence,String AttriSetsName, String AttriName, String AttriValue)
        {
            Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
            if (!m_AttributeSets.NameIsUsed[AttriSetsName])
            {
                Inventor.AttributeSet m_AttributeSet = m_AttributeSets.Add(AttriSetsName);
                Inventor.Attribute m_Attribute = m_AttributeSet.Add(AttriName, Inventor.ValueTypeEnum.kStringType, AttriValue);
            }
        }

        private static void ColourOccurance(Inventor.AssemblyDocument AssemblyDocument, Inventor.ComponentOccurrence componentOccurrence, string Colour)
        {
            Inventor.Asset asset = AssemblyDocument.Assets[Colour]; //this to sort out
            componentOccurrence.Appearance = asset;
        }

        private static void GetDocumentDefinitionTypeByOccurrence(Inventor.ComponentOccurrence componentOccurrence)
        {
            

            switch (componentOccurrence.DefinitionDocumentType)
            {
                case Inventor.DocumentTypeEnum.kAssemblyDocumentObject:
                    MessageBox.Show(componentOccurrence.DefinitionDocumentType.ToString());
                    break;
                case Inventor.DocumentTypeEnum.kPartDocumentObject:
                    MessageBox.Show(componentOccurrence.DefinitionDocumentType.ToString());
                    break;
                case Inventor.DocumentTypeEnum.kDesignElementDocumentObject:
                    MessageBox.Show(componentOccurrence.DefinitionDocumentType.ToString());
                    break;
                case Inventor.DocumentTypeEnum.kForeignModelDocumentObject:
                    MessageBox.Show(componentOccurrence.DefinitionDocumentType.ToString());
                    break;
                case Inventor.DocumentTypeEnum.kUnknownDocumentObject:
                    MessageBox.Show(componentOccurrence.DefinitionDocumentType.ToString());
                    break;
                default:
                    break;
            }
        }

        private void UserInputEvents_OnDoubleClick(Inventor.ObjectsEnumerator SelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View, Inventor.NameValueMap AdditionalInfo, out Inventor.HandlingCodeEnum HandlingCode)
        {
            HandlingCode = Inventor.HandlingCodeEnum.kEventHandled;
            //MessageBox.Show("On Double Click");
        }

        private void UserInputEvents_OnStopPreSelect(Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
           
          //MessageBox.Show("On Stop PreSelect");
        }

        private void UserInputEvents_OnUnSelect(Inventor.ObjectsEnumerator UnSelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
            
           // MessageBox.Show("On UnSelect");
        }

        private void UserInputEvents_OnPreSelect(ref object PreSelectEntity, out bool DoHighlight, ref Inventor.ObjectCollection MorePreSelectEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
            DoHighlight = true;
            //roll over
           // MessageBox.Show("On PreSelect");
        }





        ~CEventsHelper()
        {

           

            mUserInputEvents.OnSelect -= new Inventor.UserInputEventsSink_OnSelectEventHandler(UserInputEvents_OnSelect);
            mUserInputEvents.OnPreSelect -= new Inventor.UserInputEventsSink_OnPreSelectEventHandler(UserInputEvents_OnPreSelect);
            mUserInputEvents.OnUnSelect -= new Inventor.UserInputEventsSink_OnUnSelectEventHandler(UserInputEvents_OnUnSelect);
            mUserInputEvents.OnStopPreSelect -= new Inventor.UserInputEventsSink_OnStopPreSelectEventHandler(UserInputEvents_OnStopPreSelect);
            mUserInputEvents.OnDoubleClick -= new Inventor.UserInputEventsSink_OnDoubleClickEventHandler(UserInputEvents_OnDoubleClick);
            mUserInputEvents = null;
        }


    }
}
