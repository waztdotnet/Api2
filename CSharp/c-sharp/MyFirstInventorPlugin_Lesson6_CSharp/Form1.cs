//(C) Copyright 2012 by Autodesk, Inc. 

//Permission to use, copy, modify, and distribute this software
//in object code form for any purpose and without fee is hereby
//granted, provided that the above copyright notice appears in
//all copies and that both that copyright notice and the limited
//warranty and restricted rights notice below appear in all
//supporting documentation.

//AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
//AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
//MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK,
//INC. DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL
//BE UNINTERRUPTED OR ERROR FREE.

//Use, duplication, or disclosure by the U.S. Government is
//subject to restrictions set forth in FAR 52.227-19 (Commercial
//Computer Software - Restricted Rights) and DFAR 252.227-7013(c)
//(1)(ii)(Rights in Technical Data and Computer Software), as
//applicable.


using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Inventor;


namespace MyFirstInventorPlugin_Lesson6_CSharp
{
    public partial class Form1 : Form
    {
        Inventor.Application _invApp;
        bool _started = false;

        public Form1()
        {
            InitializeComponent();
            try
            {
                _invApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");

            }
            catch (Exception ex)
            {
                try
                {
                    Type invAppType = Type.GetTypeFromProgID("Inventor.Application");

                    _invApp = (Inventor.Application)System.Activator.CreateInstance(invAppType);
                    _invApp.Visible = true;

                    //Note: if the Inventor session is left running after this
                    //form is closed, there will still an be and Inventor.exe 
                    //running. We will use this Boolean to test in Form1.Designer.cs 
                    //in the dispose method whether or not the Inventor App should
                    //be shut down when the form is closed.
                    _started = true;

                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.ToString());
                    MessageBox.Show("Unable to get or start Inventor");
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            HideOrShowGroup(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HideOrShowGroup(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddOrRemoveFromGroup(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddOrRemoveFromGroup(false);
        }
        

        public void AddOrRemoveFromGroup(bool add)
        {
            if (_invApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open an Assembly document");
                return;
            }

            if (_invApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("Need to have an Assembly document active");
                return;
            }

            AssemblyDocument asmDoc = default(AssemblyDocument);
            asmDoc = (AssemblyDocument)_invApp.ActiveDocument;

            if (asmDoc.SelectSet.Count == 0)
            {
                MessageBox.Show("Need to select a Part or Sub Assembly");
                return;
            }

            SelectSet selSet = default(SelectSet);
            selSet = asmDoc.SelectSet;
            Inventor.Assets colour = asmDoc.Assets;
            try
            {
                ComponentOccurrence compOcc = default(ComponentOccurrence);
                object obj = null;
                foreach (object obj_loopVariable in selSet)
                {
                    obj = obj_loopVariable;
                    compOcc = (ComponentOccurrence)obj;
                    System.Diagnostics.Debug.Print(compOcc.Name);

                    AttributeSets attbSets = compOcc.AttributeSets;

                    if (add)
                    {
                        // Add the attributes to the ComponentOccurrence

                        if (!attbSets.NameIsUsed["myPartGroup"])
                        {
                            AttributeSet attbSet = attbSets.Add("myPartGroup");

                            Inventor.Attribute attb = attbSet.Add("PartGroup1", ValueTypeEnum.kStringType, "Group1");
                            Inventor.Asset asset = asmDoc.Assets["Red"];
                            compOcc.Appearance = asset; 
                        }
                    }
                    else
                    {
                        // Delete the attributes to the ComponentOccurrence
                        if (attbSets.NameIsUsed["myPartGroup"])
                        {
                            attbSets["myPartGroup"].Delete();
                        }

                    }

                    //compOcc.Visible = False

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Is the selected item a Component?");
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        public void HideOrShowGroup(bool hide)
        {
            if (_invApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open an Assembly document");
                return;
            }

            if (_invApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("Need to have an Assembly document active");
                return;
            }

            AssemblyDocument asmDoc = default(AssemblyDocument);
            asmDoc = (AssemblyDocument)_invApp.ActiveDocument;


            try
            {
                AttributeManager attbMan = asmDoc.AttributeManager;
                
                ObjectCollection objsCol = default(ObjectCollection);
                objsCol = attbMan.FindObjects("myPartGroup", "PartGroup1", "Group1");

                ComponentOccurrence compOcc = default(ComponentOccurrence);
                foreach (object obj in objsCol)
                {
                    compOcc = (ComponentOccurrence)obj;
                    compOcc.Visible = hide;
                    
                    //False
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem hiding component");
            }
        }

       


        private void button5_Click(object sender, EventArgs e)
        {
            if (_invApp.Documents.Count == 0)
            {
                MessageBox.Show("Need to open an Assembly document");
                return;
            }

            if (_invApp.ActiveDocument.DocumentType != DocumentTypeEnum.kAssemblyDocumentObject)
            {
                MessageBox.Show("Need to have an Assembly document active");
                return;
            }

            AssemblyDocument asmDoc = (AssemblyDocument)_invApp.ActiveDocument;
            // Get the attribute manager for the document
            AttributeManager attbMan = asmDoc.AttributeManager;

            // Find the objects with the attributes
            ObjectCollection objCol = default(ObjectCollection);
            objCol = attbMan.FindObjects("myPartGroup", "PartGroup1", "Group1");

            AttributeSets attbSets = default(AttributeSets);

            // Delete the attribute from the ComponentOccurrence
            foreach (object obj in objCol)
            {
                ComponentOccurrence compOcc = (ComponentOccurrence)obj;
                attbSets = compOcc.AttributeSets;
                attbSets["myPartGroup"].Delete();
                // Make the ComponentOccurrence visible
                compOcc.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
