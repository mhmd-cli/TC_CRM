using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TC_CRM
{
    public partial class Conversion_Analytics : Form
    {
        private DataGridView dgvConversionData;
        private Button btnExport;
        private DataSet conversionDataset;

        

        // Initialize sample dataset for conversion analytics
        private void InitializeDataset()
        {
            conversionDataset = new DataSet();

            DataTable conversionsTable = new DataTable("Conversions");
            conversionsTable.Columns.Add("Date", typeof(DateTime));
            conversionsTable.Columns.Add("Demographic", typeof(string));
            conversionsTable.Columns.Add("PurchaseType", typeof(string));
            conversionsTable.Columns.Add("OneOffPurchases", typeof(int));
            conversionsTable.Columns.Add("Memberships", typeof(int));

            // Add some sample data
            conversionsTable.Rows.Add(new DateTime(2023, 1, 1), "Young Adults", "Event", 50, 5);
            conversionsTable.Rows.Add(new DateTime(2023, 2, 1), "Young Adults", "Event", 60, 8);
            conversionsTable.Rows.Add(new DateTime(2023, 3, 1), "Young Adults", "Subscription", 70, 15);
            conversionsTable.Rows.Add(new DateTime(2023, 1, 1), "Middle Aged", "Event", 80, 12);
            conversionsTable.Rows.Add(new DateTime(2023, 2, 1), "Middle Aged", "Subscription", 90, 20);
            conversionsTable.Rows.Add(new DateTime(2023, 3, 1), "Middle Aged", "Event", 100, 25);

            conversionDataset.Tables.Add(conversionsTable);
        }

        // Initialize all UI components programmatically
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
            DataTable conversionsTable = conversionDataset.Tables["Conversions"];

            // Bind DataTable to DataGridView
            dgvConversionData.DataSource = conversionsTable;
        }

        // Handle Paint event to draw the chart
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DataTable conversionsTable = conversionDataset.Tables["Conversions"];

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
            DataTable conversionsTable = conversionDataset.Tables["Conversions"];
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
