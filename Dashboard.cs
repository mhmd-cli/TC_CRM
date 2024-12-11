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
        // Login Data, ideally would be sourced from login info
        List<string> LoginData = new List<string>();
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboard();
            // Welcome message + dashboard
            this.Text = "Welcome to the application, this is your Dashboard";
        }
        private void LoadDashboard()
        {
            // Create a DataGridView to display the benefits
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false
            };

            // Define columns
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Benefit Name", DataPropertyName = "Benefit_Name" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Benefit Description", DataPropertyName = "BenefitDescription" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Benefit Status", DataPropertyName = "BenefitStatus" });

            DataGridViewButtonColumn viewDetailsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewDetails", // Set the name of the column
                HeaderText = "View Details",
                Text = "View",
                UseColumnTextForButtonValue = true
            };
            dataGridView.Columns.Add(viewDetailsColumn);

            // Bind the data
            dataGridView.DataSource = GetData();

            // Add the DataGridView to the form
            this.Controls.Add(dataGridView);

            // Handle button clicks
            dataGridView.CellContentClick += (s, e) =>
            {
                if (e.ColumnIndex == dataGridView.Columns["ViewDetails"].Index && e.RowIndex >= 0) // Use the name here
                {
                    // Handle the view details action here
                    var selectedBenefit = (Benefit)dataGridView.Rows[e.RowIndex].DataBoundItem;
                    MessageBox.Show($"Viewing details for yourself");
                }
            };
        }
        private List<Benefit> GetData()
        {
            if (LoginData.Count >= 1) {
                // Actual Logged in details
                List<Benefit> benefits = LoginData.Select(data => new Benefit { /* Initialization here */ }).ToList();
                 return benefits;
            }
            else
            {
                // Use dummy data if no login found
                return new List<Benefit>
                {
                    new Benefit { Benefit_Name  = "Membership Rewards", BenefitDescription = "points on purchases", BenefitStatus = "Active" },
                    new Benefit { Benefit_Name  = "Online Resources", BenefitDescription = "Access to exclusive articles", BenefitStatus = "Inactive" },
                    new Benefit { Benefit_Name  = "Paid Time Off", BenefitDescription = "15 days of PTO per year", BenefitStatus = "Active" }
                };
            }
        }
    }
    public class Benefit
    {
        public string Benefit_Name { get; set; }
        public string BenefitDescription { get; set; }
        public string BenefitStatus { get; set; }
    }
}
