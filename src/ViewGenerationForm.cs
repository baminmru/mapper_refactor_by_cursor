using Microsoft.Extensions.Logging;
using mapper_refactor.Services;

namespace mapper_refactor;

public partial class ViewGenerationForm : Form
{
    private readonly ILogger<ViewGenerationForm> _logger;
    private readonly IViewGenerationService _viewGenerationService;

    public ViewGenerationForm(
        ILogger<ViewGenerationForm> logger,
        IViewGenerationService viewGenerationService)
    {
        _logger = logger;
        _viewGenerationService = viewGenerationService;
        InitializeComponent();
    }

    private async void btnGenerateViews_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtApi.Text))
        {
            MessageBox.Show("Please enter an API name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            UseWaitCursor = true;
            btnGenerateViews.Enabled = false;
            btnGenerateAll.Enabled = false;

            var (views, loaderScript) = await _viewGenerationService.GenerateViewsAsync(txtApi.Text);
            
            txtViews.Text = views;
            txtLoaderScript.Text = loaderScript;
            
            _logger.LogInformation("Successfully generated views for API {Api}", txtApi.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating views for API {Api}", txtApi.Text);
            MessageBox.Show($"Error generating views: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnGenerateViews.Enabled = true;
            btnGenerateAll.Enabled = true;
        }
    }

    private async void btnGenerateAll_Click(object sender, EventArgs e)
    {
        try
        {
            UseWaitCursor = true;
            btnGenerateViews.Enabled = false;
            btnGenerateAll.Enabled = false;

            var (views, loaderScript) = await _viewGenerationService.GenerateAllViewsAsync();
            
            txtViews.Text = views;
            txtLoaderScript.Text = loaderScript;
            
            _logger.LogInformation("Successfully generated all views");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating all views");
            MessageBox.Show($"Error generating views: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
            btnGenerateViews.Enabled = true;
            btnGenerateAll.Enabled = true;
        }
    }

    private void btnCopyViews_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtViews.Text))
        {
            Clipboard.SetText(txtViews.Text);
            _logger.LogInformation("Views SQL copied to clipboard");
            MessageBox.Show("Views SQL copied to clipboard", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnCopyLoader_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtLoaderScript.Text))
        {
            Clipboard.SetText(txtLoaderScript.Text);
            _logger.LogInformation("Loader script copied to clipboard");
            MessageBox.Show("Loader script copied to clipboard", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnSaveViews_Click(object sender, EventArgs e)
    {
        SaveToFile(txtViews.Text, "Views SQL", "SQL files (*.sql)|*.sql|All files (*.*)|*.*");
    }

    private void btnSaveLoader_Click(object sender, EventArgs e)
    {
        SaveToFile(txtLoaderScript.Text, "Loader Script", "SQL files (*.sql)|*.sql|All files (*.*)|*.*");
    }

    private void SaveToFile(string content, string fileType, string filter)
    {
        if (string.IsNullOrEmpty(content))
        {
            MessageBox.Show($"No {fileType} to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var saveDialog = new SaveFileDialog
        {
            Filter = filter,
            FilterIndex = 1,
            DefaultExt = "sql"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                File.WriteAllText(saveDialog.FileName, content);
                _logger.LogInformation("{FileType} saved to {FilePath}", fileType, saveDialog.FileName);
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving {FileType} to {FilePath}", fileType, saveDialog.FileName);
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 