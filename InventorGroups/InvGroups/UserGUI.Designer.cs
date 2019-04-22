namespace InvGroups
{
    partial class UserGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

            InputEvents.OnSelectionChange -= new CInventorInputEvents.CInventorEvents_OnSelect_DelegateHandler(InputEvents_OnSelectionChange);
            //Test to see if we started the Inventor Application.
            //If Inventor was started by running this form then call the 
            //Quit method.
            //if (_started)
            //{
            //    m_InvApplication.Quit();
            //}
            //m_InvApplication = null;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ButtonAddGroup = new System.Windows.Forms.Button();
            this.ButtonHideShowGroup = new System.Windows.Forms.Button();
            this.ButtonRemoveGroup = new System.Windows.Forms.Button();
            this.AddFor = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtTimerBox = new System.Windows.Forms.TextBox();
            this.CmbColour = new System.Windows.Forms.ComboBox();
            this.CmbAssetCat = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ButtonAddGroup
            // 
            this.ButtonAddGroup.Location = new System.Drawing.Point(31, 32);
            this.ButtonAddGroup.Name = "ButtonAddGroup";
            this.ButtonAddGroup.Size = new System.Drawing.Size(222, 55);
            this.ButtonAddGroup.TabIndex = 0;
            this.ButtonAddGroup.Text = "Add";
            this.ButtonAddGroup.UseVisualStyleBackColor = true;
            this.ButtonAddGroup.Click += new System.EventHandler(this.ButtonAddGroup_Click);
            // 
            // ButtonHideShowGroup
            // 
            this.ButtonHideShowGroup.Location = new System.Drawing.Point(31, 78);
            this.ButtonHideShowGroup.Name = "ButtonHideShowGroup";
            this.ButtonHideShowGroup.Size = new System.Drawing.Size(222, 55);
            this.ButtonHideShowGroup.TabIndex = 1;
            this.ButtonHideShowGroup.Text = "Hide/Show";
            this.ButtonHideShowGroup.UseVisualStyleBackColor = true;
            this.ButtonHideShowGroup.Click += new System.EventHandler(this.ButtonHideShowGroup_Click);
            // 
            // ButtonRemoveGroup
            // 
            this.ButtonRemoveGroup.Location = new System.Drawing.Point(259, 32);
            this.ButtonRemoveGroup.Name = "ButtonRemoveGroup";
            this.ButtonRemoveGroup.Size = new System.Drawing.Size(222, 55);
            this.ButtonRemoveGroup.TabIndex = 2;
            this.ButtonRemoveGroup.Text = "Remove";
            this.ButtonRemoveGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ButtonRemoveGroup.UseVisualStyleBackColor = true;
            this.ButtonRemoveGroup.Click += new System.EventHandler(this.ButtonRemoveGroup_Click);
            // 
            // AddFor
            // 
            this.AddFor.Location = new System.Drawing.Point(31, 152);
            this.AddFor.Name = "AddFor";
            this.AddFor.Size = new System.Drawing.Size(222, 55);
            this.AddFor.TabIndex = 3;
            this.AddFor.Text = "Add For";
            this.AddFor.UseVisualStyleBackColor = true;
            this.AddFor.Click += new System.EventHandler(this.AddFor_Click);
            // 
            // txtTimerBox
            // 
            this.txtTimerBox.Location = new System.Drawing.Point(218, 246);
            this.txtTimerBox.Name = "txtTimerBox";
            this.txtTimerBox.Size = new System.Drawing.Size(100, 20);
            this.txtTimerBox.TabIndex = 4;
            // 
            // CmbColour
            // 
            this.CmbColour.FormattingEnabled = true;
            this.CmbColour.Location = new System.Drawing.Point(363, 246);
            this.CmbColour.Name = "CmbColour";
            this.CmbColour.Size = new System.Drawing.Size(225, 21);
            this.CmbColour.TabIndex = 5;
            this.CmbColour.DropDown += new System.EventHandler(this.CmbColour_DropDown);
            this.CmbColour.DropDownClosed += new System.EventHandler(this.CmbColour_DropDownClosed);
            this.CmbColour.MouseLeave += new System.EventHandler(this.CmbColour_MouseLeave);
            // 
            // CmbAssetCat
            // 
            this.CmbAssetCat.FormattingEnabled = true;
            this.CmbAssetCat.Location = new System.Drawing.Point(363, 181);
            this.CmbAssetCat.Name = "CmbAssetCat";
            this.CmbAssetCat.Size = new System.Drawing.Size(225, 21);
            this.CmbAssetCat.TabIndex = 6;
            // 
            // UserGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CmbAssetCat);
            this.Controls.Add(this.CmbColour);
            this.Controls.Add(this.txtTimerBox);
            this.Controls.Add(this.AddFor);
            this.Controls.Add(this.ButtonRemoveGroup);
            this.Controls.Add(this.ButtonHideShowGroup);
            this.Controls.Add(this.ButtonAddGroup);
            this.Name = "UserGUI";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UserGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonAddGroup;
        private System.Windows.Forms.Button ButtonHideShowGroup;
        private System.Windows.Forms.Button ButtonRemoveGroup;
        private System.Windows.Forms.Button AddFor;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtTimerBox;
        private System.Windows.Forms.ComboBox CmbColour;
        private System.Windows.Forms.ComboBox CmbAssetCat;
    }
}

