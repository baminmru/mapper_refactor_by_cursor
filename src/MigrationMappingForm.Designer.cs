namespace mapper_refactor;

partial class MigrationMappingForm
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
        components = new System.ComponentModel.Container();
        splitContainer1 = new SplitContainer();
        groupBox1 = new GroupBox();
        txtSourceFilter = new TextBox();
        lblSourceFilter = new Label();
        lstSourceFields = new ListBox();
        lstSourceTables = new ListBox();
        label1 = new Label();
        groupBox2 = new GroupBox();
        txtDestFilter = new TextBox();
        lblDestFilter = new Label();
        lstDestFields = new ListBox();
        lstDestTables = new ListBox();
        label2 = new Label();
        panel1 = new Panel();
        btnGenerateCode = new Button();
        btnEditTransformation = new Button();
        btnRemoveMapping = new Button();
        btnAddMapping = new Button();
        btnNewMapping = new Button();
        cboMappings = new ComboBox();
        label3 = new Label();
        gridMappings = new DataGridView();
        toolTip = new ToolTip(components);
        splitContainer2 = new SplitContainer();
        grpMappingPreview = new GroupBox();
        txtMappingDetails = new TextBox();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridMappings).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
        splitContainer2.Panel1.SuspendLayout();
        splitContainer2.Panel2.SuspendLayout();
        splitContainer2.SuspendLayout();
        grpMappingPreview.SuspendLayout();
        SuspendLayout();
        
        // splitContainer2 (Main container)
        splitContainer2.Dock = DockStyle.Fill;
        splitContainer2.Orientation = Orientation.Horizontal;
        splitContainer2.Location = new Point(0, 0);
        splitContainer2.Name = "splitContainer2";
        splitContainer2.Size = new Size(1200, 800);
        splitContainer2.SplitterDistance = 500;
        splitContainer2.TabIndex = 0;

        // splitContainer1 (Top part - Source/Dest)
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Size = new Size(1200, 500);
        splitContainer1.SplitterDistance = 600;
        splitContainer1.TabIndex = 0;

        // Source Group
        groupBox1.Dock = DockStyle.Fill;
        groupBox1.Text = "Source";
        groupBox1.Padding = new Padding(10);

        // Source Filter
        lblSourceFilter.AutoSize = true;
        lblSourceFilter.Location = new Point(10, 25);
        lblSourceFilter.Text = "Filter:";
        
        txtSourceFilter.Location = new Point(60, 22);
        txtSourceFilter.Size = new Size(200, 23);
        txtSourceFilter.TextChanged += txtSourceFilter_TextChanged;
        toolTip.SetToolTip(txtSourceFilter, "Type to filter source tables");

        // Source Tables
        label1.AutoSize = true;
        label1.Location = new Point(10, 55);
        label1.Text = "Tables:";

        lstSourceTables.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lstSourceTables.Location = new Point(10, 75);
        lstSourceTables.Size = new Size(570, 200);
        lstSourceTables.SelectedIndexChanged += lstSourceTables_SelectedIndexChanged;
        toolTip.SetToolTip(lstSourceTables, "Select a source table to view its fields");

        // Source Fields
        lstSourceFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstSourceFields.Location = new Point(10, 285);
        lstSourceFields.Size = new Size(570, 200);
        toolTip.SetToolTip(lstSourceFields, "Select a field to map");

        // Destination Group
        groupBox2.Dock = DockStyle.Fill;
        groupBox2.Text = "Destination";
        groupBox2.Padding = new Padding(10);

        // Destination Filter
        lblDestFilter.AutoSize = true;
        lblDestFilter.Location = new Point(10, 25);
        lblDestFilter.Text = "Filter:";
        
        txtDestFilter.Location = new Point(60, 22);
        txtDestFilter.Size = new Size(200, 23);
        txtDestFilter.TextChanged += txtDestFilter_TextChanged;
        toolTip.SetToolTip(txtDestFilter, "Type to filter destination tables");

        // Destination Tables
        label2.AutoSize = true;
        label2.Location = new Point(10, 55);
        label2.Text = "Tables:";

        lstDestTables.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lstDestTables.Location = new Point(10, 75);
        lstDestTables.Size = new Size(570, 200);
        lstDestTables.SelectedIndexChanged += lstDestTables_SelectedIndexChanged;
        toolTip.SetToolTip(lstDestTables, "Select a destination table to view its fields");

        // Destination Fields
        lstDestFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstDestFields.Location = new Point(10, 285);
        lstDestFields.Size = new Size(570, 200);
        toolTip.SetToolTip(lstDestFields, "Select a field to map");

        // Mapping Controls Panel
        panel1.Dock = DockStyle.Top;
        panel1.Height = 40;
        panel1.Padding = new Padding(10, 5, 10, 5);

        // Mapping Selection
        label3.AutoSize = true;
        label3.Location = new Point(10, 12);
        label3.Text = "Mapping:";

        cboMappings.Location = new Point(70, 8);
        cboMappings.Size = new Size(200, 23);
        cboMappings.DropDownStyle = ComboBoxStyle.DropDownList;
        toolTip.SetToolTip(cboMappings, "Select an existing mapping or create a new one");

        // Buttons
        btnNewMapping.Location = new Point(280, 8);
        btnNewMapping.Size = new Size(75, 23);
        btnNewMapping.Text = "New";
        toolTip.SetToolTip(btnNewMapping, "Create a new mapping");

        btnAddMapping.Location = new Point(365, 8);
        btnAddMapping.Size = new Size(75, 23);
        btnAddMapping.Text = "Add";
        toolTip.SetToolTip(btnAddMapping, "Add selected fields to mapping");

        btnRemoveMapping.Location = new Point(450, 8);
        btnRemoveMapping.Size = new Size(75, 23);
        btnRemoveMapping.Text = "Remove";
        toolTip.SetToolTip(btnRemoveMapping, "Remove selected mapping");

        btnEditTransformation.Location = new Point(535, 8);
        btnEditTransformation.Size = new Size(75, 23);
        btnEditTransformation.Text = "Transform";
        toolTip.SetToolTip(btnEditTransformation, "Edit field transformation");

        btnGenerateCode.Location = new Point(620, 8);
        btnGenerateCode.Size = new Size(75, 23);
        btnGenerateCode.Text = "Generate";
        toolTip.SetToolTip(btnGenerateCode, "Generate migration code");

        // Mapping Preview Group
        grpMappingPreview.Dock = DockStyle.Fill;
        grpMappingPreview.Text = "Mapping Preview";
        grpMappingPreview.Padding = new Padding(10);

        // Mapping Details TextBox
        txtMappingDetails.Dock = DockStyle.Fill;
        txtMappingDetails.Multiline = true;
        txtMappingDetails.ReadOnly = true;
        txtMappingDetails.ScrollBars = ScrollBars.Both;
        txtMappingDetails.Font = new Font("Consolas", 9.75F, FontStyle.Regular);
        toolTip.SetToolTip(txtMappingDetails, "Preview of the current mapping details");

        // Grid
        gridMappings.Dock = DockStyle.Fill;
        gridMappings.AllowUserToAddRows = false;
        gridMappings.AllowUserToDeleteRows = false;
        gridMappings.ReadOnly = true;
        gridMappings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridMappings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridMappings.RowHeadersVisible = false;
        toolTip.SetToolTip(gridMappings, "Current mapping entries");

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(splitContainer2);
        MinimumSize = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Migration Mapping";

        // Add controls to containers
        splitContainer2.Panel1.Controls.Add(splitContainer1);
        splitContainer2.Panel2.Controls.Add(grpMappingPreview);

        splitContainer1.Panel1.Controls.Add(groupBox1);
        splitContainer1.Panel2.Controls.Add(groupBox2);

        groupBox1.Controls.Add(lblSourceFilter);
        groupBox1.Controls.Add(txtSourceFilter);
        groupBox1.Controls.Add(label1);
        groupBox1.Controls.Add(lstSourceTables);
        groupBox1.Controls.Add(lstSourceFields);

        groupBox2.Controls.Add(lblDestFilter);
        groupBox2.Controls.Add(txtDestFilter);
        groupBox2.Controls.Add(label2);
        groupBox2.Controls.Add(lstDestTables);
        groupBox2.Controls.Add(lstDestFields);

        panel1.Controls.Add(label3);
        panel1.Controls.Add(cboMappings);
        panel1.Controls.Add(btnNewMapping);
        panel1.Controls.Add(btnAddMapping);
        panel1.Controls.Add(btnRemoveMapping);
        panel1.Controls.Add(btnEditTransformation);
        panel1.Controls.Add(btnGenerateCode);

        grpMappingPreview.Controls.Add(txtMappingDetails);

        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
        splitContainer2.Panel1.ResumeLayout(false);
        splitContainer2.Panel2.ResumeLayout(false);
        splitContainer2.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)gridMappings).EndInit();
        grpMappingPreview.ResumeLayout(false);
        grpMappingPreview.PerformLayout();
        ResumeLayout(false);
    }

    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private GroupBox groupBox1;
    private TextBox txtSourceFilter;
    private Label lblSourceFilter;
    private ListBox lstSourceFields;
    private ListBox lstSourceTables;
    private Label label1;
    private GroupBox groupBox2;
    private TextBox txtDestFilter;
    private Label lblDestFilter;
    private ListBox lstDestFields;
    private ListBox lstDestTables;
    private Label label2;
    private Panel panel1;
    private Button btnGenerateCode;
    private Button btnEditTransformation;
    private Button btnRemoveMapping;
    private Button btnAddMapping;
    private Button btnNewMapping;
    private ComboBox cboMappings;
    private Label label3;
    private DataGridView gridMappings;
    private ToolTip toolTip;
    private GroupBox grpMappingPreview;
    private TextBox txtMappingDetails;
} 