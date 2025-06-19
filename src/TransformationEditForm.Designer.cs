namespace mapper_refactor;

partial class TransformationEditForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblFieldInfo = new Label();
        txtTransformation = new TextBox();
        btnOK = new Button();
        btnCancel = new Button();
        btnClear = new Button();
        btnTest = new Button();
        btnHelp = new Button();
        SuspendLayout();
        // 
        // lblFieldInfo
        // 
        lblFieldInfo.AutoSize = true;
        lblFieldInfo.Location = new Point(12, 9);
        lblFieldInfo.Name = "lblFieldInfo";
        lblFieldInfo.Size = new Size(38, 15);
        lblFieldInfo.TabIndex = 0;
        lblFieldInfo.Text = "Field: ";
        // 
        // txtTransformation
        // 
        txtTransformation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtTransformation.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        txtTransformation.Location = new Point(12, 27);
        txtTransformation.Multiline = true;
        txtTransformation.Name = "txtTransformation";
        txtTransformation.ScrollBars = ScrollBars.Both;
        txtTransformation.Size = new Size(560, 173);
        txtTransformation.TabIndex = 1;
        txtTransformation.WordWrap = false;
        // 
        // btnOK
        // 
        btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnOK.Location = new Point(416, 206);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(75, 23);
        btnOK.TabIndex = 2;
        btnOK.Text = "OK";
        btnOK.UseVisualStyleBackColor = true;
        btnOK.Click += btnOK_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(497, 206);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnClear
        // 
        btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClear.Location = new Point(12, 206);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(75, 23);
        btnClear.TabIndex = 4;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // btnTest
        // 
        btnTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnTest.Location = new Point(93, 206);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(75, 23);
        btnTest.TabIndex = 5;
        btnTest.Text = "Test";
        btnTest.UseVisualStyleBackColor = true;
        btnTest.Click += btnTest_Click;
        // 
        // btnHelp
        // 
        btnHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnHelp.Location = new Point(174, 206);
        btnHelp.Name = "btnHelp";
        btnHelp.Size = new Size(75, 23);
        btnHelp.TabIndex = 6;
        btnHelp.Text = "Help";
        btnHelp.UseVisualStyleBackColor = true;
        btnHelp.Click += btnHelp_Click;
        // 
        // TransformationEditForm
        // 
        AcceptButton = btnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(584, 241);
        Controls.Add(btnHelp);
        Controls.Add(btnTest);
        Controls.Add(btnClear);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        Controls.Add(txtTransformation);
        Controls.Add(lblFieldInfo);
        MinimumSize = new Size(600, 280);
        Name = "TransformationEditForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Transformation";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblFieldInfo;
    public TextBox txtTransformation;
    private Button btnOK;
    private Button btnCancel;
    private Button btnClear;
    private Button btnTest;
    private Button btnHelp;
} 