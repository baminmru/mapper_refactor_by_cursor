using Microsoft.Extensions.Logging;
using mapper_refactor.Services;

namespace mapper_refactor;

public partial class CodeGenerationForm : Form
{
    private readonly ILogger<CodeGenerationForm> _logger;
    private readonly ICodeGenerationService _codeGenerationService;

    public CodeGenerationForm(
        ILogger<CodeGenerationForm> logger,
        ICodeGenerationService codeGenerationService)
    {
        _logger = logger;
        _codeGenerationService = codeGenerationService;
        InitializeComponent();
    }

    private async void btnGenerateSchema_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtApi.Text))
        {
            MessageBox.Show("Please enter an API name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            UseWaitCursor = true;
            btnGenerateSchema.Enabled = false;
            btnGenerateAll.Enabled = false;

            var schema = await _codeGenerationService.GenerateSchemaAsync(txtApi.Text, txtSchema.Text);
            txtResult.Text = schema;
            _logger.LogInformation("Successfully generated schema for API {Api}", txtApi.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating schema for API {Api}", txtApi.Text);
            MessageBox.Show($"Error generating schema: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnGenerateSchema.Enabled = true;
            btnGenerateAll.Enabled = true;
        }
    }

    private async void btnGenerateAll_Click(object sender, EventArgs e)
    {
        try
        {
            UseWaitCursor = true;
            btnGenerateSchema.Enabled = false;
            btnGenerateAll.Enabled = false;

            var schemas = await _codeGenerationService.GenerateAllSchemasAsync();
            txtResult.Text = schemas;
            _logger.LogInformation("Successfully generated all schemas");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating all schemas");
            MessageBox.Show($"Error generating schemas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnGenerateSchema.Enabled = true;
            btnGenerateAll.Enabled = true;
        }
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtResult.Text))
        {
            Clipboard.SetText(txtResult.Text);
            _logger.LogInformation("Generated code copied to clipboard");
            MessageBox.Show("Code copied to clipboard", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtResult.Text))
        {
            MessageBox.Show("No code to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var saveDialog = new SaveFileDialog
        {
            Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
            FilterIndex = 1,
            DefaultExt = "sql"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                File.WriteAllText(saveDialog.FileName, txtResult.Text);
                _logger.LogInformation("Generated code saved to {FilePath}", saveDialog.FileName);
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file to {FilePath}", saveDialog.FileName);
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 