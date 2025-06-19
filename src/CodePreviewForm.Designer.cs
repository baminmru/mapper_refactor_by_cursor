namespace mapper_refactor;

partial class CodePreviewForm
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
        txtCode = new TextBox();
        btnCopy = new Button();
        btnSave = new Button();
        btnClose = new Button();
        SuspendLayout();
        // 
        // txtCode
        // 
        txtCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtCode.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        txtCode.Location = new Point(12, 12);
        txtCode.Multiline = true;
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.ScrollBars = ScrollBars.Both;
        txtCode.Size = new Size(760, 499);
        txtCode.TabIndex = 0;
        txtCode.WordWrap = false;
        // 
        // btnCopy
        // 
        btnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCopy.Location = new Point(516, 526);
        btnCopy.Name = "btnCopy";
        btnCopy.Size = new Size(75, 23);
        btnCopy.TabIndex = 1;
        btnCopy.Text = "Copy";
        btnCopy.UseVisualStyleBackColor = true;
        btnCopy.Click += btnCopy_Click;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(616, 526);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 2;
        btnSave.Text = "Save...";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.Location = new Point(697, 526);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 23);
        btnClose.TabIndex = 3;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        // 
        // CodePreviewForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 561);
        Controls.Add(btnClose);
        Controls.Add(btnSave);
        Controls.Add(btnCopy);
        Controls.Add(txtCode);
        MinimumSize = new Size(800, 600);
        Name = "CodePreviewForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Generated Code Preview";
        ResumeLayout(false);
        PerformLayout();
    }

    private TextBox txtCode;
    private Button btnCopy;
    private Button btnSave;
    private Button btnClose;
} 