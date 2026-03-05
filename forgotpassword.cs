using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Patrient_Record
{
    public partial class forgotpassword : Form
    {
        private string connectionString = "server=localhost;userid=root;password=;database=prsdb;";
        private string smtpServer = "smtp.gmail.com"; // Example: smtp.gmail.com
        private int smtpPort = 587; // Example: 587 for Gmail
        private string smtpUsername = "staffclinic27@gmail.com"; // Replace with your email address
        private string smtpPassword = "fordasystemonleh"; // Replace with your email password


        public forgotpassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogIn obj = new LogIn();
            obj.Show();
            this.Hide();
        }

        public void ResetPassword(string username, string newPassword)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE users SET password = @NewPassword WHERE username = @Username";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword);  // Storing the password in plain text
                    command.Parameters.AddWithValue("@Username", username);
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Password reset successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to reset password.");
                    }
                }
            }
        }


        private bool IsValidLogin(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT password FROM users WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var storedPassword = command.ExecuteScalar()?.ToString();
                    if (storedPassword != null && storedPassword == password)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }


        private bool IsValidUser(string username, string email)
        {

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT COUNT(1) FROM users WHERE username = @Username AND email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result == 1;
                }
            }
        }
        private bool VerifyUserCode(string username, string code)
        {
            // Here we assume you are storing the code temporarily in the database
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT VerificationCode, Expiration FROM VerificationCodes WHERE UserID = (SELECT UserID FROM Users WHERE Username = @Username)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var storedCode = reader["VerificationCode"].ToString().Trim();
                            var expiration = Convert.ToDateTime(reader["Expiration"]);

                            if (DateTime.Now > expiration)
                            {
                                MessageBox.Show("Verification code has expired.", "Verification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (storedCode == code)
                            {
                                ClearVerificationCode(username); // Ensure this clears the code from the VerificationCodes table
                                return true;
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Verification code is incorrect or expired.", "Verification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SendVerificationCodeEmail(string email, string code)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("staffclinic27@gmail.com", "cqap urjw lwuy dihw\r\n")
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("staffclinic27@gmail.com"),
                    Subject = "Verification Code",
                    Body = $"Your verification code is: {code}. Please use this code to continue with your password reset process."
                };
                mailMessage.To.Add(email);

                client.Send(mailMessage);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("SMTP Error: " + smtpEx.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void StoreVerificationCode(string username, string code)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // Assuming that UserID is a field in the Users table, which is a foreign key here
                string insertQuery = @"
            INSERT INTO verificationcodes (UserID, VerificationCode, Expiration)
            VALUES ((SELECT id FROM Users WHERE Username = @Username), @Code, @Expiration)
            ON DUPLICATE KEY UPDATE VerificationCode = @Code, Expiration = @Expiration;";

                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@Expiration", DateTime.Now.AddMinutes(30)); // expires in 30 minutes
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ClearVerificationCode(string username)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("DELETE FROM verificationcodes WHERE UserID = (SELECT id FROM Users WHERE Username = @Username)", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = usernames.Text.Trim();
            string email = emailthiz.Text.Trim();

            if (IsValidUser(username, email))
            {
                string code = GenerateVerificationCode();
                if (SendVerificationCodeEmail(email, code))
                {
                    StoreVerificationCode(username, code); // Store the code right after sending the email
                    MessageBox.Show("Verification code sent! Please check your email.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to send verification code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid username or email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void vrfycode_Click(object sender, EventArgs e)
        {
            string userEnteredCode = code.Text.Trim();
            string username = usernames.Text.Trim();

            if (VerifyUserCode(username, userEnteredCode))
            {
                MessageBox.Show("Code verified successfully! You can now reset your password.", "Code Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Resetpassword resetForm = new Resetpassword(username);
                resetForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Verification code is incorrect. Please try again.", "Verification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }
    }
}