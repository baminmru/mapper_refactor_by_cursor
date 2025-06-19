using System.Text;
using System.Data;
using mapper_refactor.Services;
using Microsoft.Extensions.Logging;

namespace mapper_refactor;

public partial class MigrationMappingForm : Form
{
    private readonly IDatabaseService _databaseService;
    private readonly ICodeGenerationService _codeGenerationService;
    private readonly IViewGenerationService _viewGenerationService;
    private readonly ILogger<MigrationMappingForm> _logger;
    private string _currentMapName = string.Empty;
    private readonly List<string> _allSourceTables = new();
    private readonly List<string> _allDestTables = new();

    public MigrationMappingForm(
        IDatabaseService databaseService,
        ICodeGenerationService codeGenerationService,
        IViewGenerationService viewGenerationService,
        ILogger<MigrationMappingForm> logger)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _codeGenerationService = codeGenerationService;
        _viewGenerationService = viewGenerationService;
        _logger = logger;

        InitializeGridColumns();
        InitializeAsync();
    }

    private void InitializeGridColumns()
    {
        gridMappings.Columns.Add("SourceTable", "Source Table");
        gridMappings.Columns.Add("SourceField", "Source Field");
        gridMappings.Columns.Add("DestTable", "Destination Table");
        gridMappings.Columns.Add("DestField", "Destination Field");
        gridMappings.Columns.Add("Transform", "Transform");
    }

    private async void InitializeAsync()
    {
        try
        {
            await LoadSourceTables();
            await LoadDestTables();
            await LoadMappingNames();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing form");
            MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadMappingNames()
    {
        try
        {
            var mappings = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT map_name FROM map_data ORDER BY map_name");

            cboMappings.Items.Clear();
            foreach (DataRow row in mappings.Rows)
            {
                cboMappings.Items.Add(row["map_name"].ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading mapping names");
            MessageBox.Show("Error loading mapping names: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadSourceTables()
    {
        try
        {
            var tables = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT table_name FROM src_data ORDER BY table_name");

            _allSourceTables.Clear();
            _allSourceTables.AddRange(tables.Rows.Cast<DataRow>().Select(row => row["table_name"].ToString() ?? string.Empty));
            lstSourceTables.Items.Clear();
            lstSourceTables.Items.AddRange(_allSourceTables.ToArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading source tables");
            MessageBox.Show("Error loading source tables: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadDestTables()
    {
        try
        {
            var tables = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT table_name FROM dest_data ORDER BY table_name");

            _allDestTables.Clear();
            _allDestTables.AddRange(tables.Rows.Cast<DataRow>().Select(row => row["table_name"].ToString() ?? string.Empty));
            lstDestTables.Items.Clear();
            lstDestTables.Items.AddRange(_allDestTables.ToArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading destination tables");
            MessageBox.Show("Error loading destination tables: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadMapping(string mapName)
    {
        try
        {
            var mappings = await _databaseService.ExecuteQueryAsync(
                $@"SELECT source_table, source_field, dest_table, dest_field, transformation
                   FROM map_data 
                   WHERE map_name = '{mapName}'
                   ORDER BY source_table, source_field");

            gridMappings.Rows.Clear();
            foreach (DataRow row in mappings.Rows)
            {
                gridMappings.Rows.Add(
                    row["source_table"].ToString(),
                    row["source_field"].ToString(),
                    row["dest_table"].ToString(),
                    row["dest_field"].ToString(),
                    row["transformation"].ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading mapping {MapName}", mapName);
            MessageBox.Show($"Error loading mapping {mapName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task SaveMapping()
    {
        if (string.IsNullOrEmpty(_currentMapName))
        {
            return;
        }

        try
        {
            // Delete existing mapping
            await _databaseService.ExecuteNonQueryAsync(
                $"DELETE FROM map_data WHERE map_name = '{_currentMapName}'");

            // Insert new mapping
            foreach (DataGridViewRow row in gridMappings.Rows)
            {
                var sourceTable = row.Cells["SourceTable"].Value?.ToString();
                var sourceField = row.Cells["SourceField"].Value?.ToString();
                var destTable = row.Cells["DestTable"].Value?.ToString();
                var destField = row.Cells["DestField"].Value?.ToString();
                var transform = row.Cells["Transform"].Value?.ToString() ?? string.Empty;

                await _databaseService.ExecuteNonQueryAsync(
                    $@"INSERT INTO map_data (map_name, source_table, source_field, dest_table, dest_field, transformation)
                       VALUES ('{_currentMapName}', '{sourceTable}', '{sourceField}', '{destTable}', '{destField}', '{transform}')");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving mapping {MapName}", _currentMapName);
            MessageBox.Show($"Error saving mapping {_currentMapName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void lstSourceTables_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstSourceTables.SelectedItem == null) return;

        try
        {
            var sourceTable = lstSourceTables.SelectedItem.ToString();
            var fields = await _databaseService.ExecuteQueryAsync(
                $"SELECT field_name, field_type, comment FROM src_data WHERE table_name = '{sourceTable}' ORDER BY field_order");

            lstSourceFields.Items.Clear();
            foreach (DataRow row in fields.Rows)
            {
                var fieldName = row["field_name"].ToString();
                var fieldType = row["field_type"].ToString();
                var comment = row["comment"].ToString();
                lstSourceFields.Items.Add($"{fieldName} ({fieldType}) - {comment}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading source fields");
            MessageBox.Show("Error loading fields: " + ex.Message);
        }
    }

    private async void lstDestTables_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstDestTables.SelectedItem == null) return;

        try
        {
            var destTable = lstDestTables.SelectedItem.ToString();
            var fields = await _databaseService.ExecuteQueryAsync(
                $"SELECT field_name, field_type, comment FROM dest_data WHERE table_name = '{destTable}' ORDER BY field_order");

            lstDestFields.Items.Clear();
            foreach (DataRow row in fields.Rows)
            {
                var fieldName = row["field_name"].ToString();
                var fieldType = row["field_type"].ToString();
                var comment = row["comment"].ToString();
                lstDestFields.Items.Add($"{fieldName} ({fieldType}) - {comment}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading destination fields");
            MessageBox.Show("Error loading fields: " + ex.Message);
        }
    }

    private void cboMappings_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboMappings.SelectedItem == null)
        {
            return;
        }

        _currentMapName = cboMappings.SelectedItem.ToString();
        LoadMapping(_currentMapName);
        UpdateMappingPreview();
    }

    private void btnAddMapping_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentMapName))
        {
            MessageBox.Show("Please select or create a mapping first.", "No Mapping Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (lstSourceTables.SelectedItem == null || lstSourceFields.SelectedItem == null)
        {
            MessageBox.Show("Please select a source table and field.", "No Source Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (lstDestTables.SelectedItem == null || lstDestFields.SelectedItem == null)
        {
            MessageBox.Show("Please select a destination table and field.", "No Destination Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var sourceTable = lstSourceTables.SelectedItem.ToString();
        var sourceField = lstSourceFields.SelectedItem.ToString();
        var destTable = lstDestTables.SelectedItem.ToString();
        var destField = lstDestFields.SelectedItem.ToString();

        gridMappings.Rows.Add(sourceTable, sourceField, destTable, destField, string.Empty);
        UpdateMappingPreview();
    }

    private void btnRemoveMapping_Click(object sender, EventArgs e)
    {
        if (gridMappings.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a mapping to remove.", "No Mapping Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        foreach (DataGridViewRow row in gridMappings.SelectedRows)
        {
            gridMappings.Rows.Remove(row);
        }

        UpdateMappingPreview();
    }

    private async void btnNewMapping_Click(object sender, EventArgs e)
    {
        using var form = new NewMappingForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            _currentMapName = form.MapName;
            await LoadMappingNames();
            cboMappings.SelectedItem = _currentMapName;
        }
    }

    private async void btnGenerateCode_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentMapName))
        {
            MessageBox.Show("Please select a mapping first.");
            return;
        }

        try
        {
            var tableScript = await _codeGenerationService.GenerateTableScriptAsync(_currentMapName);
            var viewScript = await _viewGenerationService.GenerateViewsForMappingAsync(_currentMapName);
            var loaderScript = await _codeGenerationService.GenerateLoaderScriptAsync(_currentMapName);

            var allScripts = $@"-- Table Creation Scripts
{tableScript}

-- View Creation Scripts
{viewScript}

-- Loader Scripts
{loaderScript}";

            using var form = new CodePreviewForm(allScripts);
            form.ShowDialog();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating code");
            MessageBox.Show("Error generating code: " + ex.Message);
        }
    }

    private void btnEditTransformation_Click(object sender, EventArgs e)
    {
        if (gridMappings.SelectedRows.Count != 1)
        {
            MessageBox.Show("Please select a single mapping to edit.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var row = gridMappings.SelectedRows[0];
        var sourceField = row.Cells["SourceField"].Value?.ToString();
        var destField = row.Cells["DestField"].Value?.ToString();
        var currentTransform = row.Cells["Transform"].Value?.ToString();

        using var form = new TransformationForm(sourceField, destField, currentTransform);
        if (form.ShowDialog() == DialogResult.OK)
        {
            row.Cells["Transform"].Value = form.TransformationExpression;
            UpdateMappingPreview();
        }
    }

    private void txtSourceFilter_TextChanged(object sender, EventArgs e)
    {
        FilterListBox(lstSourceTables, _allSourceTables, txtSourceFilter.Text);
    }

    private void txtDestFilter_TextChanged(object sender, EventArgs e)
    {
        FilterListBox(lstDestTables, _allDestTables, txtDestFilter.Text);
    }

    private void FilterListBox(ListBox listBox, List<string> allItems, string filterText)
    {
        if (string.IsNullOrWhiteSpace(filterText))
        {
            listBox.Items.Clear();
            listBox.Items.AddRange(allItems.ToArray());
            return;
        }

        var filteredItems = allItems
            .Where(item => item.Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        listBox.Items.Clear();
        listBox.Items.AddRange(filteredItems);
    }

    private void UpdateMappingPreview()
    {
        if (cboMappings.SelectedItem == null)
        {
            txtMappingDetails.Text = string.Empty;
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Mapping: {cboMappings.SelectedItem}");
        sb.AppendLine();

        foreach (DataGridViewRow row in gridMappings.Rows)
        {
            var sourceTable = row.Cells["SourceTable"].Value?.ToString();
            var sourceField = row.Cells["SourceField"].Value?.ToString();
            var destTable = row.Cells["DestTable"].Value?.ToString();
            var destField = row.Cells["DestField"].Value?.ToString();
            var transform = row.Cells["Transform"].Value?.ToString();

            sb.AppendLine($"Source: {sourceTable}.{sourceField}");
            sb.AppendLine($"Destination: {destTable}.{destField}");
            if (!string.IsNullOrEmpty(transform))
            {
                sb.AppendLine($"Transform: {transform}");
            }
            sb.AppendLine();
        }

        txtMappingDetails.Text = sb.ToString();
    }
} 