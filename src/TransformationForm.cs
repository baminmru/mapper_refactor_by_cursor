using System.Text;

namespace mapper_refactor;

public partial class TransformationForm : Form
{
    private readonly string _sourceField;
    private readonly string _destField;

    public string TransformationExpression { get; private set; }

    public TransformationForm(string? sourceField, string? destField, string? currentTransform)
    {
        InitializeComponent();
        _sourceField = sourceField ?? string.Empty;
        _destField = destField ?? string.Empty;
        TransformationExpression = currentTransform ?? string.Empty;

        InitializeForm();
    }

    private void InitializeForm()
    {
        Text = "Edit Transformation";
        Size = new Size(600, 400);
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;

        var lblSource = new Label
        {
            Text = "Source Field:",
            AutoSize = true,
            Location = new Point(12, 15)
        };

        var txtSource = new TextBox
        {
            Text = _sourceField,
            ReadOnly = true,
            Location = new Point(100, 12),
            Width = 200
        };

        var lblDest = new Label
        {
            Text = "Dest Field:",
            AutoSize = true,
            Location = new Point(12, 45)
        };

        var txtDest = new TextBox
        {
            Text = _destField,
            ReadOnly = true,
            Location = new Point(100, 42),
            Width = 200
        };

        var lblTransform = new Label
        {
            Text = "Transformation:",
            AutoSize = true,
            Location = new Point(12, 75)
        };

        var txtTransform = new TextBox
        {
            Text = TransformationExpression,
            Multiline = true,
            ScrollBars = ScrollBars.Both,
            Location = new Point(12, 95),
            Width = 560,
            Height = 200,
            Font = new Font("Consolas", 9.75F, FontStyle.Regular)
        };

        var btnOk = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new Point(416, 320),
            Width = 75
        };

        var btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new Point(497, 320),
            Width = 75
        };

        var toolTip = new ToolTip();
        toolTip.SetToolTip(txtTransform, "Enter the transformation expression. Use {value} to refer to the source field value.");

        btnOk.Click += (s, e) =>
        {
            TransformationExpression = txtTransform.Text;
            Close();
        };

        btnCancel.Click += (s, e) => Close();

        Controls.AddRange(new Control[]
        {
            lblSource,
            txtSource,
            lblDest,
            txtDest,
            lblTransform,
            txtTransform,
            btnOk,
            btnCancel
        });

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }
} 