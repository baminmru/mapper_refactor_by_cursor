using mapper_refactor.Services;

namespace mapper_refactor;

public partial class TransformationEditForm : Form
{
    private readonly string _mapName;
    private readonly string _sourceTable;
    private readonly string _sourceField;
    private readonly string _destTable;
    private readonly string _destField;
    private readonly IDatabaseService _databaseService;

    public string TransformationExpression => txtTransformation.Text;

    public TransformationEditForm(
        string mapName,
        string sourceTable,
        string sourceField,
        string destTable,
        string destField,
        string currentTransformation,
        IDatabaseService databaseService)
    {
        InitializeComponent();

        _mapName = mapName;
        _sourceTable = sourceTable;
        _sourceField = sourceField;
        _destTable = destTable;
        _destField = destField;
        _databaseService = databaseService;

        txtTransformation.Text = currentTransformation;
        lblFieldInfo.Text = $"Transformation for {sourceTable}.{sourceField} -> {destTable}.{destField}";
    }

    private async void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            await _databaseService.ExecuteNonQueryAsync(
                $@"UPDATE map_data 
                   SET transformation = '{txtTransformation.Text}'
                   WHERE map_name = '{_mapName}'
                   AND source_table = '{_sourceTable}'
                   AND source_field = '{_sourceField}'
                   AND dest_table = '{_destTable}'
                   AND dest_field = '{_destField}'");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error saving transformation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        txtTransformation.Clear();
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
        // TODO: Implement transformation testing
        MessageBox.Show("Transformation testing not implemented yet.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "Transformation Examples:\n\n" +
            "1. Simple cast: CAST(field AS type)\n" +
            "2. String operations: TRIM(field), UPPER(field)\n" +
            "3. Date formatting: TO_CHAR(field, 'YYYY-MM-DD')\n" +
            "4. Conditional: CASE WHEN field = 'X' THEN 'Y' ELSE 'Z' END\n" +
            "5. Numeric operations: field * 100, ROUND(field, 2)\n\n" +
            "Leave empty for direct field mapping without transformation.",
            "Transformation Help",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
} 