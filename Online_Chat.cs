using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TC_CRM
{
    public partial class Online_Chat : Form
    {
        private TextBox txtMessageInput;
        private Button btnSendMessage;
        private ListBox lstChatDisplay;
        private DataSet chatDataset;

        public Online_Chat()
        {
            InitializeComponent();
            InitializeDataset();
            InitializeUI();
            LoadChatData();
        }

        // Initialize the dataset with sample data
        private void InitializeDataset()
        {
            chatDataset = new DataSet();

            DataTable chatTable = new DataTable("ChatMessages");
            chatTable.Columns.Add("Timestamp", typeof(DateTime));
            chatTable.Columns.Add("Username", typeof(string));
            chatTable.Columns.Add("Message", typeof(string));

            // Add some sample data
            chatTable.Rows.Add(DateTime.Now.AddMinutes(-10), "User1", "Hello, everyone!");
            chatTable.Rows.Add(DateTime.Now.AddMinutes(-5), "User2", "Hi, User1! How's it going?");
            chatTable.Rows.Add(DateTime.Now, "User1", "I'm doing well, thanks! How about you?");

            chatDataset.Tables.Add(chatTable);
        }

        // Initialize UI components programmatically
        private void InitializeUI()
        {
            this.Text = "Online Chat";
            this.Size = new System.Drawing.Size(600, 400);

            // Initialize ListBox for displaying chat messages
            lstChatDisplay = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 300
            };

            // Initialize TextBox for message input
            txtMessageInput = new TextBox
            {
                Dock = DockStyle.Bottom,
                Width = 500
            };

            // Initialize Send button
            btnSendMessage = new Button
            {
                Text = "Send",
                Dock = DockStyle.Bottom
            };
            btnSendMessage.Click += BtnSendMessage_Click;

            // Add controls to the form
            this.Controls.Add(lstChatDisplay);
            this.Controls.Add(txtMessageInput);
            this.Controls.Add(btnSendMessage);
        }

        // Load and display chat messages
        private void LoadChatData()
        {
            DataTable chatTable = chatDataset.Tables["ChatMessages"];

            foreach (DataRow row in chatTable.Rows)
            {
                string timestamp = ((DateTime)row["Timestamp"]).ToString("g");
                string username = row["Username"].ToString();
                string message = row["Message"].ToString();

                lstChatDisplay.Items.Add($"[{timestamp}] {username}: {message}");
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

            string username = "CurrentUser"; // Replace with actual username
            DateTime timestamp = DateTime.Now;

            // Add message to dataset
            chatDataset.Tables["ChatMessages"].Rows.Add(timestamp, username, message);

            // Display message in ListBox
            lstChatDisplay.Items.Add($"[{timestamp:g}] {username}: {message}");

            // Clear input
            txtMessageInput.Clear();
        }
    }
}
