namespace mapper_refactor;

public partial class CodePreviewForm : Form
{
    public CodePreviewForm(string code)
    {
        InitializeComponent();
        txtCode.Text = code;
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCode.Text))
        {
            Clipboard.SetText(txtCode.Text);
            MessageBox.Show("Code copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtCode.Text))
            return;

        using var dialog = new SaveFileDialog
        {
            Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
            FilterIndex = 1,
            DefaultExt = "sql"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                File.WriteAllText(dialog.FileName, txtCode.Text);
                MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
} 