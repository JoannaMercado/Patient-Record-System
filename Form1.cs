using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patrient_Record
{
    public partial class LogIn : Form
    {
        private string connectionString = "server=localhost;userid=root;password=;database=prsdb;";

        public LogIn()
        {
            InitializeComponent();
            passtxt.UseSystemPasswordChar = true; // Assuming "password" is the name of your TextBox contro
            this.KeyDown += LogIn_KeyDown;
        }

        private void xbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginbttn_Click(object sender, EventArgs e)
        {
            string username = adminrichText.Text.Trim();
            string password = passtxt.Text.Trim(); // This should be the user's plain text password input

            try
            {
                if (AuthenticateUser(username, password))
                {
                    MessageBox.Show("Login successful!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    infodiagnosishistory obj = new infodiagnosishistory();
                    obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LogIn_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Call the button click event handler when Enter key is pressed
                loginbttn.PerformClick();
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT password FROM users WHERE username = @username"; // Ensure column and table names are correct

                using (var command = new MySqlCommand(query, connection))
                {
                    // Use explicit typing for parameters to avoid issues with AddWithValue
                    command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

                    // Execute the scalar query to fetch the password in plain text
                    var passwordInDb = command.ExecuteScalar() as string;

                    // Check if a password was retrieved and compare it directly
                    return password == passwordInDb;
                }
            }
        }



        private void clrbttn_Click(object sender, EventArgs e)
        {
            adminrichText.Text = " ";
            passtxt.Text = "";
        }

        private void passwordrichText_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            forgotpassword  obj = new forgotpassword ();
            obj.Show();
            this.Hide();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void adminrichText_TextChanged(object sender, EventArgs e)
        {

        }

        private void xbttn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forgotpassword obj = new forgotpassword();
            obj.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            registrationform  obj = new registrationform ();
            obj.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passtxt.UseSystemPasswordChar = !checkBox1.Checked;

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            passtxt.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forgotpassword obj = new forgotpassword();
            obj.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            registrationform obj = new registrationform();
            obj.Show();
            this.Hide();
        }

        private void xbttn_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}