using System.Data;
using mapper_refactor.Services;
using Microsoft.Extensions.Logging;

namespace mapper_refactor;

public partial class VisualMappingForm : Form
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<VisualMappingForm> _logger;
    private DataTable _sourceFields;
    private DataTable _destFields;

    public VisualMappingForm(
        IDatabaseService databaseService,
        ILogger<VisualMappingForm> logger)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _logger = logger;
        _sourceFields = new DataTable();
        _destFields = new DataTable();

        InitializeControls();
        LoadMappingNames();
    }

    private void InitializeControls()
    {
        // Initialize mapping names combo
        cmbMappingNames.DropDownStyle = ComboBoxStyle.DropDownList;
        
        // Initialize source fields list view
        lvSourceFields.View = View.Details;
        lvSourceFields.FullRowSelect = true;
        lvSourceFields.GridLines = true;
        lvSourceFields.Columns.Add("Field Name", 150);
        lvSourceFields.Columns.Add("Type", 100);
        lvSourceFields.Columns.Add("Comment", 200);

        // Initialize destination fields list view
        lvDestFields.View = View.Details;
        lvDestFields.FullRowSelect = true;
        lvDestFields.GridLines = true;
        lvDestFields.Columns.Add("Field Name", 150);
        lvDestFields.Columns.Add("Type", 100);
        lvDestFields.Columns.Add("Comment", 200);

        // Initialize mappings list view
        lvMappings.View = View.Details;
        lvMappings.FullRowSelect = true;
        lvMappings.GridLines = true;
        lvMappings.Columns.Add("Source Field", 150);
        lvMappings.Columns.Add("Destination Field", 150);
        lvMappings.Columns.Add("Transformation", 200);
    }

    private async void LoadMappingNames()
    {
        try
        {
            cmbMappingNames.Items.Clear();
            var mappings = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT map_name FROM map_data ORDER BY map_name");

            foreach (DataRow mapping in mappings.Rows)
            {
                var mapName = mapping["map_name"].ToString();
                if (mapName != null)
                {
                    cmbMappingNames.Items.Add(mapName);
                }
            }

            if (cmbMappingNames.Items.Count > 0)
                cmbMappingNames.SelectedIndex = 0;
            else
                EnableMappingControls(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading mapping names");
            MessageBox.Show("Error loading mapping names: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EnableMappingControls(bool enabled)
    {
        cmbSourceTables.Enabled = enabled;
        cmbDestTables.Enabled = enabled;
        lvSourceFields.Enabled = enabled;
        lvDestFields.Enabled = enabled;
        lvMappings.Enabled = enabled;
        btnAddMapping.Enabled = enabled;
        btnRemoveMapping.Enabled = enabled;
        btnEditTransformation.Enabled = enabled;
        btnSaveMappings.Enabled = enabled;
    }

    private async void cmbMappingNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbMappingNames.SelectedItem == null)
            return;

        try
        {
            var mapName = cmbMappingNames.SelectedItem.ToString();
            if (mapName == null)
                return;
            
            // Load tables for this mapping
            var tables = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT source_table, dest_table 
                   FROM map_data 
                   WHERE map_name = '{mapName}'
                   LIMIT 1");

            if (tables.Rows.Count > 0)
            {
                var sourceTable = tables.Rows[0]["source_table"].ToString();
                var destTable = tables.Rows[0]["dest_table"].ToString();

                if (sourceTable != null && destTable != null)
                {
                    await LoadSourceFields(sourceTable);
                    await LoadDestFields(destTable);
                    await LoadExistingMappings(mapName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading mapping {MapName}", cmbMappingNames.SelectedItem);
            MessageBox.Show("Error loading mapping: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadSourceFields(string tableName)
    {
        try
        {
            _sourceFields = await _databaseService.ExecuteQueryAsync(
                $@"SELECT field_name, field_type, comment, field_order 
                   FROM src_data 
                   WHERE table_name = '{tableName}' 
                   ORDER BY field_order");

            lvSourceFields.Items.Clear();
            foreach (DataRow field in _sourceFields.Rows)
            {
                var fieldName = field["field_name"].ToString() ?? "";
                var fieldType = field["field_type"].ToString() ?? "";
                var comment = field["comment"].ToString() ?? "";

                var listItem = new ListViewItem(fieldName);
                listItem.SubItems.Add(fieldType);
                listItem.SubItems.Add(comment);
                lvSourceFields.Items.Add(listItem);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading source fields");
            throw;
        }
    }

    private async Task LoadDestFields(string tableName)
    {
        try
        {
            _destFields = await _databaseService.ExecuteQueryAsync(
                $@"SELECT field_name, field_type, comment, field_order 
                   FROM dest_data 
                   WHERE table_name = '{tableName}' 
                   ORDER BY field_order");

            lvDestFields.Items.Clear();
            foreach (DataRow field in _destFields.Rows)
            {
                var fieldName = field["field_name"].ToString() ?? "";
                var fieldType = field["field_type"].ToString() ?? "";
                var comment = field["comment"].ToString() ?? "";

                var listItem = new ListViewItem(fieldName);
                listItem.SubItems.Add(fieldType);
                listItem.SubItems.Add(comment);
                lvDestFields.Items.Add(listItem);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading destination fields");
            throw;
        }
    }

    private async Task LoadExistingMappings(string mapName)
    {
        try
        {
            var mappings = await _databaseService.ExecuteQueryAsync(
                $@"SELECT source_field, dest_field, transformation
                   FROM map_data
                   WHERE map_name = '{mapName}'
                   ORDER BY source_field");

            lvMappings.Items.Clear();
            foreach (DataRow mapping in mappings.Rows)
            {
                var sourceField = mapping["source_field"].ToString() ?? "";
                var destField = mapping["dest_field"].ToString() ?? "";
                var transformation = mapping["transformation"]?.ToString() ?? "";

                var listItem = new ListViewItem(sourceField);
                listItem.SubItems.Add(destField);
                listItem.SubItems.Add(transformation);
                lvMappings.Items.Add(listItem);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading existing mappings");
            throw;
        }
    }

    private void btnAddMapping_Click(object sender, EventArgs e)
    {
        if (lvSourceFields.SelectedItems.Count == 0 || lvDestFields.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select both source and destination fields.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var sourceField = lvSourceFields.SelectedItems[0].Text;
        var destField = lvDestFields.SelectedItems[0].Text;

        var listItem = new ListViewItem(sourceField);
        listItem.SubItems.Add(destField);
        listItem.SubItems.Add("");
        lvMappings.Items.Add(listItem);
    }

    private void btnRemoveMapping_Click(object sender, EventArgs e)
    {
        if (lvMappings.SelectedItems.Count == 0)
        {
            MessageBox.Show("Please select a mapping to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        foreach (ListViewItem item in lvMappings.SelectedItems)
        {
            lvMappings.Items.Remove(item);
        }
    }

    private async void btnSaveMappings_Click(object sender, EventArgs e)
    {
        if (cmbMappingNames.SelectedItem == null)
        {
            MessageBox.Show("Please select a mapping first.", "No Mapping Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var mapName = cmbMappingNames.SelectedItem.ToString();
            if (mapName == null)
                return;

            // Delete existing mappings
            await _databaseService.ExecuteNonQueryAsync(
                $"DELETE FROM map_data WHERE map_name = '{mapName}'");

            // Insert new mappings
            foreach (ListViewItem item in lvMappings.Items)
            {
                var sourceField = item.Text;
                var destField = item.SubItems[1].Text;
                var transformation = item.SubItems[2].Text;

                await _databaseService.ExecuteNonQueryAsync(
                    $@"INSERT INTO map_data (map_name, source_field, dest_field, transformation)
                       VALUES ('{mapName}', '{sourceField}', '{destField}', '{transformation}')");
            }

            MessageBox.Show("Mappings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving mappings");
            MessageBox.Show("Error saving mappings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnNewMapping_Click(object sender, EventArgs e)
    {
        using var form = new NewMappingForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadMappingNames();
            cmbMappingNames.SelectedItem = form.MapName;
        }
    }

    private void btnEditTransformation_Click(object sender, EventArgs e)
    {
        if (lvMappings.SelectedItems.Count != 1)
        {
            MessageBox.Show("Please select a single mapping to edit.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedItem = lvMappings.SelectedItems[0];
        var mapName = cmbMappingNames.SelectedItem?.ToString();
        var sourceField = selectedItem.Text;
        var destField = selectedItem.SubItems[1].Text;
        var currentTransformation = selectedItem.SubItems[2].Text;

        if (mapName == null)
            return;

        using var form = new TransformationEditForm(
            mapName,
            sourceField,
            sourceField,
            destField,
            destField,
            currentTransformation,
            _databaseService);

        if (form.ShowDialog() == DialogResult.OK)
        {
            selectedItem.SubItems[2].Text = form.TransformationExpression;
        }
    }
} 