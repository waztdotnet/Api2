namespace SolidDecripter
{
    partial class frmDesriptor
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
            this.btnRun = new System.Windows.Forms.Button();
            this.lstNames = new System.Windows.Forms.ListBox();
            this.cmbMaterials = new System.Windows.Forms.ComboBox();
            this.cmbAppearances = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // lstNames
            // 
            this.lstNames.FormattingEnabled = true;
            this.lstNames.Location = new System.Drawing.Point(205, 12);
            this.lstNames.Name = "lstNames";
            this.lstNames.Size = new System.Drawing.Size(167, 199);
            this.lstNames.TabIndex = 4;
            // 
            // cmbMaterials
            // 
            this.cmbMaterials.FormattingEnabled = true;
            this.cmbMaterials.Location = new System.Drawing.Point(12, 218);
            this.cmbMaterials.Name = "cmbMaterials";
            this.cmbMaterials.Size = new System.Drawing.Size(150, 21);
            this.cmbMaterials.TabIndex = 6;
            this.cmbMaterials.DropDown += new System.EventHandler(this.cmbMaterials_DropDown);
            // 
            // cmbAppearances
            // 
            this.cmbAppearances.FormattingEnabled = true;
            this.cmbAppearances.Location = new System.Drawing.Point(205, 218);
            this.cmbAppearances.Name = "cmbAppearances";
            this.cmbAppearances.Size = new System.Drawing.Size(150, 21);
            this.cmbAppearances.TabIndex = 7;
            // 
            // frmDesriptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 315);
            this.Controls.Add(this.cmbAppearances);
            this.Controls.Add(this.cmbMaterials);
            this.Controls.Add(this.lstNames);
            this.Controls.Add(this.btnRun);
            this.Name = "frmDesriptor";
            this.Text = "Solid Decriptor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ListBox lstNames;
        private System.Windows.Forms.ComboBox cmbMaterials;
        private System.Windows.Forms.ComboBox cmbAppearances;
    }
}

