using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // Sample DataSet
        public static DataSet DashboardDataset = new DataSet();

        public Dashboard()
        {
            InitializeComponent();
            InitializeDataset();
            InitializeUI();
            PopulateBenefitsDataGridView();
        }

        // Initialize the dataset and populate it with sample benefit data
        private void InitializeDataset()
        {
            DataTable benefitsTable = new DataTable("Benefits");
            benefitsTable.Columns.Add("BenefitName", typeof(string));
            benefitsTable.Columns.Add("Description", typeof(string));
            benefitsTable.Columns.Add("Status", typeof(string)); // Active/Inactive

            // Add some sample data
            benefitsTable.Rows.Add("Free Workshop", "Access to exclusive workshops.", "Active");
            benefitsTable.Rows.Add("Discounted Membership", "Get a discount on next year's membership.", "Inactive");
            benefitsTable.Rows.Add("Priority Support", "Get priority support for your queries.", "Active");

            DashboardDataset.Tables.Add(benefitsTable);
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
            Online_Chat onlineChatForm = new Online_Chat();
            onlineChatForm.Show();
        }

        // Button click event to navigate back or to another form (optional)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Close the current form (You can modify this for your navigation logic)
            this.Close();
        }
    }
}
