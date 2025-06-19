namespace mapper_refactor;

partial class NewMappingForm
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
        this.lblMapName = new Label();
        this.txtMapName = new TextBox();
        this.btnOK = new Button();
        this.btnCancel = new Button();
        this.SuspendLayout();
        // 
        // lblMapName
        // 
        this.lblMapName.AutoSize = true;
        this.lblMapName.Location = new Point(12, 15);
        this.lblMapName.Name = "lblMapName";
        this.lblMapName.Size = new Size(79, 15);
        this.lblMapName.TabIndex = 0;
        this.lblMapName.Text = "Mapping Name:";
        // 
        // txtMapName
        // 
        this.txtMapName.Location = new Point(97, 12);
        this.txtMapName.Name = "txtMapName";
        this.txtMapName.Size = new Size(275, 23);
        this.txtMapName.TabIndex = 1;
        // 
        // btnOK
        // 
        this.btnOK.Location = new Point(216, 41);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new Size(75, 23);
        this.btnOK.TabIndex = 2;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new EventHandler(this.btnOK_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new Point(297, 41);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(75, 23);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
        // 
        // NewMappingForm
        // 
        this.AcceptButton = this.btnOK;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new Size(384, 76);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.txtMapName);
        this.Controls.Add(this.lblMapName);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "NewMappingForm";
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "New Mapping";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Label lblMapName;
    private TextBox txtMapName;
    private Button btnOK;
    private Button btnCancel;
} 