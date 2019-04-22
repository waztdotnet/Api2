namespace Drawing_Creation_Forms_Application
{
    partial class DrawingCreationForm
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
                _started = false; 
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.dlgOpenFileBrowser = new System.Windows.Forms.OpenFileDialog();
            this.txtOpenPath = new System.Windows.Forms.TextBox();
            this.btnCreatDrawings = new System.Windows.Forms.Button();
            this.treList = new System.Windows.Forms.TreeView();
            this.cmbDimStyles = new System.Windows.Forms.ComboBox();
            this.lblDimStyle = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblBorder = new System.Windows.Forms.Label();
            this.lblTitleBlock = new System.Windows.Forms.Label();
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbBorder = new System.Windows.Forms.ComboBox();
            this.cmbTitle_Block = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(37, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(221, 60);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "btnOpen";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // dlgOpenFileBrowser
            // 
            this.dlgOpenFileBrowser.DefaultExt = "iam";
            this.dlgOpenFileBrowser.FileName = ".iam";
            this.dlgOpenFileBrowser.Filter = "Assembly Files | *.iam";
            this.dlgOpenFileBrowser.InitialDirectory = "E:\\";
            // 
            // txtOpenPath
            // 
            this.txtOpenPath.Location = new System.Drawing.Point(37, 78);
            this.txtOpenPath.Name = "txtOpenPath";
            this.txtOpenPath.Size = new System.Drawing.Size(221, 20);
            this.txtOpenPath.TabIndex = 3;
            // 
            // btnCreatDrawings
            // 
            this.btnCreatDrawings.Location = new System.Drawing.Point(37, 104);
            this.btnCreatDrawings.Name = "btnCreatDrawings";
            this.btnCreatDrawings.Size = new System.Drawing.Size(221, 60);
            this.btnCreatDrawings.TabIndex = 4;
            this.btnCreatDrawings.Text = "Creat Drawings";
            this.btnCreatDrawings.UseVisualStyleBackColor = true;
            this.btnCreatDrawings.Click += new System.EventHandler(this.btnCreatDrawings_Click);
            // 
            // treList
            // 
            this.treList.CheckBoxes = true;
            this.treList.Location = new System.Drawing.Point(37, 445);
            this.treList.Name = "treList";
            this.treList.Size = new System.Drawing.Size(221, 120);
            this.treList.TabIndex = 7;
            // 
            // cmbDimStyles
            // 
            this.cmbDimStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDimStyles.FormattingEnabled = true;
            this.cmbDimStyles.Location = new System.Drawing.Point(37, 201);
            this.cmbDimStyles.Name = "cmbDimStyles";
            this.cmbDimStyles.Size = new System.Drawing.Size(221, 21);
            this.cmbDimStyles.Sorted = true;
            this.cmbDimStyles.TabIndex = 9;
            // 
            // lblDimStyle
            // 
            this.lblDimStyle.AutoSize = true;
            this.lblDimStyle.Location = new System.Drawing.Point(37, 182);
            this.lblDimStyle.Name = "lblDimStyle";
            this.lblDimStyle.Size = new System.Drawing.Size(82, 13);
            this.lblDimStyle.TabIndex = 10;
            this.lblDimStyle.Text = "Dimension Style";
            // 
            // cmbLayers
            // 
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Location = new System.Drawing.Point(37, 249);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Size = new System.Drawing.Size(221, 21);
            this.cmbLayers.TabIndex = 11;
            // 
            // lblBorder
            // 
            this.lblBorder.AutoSize = true;
            this.lblBorder.Location = new System.Drawing.Point(37, 339);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(82, 13);
            this.lblBorder.TabIndex = 12;
            this.lblBorder.Text = "Dimension Style";
            // 
            // lblTitleBlock
            // 
            this.lblTitleBlock.AutoSize = true;
            this.lblTitleBlock.Location = new System.Drawing.Point(34, 284);
            this.lblTitleBlock.Name = "lblTitleBlock";
            this.lblTitleBlock.Size = new System.Drawing.Size(60, 13);
            this.lblTitleBlock.TabIndex = 13;
            this.lblTitleBlock.Text = "Title_Block";
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(37, 233);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(33, 13);
            this.lblLayer.TabIndex = 14;
            this.lblLayer.Text = "Layer";
            // 
            // cmbBorder
            // 
            this.cmbBorder.FormattingEnabled = true;
            this.cmbBorder.Location = new System.Drawing.Point(37, 365);
            this.cmbBorder.Name = "cmbBorder";
            this.cmbBorder.Size = new System.Drawing.Size(218, 21);
            this.cmbBorder.TabIndex = 15;
            // 
            // cmbTitle_Block
            // 
            this.cmbTitle_Block.FormattingEnabled = true;
            this.cmbTitle_Block.Location = new System.Drawing.Point(37, 300);
            this.cmbTitle_Block.Name = "cmbTitle_Block";
            this.cmbTitle_Block.Size = new System.Drawing.Size(218, 21);
            this.cmbTitle_Block.TabIndex = 16;
            // 
            // DrawingCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 592);
            this.Controls.Add(this.cmbTitle_Block);
            this.Controls.Add(this.cmbBorder);
            this.Controls.Add(this.lblLayer);
            this.Controls.Add(this.lblTitleBlock);
            this.Controls.Add(this.lblBorder);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblDimStyle);
            this.Controls.Add(this.cmbDimStyles);
            this.Controls.Add(this.treList);
            this.Controls.Add(this.btnCreatDrawings);
            this.Controls.Add(this.txtOpenPath);
            this.Controls.Add(this.btnOpen);
            this.Name = "DrawingCreationForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.OpenFileDialog dlgOpenFileBrowser;
        private System.Windows.Forms.TextBox txtOpenPath;
        private System.Windows.Forms.Button btnCreatDrawings;
        private System.Windows.Forms.TreeView treList;
        private System.Windows.Forms.ComboBox cmbDimStyles;
        private System.Windows.Forms.Label lblDimStyle;
        private System.Windows.Forms.ComboBox cmbLayers;
        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.Label lblTitleBlock;
        private System.Windows.Forms.Label lblLayer;
        private System.Windows.Forms.ComboBox cmbBorder;
        private System.Windows.Forms.ComboBox cmbTitle_Block;
    }
}

