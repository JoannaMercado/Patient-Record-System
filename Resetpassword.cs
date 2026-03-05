using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCrypt.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Patrient_Record
{
    public partial class Resetpassword : Form
    {

        private string connectionString = "server=localhost;userid=root;password=;database=prsdb;";
        private string _username;

        public Resetpassword()
        {
            InitializeComponent();
        }

        public static string HashPassword(string password)
        {
            // Returning the password directly without hashing.
            return password; // No hashing is applied.
        }


        public Resetpassword(string username)  // Add a new constructor that accepts a username
        {
            InitializeComponent();
            _username = username.Trim ();  // Store the username passed into the constructor
        }


        private bool UpdateUserPassword(string username, string newPassword)
        {
            string plainTextPassword = newPassword; // Directly using newPassword as it is.
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE users SET password = @NewPassword WHERE username = @Username", connection);
                command.Parameters.AddWithValue("@NewPassword", plainTextPassword); // Password is now plain text.
                command.Parameters.AddWithValue("@Username", username);
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword = newpass.Text.Trim();
            string confirmPassword = cnfrmpass.Text.Trim();

            if (newPassword == confirmPassword)
            {
                if (newPassword.Length >= 8) // Ensure password length is adequate
                {
                    if (UpdateUserPassword(_username, newPassword))  // Use the stored username
                    {
                        MessageBox.Show("Your password has been updated successfully.", "Password Reset Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogIn obj = new LogIn();
                        obj.Show();
                        this.Close();
                     }
                    else
                    {
                        MessageBox.Show("Failed to update password. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Password must be at least 8 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The passwords do not match. Please re-enter your password.", "Mismatch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            LogIn obj = new LogIn ();
            obj.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            newpass.UseSystemPasswordChar = !checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cnfrmpass.UseSystemPasswordChar = !checkBox2.Checked;
        }
    }
    }
