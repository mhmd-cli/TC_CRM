using System;
using System.Data;
using System.Windows.Forms;

namespace TC_CRM
{
    public partial class Online_Chat : Form
    {
        private ComboBox cbUserAccounts;
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
            chatTable.Rows.Add(DateTime.Now.AddMinutes(-3), "Mohamed", "Excited to be here!");
            chatTable.Rows.Add(DateTime.Now.AddMinutes(-1), "Jamil", "Hi Mohamed, welcome!");

            chatDataset.Tables.Add(chatTable);
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
            cbUserAccounts.Items.AddRange(new string[] { "User1", "User2", "User3", "Mohamed", "Jamil" });
            cbUserAccounts.SelectedIndex = 0; // Default to the first user account

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

        
    }
}
