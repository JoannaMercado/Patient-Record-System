using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Patrient_Record
{
    public partial class patient : Form
    {
        private readonly string connectionString = "server=localhost;port=3306;database=prsdb;uid=root;password=;";
        public patient()
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
                    string query = "SELECT PID, Name, Age, Address, Status, BirthDay, ContactNumber, Company, Position, LMP FROM patientinfo";

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

        private void xbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void homebttn_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }
        private void addbttn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Missing Information");
                return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out int age))
            {
                MessageBox.Show("Invalid age format");
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO patientinfo (Name, Age, Address, Status, BirthDay, ContactNumber, Company, Position, LMP) VALUES (@Name, @Age, @Address, @Status, @BirthDay, @ContactNumber, @Company, @Position, @LMP)";
                    var cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Address", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", comboBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@BirthDay", textBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactNumber", textBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@Company", textBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@Position", textBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@LMP", textBox9.Text.Trim());

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

        private void patient_Load(object sender, EventArgs e)
        {
            Displaypatient();
        }


        private void resetbttn_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
            textBox2.Text = " ";
            textBox3.Text = " ";
            textBox4.Text = " ";
            comboBox1 .Text = " ";
            textBox5.Text = " ";
            textBox6.Text = " ";
            textBox7.Text = " ";
            textBox8.Text = " ";
            textBox9.Text = " ";
        }

        private void deletebttn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
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
                        cmd.Parameters.AddWithValue("@PID", textBox1.Text);
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

        private void updatebttn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "UPDATE patientinfo SET Name = @Name, Age = @Age, Address = @Address, Status = @Status, BirthDay = @BirthDay, ContactNumber = @ContactNumber, PCompany = @Company, Position = @Position, LMP = @LMP WHERE PID = @PID";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@PID", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", int.Parse(textBox3.Text.Trim())); // Assuming Age is an integer
                        cmd.Parameters.AddWithValue("@Address", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", textBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", textBox6.Text.Trim());
                        cmd.Parameters.AddWithValue("@Company", textBox7.Text.Trim());
                        cmd.Parameters.AddWithValue("@Position", textBox8.Text.Trim());
                        cmd.Parameters.AddWithValue("@LMP", textBox9.Text.Trim());

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                textBox6.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                textBox7.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                textBox8.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                textBox9.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT PID, Name, Address, Status, Birthday, ContactNumber, Company, Position, LMP FROM patientinfo WHERE Name LIKE @search";

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

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "^[0-9]*$"))
            {
                // If it does, remove the non-numeric characters
                textBox3.Text = System.Text.RegularExpressions.Regex.Replace(textBox3.Text, "[^0-9]", "");
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "^[0-9]*$"))
            {
                // If it does, remove the non-numeric characters
                textBox6.Text = System.Text.RegularExpressions.Regex.Replace(textBox6.Text, "[^0-9]", "");
            }
        }
    }
}

