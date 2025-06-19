namespace mapper_refactor;

partial class ViewGenerationForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtApi;
    private TextBox txtViews;
    private TextBox txtLoaderScript;
    private Button btnGenerateViews;
    private Button btnGenerateAll;
    private Button btnCopyViews;
    private Button btnCopyLoader;
    private Button btnSaveViews;
    private Button btnSaveLoader;
    private Label lblApi;
    private Label lblViews;
    private Label lblLoaderScript;
    private SplitContainer splitContainer;

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

        // Generate buttons
        this.btnGenerateViews = new Button();
        this.btnGenerateViews.Text = "Generate Views";
        this.btnGenerateViews.Location = new System.Drawing.Point(320, 12);
        this.btnGenerateViews.Size = new System.Drawing.Size(120, 30);
        this.btnGenerateViews.Click += new System.EventHandler(this.btnGenerateViews_Click);

        this.btnGenerateAll = new Button();
        this.btnGenerateAll.Text = "Generate All";
        this.btnGenerateAll.Location = new System.Drawing.Point(450, 12);
        this.btnGenerateAll.Size = new System.Drawing.Size(120, 30);
        this.btnGenerateAll.Click += new System.EventHandler(this.btnGenerateAll_Click);

        // Split container for views and loader script
        this.splitContainer = new SplitContainer();
        this.splitContainer.Location = new System.Drawing.Point(12, 50);
        this.splitContainer.Size = new System.Drawing.Size(960, 500);
        this.splitContainer.SplitterDistance = 480;
        this.splitContainer.Orientation = Orientation.Horizontal;
        this.splitContainer.Panel1MinSize = 200;
        this.splitContainer.Panel2MinSize = 200;

        // Views panel
        this.lblViews = new Label();
        this.lblViews.Text = "Generated Views:";
        this.lblViews.Location = new System.Drawing.Point(0, 0);
        this.lblViews.Size = new System.Drawing.Size(100, 20);
        this.splitContainer.Panel1.Controls.Add(this.lblViews);

        this.txtViews = new TextBox();
        this.txtViews.Location = new System.Drawing.Point(0, 25);
        this.txtViews.Size = new System.Drawing.Size(960, 200);
        this.txtViews.Multiline = true;
        this.txtViews.ScrollBars = ScrollBars.Both;
        this.txtViews.WordWrap = false;
        this.txtViews.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.splitContainer.Panel1.Controls.Add(this.txtViews);

        this.btnCopyViews = new Button();
        this.btnCopyViews.Text = "Copy Views";
        this.btnCopyViews.Location = new System.Drawing.Point(0, 230);
        this.btnCopyViews.Size = new System.Drawing.Size(100, 30);
        this.btnCopyViews.Click += new System.EventHandler(this.btnCopyViews_Click);
        this.splitContainer.Panel1.Controls.Add(this.btnCopyViews);

        this.btnSaveViews = new Button();
        this.btnSaveViews.Text = "Save Views";
        this.btnSaveViews.Location = new System.Drawing.Point(110, 230);
        this.btnSaveViews.Size = new System.Drawing.Size(100, 30);
        this.btnSaveViews.Click += new System.EventHandler(this.btnSaveViews_Click);
        this.splitContainer.Panel1.Controls.Add(this.btnSaveViews);

        // Loader script panel
        this.lblLoaderScript = new Label();
        this.lblLoaderScript.Text = "Loader Script:";
        this.lblLoaderScript.Location = new System.Drawing.Point(0, 0);
        this.lblLoaderScript.Size = new System.Drawing.Size(100, 20);
        this.splitContainer.Panel2.Controls.Add(this.lblLoaderScript);

        this.txtLoaderScript = new TextBox();
        this.txtLoaderScript.Location = new System.Drawing.Point(0, 25);
        this.txtLoaderScript.Size = new System.Drawing.Size(960, 200);
        this.txtLoaderScript.Multiline = true;
        this.txtLoaderScript.ScrollBars = ScrollBars.Both;
        this.txtLoaderScript.WordWrap = false;
        this.txtLoaderScript.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.splitContainer.Panel2.Controls.Add(this.txtLoaderScript);

        this.btnCopyLoader = new Button();
        this.btnCopyLoader.Text = "Copy Loader";
        this.btnCopyLoader.Location = new System.Drawing.Point(0, 230);
        this.btnCopyLoader.Size = new System.Drawing.Size(100, 30);
        this.btnCopyLoader.Click += new System.EventHandler(this.btnCopyLoader_Click);
        this.splitContainer.Panel2.Controls.Add(this.btnCopyLoader);

        this.btnSaveLoader = new Button();
        this.btnSaveLoader.Text = "Save Loader";
        this.btnSaveLoader.Location = new System.Drawing.Point(110, 230);
        this.btnSaveLoader.Size = new System.Drawing.Size(100, 30);
        this.btnSaveLoader.Click += new System.EventHandler(this.btnSaveLoader_Click);
        this.splitContainer.Panel2.Controls.Add(this.btnSaveLoader);

        // Form settings
        this.ClientSize = new System.Drawing.Size(984, 561);
        this.Controls.AddRange(new Control[] {
            this.lblApi, this.txtApi,
            this.btnGenerateViews, this.btnGenerateAll,
            this.splitContainer
        });

        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "View Generation";
    }
} 