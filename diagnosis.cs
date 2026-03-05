using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Patrient_Record
{
    public partial class diagnosis : Form
    {
        public diagnosis()
        {
            InitializeComponent();
            Displaypatient();
        }
        private void Displaypatient()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PID, Name, Symptoms, Diagnostic_Test, Medicines FROM patientinfo";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void homebttn_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void xbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addbttn_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO patientinfo (PID, Name, Symptoms, Diagnostic_Test, Medicines) VALUES (@PID, @Name, @Symptoms, @Diagnostic_Test, @Medicines)";
                    var cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@PID", PId.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", Patienttxtbox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Symptoms", symptomstextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Diagnostic_Test", diagnostictextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Medicines", medstextBox.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Entered Successfully");
                        Displaypatient();  // Refresh the DataGridView to show the new data
                    }
                    else
                    {
                        MessageBox.Show("No record was inserted. Please check your data.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Failed to add: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void updatebttn_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE patientinfo SET PID = @PID, Patient_Name = @Patient_Name, Symptoms = @Symptoms, Diagnostioc_Test = @Diagnostioc_Test, Medicines = @Medicines WHERE PID = @PID";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PID", PId.Text.Trim());
                cmd.Parameters.AddWithValue("@Patient_Name", Patienttxtbox.Text.Trim()); // Assuming Age is an integer
                cmd.Parameters.AddWithValue("@Symptoms", symptomstextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Diagnostic_Test", diagnostictextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Medicines", medstextBox.Text.Trim());

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record Updated Successfully");
                    Displaypatient();  // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show("No record was updated. Please check your data.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT PID, Name, Symptoms, Diagnostic_Test, Medicines FROM patientinfo WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textsearch.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void deletebttn_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";
            try
            {
                if (string.IsNullOrWhiteSpace(PId.Text))
                {
                    MessageBox.Show("Enter the Patient ID");
                }
                else
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "DELETE FROM patientinfo WHERE PID = @PID";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@PID", PId.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record Deleted Successfully");
                            Displaypatient();  // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No record was deleted. Please check your data.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetbttn_Click(object sender, EventArgs e)
        {
            PId.Text = "";
            Patienttxtbox.Text = "";
            symptomstextBox.Text = "";
            diagnostictextBox.Text = "";
            medstextBox.Text = "";
        }
    }
}


