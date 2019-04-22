namespace InventorAutoSpool
    {
    partial class ProjectProperties
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
            this.ProjectFolderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.btnProjectSaveFolder = new System.Windows.Forms.Button();
            this.txtProjectSaveFolder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ProjectFolderBrowserDlg
            // 
            this.ProjectFolderBrowserDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.ProjectFolderBrowserDlg.SelectedPath = "F:\\Test";
            // 
            // btnProjectSaveFolder
            // 
            this.btnProjectSaveFolder.Location = new System.Drawing.Point(358, 31);
            this.btnProjectSaveFolder.Name = "btnProjectSaveFolder";
            this.btnProjectSaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btnProjectSaveFolder.TabIndex = 6;
            this.btnProjectSaveFolder.Text = "Folder";
            this.btnProjectSaveFolder.UseVisualStyleBackColor = true;
            // 
            // txtProjectSaveFolder
            // 
            this.txtProjectSaveFolder.Location = new System.Drawing.Point(12, 31);
            this.txtProjectSaveFolder.Name = "txtProjectSaveFolder";
            this.txtProjectSaveFolder.Size = new System.Drawing.Size(328, 20);
            this.txtProjectSaveFolder.TabIndex = 5;
            // 
            // ProjectProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 271);
            this.Controls.Add(this.btnProjectSaveFolder);
            this.Controls.Add(this.txtProjectSaveFolder);
            this.Name = "ProjectProperties";
            this.Text = "Project Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog ProjectFolderBrowserDlg;
        private System.Windows.Forms.Button btnProjectSaveFolder;
        private System.Windows.Forms.TextBox txtProjectSaveFolder;
        }
    }