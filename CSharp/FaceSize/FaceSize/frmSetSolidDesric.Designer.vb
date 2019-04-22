<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSetSolidDescription
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnSetPartSize = New System.Windows.Forms.Button()
        Me.lstSizes = New System.Windows.Forms.ListBox()
        Me.cmbSetPlane = New System.Windows.Forms.ComboBox()
        Me.btnPickPart = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnSetPartSize
        '
        Me.btnSetPartSize.Location = New System.Drawing.Point(12, 109)
        Me.btnSetPartSize.Name = "btnSetPartSize"
        Me.btnSetPartSize.Size = New System.Drawing.Size(90, 23)
        Me.btnSetPartSize.TabIndex = 1
        Me.btnSetPartSize.Text = "Set Part Size"
        Me.btnSetPartSize.UseVisualStyleBackColor = True
        '
        'lstSizes
        '
        Me.lstSizes.FormattingEnabled = True
        Me.lstSizes.Location = New System.Drawing.Point(12, 149)
        Me.lstSizes.Name = "lstSizes"
        Me.lstSizes.Size = New System.Drawing.Size(121, 30)
        Me.lstSizes.TabIndex = 2
        '
        'cmbSetPlane
        '
        Me.cmbSetPlane.DisplayMember = "1"
        Me.cmbSetPlane.FormattingEnabled = True
        Me.cmbSetPlane.Items.AddRange(New Object() {"XY Plane", "YZ Plane", "XZ Plane"})
        Me.cmbSetPlane.Location = New System.Drawing.Point(12, 82)
        Me.cmbSetPlane.Name = "cmbSetPlane"
        Me.cmbSetPlane.Size = New System.Drawing.Size(121, 21)
        Me.cmbSetPlane.TabIndex = 3
        Me.cmbSetPlane.Text = "XY Plane"
        '
        'btnPickPart
        '
        Me.btnPickPart.Location = New System.Drawing.Point(395, 46)
        Me.btnPickPart.Name = "btnPickPart"
        Me.btnPickPart.Size = New System.Drawing.Size(75, 23)
        Me.btnPickPart.TabIndex = 4
        Me.btnPickPart.Text = "PickPart"
        Me.btnPickPart.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnPickPart.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(187, 35)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmSetSolidDescription
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 581)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.btnPickPart)
        Me.Controls.Add(Me.cmbSetPlane)
        Me.Controls.Add(Me.lstSizes)
        Me.Controls.Add(Me.btnSetPartSize)
        Me.Controls.Add(Me.Button1)
        Me.Name = "frmSetSolidDescription"
        Me.Text = "rcadz profiler"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents btnSetPartSize As Button
    Friend WithEvents lstSizes As ListBox
    Friend WithEvents cmbSetPlane As ComboBox
    Friend WithEvents btnPickPart As Button
    Friend WithEvents Button2 As Button
End Class
