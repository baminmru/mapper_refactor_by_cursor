using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mapper_refactor.Services;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace mapper_refactor;

public partial class LoginForm : Form
{
    private readonly IConfiguration _configuration;
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<LoginForm> _logger;
    private const string ConfigFile = "appsettings.json";

    public string? ConnectionString { get; private set; }

    public LoginForm(
        IConfiguration configuration,
        IDatabaseService databaseService,
        ILogger<LoginForm> logger)
    {
        InitializeComponent();
        _configuration = configuration;
        _databaseService = databaseService;
        _logger = logger;

        LoadConnectionSettings();
        LoadSavedConnections();
    }

    private JsonObject LoadJsonConfig()
    {
        var jsonString = File.ReadAllText(ConfigFile);
        var jsonNode = JsonNode.Parse(jsonString);
        return jsonNode as JsonObject ?? new JsonObject();
    }

    private void SaveJsonConfig(JsonObject rootObject)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(ConfigFile, rootObject.ToJsonString(options));
    }

    private JsonObject GetOrCreateConnectionStrings(JsonObject rootObject)
    {
        if (rootObject["ConnectionStrings"] is not JsonObject connectionStrings)
        {
            connectionStrings = new JsonObject();
            rootObject["ConnectionStrings"] = connectionStrings;
        }
        return connectionStrings;
    }

    private void LoadConnectionSettings()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                return;

            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            txtServer.Text = builder.Host;
            txtPort.Text = builder.Port.ToString();
            txtDatabase.Text = builder.Database;
            txtUsername.Text = builder.Username;
            txtPassword.Text = builder.Password;
            chkRememberPassword.Checked = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading connection settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadSavedConnections()
    {
        var connections = _configuration.GetSection("ConnectionStrings").GetChildren();
        foreach (var connection in connections)
        {
            cmbSavedConnections.Items.Add(connection.Key);
        }
    }

    private async void btnConnect_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs())
            return;

        try
        {
            btnConnect.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var connectionString = BuildConnectionString();
            await _databaseService.SetConnectionStringAsync(connectionString);

            // Test connection
            await _databaseService.TestConnectionAsync();

            // Save settings if requested
            if (chkRememberPassword.Checked)
            {
                SaveConnectionSettings(connectionString);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnConnect.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(txtServer.Text))
        {
            MessageBox.Show("Please enter a server address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtServer.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtPort.Text) || !int.TryParse(txtPort.Text, out _))
        {
            MessageBox.Show("Please enter a valid port number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPort.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtDatabase.Text))
        {
            MessageBox.Show("Please enter a database name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDatabase.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUsername.Focus();
            return false;
        }

        return true;
    }

    private string BuildConnectionString()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = txtServer.Text.Trim(),
            Port = int.Parse(txtPort.Text.Trim()),
            Database = txtDatabase.Text.Trim(),
            Username = txtUsername.Text.Trim(),
            Password = txtPassword.Text,
            Pooling = true,
            MinPoolSize = 1,
            MaxPoolSize = 20,
            CommandTimeout = 300
        };

        return builder.ToString();
    }

    private void SaveConnectionSettings(string connectionString)
    {
        try
        {
            var rootObject = LoadJsonConfig();
            var connectionStrings = GetOrCreateConnectionStrings(rootObject);
            connectionStrings["DefaultConnection"] = connectionString;
            SaveJsonConfig(rootObject);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving connection settings: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow only digits and control characters
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
        try
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = txtServer.Text,
                Port = int.Parse(txtPort.Text),
                Database = txtDatabase.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Text
            };

            using var conn = new NpgsqlConnection(builder.ConnectionString);
            conn.Open();
            MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtServer.Text) ||
            string.IsNullOrWhiteSpace(txtPort.Text) ||
            string.IsNullOrWhiteSpace(txtDatabase.Text) ||
            string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            MessageBox.Show("Please fill in all required fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = txtServer.Text,
            Port = int.Parse(txtPort.Text),
            Database = txtDatabase.Text,
            Username = txtUsername.Text,
            Password = txtPassword.Text
        };

        ConnectionString = builder.ConnectionString;

        if (chkSave.Checked)
        {
            SaveConnection();
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void cmbSavedConnections_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbSavedConnections.SelectedItem != null)
        {
            var connectionName = cmbSavedConnections.SelectedItem.ToString();
            var connectionString = _configuration.GetConnectionString(connectionName);

            if (!string.IsNullOrEmpty(connectionString))
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);
                txtServer.Text = builder.Host;
                txtPort.Text = builder.Port.ToString();
                txtDatabase.Text = builder.Database;
                txtUsername.Text = builder.Username;
                txtPassword.Text = builder.Password;
            }
        }
    }

    private void SaveConnection()
    {
        if (string.IsNullOrWhiteSpace(txtConnectionName.Text))
        {
            MessageBox.Show("Please enter a name for the connection", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var rootObject = LoadJsonConfig();
            var connectionStrings = GetOrCreateConnectionStrings(rootObject);
            connectionStrings[txtConnectionName.Text] = ConnectionString;
            SaveJsonConfig(rootObject);

            if (!cmbSavedConnections.Items.Contains(txtConnectionName.Text))
            {
                cmbSavedConnections.Items.Add(txtConnectionName.Text);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving connection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (cmbSavedConnections.SelectedItem == null)
        {
            MessageBox.Show("Please select a connection to delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var connectionName = cmbSavedConnections.SelectedItem.ToString();
            var rootObject = LoadJsonConfig();

            if (rootObject["ConnectionStrings"] is JsonObject connectionStrings)
            {
                connectionStrings.Remove(connectionName);
                SaveJsonConfig(rootObject);
                cmbSavedConnections.Items.Remove(connectionName);
                ClearFields();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting connection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearFields()
    {
        txtServer.Clear();
        txtPort.Clear();
        txtDatabase.Clear();
        txtUsername.Clear();
        txtPassword.Clear();
        txtConnectionName.Clear();
        cmbSavedConnections.SelectedIndex = -1;
    }
} 