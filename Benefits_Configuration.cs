using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TC_CRM
{
    public partial class BenefitsConfiguration : Form
            public BenefitsConfiguration()
        {
            InitializeComponent();
            InitializeDataset();
            InitializeUI();
            LoadBenefitsData();
        }

        // Initialize the dataset with sample data
        private void InitializeDataset()
        {
            benefitsDataset = new DataSet();

            DataTable benefitsTable = new DataTable("Benefits");
            benefitsTable.Columns.Add("MembershipType", typeof(string));
            benefitsTable.Columns.Add("BenefitName", typeof(string));
            benefitsTable.Columns.Add("Description", typeof(string));

            // Add some sample data
            benefitsTable.Rows.Add("Gold", "Priority Support", "Access to priority customer support.");
            benefitsTable.Rows.Add("Gold", "Free Workshops", "Access to exclusive workshops.");
            benefitsTable.Rows.Add("Silver", "Discounted Membership", "Get a discount on membership renewal.");
            benefitsTable.Rows.Add("Silver", "Online Resources", "Access to premium online resources.");
            benefitsTable.Rows.Add("Bronze", "Newsletter", "Receive our monthly newsletter.");

            benefitsDataset.Tables.Add(benefitsTable);
        }

        // Initialize UI components programmatically
        private void InitializeUI()
        {
            this.Text = "Membership Benefits Configuration";
            this.Size = new System.Drawing.Size(800, 600);

            // Initialize ComboBox for membership types
            cbMembershipTypes = new ComboBox
            {
                Location = new System.Drawing.Point(20, 300),
                Size = new System.Drawing.Size(200, 24)
            };
            cbMembershipTypes.Items.AddRange(new string[] { "Gold", "Silver", "Bronze" });
            cbMembershipTypes.SelectedIndexChanged += CbMembershipTypes_SelectedIndexChanged;

            // Initialize DataGridView for benefits
            dgvBenefits = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Initialize Add, Remove, and Save buttons
            btnAddBenefit = new Button
            {
                Text = "Add Benefit",
                Location = new System.Drawing.Point(20, 350)
            };
            btnAddBenefit.Click += BtnAddBenefit_Click;

            btnRemoveBenefit = new Button
            {
                Text = "Remove Benefit",
                Location = new System.Drawing.Point(120, 350)
            };
            btnRemoveBenefit.Click += BtnRemoveBenefit_Click;

            btnSaveChanges = new Button
            {
                Text = "Save Changes",
                Location = new System.Drawing.Point(220, 350)
            };
            btnSaveChanges.Click += BtnSaveChanges_Click;

            // Add controls to the form
            this.Controls.Add(cbMembershipTypes);
            this.Controls.Add(dgvBenefits);
            this.Controls.Add(btnAddBenefit);
            this.Controls.Add(btnRemoveBenefit);
            this.Controls.Add(btnSaveChanges);
        }

        // Load and display benefits data based on selected membership type
        private void LoadBenefitsData()
        {
            string selectedMembershipType = cbMembershipTypes.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedMembershipType))
            {
                DataTable benefitsTable = benefitsDataset.Tables["Benefits"];
                var filteredBenefits = benefitsTable.AsEnumerable()
                    .Where(row => row.Field<string>("MembershipType") == selectedMembershipType)
                    .CopyToDataTable();

                dgvBenefits.DataSource = filteredBenefits;
            }
        }

        // Event handler for ComboBox selection change
        private void CbMembershipTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBenefitsData();
        }

        // Event handler for Add Benefit button click
        private void BtnAddBenefit_Click(object sender, EventArgs e)
        {
            if (cbMembershipTypes.SelectedItem == null)
            {
                MessageBox.Show("Please select a membership type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string membershipType = cbMembershipTypes.SelectedItem.ToString();
            string benefitName = Prompt.ShowDialog("Enter Benefit Name:", "Add Benefit");
            string description = Prompt.ShowDialog("Enter Benefit Description:", "Add Benefit");

            if (string.IsNullOrEmpty(benefitName) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Benefit name and description cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            benefitsDataset.Tables["Benefits"].Rows.Add(membershipType, benefitName, description);
            LoadBenefitsData();
        }

        // Event handler for Remove Benefit button click
        private void BtnRemoveBenefit_Click(object sender, EventArgs e)
        {
            if (dgvBenefits.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a benefit to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in dgvBenefits.SelectedRows)
            {
                dgvBenefits.Rows.Remove(row);
            }

            MessageBox.Show("Selected benefit(s) removed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Event handler for Save Changes button click
        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate and save changes to the dataset
                Validate();
                dgvBenefits.EndEdit();
                MessageBox.Show("Changes saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Utility class for input dialog
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
    }
}
