using System;
using System.Reflection;
using System.Windows.Forms;
//[module: NonNullTypes]
namespace InvGroups
{
    
    class CGroup
    {
        public CGroup()
        {
            
        }

        public void RemoveGroup(ref Inventor.AssemblyDocument m_AssemblyDocument, String GroupName, String GroupNumber)
        {

            if (m_AssemblyDocument.SelectSet.Count == 0)
            {
                MessageBox.Show("Need to select a Part or Sub Assembly");
                return;
            }
            Inventor.Assets colour = m_AssemblyDocument.Assets;



                foreach (var item in m_AssemblyDocument.SelectSet)
                {
                    //Inventor.ComponentOccurrence occurrence = (Inventor.ComponentOccurrence)item;
                    object objUnKnown = item;
                    System.Type invokeType = objUnKnown.GetType();
                    object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);
                    Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;
                    System.Diagnostics.Debug.WriteLine(objTypeEum.ToString());

                    if (objTypeEum == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                    {
                        Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)item;//(Inventor.ComponentOccurrence)objUnKnown;
                        Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
                        System.Diagnostics.Debug.Print(componentOccurrence.Name + "Remove");


                            //Delete the attributes to the ComponentOccurrence
                            if (m_AttributeSets.NameIsUsed[GroupName])
                            {
                                m_AttributeSets[GroupName].Delete();
                                componentOccurrence.AppearanceSourceType = Inventor.AppearanceSourceTypeEnum.kPartAppearance;
                            }
                        

                    }
                else if (objTypeEum == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject)
                {
                    Inventor.RectangularOccurrencePattern RectangularOccurrencePattern = (Inventor.RectangularOccurrencePattern)item;

                    foreach (var Ritem in RectangularOccurrencePattern.OccurrencePatternElements)
                    {
                        Inventor.OccurrencePatternElement occurrencePatternElement = (Inventor.OccurrencePatternElement)Ritem;
                        foreach (var Occitem in occurrencePatternElement.Occurrences)
                        {
                            Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)Occitem;
                            Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
                            if (m_AttributeSets.NameIsUsed[GroupName])
                            {
                                m_AttributeSets[GroupName].Delete();
                                componentOccurrence.AppearanceSourceType = Inventor.AppearanceSourceTypeEnum.kPartAppearance;
                                System.Diagnostics.Debug.Print(componentOccurrence.Name + "Remove Att and color");
                            }
                        }

                    }
                }
            }


        }

        public void AddFor(ref Inventor.AssemblyDocument m_AssemblyDocument, string GroupName,int cnt, Inventor.SelectSet selectSet)
        {
            AddForAttributes(ref m_AssemblyDocument,selectSet ,GroupName,cnt);
        }

        public void AddEach(ref Inventor.AssemblyDocument m_AssemblyDocument, string GroupName)
        {
            AddEachAttributes(ref m_AssemblyDocument,true, GroupName,"5");
        }

        private static void AddEachAttributes(ref Inventor.AssemblyDocument m_AssemblyDocument, bool SelectAllArrays, String GroupName, String GroupNumber)
        {

            if (m_AssemblyDocument.SelectSet.Count == 0)
            {
                MessageBox.Show("Need to select a Part or Sub Assembly");
                return;
            }
            Inventor.Assets colour = m_AssemblyDocument.Assets;
         
            try
            {
                foreach (var item in m_AssemblyDocument.SelectSet)
                {
                    object objUnKnown = item;
                    System.Type invokeType = objUnKnown.GetType();
                    object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);
                    Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;
                    System.Diagnostics.Debug.WriteLine(objTypeEum.ToString());

                    if (objTypeEum == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                    {
                        Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)item;//(Inventor.ComponentOccurrence)objUnKnown;
                        Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
                        System.Diagnostics.Debug.Print(componentOccurrence.Name);
                        // Add the attributes to the ComponentOccurrence Name = "GroupObject";
                        if (!m_AttributeSets.NameIsUsed[GroupName])
                        {
                            Inventor.AttributeSet m_AttributeSet = m_AttributeSets.Add(GroupName);
                            Inventor.Attribute m_Attribute = m_AttributeSet.Add(GroupName + "A", Inventor.ValueTypeEnum.kStringType, "G");
                            Inventor.Asset asset = m_AssemblyDocument.Assets["Red"]; //this to sort out
                            componentOccurrence.Appearance = asset;
                        }
                    }
                    else if (objTypeEum == Inventor.ObjectTypeEnum.kRectangularOccurrencePatternObject && SelectAllArrays)
                    {
                        Inventor.RectangularOccurrencePattern RectangularOccurrencePattern = (Inventor.RectangularOccurrencePattern)item;

                        foreach (var Ritem in RectangularOccurrencePattern.OccurrencePatternElements)
                        {
                            Inventor.OccurrencePatternElement occurrencePatternElement = (Inventor.OccurrencePatternElement)Ritem;
                            foreach (var Occitem in occurrencePatternElement.Occurrences)
                            {

                                Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)Occitem;
                                Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;
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
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Is the selected item a Component?");
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private static void AddForAttributes(ref Inventor.AssemblyDocument m_AssemblyDocument, Inventor.SelectSet selectSet, string GroupName,int cnt)
        {
            // int cnt = m_AssemblyDocument.SelectSet.Count;
           // Inventor.SelectSet selectSet = m_AssemblyDocument.SelectSet;
            // m_AssemblyDocument.SelectSet.GetEnumerator().Current.GetType();
            for (int i = 1; i <=cnt ; i++)
                {
               // object itm = selectSet[i];
                object objUnKnown = selectSet[i];
                
                System.Type invokeType = objUnKnown.GetType();
                object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);
                Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;


                if (objTypeEum == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                {
                    Inventor.ComponentOccurrence componentOccurrence = (Inventor.ComponentOccurrence)objUnKnown;
                Inventor.AttributeSets m_AttributeSets = componentOccurrence.AttributeSets;

                //    System.Diagnostics.Debug.Print(componentOccurrence.Name);
                //    System.Diagnostics.Debug.Print("Counter: " + i.ToString());

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
        }

        private bool ObjType(object objUnKnown)
        {
            if (!(objUnKnown is null))
            {
                System.Type invokeType = objUnKnown.GetType();

                object tmp = invokeType.InvokeMember("Type", BindingFlags.GetProperty, null, objUnKnown, null);

                Inventor.ObjectTypeEnum objTypeEum = (Inventor.ObjectTypeEnum)tmp;
                //System.Diagnostics.Debug.WriteLine(objTypeEum.ToString());

                if (objTypeEum == Inventor.ObjectTypeEnum.kComponentOccurrenceObject)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            return false;
        }

        public void HideOrShowGroup(ref Inventor.AssemblyDocument m_AssemblyDocument, bool hide, String GroupName, String GroupNumber)
        {
            try
            {
                Inventor.AttributeManager attbMan = m_AssemblyDocument.AttributeManager;

                Inventor.ObjectCollection objCol = default(Inventor.ObjectCollection);
                objCol = attbMan.FindObjects(GroupName, GroupName + "A", "G");

                Inventor.ComponentOccurrence componentoccurrence = default(Inventor.ComponentOccurrence);
                int length = objCol.Count;
                for (int i = 1; i <= length; i++)
                {
                    componentoccurrence = (Inventor.ComponentOccurrence)objCol[i];
                    componentoccurrence.Visible = hide;
                }

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Problem hiding component");
            }
        }




    }
}
