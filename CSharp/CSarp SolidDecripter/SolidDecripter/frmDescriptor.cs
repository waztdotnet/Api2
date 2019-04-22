using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolidDecripter
{
    public partial class frmDesriptor : Form
    {
        
        private Inventor.Application InvApp = null;
        private bool QuitInventor = false;

        public frmDesriptor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        //            private Inventor.Application InvApp = null;
        //private bool QuitInventor = false;

            try
            {
                InvApp = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Starting a New Inventor Session");
            }
            if (InvApp == null)
            {
                Type inventorAppType = System.Type.GetTypeFromProgID("Inventor.Application");
                InvApp = System.Activator.CreateInstance(inventorAppType) as Inventor.Application;
                InvApp.Visible = true;
                QuitInventor = true;
            }

          

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
                Inventor.AssemblyDocument mAssemblyDocument;
                Inventor.Document mDocument = InvApp.ActiveDocument;
           
                if (mDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
                        {
                        mAssemblyDocument = (Inventor.AssemblyDocument)mDocument;
                        Inventor.PartDocument mPartDocument;
                       // Inventor.PartDocument mDocumentPart;
                        foreach (Inventor.ComponentOccurrence mComponentOccurrence in mAssemblyDocument.ComponentDefinition.Occurrences)
                        { 
                            if (mComponentOccurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
                                {
                                    mPartDocument = mComponentOccurrence.Definition.Document;
                                    GetExtents(ref mPartDocument);
                                    if(cmbMaterials.SelectedIndex != -1 ) 
                                    { 
                                        mPartDocument.ComponentDefinition.Material.Name = cmbMaterials.Items[cmbMaterials.SelectedIndex].ToString();
                                        lstNames.Items.Add(mPartDocument.DisplayName + "...Updated");
                                    }
                                }
                        }
                }
                else
                {
                     MessageBox.Show("Not an Assembly");
                }
        }


        private void GetMaterial(ref Inventor.AssemblyDocument mAssemblyDocument)
        {
                foreach (Inventor.Material m_material in mAssemblyDocument.Materials)
                {
                    cmbMaterials.Items.Add(m_material.Name);
                }
 
        }
        //Single Call multi add
        private void SetUserParameters(ref Inventor.PartDocument PartDocument,  NameValueCollection ParameterNameValues, Inventor.UnitsTypeEnum units)
        {         
            Inventor.UserParameter m_UserParameter;

            Collection<string> ParameterNameCach = new Collection<string>() ;
           
            if ( PartDocument.ComponentDefinition.Parameters.UserParameters.Count == 0) //Add from Scratch userPars
            {
                foreach (string namedParameter in ParameterNameValues.Keys)
                {
                    m_UserParameter = PartDocument.ComponentDefinition.Parameters.UserParameters.AddByExpression(namedParameter, ParameterNameValues[namedParameter], units);
                    m_UserParameter.ExposedAsProperty = true;
                }
            }
            else
            {
                foreach (Inventor.UserParameter up in PartDocument.ComponentDefinition.Parameters.UserParameters)
                {
                    ParameterNameCach .Add(up.Name);
                }

                foreach (string namedParameter in ParameterNameValues.Keys)
                {                    
                    if (!ParameterNameCach.Contains(namedParameter)  )
                    {
                        m_UserParameter = PartDocument.ComponentDefinition.Parameters.UserParameters.AddByExpression(namedParameter, ParameterNameValues[namedParameter], units);
                        m_UserParameter.ExposedAsProperty = true;
                    }
                    else
                    {
                        Inventor.UserParameter _UserParameter;
                        _UserParameter = PartDocument.ComponentDefinition.Parameters.UserParameters[namedParameter];
                        _UserParameter.Expression = ParameterNameValues.Get(namedParameter) + "mm";
                        _UserParameter.CustomPropertyFormat.ShowTrailingZeros = false;
                        _UserParameter.CustomPropertyFormat.ShowUnitsString  = false;
                        _UserParameter.CustomPropertyFormat.Precision = Inventor.CustomPropertyPrecisionEnum.kOneDecimalPlacePrecision;
                        _UserParameter.ExposedAsProperty = true;
                    }
                }
            }
            Inventor.PropertySet m_Propertyset = PartDocument.PropertySets["{32853F0F-3444-11D1-9E93-0060B03C1CA6}"];
            Inventor.Property m_Property = m_Propertyset["Description"];
            m_Property.Expression = "=<Material> <Length>X<Width>X<Thickness>";
        }

        private void GetExtents(ref Inventor.PartDocument PartDocument )
        {

            Inventor.ComponentDefinition mComponentDefinition;
            Inventor.Box mBox;
            double Length;
            double Width;
            double Thickness;
            double[] SortArray = new double[3] ;

            mComponentDefinition = (Inventor.ComponentDefinition)PartDocument.ComponentDefinition;
             
            foreach (Inventor.SurfaceBody mSurfaceBody in mComponentDefinition.SurfaceBodies)
            {
                mBox = mSurfaceBody.RangeBox;
                mBox.Extend(mSurfaceBody.RangeBox.MinPoint);
                mBox.Extend(mSurfaceBody.RangeBox.MaxPoint);

                if (mBox != null )
                {
                    Length = mBox.MaxPoint.X - mBox.MinPoint.X;
                SortArray[0] = Length;
                Width = mBox.MaxPoint.Y - mBox.MinPoint.Y;
                SortArray[1] = Width;
                Thickness = mBox.MaxPoint.Z - mBox.MinPoint.Z;
                SortArray[2] = Thickness;
                }

                Array.Sort(SortArray);
                Length = SortArray[2];
                Width = SortArray[1];
                Thickness = SortArray[0];
                Length = Length * 10;
                Width = Width * 10;
                Thickness = Thickness * 10;

                NameValueCollection ParameterNameValues = new NameValueCollection();
                ParameterNameValues.Add("Length", Length.ToString());
                ParameterNameValues.Add("Width", Width.ToString());
                ParameterNameValues.Add("Thickness", Thickness.ToString());
                SetUserParameters(ref PartDocument, ParameterNameValues, Inventor.UnitsTypeEnum.kMillimeterLengthUnits );

            } 
        }

        //Single Call and add
        private void SetUserParameters(ref Inventor.PartDocument PartDocument, string UserParameterName, string ParameterValue, Inventor.UnitsTypeEnum units)
        {
            bool found = false;
            Inventor.UserParameter m_UserParameter;
            foreach (Inventor.UserParameter UserParameter in PartDocument.ComponentDefinition.Parameters.UserParameters)
            {
                if (UserParameter.Name == UserParameterName)
                {
                    found = true;
                    UserParameter.Expression = ParameterValue + "mm";
                    UserParameter.ExposedAsProperty = true;
                    break;
                }
            }

            if (!found)
            {
                m_UserParameter = PartDocument.ComponentDefinition.Parameters.UserParameters.AddByExpression(UserParameterName, ParameterValue, units);
                m_UserParameter.ExposedAsProperty = true;
            }


        }
        private void SetCustomPropertys(ref Inventor.PartDocument PartDocument, string PropertyName, string PropertySetName)
        {

            Inventor.PropertySet m_Propertyset = PartDocument.PropertySets["{32853F0F-3444-11D1-9E93-0060B03C1CA6}"];
            Inventor.Property m_Property = m_Propertyset[Inventor.PropertiesForDesignTrackingPropertiesEnum.kDescriptionDesignTrackingProperties];
            m_Property.Expression = "=<Length>";
            //Inventor.PropertiesForDesignTrackingPropertiesEnum.kDescriptionDesignTrackingProperties



            foreach (Inventor.PropertySet Propertyset in PartDocument.PropertySets)
            {
                if (Propertyset.Name == PropertySetName) //"Inventor User Defined Properties"
                {
                    if (Propertyset.Count != 0)
                    {
                        foreach (Inventor.Property mProperty in Propertyset)
                        {
                            if (mProperty.Name == PropertyName)
                            {
                                //MessageBox.Show("");
                            }
                            else
                            {

                                //ADD
                            }
                        }
                    }
                }

            }

        } //Single Call   

        private void cmbMaterials_DropDown(object sender, EventArgs e)
        {
            Inventor.Document mDocument = InvApp.ActiveDocument;
            if (mDocument.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                Inventor.AssemblyDocument mAssemblyDocument = (Inventor.AssemblyDocument)mDocument;
                GetMaterial(ref mAssemblyDocument);
            }
            
        }
    }
}
