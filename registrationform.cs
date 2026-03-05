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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Patrient_Record
{

    public partial class registrationform : Form
    {

        public registrationform()
        {
            InitializeComponent();
            password.UseSystemPasswordChar = true; // Assuming "password" is the name of your TextBox control

        }

        private void registrationform_Load(object sender, EventArgs e)
        {

        }
        private void RegisterUser(string username, string password, string email)
        {
            string connectionString = "server=localhost;userid=root;password=;database=prsdb;";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("INSERT INTO users (username, password, email) VALUES (@username, @password, @email)", connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password); // Consider hashing the password
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during registration: " + ex.Message);
                }
            }
        }

        private void registerbttn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text) ||
         string.IsNullOrWhiteSpace(password.Text) ||
         string.IsNullOrWhiteSpace(email.Text))
            {
                // Display an error message
                MessageBox.Show("Please fill in all the required fields (Username, Password, and Email) before registering.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                RegisterUser(username.Text, password.Text, email.Text);
                MessageBox.Show("Successfully Registered!! ");
                LogIn obj = new LogIn();
                obj.Show();
                this.Hide();
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogIn obj = new LogIn();
            obj.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
