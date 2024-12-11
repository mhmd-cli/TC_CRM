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
    public partial class Digital_Content_Modules : Form
    {
        private DataGridView dataGridViewModules;
        private Button btnBack;
        private Button btnBook;
        private Label lblModulesTitle;
        private Label lblModulesDescription;
        private Label lblStatus;

        // Sample DataSet for digital content modules
        public static DataSet ModulesDataset = new DataSet();

        public Digital_Content_Modules()
        {
            InitializeComponent();
            InitializeDataset();
            InitializeUI();
            PopulateModulesDataGridView();
        }

        // Initialize the dataset and populate it with sample digital content module data
        private void InitializeDataset()
        {
            DataTable modulesTable = new DataTable("Modules");
            modulesTable.Columns.Add("ModuleName", typeof(string));
            modulesTable.Columns.Add("Description", typeof(string));
            modulesTable.Columns.Add("Available", typeof(bool)); // True if module can be booked

            // Add some sample data
            modulesTable.Rows.Add("Intro to Culture", "An introductory course to cultural awareness.", true);
            modulesTable.Rows.Add("Advanced Leadership", "Learn advanced leadership skills for team management.", true);
            modulesTable.Rows.Add("Community Building", "Focuses on building stronger communities.", false); // Example of a module that can't be booked

            ModulesDataset.Tables.Add(modulesTable);
        }


        // Populate the DataGridView with modules data from the dataset
        private void PopulateModulesDataGridView()
        {
            DataTable modulesTable = ModulesDataset.Tables["Modules"];
            dataGridViewModules.DataSource = modulesTable;
        }

        // Handle cell click event for the "Book" button
        private void dataGridViewModules_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewModules.Columns["BookButton"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewModules.Rows[e.RowIndex];
                string moduleName = row.Cells["ModuleName"].Value.ToString();
                bool isAvailable = Convert.ToBoolean(row.Cells["Available"].Value);

                if (isAvailable)
                {
                    // Confirm booking and change the module's status
                    MessageBox.Show($"Module '{moduleName}' has been successfully booked!", "Booking Confirmation");
                    row.Cells["Available"].Value = false; // Mark as booked
                    lblStatus.Text = "Status: Module has been booked";
                }
                else
                {
                    MessageBox.Show("This module is not available for booking.", "Booking Error");
                }
            }
        }

        // Button click event to book the selected module
        private void btnBook_Click(object sender, EventArgs e)
        {
            // This button click is primarily redundant due to in-cell booking.
            // However, you can add further actions here if needed for your requirements.
            MessageBox.Show("Select a module from the list to book", "Booking Information");
        }

        // Button click event to navigate back or to another form
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Close the current form (You can modify this for your navigation logic)
            this.Close();
        }
    }
}
