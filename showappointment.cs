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
    public partial class showappointment : Form
    {
        public showappointment()
        {
            InitializeComponent();
            Displayshow();
        }



        private void Displayshow()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PID, Name, AppointmentDateTime, VisitType, AppointmentStatus, Email, CreatedDate FROM patientinfo";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dataGridView1.AutoGenerateColumns = true; // Enable auto generation of columns
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
