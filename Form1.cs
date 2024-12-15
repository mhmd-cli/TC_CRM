using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace User_Story
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
          
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;
                string passwordHash = HashingHelper.HashPassword(password);

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("All fields are required.");
                    return;
                }

                DatabaseHelper db = new DatabaseHelper();
                try
                {
                    if (db.RegisterUser(username, email, passwordHash))
                    {
                        MessageBox.Show("Signup successful!");
                    Form2 loginPage = new Form2();
                    this.Hide();
                    loginPage.ShowDialog();
;
                    }
                    else
                    {
                        MessageBox.Show("Signup failed.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            

        }
    }
}
