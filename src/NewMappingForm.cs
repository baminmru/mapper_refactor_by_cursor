namespace mapper_refactor;

public partial class NewMappingForm : Form
{
    public string MapName { get; private set; } = string.Empty;

    public NewMappingForm()
    {
        InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMapName.Text))
        {
            MessageBox.Show("Please enter a mapping name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MapName = txtMapName.Text.Trim();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
} 