using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TC_CRM
{
    public partial class Conversion_Analytics : Form
    {
        private DataGridView dgvConversionData;
        private Button btnExport;
        private DataSet conversionDataset;

        // MySQL Connection String
        private string connectionString = "Server=localhost;Database=TC_CRM;Uid=root;Pwd=;";

        public Conversion_Analytics()
        {
            InitializeComponent();
            InitializeUI();
            LoadConversionData();
        }

        // Initialize UI components programmatically
        private void InitializeUI()
        {
            this.Text = "Conversion Analytics";
            this.Size = new System.Drawing.Size(800, 600);
            this.Paint += new PaintEventHandler(OnPaint);

            // Initialize DataGridView for demographic and purchase type sorting
            dgvConversionData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Initialize Export button
            btnExport = new Button
            {
                Text = "Export Report",
                Dock = DockStyle.Bottom,
                Height = 30
            };

            btnExport.Click += BtnExport_Click;

            // Add controls to the form
            this.Controls.Add(dgvConversionData);
            this.Controls.Add(btnExport);
        }

        // Load and process data for conversion rates
        private void LoadConversionData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Date, Demographic, PurchaseType, OneOffPurchases, Memberships FROM Conversions", conn);
                DataTable conversionsTable = new DataTable();
                adapter.Fill(conversionsTable);

                conversionDataset = new DataSet();
                conversionDataset.Tables.Add(conversionsTable);
                dgvConversionData.DataSource = conversionDataset.Tables[0];
            }
        }

        // Handle Paint event to draw the chart
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DataTable conversionsTable = conversionDataset.Tables[0];

            if (conversionsTable.Rows.Count == 0) return;

            var dates = conversionsTable.AsEnumerable().Select(r => r.Field<DateTime>("Date")).ToArray();
            var conversionRates = conversionsTable.AsEnumerable()
                                                  .Select(r => (double)r.Field<int>("Memberships") / r.Field<int>("OneOffPurchases") * 100)
                                                  .ToArray();

            // Define chart area
            Rectangle chartArea = new Rectangle(50, 50, 700, 200);

            // Draw chart axes
            g.DrawRectangle(Pens.Black, chartArea);

            // Draw conversion rates as line chart
            for (int i = 1; i < conversionRates.Length; i++)
            {
                g.DrawLine(Pens.Blue,
                    new PointF(chartArea.Left + (i - 1) * (chartArea.Width / (conversionRates.Length - 1)), chartArea.Bottom - (float)conversionRates[i - 1]),
                    new PointF(chartArea.Left + i * (chartArea.Width / (conversionRates.Length - 1)), chartArea.Bottom - (float)conversionRates[i]));
            }

            // Draw data points
            for (int i = 0; i < conversionRates.Length; i++)
            {
                g.FillEllipse(Brushes.Red,
                    chartArea.Left + i * (chartArea.Width / (conversionRates.Length - 1)) - 2,
                    chartArea.Bottom - (float)conversionRates[i] - 2,
                    4, 4);
            }
        }

        // Export report button click event
        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Implement export functionality here (e.g., export to CSV or Excel)
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = "ConversionAnalyticsReport.csv"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToCsv(saveFileDialog.FileName);
            }
        }

        // Export DataTable to CSV
        private void ExportToCsv(string filePath)
        {
            DataTable conversionsTable = conversionDataset.Tables[0];
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                // Write header
                for (int i = 0; i < conversionsTable.Columns.Count; i++)
                {
                    writer.Write(conversionsTable.Columns[i]);
                    if (i < conversionsTable.Columns.Count - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Write data
                foreach (DataRow row in conversionsTable.Rows)
                {
                    for (int i = 0; i < conversionsTable.Columns.Count; i++)
                    {
                        writer.Write(row[i]);
                        if (i < conversionsTable.Columns.Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }

            MessageBox.Show("Report exported successfully!", "Export Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
