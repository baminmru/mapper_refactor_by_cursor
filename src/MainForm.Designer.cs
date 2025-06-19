namespace mapper_refactor;

partial class MainForm
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
        this.components = new System.ComponentModel.Container();
        this.Text = "Mapper";
        this.Size = new System.Drawing.Size(800, 600);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
    }
} 