namespace mapper_refactor;

partial class LoginForm
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
        lblServer = new Label();
        txtServer = new TextBox();
        lblPort = new Label();
        txtPort = new TextBox();
        lblDatabase = new Label();
        txtDatabase = new TextBox();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnConnect = new Button();
        btnCancel = new Button();
        chkRememberPassword = new CheckBox();
        cmbSavedConnections = new ComboBox();
        lblSavedConnections = new Label();
        txtConnectionName = new TextBox();
        lblConnectionName = new Label();
        chkSave = new CheckBox();
        btnTest = new Button();
        btnDelete = new Button();
        SuspendLayout();
        // 
        // lblSavedConnections
        // 
        lblSavedConnections.AutoSize = true;
        lblSavedConnections.Location = new Point(12, 15);
        lblSavedConnections.Name = "lblSavedConnections";
        lblSavedConnections.Size = new Size(71, 15);
        lblSavedConnections.TabIndex = 0;
        lblSavedConnections.Text = "Saved Connections:";
        // 
        // cmbSavedConnections
        // 
        cmbSavedConnections.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSavedConnections.FormattingEnabled = true;
        cmbSavedConnections.Location = new Point(89, 12);
        cmbSavedConnections.Name = "cmbSavedConnections";
        cmbSavedConnections.Size = new Size(202, 23);
        cmbSavedConnections.TabIndex = 1;
        cmbSavedConnections.SelectedIndexChanged += cmbSavedConnections_SelectedIndexChanged;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(297, 12);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(75, 23);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // lblServer
        // 
        lblServer.AutoSize = true;
        lblServer.Location = new Point(12, 44);
        lblServer.Name = "lblServer";
        lblServer.Size = new Size(41, 15);
        lblServer.TabIndex = 3;
        lblServer.Text = "Server:";
        // 
        // txtServer
        // 
        txtServer.Location = new Point(89, 41);
        txtServer.Name = "txtServer";
        txtServer.Size = new Size(283, 23);
        txtServer.TabIndex = 4;
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Location = new Point(12, 73);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(32, 15);
        lblPort.TabIndex = 5;
        lblPort.Text = "Port:";
        // 
        // txtPort
        // 
        txtPort.Location = new Point(89, 70);
        txtPort.Name = "txtPort";
        txtPort.Size = new Size(100, 23);
        txtPort.TabIndex = 6;
        txtPort.Text = "5432";
        txtPort.KeyPress += txtPort_KeyPress;
        // 
        // lblDatabase
        // 
        lblDatabase.AutoSize = true;
        lblDatabase.Location = new Point(12, 102);
        lblDatabase.Name = "lblDatabase";
        lblDatabase.Size = new Size(58, 15);
        lblDatabase.TabIndex = 7;
        lblDatabase.Text = "Database:";
        // 
        // txtDatabase
        // 
        txtDatabase.Location = new Point(89, 99);
        txtDatabase.Name = "txtDatabase";
        txtDatabase.Size = new Size(283, 23);
        txtDatabase.TabIndex = 8;
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(12, 131);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(63, 15);
        lblUsername.TabIndex = 9;
        lblUsername.Text = "Username:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(89, 128);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(283, 23);
        txtUsername.TabIndex = 10;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(12, 160);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(60, 15);
        lblPassword.TabIndex = 11;
        lblPassword.Text = "Password:";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(89, 157);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(283, 23);
        txtPassword.TabIndex = 12;
        // 
        // lblConnectionName
        // 
        lblConnectionName.AutoSize = true;
        lblConnectionName.Location = new Point(12, 189);
        lblConnectionName.Name = "lblConnectionName";
        lblConnectionName.Size = new Size(71, 15);
        lblConnectionName.TabIndex = 13;
        lblConnectionName.Text = "Save As:";
        // 
        // txtConnectionName
        // 
        txtConnectionName.Location = new Point(89, 186);
        txtConnectionName.Name = "txtConnectionName";
        txtConnectionName.Size = new Size(202, 23);
        txtConnectionName.TabIndex = 14;
        // 
        // chkSave
        // 
        chkSave.AutoSize = true;
        chkSave.Location = new Point(89, 215);
        chkSave.Name = "chkSave";
        chkSave.Size = new Size(132, 19);
        chkSave.TabIndex = 15;
        chkSave.Text = "Save Connection";
        chkSave.UseVisualStyleBackColor = true;
        // 
        // btnTest
        // 
        btnTest.Location = new Point(135, 244);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(75, 23);
        btnTest.TabIndex = 16;
        btnTest.Text = "Test";
        btnTest.UseVisualStyleBackColor = true;
        btnTest.Click += btnTest_Click;
        // 
        // btnConnect
        // 
        btnConnect.Location = new Point(216, 244);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(75, 23);
        btnConnect.TabIndex = 17;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        btnConnect.Click += btnConnect_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(297, 244);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 18;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // chkRememberPassword
        // 
        chkRememberPassword.AutoSize = true;
        chkRememberPassword.Location = new Point(240, 215);
        chkRememberPassword.Name = "chkRememberPassword";
        chkRememberPassword.Size = new Size(132, 19);
        chkRememberPassword.TabIndex = 19;
        chkRememberPassword.Text = "Remember Password";
        chkRememberPassword.UseVisualStyleBackColor = true;
        // 
        // LoginForm
        // 
        AcceptButton = btnConnect;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(384, 279);
        Controls.Add(chkRememberPassword);
        Controls.Add(btnCancel);
        Controls.Add(btnConnect);
        Controls.Add(btnTest);
        Controls.Add(chkSave);
        Controls.Add(txtConnectionName);
        Controls.Add(lblConnectionName);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(txtUsername);
        Controls.Add(lblUsername);
        Controls.Add(txtDatabase);
        Controls.Add(lblDatabase);
        Controls.Add(txtPort);
        Controls.Add(lblPort);
        Controls.Add(txtServer);
        Controls.Add(lblServer);
        Controls.Add(btnDelete);
        Controls.Add(cmbSavedConnections);
        Controls.Add(lblSavedConnections);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Database Connection";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblServer;
    private TextBox txtServer;
    private Label lblPort;
    private TextBox txtPort;
    private Label lblDatabase;
    private TextBox txtDatabase;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnConnect;
    private Button btnCancel;
    private CheckBox chkRememberPassword;
    private ComboBox cmbSavedConnections;
    private Label lblSavedConnections;
    private TextBox txtConnectionName;
    private Label lblConnectionName;
    private CheckBox chkSave;
    private Button btnTest;
    private Button btnDelete;
} 