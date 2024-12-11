﻿using System;
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

        // Initialize all UI components programmatically
        private void InitializeUI()
        {
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

            // Initialize Labels for Welcome Message and User Info
            lblWelcome = new Label
            {
                Location = new System.Drawing.Point(12, 20),
                Size = new System.Drawing.Size(200, 20),
                Text = "Welcome to your Dashboard"
            };

            lblUserName = new Label
            {
                Location = new System.Drawing.Point(12, 50),
                Size = new System.Drawing.Size(200, 20),
                Text = "User: Mohamed Abdelrahman"
            };

            lblBenefitsStatus = new Label
            {
                Location = new System.Drawing.Point(12, 70),
                Size = new System.Drawing.Size(300, 20),
                Text = "Your Active Benefits: " + (GetActiveBenefitsCount()+1)
            };

            // Add controls to the form
            Controls.Add(dataGridViewBenefits);
            Controls.Add(btnBack);
            Controls.Add(lblWelcome);
            Controls.Add(lblUserName);
            Controls.Add(lblBenefitsStatus);
        }

        // Populate the DataGridView with benefits data from the dataset
        private void PopulateBenefitsDataGridView()
        {
            DataTable benefitsTable = DashboardDataset.Tables["Benefits"];
            dataGridViewBenefits.DataSource = benefitsTable;
        }

        // Method to count active benefits
        private int GetActiveBenefitsCount()
        {
            DataTable benefitsTable = DashboardDataset.Tables["Benefits"];
            int activeBenefitsCount = benefitsTable.AsEnumerable()
                                                    .Count(row => row.Field<string>("Status") == "Active");
            return activeBenefitsCount;
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

        // Button click event to navigate back or to another form (optional)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Close the current form (You can modify this for your navigation logic)
            this.Close();
        }
    }
}