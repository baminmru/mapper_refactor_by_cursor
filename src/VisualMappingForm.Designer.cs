namespace mapper_refactor;

partial class VisualMappingForm
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cmbMappingNames;
    private ComboBox cmbSourceTables;
    private ComboBox cmbDestTables;
    private Button btnNewMapping;
    private ListView lvSourceFields;
    private ListView lvDestFields;
    private ListView lvMappings;
    private Button btnAddMapping;
    private Button btnRemoveMapping;
    private Button btnEditTransformation;
    private Button btnSaveMappings;
    private Label lblSourceFields;
    private Label lblDestFields;
    private Label lblMappings;
    private Label lblMappingName;

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
        cmbMappingNames = new ComboBox();
        cmbSourceTables = new ComboBox();
        cmbDestTables = new ComboBox();
        lvSourceFields = new ListView();
        lvDestFields = new ListView();
        lvMappings = new ListView();
        btnAddMapping = new Button();
        btnRemoveMapping = new Button();
        btnEditTransformation = new Button();
        btnSaveMappings = new Button();
        btnNewMapping = new Button();
        lblSourceFields = new Label();
        lblDestFields = new Label();
        lblMappings = new Label();
        lblMappingName = new Label();
        SuspendLayout();
        // 
        // cmbMappingNames
        // 
        cmbMappingNames.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMappingNames.FormattingEnabled = true;
        cmbMappingNames.Location = new Point(12, 12);
        cmbMappingNames.Name = "cmbMappingNames";
        cmbMappingNames.Size = new Size(200, 23);
        cmbMappingNames.TabIndex = 0;
        cmbMappingNames.SelectedIndexChanged += cmbMappingNames_SelectedIndexChanged;
        // 
        // cmbSourceTables
        // 
        cmbSourceTables.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSourceTables.FormattingEnabled = true;
        cmbSourceTables.Location = new Point(12, 50);
        cmbSourceTables.Name = "cmbSourceTables";
        cmbSourceTables.Size = new Size(200, 23);
        cmbSourceTables.TabIndex = 1;
        // 
        // cmbDestTables
        // 
        cmbDestTables.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDestTables.FormattingEnabled = true;
        cmbDestTables.Location = new Point(600, 50);
        cmbDestTables.Name = "cmbDestTables";
        cmbDestTables.Size = new Size(200, 23);
        cmbDestTables.TabIndex = 2;
        // 
        // lvSourceFields
        // 
        lvSourceFields.Location = new Point(12, 90);
        lvSourceFields.Name = "lvSourceFields";
        lvSourceFields.Size = new Size(500, 300);
        lvSourceFields.TabIndex = 3;
        lvSourceFields.UseCompatibleStateImageBehavior = false;
        lvSourceFields.View = View.Details;
        // 
        // lvDestFields
        // 
        lvDestFields.Location = new Point(600, 90);
        lvDestFields.Name = "lvDestFields";
        lvDestFields.Size = new Size(500, 300);
        lvDestFields.TabIndex = 4;
        lvDestFields.UseCompatibleStateImageBehavior = false;
        lvDestFields.View = View.Details;
        // 
        // lvMappings
        // 
        lvMappings.Location = new Point(12, 450);
        lvMappings.Name = "lvMappings";
        lvMappings.Size = new Size(1088, 250);
        lvMappings.TabIndex = 5;
        lvMappings.UseCompatibleStateImageBehavior = false;
        lvMappings.View = View.Details;
        // 
        // btnAddMapping
        // 
        btnAddMapping.Location = new Point(12, 400);
        btnAddMapping.Name = "btnAddMapping";
        btnAddMapping.Size = new Size(75, 23);
        btnAddMapping.TabIndex = 6;
        btnAddMapping.Text = "Add Mapping";
        btnAddMapping.UseVisualStyleBackColor = true;
        btnAddMapping.Click += btnAddMapping_Click;
        // 
        // btnRemoveMapping
        // 
        btnRemoveMapping.Location = new Point(120, 400);
        btnRemoveMapping.Name = "btnRemoveMapping";
        btnRemoveMapping.Size = new Size(75, 23);
        btnRemoveMapping.TabIndex = 7;
        btnRemoveMapping.Text = "Remove Mapping";
        btnRemoveMapping.UseVisualStyleBackColor = true;
        btnRemoveMapping.Click += btnRemoveMapping_Click;
        // 
        // btnEditTransformation
        // 
        btnEditTransformation.Location = new Point(240, 400);
        btnEditTransformation.Name = "btnEditTransformation";
        btnEditTransformation.Size = new Size(138, 23);
        btnEditTransformation.TabIndex = 8;
        btnEditTransformation.Text = "Edit Transformation";
        btnEditTransformation.UseVisualStyleBackColor = true;
        btnEditTransformation.Click += btnEditTransformation_Click;
        // 
        // btnSaveMappings
        // 
        btnSaveMappings.Location = new Point(360, 400);
        btnSaveMappings.Name = "btnSaveMappings";
        btnSaveMappings.Size = new Size(75, 23);
        btnSaveMappings.TabIndex = 9;
        btnSaveMappings.Text = "Save Mappings";
        btnSaveMappings.UseVisualStyleBackColor = true;
        btnSaveMappings.Click += btnSaveMappings_Click;
        // 
        // btnNewMapping
        // 
        btnNewMapping.Location = new Point(220, 12);
        btnNewMapping.Name = "btnNewMapping";
        btnNewMapping.Size = new Size(75, 23);
        btnNewMapping.TabIndex = 10;
        btnNewMapping.Text = "New Mapping";
        btnNewMapping.UseVisualStyleBackColor = true;
        btnNewMapping.Click += btnNewMapping_Click;
        // 
        // lblSourceFields
        // 
        lblSourceFields.AutoSize = true;
        lblSourceFields.Location = new Point(12, 72);
        lblSourceFields.Name = "lblSourceFields";
        lblSourceFields.Size = new Size(79, 15);
        lblSourceFields.TabIndex = 11;
        lblSourceFields.Text = "Source Fields:";
        // 
        // lblDestFields
        // 
        lblDestFields.AutoSize = true;
        lblDestFields.Location = new Point(600, 72);
        lblDestFields.Name = "lblDestFields";
        lblDestFields.Size = new Size(102, 15);
        lblDestFields.TabIndex = 12;
        lblDestFields.Text = "Destination Fields:";
        // 
        // lblMappings
        // 
        lblMappings.AutoSize = true;
        lblMappings.Location = new Point(12, 432);
        lblMappings.Name = "lblMappings";
        lblMappings.Size = new Size(62, 15);
        lblMappings.TabIndex = 13;
        lblMappings.Text = "Mappings:";
        // 
        // lblMappingName
        // 
        lblMappingName.AutoSize = true;
        lblMappingName.Location = new Point(12, 10);
        lblMappingName.Name = "lblMappingName";
        lblMappingName.Size = new Size(89, 15);
        lblMappingName.TabIndex = 14;
        lblMappingName.Text = "Mapping Name:";
        // 
        // VisualMappingForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(lblMappingName);
        Controls.Add(lblMappings);
        Controls.Add(lblDestFields);
        Controls.Add(lblSourceFields);
        Controls.Add(btnSaveMappings);
        Controls.Add(btnEditTransformation);
        Controls.Add(btnRemoveMapping);
        Controls.Add(btnAddMapping);
        Controls.Add(lvMappings);
        Controls.Add(lvDestFields);
        Controls.Add(lvSourceFields);
        Controls.Add(btnNewMapping);
        Controls.Add(cmbMappingNames);
        Controls.Add(cmbSourceTables);
        Controls.Add(cmbDestTables);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "VisualMappingForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Visual Mapping";
        ResumeLayout(false);
        PerformLayout();
    }
} 