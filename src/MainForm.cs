using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using mapper_refactor.Services;

namespace mapper_refactor;

public partial class MainForm : Form
{
    private readonly ILogger<MainForm> _logger;
    private readonly IDatabaseService _databaseService;

    public MainForm(ILogger<MainForm> logger, IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            // TODO: Load connection string from configuration
            var connectionString = "Server=localhost;Port=5432;Database=mapper;User Id=postgres;Password=;";
            await _databaseService.SetConnectionStringAsync(connectionString);
            
            try
            {
                await _databaseService.TestConnectionAsync();
                _logger.LogInformation("Application initialized successfully");
            }
            catch
            {
                MessageBox.Show("Failed to connect to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing application");
            MessageBox.Show("Error initializing application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        // No need to explicitly disconnect since Npgsql handles connection pooling
    }
} 