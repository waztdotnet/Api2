using System;
using System.Reflection;
using System.Windows.Forms;

namespace EventsInventLib
{
    internal class CInvInputEvents
    {
        private Inventor.Application _InvApplication = null;

        //private Inventor.AssemblyEvents m_AssemblyEvents;
        //private Inventor.ApplicationEvents mApplicationEvents;
        private Inventor.UserInputEvents mUserInputEvents;

        private Inventor.MouseEvents mMouseEvents;
        private Inventor.InteractionEvents mInteractionEvents;

        public delegate void CInventorEvents_OnSelect_DelegateHandler(ref Inventor.AssemblyDocument AssemblyDocument, ref Inventor.ObjectsEnumerator JustSelected);

        public event CInventorEvents_OnSelect_DelegateHandler OnSelectionChange;

        public delegate void CInventorEventsDelegateHandler(string args, Inventor.ObjectsEnumerator JustSelected);

        // public event CInventorEventsDelegateHandler OnUnSelectionChange;

        public CInvInputEvents(Inventor.Application The_Application)
        {
            _InvApplication = The_Application;
            InitiateEvents();
        }

        private void InitiateEvents()
        {
            mUserInputEvents = _InvApplication.CommandManager.UserInputEvents;
            mInteractionEvents = _InvApplication.CommandManager.CreateInteractionEvents();
            mMouseEvents = mInteractionEvents.MouseEvents;
            mUserInputEvents.OnSelect += new Inventor.UserInputEventsSink_OnSelectEventHandler(UserInputEvents_OnSelect);
            mUserInputEvents.OnPreSelect += new Inventor.UserInputEventsSink_OnPreSelectEventHandler(UserInputEvents_OnPreSelect);
            mUserInputEvents.OnUnSelect += new Inventor.UserInputEventsSink_OnUnSelectEventHandler(UserInputEvents_OnUnSelect);
            mUserInputEvents.OnStopPreSelect += new Inventor.UserInputEventsSink_OnStopPreSelectEventHandler(UserInputEvents_OnStopPreSelect);
            mUserInputEvents.OnDoubleClick += new Inventor.UserInputEventsSink_OnDoubleClickEventHandler(UserInputEvents_OnDoubleClick);
            mMouseEvents.OnMouseUp += new Inventor.MouseEventsSink_OnMouseUpEventHandler(MouseEventsSink_OnMouseUp);
            mMouseEvents.OnMouseDown += new Inventor.MouseEventsSink_OnMouseDownEventHandler(MouseEventsSink_OnMouseDown);
        }

        private void MouseEventsSink_OnMouseDown(Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
        }

        private void MouseEventsSink_OnMouseUp(Inventor.MouseButtonEnum Button, Inventor.ShiftStateEnum ShiftKeys, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
        }

        private void UserInputEvents_OnSelect(Inventor.ObjectsEnumerator JustSelectedEntities, ref Inventor.ObjectCollection MoreSelectedEntities, Inventor.SelectionDeviceEnum SelectionDevice, Inventor.Point ModelPosition, Inventor.Point2d ViewPosition, Inventor.View View)
        {
            if (View.Parent != null)
            {
                Inventor.Document Document = (Inventor.Document)View.Parent;
                if (Document.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
                {
                    Inventor.AssemblyDocument AssemblyDocument = (Inventor.AssemblyDocument)Document;

                    OnSelectionChange(ref AssemblyDocument, ref JustSelectedEntities);
                }
                else
                {
                    Inventor.AssemblyDocument AssemblyDocument = null;
                    JustSelectedEntities = null;
                    OnSelectionChange(ref AssemblyDocument, ref JustSelectedEntities);
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

        private static void AddAttributeSets(Inventor.ComponentOccurrence componentOccurrence, String AttriSetsName, String AttriName, String AttriValue)
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

        ~CInvInputEvents()
        {
            mUserInputEvents.OnSelect -= new Inventor.UserInputEventsSink_OnSelectEventHandler(UserInputEvents_OnSelect);
            mUserInputEvents.OnPreSelect -= new Inventor.UserInputEventsSink_OnPreSelectEventHandler(UserInputEvents_OnPreSelect);
            mUserInputEvents.OnUnSelect -= new Inventor.UserInputEventsSink_OnUnSelectEventHandler(UserInputEvents_OnUnSelect);
            mUserInputEvents.OnStopPreSelect -= new Inventor.UserInputEventsSink_OnStopPreSelectEventHandler(UserInputEvents_OnStopPreSelect);
            mUserInputEvents.OnDoubleClick -= new Inventor.UserInputEventsSink_OnDoubleClickEventHandler(UserInputEvents_OnDoubleClick);
            mUserInputEvents = null;
            mMouseEvents.OnMouseUp -= new Inventor.MouseEventsSink_OnMouseUpEventHandler(MouseEventsSink_OnMouseUp);
            mMouseEvents.OnMouseDown -= new Inventor.MouseEventsSink_OnMouseDownEventHandler(MouseEventsSink_OnMouseDown);
            mMouseEvents = null;
        }
    }
}