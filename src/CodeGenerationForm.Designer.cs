namespace mapper_refactor;

partial class CodeGenerationForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtApi;
    private TextBox txtSchema;
    private TextBox txtResult;
    private Button btnGenerateSchema;
    private Button btnGenerateAll;
    private Button btnCopy;
    private Button btnSave;
    private Label lblApi;
    private Label lblSchema;
    private Label lblResult;

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
        this.components = new System.ComponentModel.Container();

        // API input
        this.lblApi = new Label();
        this.lblApi.Text = "API Name:";
        this.lblApi.Location = new System.Drawing.Point(12, 15);
        this.lblApi.Size = new System.Drawing.Size(80, 20);

        this.txtApi = new TextBox();
        this.txtApi.Location = new System.Drawing.Point(100, 12);
        this.txtApi.Size = new System.Drawing.Size(200, 23);

        // Schema input
        this.lblSchema = new Label();
        this.lblSchema.Text = "Schema:";
        this.lblSchema.Location = new System.Drawing.Point(12, 45);
        this.lblSchema.Size = new System.Drawing.Size(80, 20);

        this.txtSchema = new TextBox();
        this.txtSchema.Location = new System.Drawing.Point(100, 42);
        this.txtSchema.Size = new System.Drawing.Size(200, 23);
        this.txtSchema.Text = "migration";

        // Generate buttons
        this.btnGenerateSchema = new Button();
        this.btnGenerateSchema.Text = "Generate Schema";
        this.btnGenerateSchema.Location = new System.Drawing.Point(320, 12);
        this.btnGenerateSchema.Size = new System.Drawing.Size(120, 30);
        this.btnGenerateSchema.Click += new System.EventHandler(this.btnGenerateSchema_Click);

        this.btnGenerateAll = new Button();
        this.btnGenerateAll.Text = "Generate All";
        this.btnGenerateAll.Location = new System.Drawing.Point(320, 42);
        this.btnGenerateAll.Size = new System.Drawing.Size(120, 30);
        this.btnGenerateAll.Click += new System.EventHandler(this.btnGenerateAll_Click);

        // Result area
        this.lblResult = new Label();
        this.lblResult.Text = "Generated SQL:";
        this.lblResult.Location = new System.Drawing.Point(12, 80);
        this.lblResult.Size = new System.Drawing.Size(100, 20);

        this.txtResult = new TextBox();
        this.txtResult.Location = new System.Drawing.Point(12, 105);
        this.txtResult.Size = new System.Drawing.Size(760, 400);
        this.txtResult.Multiline = true;
        this.txtResult.ScrollBars = ScrollBars.Both;
        this.txtResult.WordWrap = false;
        this.txtResult.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

        // Copy and Save buttons
        this.btnCopy = new Button();
        this.btnCopy.Text = "Copy to Clipboard";
        this.btnCopy.Location = new System.Drawing.Point(12, 515);
        this.btnCopy.Size = new System.Drawing.Size(120, 30);
        this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);

        this.btnSave = new Button();
        this.btnSave.Text = "Save to File";
        this.btnSave.Location = new System.Drawing.Point(142, 515);
        this.btnSave.Size = new System.Drawing.Size(120, 30);
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

        // Form settings
        this.ClientSize = new System.Drawing.Size(784, 561);
        this.Controls.AddRange(new Control[] {
            this.lblApi, this.txtApi,
            this.lblSchema, this.txtSchema,
            this.btnGenerateSchema, this.btnGenerateAll,
            this.lblResult, this.txtResult,
            this.btnCopy, this.btnSave
        });

        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Code Generation";
    }
} 