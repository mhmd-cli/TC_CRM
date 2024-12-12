using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TC_CRM
{
    public partial class BenefitsConfiguration : Form
    {
        private DataGridView dgvBenefits;
        private ComboBox cbMembershipTypes;
        private Button btnAddBenefit;
        private Button btnRemoveBenefit;
        private Button btnSaveChanges;

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=TC_CRM;Uid=root;Pwd=;";

        public BenefitsConfiguration()
        {
            InitializeComponent();
            InitializeUI();
            LoadMembershipTypes();
            LoadBenefitsData();
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

        // Load membership types from the database into ComboBox
        private void LoadMembershipTypes()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT MembershipType FROM MembershipBenefits", conn);
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbMembershipTypes.Items.Add(reader.GetString("MembershipType"));
                    }
                }
                cbMembershipTypes.SelectedIndex = 0; // Default to the first membership type
            }
        }

        // Load and display benefits data based on selected membership type
        private void LoadBenefitsData()
        {
            string selectedMembershipType = cbMembershipTypes.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedMembershipType))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(
                        "SELECT BenefitName, Description FROM MembershipBenefits WHERE MembershipType = @MembershipType", conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@MembershipType", selectedMembershipType);

                    DataTable benefitsTable = new DataTable();
                    adapter.Fill(benefitsTable);
                    dgvBenefits.DataSource = benefitsTable;
                }
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

            // Current issue with adding or changing a benefit: it can't add or change, except if the value already exists in the database.
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO MembershipBenefits (MembershipType, BenefitName, Description) VALUES (@MembershipType, @BenefitName, @Description)", conn);
                cmd.Parameters.AddWithValue("@MembershipType", membershipType);
                cmd.Parameters.AddWithValue("@BenefitName", benefitName);
                cmd.Parameters.AddWithValue("@Description", description);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

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
                string benefitName = row.Cells["BenefitName"].Value.ToString();
                string membershipType = cbMembershipTypes.SelectedItem.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM MembershipBenefits WHERE MembershipType = @MembershipType AND BenefitName = @BenefitName", conn);
                    cmd.Parameters.AddWithValue("@MembershipType", membershipType);
                    cmd.Parameters.AddWithValue("@BenefitName", benefitName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

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
