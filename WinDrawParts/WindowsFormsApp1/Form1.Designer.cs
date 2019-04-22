namespace WindowsDrawApp
{
    partial class DrawParts
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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ListComponents = new System.Windows.Forms.ListBox();
            this.btnMultiPart = new System.Windows.Forms.Button();
            this.txtPartID = new System.Windows.Forms.TextBox();
            this.cmbDrgTemplates = new System.Windows.Forms.ComboBox();
            this.cmbView = new System.Windows.Forms.ComboBox();
            this.txtViewLabels = new System.Windows.Forms.TextBox();
            this.cmbScales = new System.Windows.Forms.ComboBox();
            this.cmbSheetSize = new System.Windows.Forms.ComboBox();
            this.btnSinglePart = new System.Windows.Forms.Button();
            this.RdDrawBy = new System.Windows.Forms.RadioButton();
            this.RdDrawByAssembleyParts = new System.Windows.Forms.RadioButton();
            this.GbxDrawing = new System.Windows.Forms.GroupBox();
            this.GrpNCExport = new System.Windows.Forms.GroupBox();
            this.CmbSaveFolders = new System.Windows.Forms.ComboBox();
            this.BtnFolderBrowser = new System.Windows.Forms.Button();
            this.BtnExport = new System.Windows.Forms.Button();
            this.GbxDrawing.SuspendLayout();
            this.GrpNCExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListComponents
            // 
            this.ListComponents.Dock = System.Windows.Forms.DockStyle.Left;
            this.ListComponents.FormattingEnabled = true;
            this.ListComponents.HorizontalScrollbar = true;
            this.ListComponents.Location = new System.Drawing.Point(0, 0);
            this.ListComponents.Name = "ListComponents";
            this.ListComponents.ScrollAlwaysVisible = true;
            this.ListComponents.Size = new System.Drawing.Size(237, 527);
            this.ListComponents.TabIndex = 0;
            // 
            // btnMultiPart
            // 
            this.btnMultiPart.Location = new System.Drawing.Point(407, 37);
            this.btnMultiPart.Name = "btnMultiPart";
            this.btnMultiPart.Size = new System.Drawing.Size(75, 23);
            this.btnMultiPart.TabIndex = 1;
            this.btnMultiPart.Text = "Multi Part";
            this.btnMultiPart.UseVisualStyleBackColor = true;
            this.btnMultiPart.Click += new System.EventHandler(this.BtnMultiPart_Click);
            // 
            // txtPartID
            // 
            this.txtPartID.AutoCompleteCustomSource.AddRange(new string[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "H",
            "W",
            "P"});
            this.txtPartID.Location = new System.Drawing.Point(19, 58);
            this.txtPartID.Name = "txtPartID";
            this.txtPartID.Size = new System.Drawing.Size(61, 20);
            this.txtPartID.TabIndex = 3;
            this.txtPartID.Text = "P";
            // 
            // cmbDrgTemplates
            // 
            this.cmbDrgTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDrgTemplates.FormattingEnabled = true;
            this.cmbDrgTemplates.Location = new System.Drawing.Point(18, 95);
            this.cmbDrgTemplates.Name = "cmbDrgTemplates";
            this.cmbDrgTemplates.Size = new System.Drawing.Size(463, 21);
            this.cmbDrgTemplates.TabIndex = 4;
            this.cmbDrgTemplates.SelectedIndexChanged += new System.EventHandler(this.CmbDrgTemplates_SelectedIndexChanged);
            // 
            // cmbView
            // 
            this.cmbView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbView.FormattingEnabled = true;
            this.cmbView.Items.AddRange(new object[] {
            "Default",
            "Top",
            "Bottom",
            "Left",
            "Right",
            "Front",
            "Back"});
            this.cmbView.Location = new System.Drawing.Point(18, 123);
            this.cmbView.Name = "cmbView";
            this.cmbView.Size = new System.Drawing.Size(61, 21);
            this.cmbView.TabIndex = 6;
            this.cmbView.SelectedIndexChanged += new System.EventHandler(this.CmbView_SelectedIndexChanged);
            // 
            // txtViewLabels
            // 
            this.txtViewLabels.Location = new System.Drawing.Point(18, 164);
            this.txtViewLabels.Name = "txtViewLabels";
            this.txtViewLabels.Size = new System.Drawing.Size(100, 20);
            this.txtViewLabels.TabIndex = 7;
            this.txtViewLabels.Text = "ViewLabel";
            // 
            // cmbScales
            // 
            this.cmbScales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScales.FormattingEnabled = true;
            this.cmbScales.Items.AddRange(new object[] {
            "1:1",
            "1:3",
            "1:4",
            "1:5",
            "1:7",
            "1:10",
            "1:12",
            "1:15",
            "1:20",
            "1:25",
            "1:30"});
            this.cmbScales.Location = new System.Drawing.Point(155, 123);
            this.cmbScales.Name = "cmbScales";
            this.cmbScales.Size = new System.Drawing.Size(121, 21);
            this.cmbScales.TabIndex = 8;
            // 
            // cmbSheetSize
            // 
            this.cmbSheetSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSheetSize.FormattingEnabled = true;
            this.cmbSheetSize.Items.AddRange(new object[] {
            "A1",
            "A2",
            "A3",
            "A4"});
            this.cmbSheetSize.Location = new System.Drawing.Point(360, 122);
            this.cmbSheetSize.Name = "cmbSheetSize";
            this.cmbSheetSize.Size = new System.Drawing.Size(121, 21);
            this.cmbSheetSize.TabIndex = 9;
            // 
            // btnSinglePart
            // 
            this.btnSinglePart.Location = new System.Drawing.Point(326, 37);
            this.btnSinglePart.Name = "btnSinglePart";
            this.btnSinglePart.Size = new System.Drawing.Size(75, 23);
            this.btnSinglePart.TabIndex = 10;
            this.btnSinglePart.Text = "Single Part Drawing";
            this.btnSinglePart.UseVisualStyleBackColor = true;
            this.btnSinglePart.Click += new System.EventHandler(this.BtnSinglePart_Click);
            // 
            // RdDrawBy
            // 
            this.RdDrawBy.AutoSize = true;
            this.RdDrawBy.Checked = true;
            this.RdDrawBy.Location = new System.Drawing.Point(18, 19);
            this.RdDrawBy.Name = "RdDrawBy";
            this.RdDrawBy.Size = new System.Drawing.Size(119, 17);
            this.RdDrawBy.TabIndex = 11;
            this.RdDrawBy.TabStop = true;
            this.RdDrawBy.Text = "Draw By Single Part";
            this.RdDrawBy.UseVisualStyleBackColor = true;
            this.RdDrawBy.CheckedChanged += new System.EventHandler(this.RdDrawBy_CheckedChanged);
            // 
            // RdDrawByAssembleyParts
            // 
            this.RdDrawByAssembleyParts.AutoSize = true;
            this.RdDrawByAssembleyParts.Location = new System.Drawing.Point(19, 35);
            this.RdDrawByAssembleyParts.Name = "RdDrawByAssembleyParts";
            this.RdDrawByAssembleyParts.Size = new System.Drawing.Size(145, 17);
            this.RdDrawByAssembleyParts.TabIndex = 12;
            this.RdDrawByAssembleyParts.TabStop = true;
            this.RdDrawByAssembleyParts.Text = "Draw By Assembley Parts";
            this.RdDrawByAssembleyParts.UseVisualStyleBackColor = true;
            // 
            // GbxDrawing
            // 
            this.GbxDrawing.Controls.Add(this.RdDrawBy);
            this.GbxDrawing.Controls.Add(this.RdDrawByAssembleyParts);
            this.GbxDrawing.Controls.Add(this.btnMultiPart);
            this.GbxDrawing.Controls.Add(this.txtPartID);
            this.GbxDrawing.Controls.Add(this.btnSinglePart);
            this.GbxDrawing.Controls.Add(this.cmbDrgTemplates);
            this.GbxDrawing.Controls.Add(this.cmbSheetSize);
            this.GbxDrawing.Controls.Add(this.cmbView);
            this.GbxDrawing.Controls.Add(this.cmbScales);
            this.GbxDrawing.Controls.Add(this.txtViewLabels);
            this.GbxDrawing.Location = new System.Drawing.Point(247, 12);
            this.GbxDrawing.Name = "GbxDrawing";
            this.GbxDrawing.Size = new System.Drawing.Size(495, 195);
            this.GbxDrawing.TabIndex = 13;
            this.GbxDrawing.TabStop = false;
            this.GbxDrawing.Text = "Drawing";
            // 
            // GrpNCExport
            // 
            this.GrpNCExport.Controls.Add(this.CmbSaveFolders);
            this.GrpNCExport.Controls.Add(this.BtnFolderBrowser);
            this.GrpNCExport.Controls.Add(this.BtnExport);
            this.GrpNCExport.Location = new System.Drawing.Point(247, 213);
            this.GrpNCExport.Name = "GrpNCExport";
            this.GrpNCExport.Size = new System.Drawing.Size(495, 211);
            this.GrpNCExport.TabIndex = 15;
            this.GrpNCExport.TabStop = false;
            this.GrpNCExport.Text = "Export NC";
            // 
            // CmbSaveFolders
            // 
            this.CmbSaveFolders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbSaveFolders.FormattingEnabled = true;
            this.CmbSaveFolders.Location = new System.Drawing.Point(6, 48);
            this.CmbSaveFolders.Name = "CmbSaveFolders";
            this.CmbSaveFolders.Size = new System.Drawing.Size(420, 21);
            this.CmbSaveFolders.TabIndex = 16;
            // 
            // BtnFolderBrowser
            // 
            this.BtnFolderBrowser.Location = new System.Drawing.Point(101, 75);
            this.BtnFolderBrowser.Name = "BtnFolderBrowser";
            this.BtnFolderBrowser.Size = new System.Drawing.Size(89, 23);
            this.BtnFolderBrowser.TabIndex = 17;
            this.BtnFolderBrowser.Text = "Save Folder";
            this.BtnFolderBrowser.UseVisualStyleBackColor = true;
            this.BtnFolderBrowser.Click += new System.EventHandler(this.BtnFolderBrowser_Click);
            // 
            // BtnExport
            // 
            this.BtnExport.Location = new System.Drawing.Point(6, 75);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(89, 23);
            this.BtnExport.TabIndex = 0;
            this.BtnExport.Text = "Export All";
            this.BtnExport.UseVisualStyleBackColor = true;
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // DrawParts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 527);
            this.Controls.Add(this.GrpNCExport);
            this.Controls.Add(this.GbxDrawing);
            this.Controls.Add(this.ListComponents);
            this.Name = "DrawParts";
            this.Text = "Draw Parts";
            this.Load += new System.EventHandler(this.DrawParts_Load);
            this.GbxDrawing.ResumeLayout(false);
            this.GbxDrawing.PerformLayout();
            this.GrpNCExport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListComponents;
        private System.Windows.Forms.Button btnMultiPart;
        private System.Windows.Forms.TextBox txtPartID;
        private System.Windows.Forms.ComboBox cmbDrgTemplates;
        private System.Windows.Forms.ComboBox cmbView;
        private System.Windows.Forms.TextBox txtViewLabels;
        private System.Windows.Forms.ComboBox cmbScales;
        private System.Windows.Forms.ComboBox cmbSheetSize;
        private System.Windows.Forms.Button btnSinglePart;
        private System.Windows.Forms.RadioButton RdDrawBy;
        private System.Windows.Forms.RadioButton RdDrawByAssembleyParts;
        private System.Windows.Forms.GroupBox GbxDrawing;
        private System.Windows.Forms.GroupBox GrpNCExport;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.ComboBox CmbSaveFolders;
        private System.Windows.Forms.Button BtnFolderBrowser;
    }
}

