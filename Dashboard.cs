using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TC_CRM
{
    public partial class Dashboard : Form
    {
        private DataGridView dataGridViewBenefits;
        private Button btnBack;
        private Button btnOpenDigitalContentModules;
        private Button btnConversionAnalytics;
        private Button btnBenefitsConfiguration;
        private Button btnOnlineChat;
        private Label lblWelcome;
        private Label lblUserName;
        private Label lblBenefitsStatus;

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=TC_CRM;Uid=root;Pwd=;";

        public Dashboard()
        {
            InitializeComponent();
            InitializeUI();
            this.Text = "Dashboard";
            PopulateBenefitsDataGridView();
        }

        // Initialize all UI components programmatically
        private void InitializeUI()
        {
            // Set form size to fit all buttons
            this.Size = new System.Drawing.Size(850, 500);

            // Initialize DataGridView
            dataGridViewBenefits = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Location = new System.Drawing.Point(12, 100),
                Name = "dataGridViewBenefits",
                ReadOnly = true,
                RowHeadersWidth = 51,
                RowTemplate = { Height = 24 },
                Size = new System.Drawing.Size(760, 300)
            };

            // Add a "Details" button column dynamically to show benefit details
            DataGridViewButtonColumn detailsButtonColumn = new DataGridViewButtonColumn
            {
                Name = "DetailsButton",
                Text = "View Details",
                UseColumnTextForButtonValue = true
            };

            // Add the "Details" button column to the DataGridView
            if (!dataGridViewBenefits.Columns.Contains("DetailsButton"))
            {
                dataGridViewBenefits.Columns.Add(detailsButtonColumn);
            }

            dataGridViewBenefits.CellContentClick += dataGridViewBenefits_CellContentClick;

            // Initialize Back button
            btnBack = new Button
            {
                Location = new System.Drawing.Point(12, 420),
                Name = "btnBack",
                Size = new System.Drawing.Size(75, 23),
                Text = "Back"
            };

            btnBack.Click += btnBack_Click;

            // Initialize Button to Open Digital Content Modules Form
            btnOpenDigitalContentModules = new Button
            {
                Location = new System.Drawing.Point(100, 420),
                Name = "btnOpenDigitalContentModules",
                Size = new System.Drawing.Size(150, 23),
                Text = "Digital Content Modules"
            };

            btnOpenDigitalContentModules.Click += btnOpenDigitalContentModules_Click;

            // Initialize Button to Open Conversion Analytics Form
            btnConversionAnalytics = new Button
            {
                Location = new System.Drawing.Point(260, 420),
                Name = "btnConversionAnalytics",
                Size = new System.Drawing.Size(150, 23),
                Text = "Conversion Analytics"
            };

            btnConversionAnalytics.Click += btnConversionAnalytics_Click;

            // Initialize Button to Open Benefits Configuration Form
            btnBenefitsConfiguration = new Button
            {
                Location = new System.Drawing.Point(420, 420),
                Name = "btnBenefitsConfiguration",
                Size = new System.Drawing.Size(150, 23),
                Text = "Benefits Configuration"
            };

            btnBenefitsConfiguration.Click += btnBenefitsConfiguration_Click;

            // Initialize Button to Open Online Chat Form
            btnOnlineChat = new Button
            {
                Location = new System.Drawing.Point(580, 420),
                Name = "btnOnlineChat",
                Size = new System.Drawing.Size(100, 23),
                Text = "Online Chat"
            };

            btnOnlineChat.Click += btnOnlineChat_Click;

            // Initialize Labels for Welcome Message and User Info
            lblWelcome = new Label
            {
                Location = new System.Drawing.Point(12, 20),
                Size = new System.Drawing.Size(300, 20),
                Text = "Welcome to your Dashboard"
            };

            lblUserName = new Label
            {
                Location = new System.Drawing.Point(12, 50),
                Size = new System.Drawing.Size(300, 20)
            };

            lblBenefitsStatus = new Label
            {
                Location = new System.Drawing.Point(12, 70),
                Size = new System.Drawing.Size(300, 20),
                Text = "Your Active Benefits: " + GetActiveBenefitsCount()
            };

            // Add controls to the form
            Controls.Add(dataGridViewBenefits);
            Controls.Add(btnBack);
            Controls.Add(btnOpenDigitalContentModules); // Add new button to the form
            Controls.Add(btnConversionAnalytics); // Add new button to the form
            Controls.Add(btnBenefitsConfiguration); // Add new button to the form
            Controls.Add(btnOnlineChat); // Add new button to the form
            Controls.Add(lblWelcome);
            Controls.Add(lblUserName);
            Controls.Add(lblBenefitsStatus);

            // Set the username label text from database
            SetUserName();
        }

        private void SetUserName()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT username FROM Users WHERE uid = 1", conn); // Assuming you want to fetch user with uid 1
                conn.Open();
                lblUserName.Text = "User: " + cmd.ExecuteScalar().ToString();
            }
        }

        // Populate the DataGridView with benefits data from the MySQL database
        private void PopulateBenefitsDataGridView()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Benefits", conn);
                DataTable benefitsTable = new DataTable();
                adapter.Fill(benefitsTable);
                dataGridViewBenefits.DataSource = benefitsTable;
            }
        }

        // Method to count active benefits
        private int GetActiveBenefitsCount()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Benefits WHERE Status = 'Active'", conn);
                conn.Open();
                int activeBenefitsCount = Convert.ToInt32(cmd.ExecuteScalar());
                return activeBenefitsCount+1;
            }
        }

        // Handle cell click event for the "Details" button
        private void dataGridViewBenefits_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewBenefits.Columns["DetailsButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBenefits.Rows[e.RowIndex];
                string benefitName = row.Cells["BenefitName"].Value.ToString();
                string description = row.Cells["Description"].Value.ToString();
                string status = row.Cells["Status"].Value.ToString();

                // Show a message box with the benefit details
                MessageBox.Show($"Benefit: {benefitName}\nDescription: {description}\nStatus: {status}", "Benefit Details");
            }
        }

        // Button click event to open Digital Content Modules form
        private void btnOpenDigitalContentModules_Click(object sender, EventArgs e)
        {
            Digital_Content_Modules digitalContentForm = new Digital_Content_Modules();
            digitalContentForm.Show();
        }

        // Button click event to open Conversion Analytics form
        private void btnConversionAnalytics_Click(object sender, EventArgs e)
        {
            Conversion_Analytics conversionAnalyticsForm = new Conversion_Analytics();
            conversionAnalyticsForm.Show();
        }

        // Button click event to open Benefits Configuration form
        private void btnBenefitsConfiguration_Click(object sender, EventArgs e)
        {
            BenefitsConfiguration benefitsConfigurationForm = new BenefitsConfiguration();
            benefitsConfigurationForm.Show();
        }

        // Button click event to open Online Chat form
        private void btnOnlineChat_Click(object sender, EventArgs e)
        {
            Online_Chat onlineChatForm = new Online_Chat(); onlineChatForm.Show();
        }
        // Button click event to navigate back or to another form (optional)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Close the current form (You can modify this for your navigation logic)
            this.Close();
        }
    }
}