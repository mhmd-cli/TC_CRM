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


        // Initialize all UI components programmatically
        private void InitializeUI()
        {
            // Initialize DataGridView
            dataGridViewModules = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Location = new System.Drawing.Point(12, 100),
                Name = "dataGridViewModules",
                ReadOnly = true,
                RowHeadersWidth = 51,
                RowTemplate = { Height = 24 },
                Size = new System.Drawing.Size(760, 300)
            };

            // Add "Book" button column to DataGridView
            DataGridViewButtonColumn bookButtonColumn = new DataGridViewButtonColumn
            {
                Name = "BookButton",
                Text = "Book",
                UseColumnTextForButtonValue = true
            };

            // Add the "Book" button column to the DataGridView
            if (!dataGridViewModules.Columns.Contains("BookButton"))
            {
                dataGridViewModules.Columns.Add(bookButtonColumn);
            }

            dataGridViewModules.CellContentClick += dataGridViewModules_CellContentClick;

            // Initialize Book button
            btnBook = new Button
            {
                Location = new System.Drawing.Point(12, 420),
                Name = "btnBook",
                Size = new System.Drawing.Size(75, 23),
                Text = "Book Module"
            };

            btnBook.Click += btnBook_Click;

            // Initialize Back button
            btnBack = new Button
            {
                Location = new System.Drawing.Point(100, 420),
                Name = "btnBack",
                Size = new System.Drawing.Size(75, 23),
                Text = "Back"
            };

            btnBack.Click += btnBack_Click;

            // Initialize Labels for Modules Information
            lblModulesTitle = new Label
            {
                Location = new System.Drawing.Point(12, 20),
                Size = new System.Drawing.Size(200, 20),
                Text = "Available Digital Content Modules"
            };

            lblModulesDescription = new Label
            {
                Location = new System.Drawing.Point(12, 50),
                Size = new System.Drawing.Size(200, 20),
                Text = "Select a module to book"
            };

            lblStatus = new Label
            {
                Location = new System.Drawing.Point(12, 70),
                Size = new System.Drawing.Size(200, 20),
                Text = "Status: All modules are available"
            };

            // Add controls to the form
            Controls.Add(dataGridViewModules);
            Controls.Add(btnBook);
            Controls.Add(btnBack);
            Controls.Add(lblModulesTitle);
            Controls.Add(lblModulesDescription);
            Controls.Add(lblStatus);
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
