using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TC_CRM
{
    public partial class Online_Chat : Form
    {
        private ComboBox cbUserAccounts;
        private TextBox txtMessageInput;
        private Button btnSendMessage;
        private ListBox lstChatDisplay;
        private DataSet chatDataset;

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=TC_CRM;Uid=root;Pwd=;";

        public Online_Chat()
        {
            InitializeComponent();
            InitializeUI();
            LoadUserAccounts();
            LoadChatData();
        }

        // Initialize UI components programmatically
        private void InitializeUI()
        {
            this.Text = "Online Chat";
            this.Size = new System.Drawing.Size(600, 400);

            // Initialize ComboBox for user accounts
            cbUserAccounts = new ComboBox
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 24)
            };

            // Initialize ListBox for displaying chat messages
            lstChatDisplay = new ListBox
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(540, 250)
            };

            // Initialize TextBox for message input
            txtMessageInput = new TextBox
            {
                Location = new System.Drawing.Point(20, 320),
                Size = new System.Drawing.Size(440, 24)
            };

            // Initialize Send button
            btnSendMessage = new Button
            {
                Text = "Send",
                Location = new System.Drawing.Point(480, 320),
                Size = new System.Drawing.Size(80, 24)
            };
            btnSendMessage.Click += BtnSendMessage_Click;

            // Add controls to the form
            this.Controls.Add(cbUserAccounts);
            this.Controls.Add(lstChatDisplay);
            this.Controls.Add(txtMessageInput);
            this.Controls.Add(btnSendMessage);
        }

        // Load user accounts from the database into ComboBox
        private void LoadUserAccounts()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT username FROM Users", conn);
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbUserAccounts.Items.Add(reader.GetString("username"));
                    }
                }
                cbUserAccounts.SelectedIndex = 0; // Default to the first user account
            }
        }

        // Load and display chat messages from the database
        private void LoadChatData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Timestamp, Username, Message FROM ChatMessages", conn);
                DataTable chatTable = new DataTable();
                adapter.Fill(chatTable);

                foreach (DataRow row in chatTable.Rows)
                {
                    string timestamp = ((DateTime)row["Timestamp"]).ToString("g");
                    string username = row["Username"].ToString();
                    string message = row["Message"].ToString();

                    lstChatDisplay.Items.Add($"[{timestamp}] {username}: {message}");
                }
            }
        }

        // Event handler for Send button click
        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            string message = txtMessageInput.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Message cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = cbUserAccounts.SelectedItem.ToString();
            DateTime timestamp = DateTime.Now;

            // Add message to database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO ChatMessages (Timestamp, Username, Message) VALUES (@Timestamp, @Username, @Message)", conn);
                cmd.Parameters.AddWithValue("@Timestamp", timestamp);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Message", message);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Display message in ListBox
            lstChatDisplay.Items.Add($"[{timestamp:g}] {username}: {message}");

            // Clear input
            txtMessageInput.Clear();
        }
    }
}
