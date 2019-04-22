namespace InventorTestWinApp
{
	partial class TestFrm
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
			if(disposing && ( components != null ))
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
			this.btnTest = new System.Windows.Forms.Button();
			this.txtValue1 = new System.Windows.Forms.TextBox();
			this.txtValue2 = new System.Windows.Forms.TextBox();
			this.txtValue3 = new System.Windows.Forms.TextBox();
			this.lblExpression1 = new System.Windows.Forms.Label();
			this.lblExpression2 = new System.Windows.Forms.Label();
			this.lblExpression3 = new System.Windows.Forms.Label();
			this.txtValue4 = new System.Windows.Forms.TextBox();
			this.txtValue5 = new System.Windows.Forms.TextBox();
			this.txtValue6 = new System.Windows.Forms.TextBox();
			this.lblExpression4 = new System.Windows.Forms.Label();
			this.lblExpression5 = new System.Windows.Forms.Label();
			this.lblExpression6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(89, 12);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 0;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// txtValue1
			// 
			this.txtValue1.Location = new System.Drawing.Point(78, 50);
			this.txtValue1.Name = "txtValue1";
			this.txtValue1.Size = new System.Drawing.Size(100, 20);
			this.txtValue1.TabIndex = 1;
			this.txtValue1.Text = "255";
			// 
			// txtValue2
			// 
			this.txtValue2.Location = new System.Drawing.Point(78, 76);
			this.txtValue2.Name = "txtValue2";
			this.txtValue2.Size = new System.Drawing.Size(100, 20);
			this.txtValue2.TabIndex = 2;
			this.txtValue2.Text = "901";
			// 
			// txtValue3
			// 
			this.txtValue3.Location = new System.Drawing.Point(78, 102);
			this.txtValue3.Name = "txtValue3";
			this.txtValue3.Size = new System.Drawing.Size(100, 20);
			this.txtValue3.TabIndex = 3;
			this.txtValue3.Text = "50000";
			// 
			// lblExpression1
			// 
			this.lblExpression1.AutoSize = true;
			this.lblExpression1.Location = new System.Drawing.Point(75, 223);
			this.lblExpression1.Name = "lblExpression1";
			this.lblExpression1.Size = new System.Drawing.Size(13, 13);
			this.lblExpression1.TabIndex = 4;
			this.lblExpression1.Text = "0";
			// 
			// lblExpression2
			// 
			this.lblExpression2.AutoSize = true;
			this.lblExpression2.Location = new System.Drawing.Point(75, 248);
			this.lblExpression2.Name = "lblExpression2";
			this.lblExpression2.Size = new System.Drawing.Size(13, 13);
			this.lblExpression2.TabIndex = 5;
			this.lblExpression2.Text = "0";
			// 
			// lblExpression3
			// 
			this.lblExpression3.AutoSize = true;
			this.lblExpression3.Location = new System.Drawing.Point(75, 273);
			this.lblExpression3.Name = "lblExpression3";
			this.lblExpression3.Size = new System.Drawing.Size(13, 13);
			this.lblExpression3.TabIndex = 6;
			this.lblExpression3.Text = "0";
			// 
			// txtValue4
			// 
			this.txtValue4.Location = new System.Drawing.Point(78, 129);
			this.txtValue4.Name = "txtValue4";
			this.txtValue4.Size = new System.Drawing.Size(100, 20);
			this.txtValue4.TabIndex = 7;
			this.txtValue4.Text = "100";
			// 
			// txtValue5
			// 
			this.txtValue5.Location = new System.Drawing.Point(78, 156);
			this.txtValue5.Name = "txtValue5";
			this.txtValue5.Size = new System.Drawing.Size(100, 20);
			this.txtValue5.TabIndex = 8;
			this.txtValue5.Text = "18";
			// 
			// txtValue6
			// 
			this.txtValue6.Location = new System.Drawing.Point(78, 183);
			this.txtValue6.Name = "txtValue6";
			this.txtValue6.Size = new System.Drawing.Size(100, 20);
			this.txtValue6.TabIndex = 9;
			this.txtValue6.Text = "16";
			// 
			// lblExpression4
			// 
			this.lblExpression4.AutoSize = true;
			this.lblExpression4.Location = new System.Drawing.Point(75, 298);
			this.lblExpression4.Name = "lblExpression4";
			this.lblExpression4.Size = new System.Drawing.Size(13, 13);
			this.lblExpression4.TabIndex = 10;
			this.lblExpression4.Text = "0";
			// 
			// lblExpression5
			// 
			this.lblExpression5.AutoSize = true;
			this.lblExpression5.Location = new System.Drawing.Point(75, 323);
			this.lblExpression5.Name = "lblExpression5";
			this.lblExpression5.Size = new System.Drawing.Size(13, 13);
			this.lblExpression5.TabIndex = 11;
			this.lblExpression5.Text = "0";
			// 
			// lblExpression6
			// 
			this.lblExpression6.AutoSize = true;
			this.lblExpression6.Location = new System.Drawing.Point(75, 348);
			this.lblExpression6.Name = "lblExpression6";
			this.lblExpression6.Size = new System.Drawing.Size(13, 13);
			this.lblExpression6.TabIndex = 12;
			this.lblExpression6.Text = "0";
			// 
			// TestFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(273, 408);
			this.Controls.Add(this.lblExpression6);
			this.Controls.Add(this.lblExpression5);
			this.Controls.Add(this.lblExpression4);
			this.Controls.Add(this.txtValue6);
			this.Controls.Add(this.txtValue5);
			this.Controls.Add(this.txtValue4);
			this.Controls.Add(this.lblExpression3);
			this.Controls.Add(this.lblExpression2);
			this.Controls.Add(this.lblExpression1);
			this.Controls.Add(this.txtValue3);
			this.Controls.Add(this.txtValue2);
			this.Controls.Add(this.txtValue1);
			this.Controls.Add(this.btnTest);
			this.Name = "TestFrm";
			this.Text = "Form1";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TestFrm_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.TextBox txtValue1;
		private System.Windows.Forms.TextBox txtValue2;
		private System.Windows.Forms.TextBox txtValue3;
		private System.Windows.Forms.Label lblExpression1;
		private System.Windows.Forms.Label lblExpression2;
		private System.Windows.Forms.Label lblExpression3;
		private System.Windows.Forms.TextBox txtValue4;
		private System.Windows.Forms.TextBox txtValue5;
		private System.Windows.Forms.TextBox txtValue6;
		private System.Windows.Forms.Label lblExpression4;
		private System.Windows.Forms.Label lblExpression5;
		private System.Windows.Forms.Label lblExpression6;
	}
}

