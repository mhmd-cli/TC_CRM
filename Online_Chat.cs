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

            string username = cbUserAccounts.SelectedItem.ToString();
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
