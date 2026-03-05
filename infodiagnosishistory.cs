using Vonage;
using Vonage.Request;
using Vonage.Messaging;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;
using System.Diagnostics;
using Patient_Record; // Make sure to include this namespace
using System.IO;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace Patrient_Record
{
    public partial class infodiagnosishistory : Form
    {
        string connectionString = "Server=localhost;Port=3306;Database=prsdb;Uid=root;";
        private List<CheckBox> conditionCheckBoxes;
        private List<string> medicationsList;
        private string smtpServer = "smtp.gmail.com"; // Example: smtp.gmail.com
        private int smtpPort = 587; // Example: 587 for Gmail
        private string smtpUsername = "staffclinic27@gmail.com"; // Replace with your email address
        private string smtpPassword = "fordasystemonleh"; // Replace with your email password
        private PrintDocument printDocument = new PrintDocument();
        private RichTextBox richTextBox1 = new RichTextBox();
        private PictureBox pictureBox2 = new PictureBox();

        private Dictionary<string, List<string>> symptomMedicineMap = new Dictionary<string, List<string>>
    {
        { "Headache", new List<string> { "Paracetamol", "Ibuprofen", "Aspirin" } },
        { "Stomachache", new List<string> { "Antacids", "Simethicone", "Peppto-Bismol"} },
        { "Cough", new List<string> { "CoughSyruo", "CoughDrops", "Lozenges" } }
    };


        public infodiagnosishistory()
        {
            InitializeComponent();
            Displaypatient();
            Displaydiagnosis();
            Displayhistory();
            Displayhealthcare();
            Displayappointments();
            InitializeConditionCheckBoxes();
            InitializeFamilyMedicalHistoryCheckBoxes();
            SetupDateTimePicker();
            pictureBox2.Image = Image.FromFile("C:\\Users\\tala\\Downloads\\medical logo.jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Size = new Size(100, 100);
            this.Controls.Add(pictureBox2);

            // Configure the RichTextBox
            richTextBox1.Size = new Size(400, 300);
            this.Controls.Add(richTextBox1);

            // Configure PrintDocument
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            comboBox11.SelectedIndexChanged += comboBox11_SelectedIndexChanged;
            dateTimePickerBirthdate.ValueChanged += DateTimePickerBirthdate_ValueChanged;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView3.CellDoubleClick += dataGridView3_CellDoubleClick;
            dataGridView4.CellDoubleClick += dataGridView4_CellDoubleClick;
            dataGridView5.CellDoubleClick += dataGridView5_CellDoubleClick;

            comboBoxSymptoms.SelectedIndexChanged += comboBoxSymptoms_SelectedIndexChanged;
            comboBoxMedicine.SelectedIndexChanged += comboBoxMedicine_SelectedIndexChanged;

        }

        private void infodiagnosishistory_Load(object sender, EventArgs e)
        {
            // Load your data into the DataGridView here
            LoadDataIntoDataGridView();
            // Call the highlighting function
            HighlightNextDayAppointments();
        }


        private void LoadDataIntoDataGridView()
        {
            // Connection string to your MySQL database
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT PID, Name, AppointmentDateTime, VisitType, AppointmentStatus, CreatedDate, Email FROM patientinfo";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView5.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }



        private void Displaypatient()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PID, Name, Age, Address, Status, Gender, BirthDay, ContactNumber, Company, Position, LMP, Email, BP, PR, WT, TEMP, SPO2, Record_Date FROM patientinfo";

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

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupSortOptions();
            LoadData1();
            SetupSortOptions1();
            LoadData2();
            SetupSortOptions2();
        }


        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure that the selected item is not null
            if (comboBox11.SelectedItem != null)
            {
                // Check the selected gender
                if (comboBox11.SelectedItem.ToString() == "Female")
                {
                    // Show the LMP TextBox if "Female" is selected
                    textBox9.Visible = true;
                    label9.Visible = true;
                }
                else
                {
                    // Hide the LMP TextBox if "Male" or any other option is selected
                    textBox9.Visible = false;
                    label9.Visible = false;
                }
            }
            else
            {
                // Optionally, hide the LMP TextBox when there is no selection
                textBox9.Visible = false;
                label9.Visible = false;
            }
        }



        private void LoadData()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Columns.Add("Address", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("BirthDay", typeof(DateTime));
            dataTable.Columns.Add("ContactNumber", typeof(string));
            dataTable.Columns.Add("Company", typeof(string));
            dataTable.Columns.Add("Position", typeof(string));
            dataTable.Columns.Add("LMP", typeof(string));
            dataTable.Columns.Add("Record_Date", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));


            dataGridView5.DataSource = dataTable;
        }


        private void SetupSortOptions()
        {
            comboBoxSortOptions.Items.Add("Alphabetically");
            comboBoxSortOptions.Items.Add("By Date");
            comboBoxSortOptions.SelectedIndex = 0;
        }

        private void LoadData1()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Columns.Add("Address", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Gender", typeof(string));
            dataTable.Columns.Add("BirthDay", typeof(DateTime));
            dataTable.Columns.Add("ContactNumber", typeof(string));
            dataTable.Columns.Add("Company", typeof(string));
            dataTable.Columns.Add("Position", typeof(string));
            dataTable.Columns.Add("LMP", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));


            dataGridView1.DataSource = dataTable;
        }


        private void SetupSortOptions1()
        {
            comboBoxSortOptions.Items.Add("Alphabetically");
            comboBoxSortOptions.Items.Add("By Date");
            comboBoxSortOptions.SelectedIndex = 0;
        }


        private void LoadData2()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("PID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));
            dataTable.Columns.Add("Address", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("BirthDay", typeof(DateTime));
            dataTable.Columns.Add("ContactNumber", typeof(string));
            dataTable.Columns.Add("Company", typeof(string));
            dataTable.Columns.Add("Position", typeof(string));
            dataTable.Columns.Add("LMP", typeof(string));
            dataTable.Columns.Add("Record_Date", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));



            dataGridView2.DataSource = dataTable;
        }


        private void SetupSortOptions2()
        {
            comboBoxSortOptions.Items.Add("Alphabetically");
            comboBoxSortOptions.Items.Add("By Date");
            comboBoxSortOptions.SelectedIndex = 0;
        }


        private void Displaydiagnosis()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT  Name, Age, DiagnosisName, Symptoms, WhenItStarted, Days, DurationType, Severity, Diagnostic_Test, Medicines, Type, NotesForSymptoms  FROM patientinfo";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void Displayhistory()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Name, Age, Gender, MedicalConditions, FamilyMedicalHistory, Medications, Dosage, Allergies, Reaction, SmokingStatus, SmokingFrequency, SmokingQuantity, AlcoholStatus, AlcoholFrequency, AlcoholQuantity FROM patientinfo";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView3.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void Displayhealthcare()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM healthcare";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void Displayappointments()
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT PID, Name, AppointmentDateTime, VisitType, AppointmentStatus, ContactNumber, Email, CreatedDate FROM patientinfo";

                    // Correctly instantiate the MySqlCommand object
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

                    MySqlDataAdapter sda = new MySqlDataAdapter(mySqlCommand);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    dataGridView5.AutoGenerateColumns = true; // Enable auto generation of columns
                    dataGridView5.DataSource = dt;
                    dataGridView1.MultiSelect = true;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void InitializeConditionCheckBoxes()
        {
            conditionCheckBoxes = new List<CheckBox>();

            // Medical conditions
            string[] medicalConditions = { "Diabetes", "Hypertension", "Asthma", "Arthritis", "Cancer", "Depression", "Anxiety", "Obesity", "Epilepsy" };
            // Create checkboxes for medical conditions
            int yPos = 10;
            foreach (string condition in medicalConditions)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = condition;
                checkBox.AutoSize = true;
                checkBox.Location = new System.Drawing.Point(10, yPos);
                yPos += 25;

                conditionCheckBoxes.Add(checkBox);
                Controls.Add(checkBox);
            }
        }

        private void InitializeFamilyMedicalHistoryCheckBoxes()
        {
            string[] familyMedicalHistory = { "Heart Disease", "High Blood Pressure", "Stroke", "Diabetes", "Cancer", "Obesity", "Arthritis", "Alzheimer's Disease", "Asthma" };

            List<CheckBox> familyMedicalHistoryCheckBoxes = new List<CheckBox>();

            int yPos = 10;
            foreach (string condition in familyMedicalHistory)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = condition;
                checkBox.AutoSize = true;
                checkBox.Location = new System.Drawing.Point(10, yPos);
                yPos += 25;

                familyMedicalHistoryCheckBoxes.Add(checkBox);
                Controls.Add(checkBox);
            }
        }



        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private int lastPatientID = 0;

        private string GeneratePatientID()
        {
            // Initialize ID to 1 (or the starting point you prefer)
            int newID = 1;

            try
            {
                // Query the database to find the highest existing ID
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to get the maximum ID currently in the database
                    string query = "SELECT MAX(PID) FROM patientinfo";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            newID = Convert.ToInt32(result) + 1;  // Increment the highest ID
                        }
                    }

                    // Check if the newID is within the desired range
                    if (newID > 9999999)
                    {
                        throw new InvalidOperationException("Exceeded maximum number of patient IDs.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                MessageBox.Show("Error generating Patient ID: " + ex.Message);
                throw;  // Optionally rethrow the exception
            }

            // Return the new ID as a string
            return newID.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Your logic to generate the Patient ID
            string generatedPatientID = GeneratePatientID();

            // Display the generated ID in the label and make it visible
            labelPID.Text = generatedPatientID;
            labelPID.Visible = true;

            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBoxAge.Text))
            {
                MessageBox.Show("Missing Information");
                return;
            }

            if (!int.TryParse(textBoxAge.Text.Trim(), out int age))
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
                    string query = @"INSERT INTO patientinfo 
                            (PID, Name, Age, Address, Status, Gender, BirthDay, ContactNumber, Company, Position, LMP, BP, PR, WT, TEMP, SPO2, Record_Date) 
                            VALUES (@PID, @Name, @Age, @Address, @Status, @Gender, @Birthday, @ContactNumber, @Company, @Position, @LMP, @BP, @PR, @WT, @TEMP, @SPO2, @Record_Date)";

                    var cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@PID", labelPID.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Address", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", comboBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", comboBox11.SelectedItem.ToString().Trim());
                    cmd.Parameters.AddWithValue("@Birthday", dateTimePickerBirthdate.Value);
                    cmd.Parameters.AddWithValue("@ContactNumber", textBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@Company", textBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@Position", textBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@LMP", textBox9.Text.Trim());
                    cmd.Parameters.AddWithValue("@BP", textBoxBP.Text.Trim());
                    cmd.Parameters.AddWithValue("@PR", textBoxPR.Text.Trim());
                    cmd.Parameters.AddWithValue("@WT", textBoxWT.Text.Trim());
                    cmd.Parameters.AddWithValue("@TEMP", textBoxTEMP.Text.Trim());
                    cmd.Parameters.AddWithValue("@SPO2", textBoxSPO2.Text.Trim());

                    // Add the record date
                    cmd.Parameters.AddWithValue("@Record_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Entered Successfully");

                        // Automatically show patient information in Diagnosis section
                        textBox12.Text = textBox2.Text.Trim(); // Assuming patient name is entered in textBox2

                        // Refresh the DataGridView to show the new data
                        Displaypatient();
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


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a record is selected
                if (string.IsNullOrWhiteSpace(labelPID.Text))
                {
                    MessageBox.Show("Select a record first.");
                    return;
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBoxAge.Text))
                {
                    MessageBox.Show("Missing Information");
                    return;
                }

                // Validate Age input
                if (!int.TryParse(textBoxAge.Text.Trim(), out int age))
                {
                    MessageBox.Show("Invalid age format");
                    return;
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"UPDATE patientinfo 
                             SET Name = @Name, Age = @Age, Address = @Address, Status = @Status, 
                                 Gender = @Gender, BirthDay = @BirthDay, ContactNumber = @ContactNumber, 
                                 Company = @Company, Position = @Position, LMP = @LMP, 
                                 Email = @Email, BP = @BP, PR = @PR, WT = @WT, TEMP = @TEMP, 
                                 SPO2 = @SPO2 
                             WHERE PID = @PID";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Address", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", comboBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", comboBox11.SelectedItem.ToString().Trim());
                    cmd.Parameters.AddWithValue("@BirthDay", dateTimePickerBirthdate.Value);
                    cmd.Parameters.AddWithValue("@ContactNumber", textBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@Company", textBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@Position", textBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@LMP", textBox9.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", textBox11.Text.Trim());
                    cmd.Parameters.AddWithValue("@BP", textBoxBP.Text.Trim());
                    cmd.Parameters.AddWithValue("@PR", textBoxPR.Text.Trim());
                    cmd.Parameters.AddWithValue("@WT", textBoxWT.Text.Trim());
                    cmd.Parameters.AddWithValue("@TEMP", textBoxTEMP.Text.Trim());
                    cmd.Parameters.AddWithValue("@SPO2", textBoxSPO2.Text.Trim());
                    cmd.Parameters.AddWithValue("@PID", labelPID.Text.Trim());

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                // Ensure a record is selected
                if (string.IsNullOrWhiteSpace(labelPID.Text))
                {
                    MessageBox.Show("Select a record first.");
                    return;
                }

                // Display confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "DELETE FROM patientinfo WHERE PID = @PID";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@PID", labelPID.Text.Trim());
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

        private void button4_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            labelPID.Text = string.Empty;
            labelPID.Visible = false; // Optionally hide the label if needed

            textBox2.Text = string.Empty;
            textBoxAge.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedIndex = -1; // Clear the selection
            comboBox11.SelectedIndex = -1; // Clear the selection
            dateTimePickerBirthdate.Value = DateTime.Today; // Set to today's date
            textBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;
            textBoxBP.Text = string.Empty;
            textBoxPR.Text = string.Empty;
            textBoxWT.Text = string.Empty;
            textBoxTEMP.Text = string.Empty;
            textBoxSPO2.Text = string.Empty;

            // Ensure that SelectedItem does not cause any issues
            if (comboBox11.SelectedItem == null)
            {
                comboBox11.SelectedText = string.Empty; // Reset selected text in case of null
            }
        }



        private void button6_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT PID, Name, Address, Status, Gender, Birthday, ContactNumber, Company, Position, LMP FROM patientinfo WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textBox10.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBoxAge.Text, "^[0-9]*$"))
            {
                // If it does, remove the non-numeric characters
                textBoxAge.Text = System.Text.RegularExpressions.Regex.Replace(textBoxAge.Text, "[^0-9]", "");
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

        private void DgnssAddbttn_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO patientinfo (PID, Name, Age, DiagnosisName, Symptoms, WhenItStarted, Days, DurationType, Severity, Diagnostic_Test, Medicines, Type, NotesForSymptoms) VALUES (@PID, @Name, @Age, @DiagnosisName, @Symptoms, @WhenItStarted, @Days, @DurationType, @Severity, @Diagnostic_Test, @Medicines, @Type, @NotesForSymptoms)";
                    var cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", textBox12.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", textBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiagnosisName", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@Symptoms", comboBoxSymptoms.Text.Trim());
                    cmd.Parameters.AddWithValue("@WhenItStarted", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@Days", comboBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@DurationType", comboBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@Severity", comboBox12.Text.Trim());
                    cmd.Parameters.AddWithValue("@Diagnostic_Test", comboBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@Medicines", comboBoxMedicine.Text.Trim());
                    cmd.Parameters.AddWithValue("@Type", comboBoxMedicineType.Text.Trim());
                    cmd.Parameters.AddWithValue("@NotesForSymptoms", richTextBox2.Text.Trim());


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Entered Successfully");
                        Displaydiagnosis();  // Refresh the DataGridView to show the new data
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

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a record is selected
                if (string.IsNullOrWhiteSpace(textBox12.Text))
                {
                    MessageBox.Show("Select a record first.");
                    return;
                }

                // Validate essential inputs
                if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(comboBoxSymptoms.Text))
                {
                    MessageBox.Show("Missing essential information (e.g., Diagnosis Name or Symptoms).");
                    return;
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"UPDATE patientinfo 
                             SET Name = @Name, Age = @Age, DiagnosisName = @DiagnosisName, Symptoms = @Symptoms, WhenItStarted = @WhenItStarted, Days = @Days, DurationType = @DurationType, Severity = @Severity, Diagnostic_Test = @Diagnostic_Test, Medicines = @Medicines, Type = @Type, NotesForSymptoms = @NotesForSymptoms  WHERE Name = @Name";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", textBox12.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", textBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiagnosisName", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@Symptoms", comboBoxSymptoms.Text.Trim());
                    cmd.Parameters.AddWithValue("@WhenItStarted", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@Days", comboBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@DurationType", comboBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@Severity", comboBox12.Text.Trim());
                    cmd.Parameters.AddWithValue("@Diagnostic_Test", comboBox8.Text.Trim());
                    cmd.Parameters.AddWithValue("@Medicines", comboBoxMedicine.Text.Trim());
                    cmd.Parameters.AddWithValue("@Type", comboBoxMedicineType.Text.Trim());
                    cmd.Parameters.AddWithValue("@NotesForSymptoms", richTextBox2.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Updated Successfully");
                        Displaydiagnosis();  // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check your data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    // Display confirmation dialog
                    DialogResult result = MessageBox.Show("Are you sure you want to delete the selected record(s)?", "Confirmation", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        using (MySqlConnection con = new MySqlConnection(connectionString))
                        {
                            con.Open();

                            // Loop through all selected rows
                            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                            {
                                // Get the PID of the selected row
                                string selectedPID = row.Cells["PID"].Value.ToString();

                                // Delete the record with the selected PID
                                string query = "DELETE FROM patientinfo WHERE Name = @Name";
                                MySqlCommand cmd = new MySqlCommand(query, con);
                                cmd.Parameters.AddWithValue("@Name", selectedPID);
                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Record(s) Deleted Successfully");
                            Displaydiagnosis(); // Refresh the DataGridView
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a record to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox3.Text = "";
            comboBoxSymptoms.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox7.Text = "";
            comboBox6.Text = "";
            comboBox8.Text = "";
            comboBox12.Text = "";
            comboBoxMedicine.Text = "";
            comboBoxMedicineType.Text = "";
            richTextBox2.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT PID, Name, Age, DiagnosisName, Symptoms, WhenIStarted, Days, DurationType, Severity, Diagnostic_Test, Medicines, Type, NotesForSymptoms  FROM patientinfo WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textBox12.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView2.DataSource = dt;
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT * FROM patientinfo WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textBox17.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView3.DataSource = dt;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            LogIn login = new LogIn();
            login.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";
            using (var con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO healthcare (ID, Name, Specialization, ContactNumber) VALUES (@ID, @Name, @Specialization, @ContactNumber)";
                    var cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ID", textBox18.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", textBox19.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", textBox20.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactNumber", textBox21.Text.Trim());
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Entered Successfully");
                        Displayhealthcare();  // Refresh the DataGridView to show the new data
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

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox18.Text))
            {
                MessageBox.Show("Please select a record to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = "server=localhost;user=root;password=;database=prsdb;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE healthcare SET Name = @Name, Specialization = @Specialization, `Contact Number` = @ContactNumber WHERE ID = @ID";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@ID", textBox18.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", textBox19.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", textBox20.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactNumber", textBox21.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Updated Successfully");
                        Displayhealthcare();  // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check your data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox18.Text))
            {
                MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string connectionString = "server=localhost;user=root;password=;database=prsdb;";
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "DELETE FROM healthcare WHERE ID = @ID";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ID", textBox18.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record Deleted Successfully");
                            Displayhealthcare();  // Refresh the DataGridView
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
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT * FROM healthcare WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textBox22.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Displaydiagnosis();

        }



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Ensure a valid row index
                {
                    // Get the selected row
                    DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                    // Populate controls with the selected record's details
                    textBox12.Text = row.Cells["Name"].Value.ToString();
                    textBox5.Text = row.Cells["Age"].Value.ToString();
                    textBox3.Text = row.Cells["DiagnosisName"].Value.ToString();
                    comboBoxSymptoms.Text = row.Cells["Symptoms"].Value.ToString();
                    dateTimePicker1.Value = (DateTime)row.Cells["WhenItStarted"].Value;
                    comboBox7.Text = row.Cells["Days"].Value.ToString();
                    comboBox6.Text = row.Cells["DurationType"].Value.ToString();
                    comboBox12.Text = row.Cells["Severity"].Value.ToString();
                    comboBox8.Text = row.Cells["Diagnostic_Test"].Value.ToString();
                    comboBoxMedicine.Text = row.Cells["Medicines"].Value.ToString();
                    comboBoxMedicineType.Text = row.Cells["Type"].Value.ToString();
                    richTextBox2.Text = row.Cells["NotesForSymptoms"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)  // Ensure a valid row index
                {
                    // Get the selected row
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Populate controls with the selected record's details
                    labelPID.Text = row.Cells["PID"].Value.ToString();
                    labelPID.Visible = true;  // Make the label visible
                    textBox2.Text = row.Cells["Name"].Value.ToString();
                    textBoxAge.Text = row.Cells["Age"].Value.ToString();
                    textBox4.Text = row.Cells["Address"].Value.ToString();
                    comboBox1.Text = row.Cells["Status"].Value.ToString();
                    comboBox11.Text = row.Cells["Gender"].Value.ToString(); // Changed to Text for consistency
                    dateTimePickerBirthdate.Value = Convert.ToDateTime(row.Cells["BirthDay"].Value);
                    textBox6.Text = row.Cells["ContactNumber"].Value.ToString();
                    textBox7.Text = row.Cells["Company"].Value.ToString();
                    textBox8.Text = row.Cells["Position"].Value.ToString();
                    textBox9.Text = row.Cells["LMP"].Value.ToString();
                    textBox11.Text = row.Cells["Email"].Value.ToString();
                    textBoxBP.Text = row.Cells["BP"].Value.ToString();
                    textBoxPR.Text = row.Cells["PR"].Value.ToString();
                    textBoxWT.Text = row.Cells["WT"].Value.ToString();
                    textBoxTEMP.Text = row.Cells["TEMP"].Value.ToString();
                    textBoxSPO2.Text = row.Cells["SPO2"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void button17_Click(object sender, EventArgs e)
        {
            Displayhistory();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

            // Correct the query by removing the comma before FROM
            string query = "SELECT Name,  AppointmentDateTime, VisitType, AppointmentStatus, CreatedDate FROM patientinfo WHERE Name LIKE @search";

            // Use MySqlConnection and MySqlCommand from MySql.Data.MySqlClient
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@search", "%" + textBox27.Text + "%");
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView5.DataSource = dt;
            }
        }


        private void SetupDateTimePicker()
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm";
        }

        private void ScheduleAppointmentReminderEmail(string email, DateTime appointmentDateTime)
        {
            try
            {
                // Calculate the notification date/time (1 day before the appointment)
                DateTime notificationDateTime = appointmentDateTime.AddDays(-1); // Added this line

                // Calculate the delay until the notification should occur
                TimeSpan delay = notificationDateTime - DateTime.Now; // Added this line

                // Ensure the delay is positive; if not, do not schedule the timer
                if (delay.TotalMilliseconds > 0)
                {
                    // Use a System.Threading.Timer to trigger this method at the specified date/time
                    System.Threading.TimerCallback timerCallback = state =>
                    {
                        bool emailSent = SendAppointmentReminderEmail(email, appointmentDateTime);
                        if (!emailSent)
                        {
                            MessageBox.Show($"Failed to send reminder email to {email}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    };

                    // Create a System.Threading.Timer that triggers the TimerCallback method after the specified delay
                    System.Threading.Timer timer = new System.Threading.Timer(timerCallback, null, delay, Timeout.InfiniteTimeSpan);

                    MessageBox.Show($"Reminder scheduled for {email} at {notificationDateTime}", "Reminder Scheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Appointment is already due or invalid notification time.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to schedule email notification: {ex.Message}", "Email Scheduling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SendAppointmentReminderEmail(string email, DateTime appointmentDateTime)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("staffclinic27@gmail.com", "cqap urjw lwuy dihw\r\n"); // Use the correct app-specific password

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("staffclinic27@gmail.com");
                        mailMessage.To.Add(email);
                        mailMessage.Subject = "Appointment Reminder";
                        mailMessage.Body = $"Dear Patient,<br><br>This is a reminder that you have an appointment scheduled for {appointmentDateTime}. Please confirm your presence.<br><br>Regards,<br>The Clinic";
                        mailMessage.IsBodyHtml = true;

                        client.Send(mailMessage);
                    }
                }

                MessageBox.Show($"Reminder email sent to {email}", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show($"SMTP error: {smtpEx.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            Displaypatient();
        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=prsdb;";
            string medicalConditions = GetCheckedItems(new CheckBox[]
            {
        DiabetescheckBox, HypertensioncheckBox, AsthmacheckBox, AthritischeckBox,
        CancercheckBox, DepressioncheckBox, AnxietycheckBox, ObesitycheckBox, EpilepsycheckBox
            });

            string familyMedicalHistory = GetCheckedItems(new CheckBox[]
            {
        checkBox10, checkBox11, checkBox12, checkBox13, checkBox14,
        checkBox15, checkBox16, checkBox17, checkBox18
            });

            string medications = GetCombinedItems(new string[] { combo1.Text, combo2.Text, combo3.Text });
            string dosages = GetCombinedItems(new string[] { combo4.Text, combo5.Text, combo6.Text });

            string allergies = GetCombinedItems(new string[] { combo9.Text, combo10.Text });
            string reactions = GetCombinedItems(new string[] { combo11.Text, combo12.Text });

            // Lifestyle habits
            string smokingStatus = smokingYesCheckBox.Checked ? "Yes" : smokingNoCheckBox.Checked ? "No" : "Not Specified";
            string smokingFrequency = smokingYesCheckBox.Checked ? smokingFrequencyComboBox.SelectedItem?.ToString() : "N/A";
            string smokingQuantity = smokingYesCheckBox.Checked ? smokingQuantityComboBox.SelectedItem?.ToString() : "N/A";

            string alcoholStatus = alcoholYesCheckBox.Checked ? "Yes" : alcoholNoCheckBox.Checked ? "No" : "Not Specified";
            string alcoholFrequency = alcoholYesCheckBox.Checked ? alcoholFrequencyComboBox.SelectedItem?.ToString() : "N/A";
            string alcoholQuantity = alcoholYesCheckBox.Checked ? alcoholQuantityComboBox.SelectedItem?.ToString() : "N/A";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE patientinfo SET MedicalConditions = @MedicalConditions, FamilyMedicalHistory = @FamilyMedicalHistory, Medications = @Medications, Dosage = @Dosage, Allergies = @Allergies, Reaction = @Reaction, SmokingStatus = @SmokingStatus, SmokingFrequency = @SmokingFrequency, SmokingQuantity = @SmokingQuantity, AlcoholStatus = @AlcoholStatus, AlcoholFrequency = @AlcoholFrequency, AlcoholQuantity = @AlcoholQuantity WHERE Name = @Name";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@MedicalConditions", medicalConditions);
                    command.Parameters.AddWithValue("@FamilyMedicalHistory", familyMedicalHistory);
                    command.Parameters.AddWithValue("@Medications", medications);
                    command.Parameters.AddWithValue("@Dosage", dosages);
                    command.Parameters.AddWithValue("@Allergies", allergies);
                    command.Parameters.AddWithValue("@Reaction", reactions);
                    command.Parameters.AddWithValue("@SmokingStatus", smokingStatus);
                    command.Parameters.AddWithValue("@SmokingFrequency", smokingFrequency);
                    command.Parameters.AddWithValue("@SmokingQuantity", smokingQuantity);
                    command.Parameters.AddWithValue("@AlcoholStatus", alcoholStatus);
                    command.Parameters.AddWithValue("@AlcoholFrequency", alcoholFrequency);
                    command.Parameters.AddWithValue("@AlcoholQuantity", alcoholQuantity);
                    command.Parameters.AddWithValue("@Name", textBox40.Text); // Ensure you have a patient name or ID to update

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Saved successfully!");
                        Displayhistory();  // Refresh the DataGridView or form
                    }
                    else
                    {
                        MessageBox.Show("Failed to save. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Helper methods to get checked items from checkboxes
        private string GetCheckedItems(CheckBox[] checkBoxes)
        {
            List<string> checkedItems = new List<string>();
            foreach (var checkBox in checkBoxes)
            {
                if (checkBox.Checked)
                {
                    checkedItems.Add(checkBox.Text);
                }
            }
            return string.Join(",", checkedItems);
        }

        // Helper methods to combine items from ComboBoxes
        private string GetCombinedItems(string[] items)
        {
            return string.Join(",", items);
        }



        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)  // Ensure a valid row index
                {
                    // Get the selected row
                    DataGridViewRow row = dataGridView3.Rows[e.RowIndex];

                    // Fill in the patient details (e.g., Name, Medical History)
                    textBox40.Text = row.Cells["Name"].Value.ToString();

                    // Set medical conditions and family medical history checkboxes
                    SetMedicalConditionCheckBoxes(row.Cells["MedicalConditions"].Value.ToString());
                    SetFamilyMedicalHistoryCheckBoxes(row.Cells["FamilyMedicalHistory"].Value.ToString());

                    // Set medication, dosage, allergies, and reactions from ComboBoxes
                    SetComboBoxValues(new System.Windows.Forms.ComboBox[] { combo1, combo2, combo3 }, row.Cells["Medications"].Value.ToString());
                    SetComboBoxValues(new System.Windows.Forms.ComboBox[] { combo4, combo5, combo6 }, row.Cells["Dosage"].Value.ToString());
                    SetComboBoxValues(new System.Windows.Forms.ComboBox[] { combo9, combo10 }, row.Cells["Allergies"].Value.ToString());
                    SetComboBoxValues(new System.Windows.Forms.ComboBox[] { combo11, combo12 }, row.Cells["Reaction"].Value.ToString());

                    // Handle Smoking Status (Checkbox for Yes/No)
                    if (row.Cells["SmokingStatus"].Value.ToString() == "Yes")
                    {
                        smokingYesCheckBox.Checked = true;
                        smokingNoCheckBox.Checked = false;
                    }
                    else if (row.Cells["SmokingStatus"].Value.ToString() == "No")
                    {
                        smokingNoCheckBox.Checked = true;
                        smokingYesCheckBox.Checked = false;
                    }
                    else
                    {
                        smokingYesCheckBox.Checked = false;
                        smokingNoCheckBox.Checked = false;
                    }

                    // Set the Smoking Frequency and Quantity ComboBoxes
                    smokingFrequencyComboBox.Text = row.Cells["SmokingFrequency"].Value.ToString();
                    smokingQuantityComboBox.Text = row.Cells["SmokingQuantity"].Value.ToString();

                    // Handle Alcohol Status (Checkbox for Yes/No)
                    if (row.Cells["AlcoholStatus"].Value.ToString() == "Yes")
                    {
                        alcoholYesCheckBox.Checked = true;
                        alcoholNoCheckBox.Checked = false;
                    }
                    else if (row.Cells["AlcoholStatus"].Value.ToString() == "No")
                    {
                        alcoholNoCheckBox.Checked = true;
                        alcoholYesCheckBox.Checked = false;
                    }
                    else
                    {
                        alcoholYesCheckBox.Checked = false;
                        alcoholNoCheckBox.Checked = false;
                    }

                    // Set the Alcohol Frequency and Quantity ComboBoxes
                    alcoholFrequencyComboBox.Text = row.Cells["AlcoholFrequency"].Value.ToString();
                    alcoholQuantityComboBox.Text = row.Cells["AlcoholQuantity"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Helper method to set the medical condition checkboxes
        private void SetMedicalConditionCheckBoxes(string medicalConditions)
        {
            DiabetescheckBox.Checked = medicalConditions.Contains("Diabetes");
            HypertensioncheckBox.Checked = medicalConditions.Contains("Hypertension");
            AsthmacheckBox.Checked = medicalConditions.Contains("Asthma");
            AthritischeckBox.Checked = medicalConditions.Contains("Athritis");
            CancercheckBox.Checked = medicalConditions.Contains("Cancer");
            DepressioncheckBox.Checked = medicalConditions.Contains("Depression");
            AnxietycheckBox.Checked = medicalConditions.Contains("Anxiety");
            ObesitycheckBox.Checked = medicalConditions.Contains("Obesity");
            EpilepsycheckBox.Checked = medicalConditions.Contains("Epilepsy");
        }

        // Helper method to set the family medical history checkboxes
        private void SetFamilyMedicalHistoryCheckBoxes(string familyHistory)
        {
            checkBox10.Checked = familyHistory.Contains("Diabetes");  // Replace "Condition1" with actual condition names
            checkBox11.Checked = familyHistory.Contains("Hypertension");
            checkBox12.Checked = familyHistory.Contains("Asthma");
            checkBox13.Checked = familyHistory.Contains("Athritis");
            checkBox14.Checked = familyHistory.Contains("Cancer");
            checkBox15.Checked = familyHistory.Contains("Depression");
            checkBox16.Checked = familyHistory.Contains("Anxiety");
            checkBox17.Checked = familyHistory.Contains("Obesity");
            checkBox18.Checked = familyHistory.Contains("Epilepsy");
        }

        // Generic method to set ComboBox values based on saved data
        private void SetComboBoxValues(System.Windows.Forms.ComboBox[] comboBoxes, string values)
        {
            string[] splitValues = values.Split(',');  // Assuming the data is saved as comma-separated values
            for (int i = 0; i < comboBoxes.Length; i++)
            {
                comboBoxes[i].Text = splitValues.Length > i ? splitValues[i] : "";
            }
        }


        private void button20_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox23.Text))
            {
                MessageBox.Show("Please select an appointment to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE patientinfo SET  VisitType = @VisitType, AppointmentDateTime = @AppointmentDateTime, AppointmentStatus = @AppointmentStatus WHERE Name = @Name";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Name", textBox23.Text.Trim());
                    command.Parameters.AddWithValue("@VisitType", comboBox3.Text.Trim());
                    command.Parameters.AddWithValue("@AppointmentDateTime", dateTimePicker2.Value);
                    command.Parameters.AddWithValue("@AppointmentStatus", comboBox2.Text.Trim());

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Appointment updated successfully!");
                        Displayappointments();  // Refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Failed to update appointment. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox23.Text))
            {
                MessageBox.Show("Please select an appointment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM patientinfo WHERE Name = @Name";
                        MySqlCommand command = new MySqlCommand(query, connection);

                        command.Parameters.AddWithValue("@Name", textBox23.Text.Trim());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment deleted successfully!");
                            Displayappointments();  // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No appointment found with the given Patient ID. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void dataGridView5_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                // Safely retrieve the details of the double-clicked record
                if (dataGridView5.Rows[e.RowIndex].Cells["Name"].Value != null)
                    textBox23.Text = dataGridView5.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                if (dataGridView5.Rows[e.RowIndex].Cells["VisitType"].Value != null)
                    comboBox3.Text = dataGridView5.Rows[e.RowIndex].Cells["VisitType"].Value.ToString();

                DateTime dateValue;
                if (DateTime.TryParse(dataGridView5.Rows[e.RowIndex].Cells["AppointmentDateTime"].Value?.ToString(), out dateValue))
                {
                    dateTimePicker2.Value = dateValue;
                }
                else
                {
                    MessageBox.Show("Invalid date format in selected record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (dataGridView5.Rows[e.RowIndex].Cells["AppointmentStatus"].Value != null)
                    comboBox2.Text = dataGridView5.Rows[e.RowIndex].Cells["AppointmentStatus"].Value.ToString();
            }
        }


        private void button24_Click(object sender, EventArgs e)
        {
            Displayappointments();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox40.Text))
            {
                MessageBox.Show("Please select a record to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "DELETE FROM patientinfo WHERE Name = @Name";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Name", textBox40.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record Deleted Successfully");
                            Displayhistory();  // Refresh the DataGridView
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
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void dataGridView4_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                // Retrieve the details of the double-clicked record
                textBox18.Text = dataGridView4.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                textBox19.Text = dataGridView4.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                textBox20.Text = dataGridView4.Rows[e.RowIndex].Cells["Specialization"].Value.ToString();
                textBox21.Text = dataGridView4.Rows[e.RowIndex].Cells["ContactNumber"].Value.ToString();
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            if (comboBoxSortOptions.SelectedItem.ToString() == "Alphabetically")
            {
                SortByName();
            }
            else if (comboBoxSortOptions.SelectedItem.ToString() == "By Date")
            {
                SortByDate();
            }
        }
        private void SortByName()
        {
            DataTable dt = (DataTable)dataGridView5.DataSource;
            dt.DefaultView.Sort = "Name ASC";
            dataGridView5.DataSource = dt;
        }

        private void SortByDate()
        {
            DataTable dt = (DataTable)dataGridView5.DataSource;
            dt.DefaultView.Sort = "CreatedDate ASC";
            dataGridView5.DataSource = dt;
        }

        private void comboBoxSortOptions1_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem.ToString() == "Alphabetically")
            {
                SortByName1();
            }
            else if (comboBox4.SelectedItem.ToString() == "By Date")
            {
                SortByDate1();
            }
        }

        private void SortByName1()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            dt.DefaultView.Sort = "Name ASC";
            dataGridView1.DataSource = dt;
        }

        private void SortByDate1()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            dt.DefaultView.Sort = "Record_Date ASC";
            dataGridView1.DataSource = dt;
        }

        private void buttonsort2_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedItem.ToString() == "Alphabetically")
            {
                SortByName2();
            }
            else if (comboBox5.SelectedItem.ToString() == "Patient ID")
            {
                SortByDate2();
            }

        }

        private void SortByName2()
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;
            dt.DefaultView.Sort = "Name ASC";
            dataGridView2.DataSource = dt;
        }

        private void SortByDate2()
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;
            dt.DefaultView.Sort = "PID ASC";
            dataGridView2.DataSource = dt;
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            showappointment showappointment = new showappointment();
            showappointment.Show();
        }

        private async Task<bool> SendReminderSmsAsync(string phoneNumber, string message)
        {
            try
            {
                // Create the Vonage credentials
                var credentials = Credentials.FromApiKeyAndSecret("41ab86ee", "QeXL330qp3w3lAbB");

                // Create the Vonage client
                var vonageClient = new VonageClient(credentials);

                // Create and send the SMS request
                var response = await vonageClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = phoneNumber,
                    From = "+1234567890", // Your sender ID
                    Text = message
                });

                // Check the response
                if (response.Messages[0].Status == "0")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Failed to send SMS: " + response.Messages[0].ErrorText, "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send SMS: " + ex.Message, "SMS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Event handler for the send reminder button click
        private async void sendreminder_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                List<Task> tasks = new List<Task>();  // List to hold the async tasks
                StringBuilder failedReminders = new StringBuilder();  // To track failures

                foreach (DataGridViewRow selectedRow in dataGridView5.SelectedRows)
                {
                    string email = selectedRow.Cells["Email"].Value.ToString();
                    string name = selectedRow.Cells["Name"].Value.ToString();
                    DateTime appointmentDateTime = DateTime.Parse(selectedRow.Cells["AppointmentDateTime"].Value.ToString());
                    string visitType = selectedRow.Cells["VisitType"].Value.ToString();
                    string phoneNumber = selectedRow.Cells["ContactNumber"].Value.ToString(); // Assuming you have a phone number column

                    string subject = "Appointment Reminder";
                    string body = $"Dear {name},\n\nThis is a reminder that you have an appointment scheduled for {appointmentDateTime} for a {visitType}.\n\nBest regards,\nLCC1 MEDICAL";

                    // Add the email sending and SMS sending tasks to the list
                    tasks.Add(Task.Run(() =>
                    {
                        bool emailSent = SendReminderEmail(email, subject, body);
                        if (!emailSent)
                        {
                            failedReminders.AppendLine($"Failed to send email to {name} at {email}");
                        }
                    }));

                    tasks.Add(Task.Run(async () =>
                    {
                        bool smsSent = await SendReminderSmsAsync(phoneNumber, body);
                        if (!smsSent)
                        {
                            failedReminders.AppendLine($"Failed to send SMS to {name} at {phoneNumber}");
                        }
                    }));
                }

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);

                // Display success or failure message
                if (failedReminders.Length > 0)
                {
                    MessageBox.Show($"Some reminders failed:\n\n{failedReminders}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("All reminders sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select one or more clients from the list to send reminders.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private bool SendReminderEmail(string email, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("staffclinic27@gmail.com", "cqap urjw lwuy dihw");
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("staffclinic27@gmail.com"),
                    Subject = subject,
                    Body = body
                };
                mailMessage.To.Add(email);
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void HighlightNextDayAppointments()
        {
            foreach (DataGridViewRow row in dataGridView5.Rows)
            {
                if (row.Cells["AppointmentDateTime"].Value != null && DateTime.TryParse(row.Cells["AppointmentDateTime"].Value.ToString(), out DateTime appointmentDate))
                {
                    // Debugging information
                    Console.WriteLine($"Row: {row.Index}, AppointmentDateTime: {appointmentDate}");

                    if (appointmentDate.Date == DateTime.Now.AddDays(1).Date)
                    {
                        row.Cells["PID"].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }

       

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // Prevents any key input
        }

        private void Button_Generate_Click(object sender, EventArgs e)
        {
            string clinicName = "LCC1 MEDICAL AND DIAGNOSTIC CENTER";
            string address = "312 GALAS ST. BIGNAY VALENZUELA CITY";
            string license = "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2";
            string phone = "Phone: 09566481052";
            string email = "Email: staffclinic27@gmail.com";

            // Create the "Medical Certificate" section with lines above and below
            string medicalCertificateTitle = "Medical Certificate";
            string certificateContent = $@"
{clinicName}
{address}
{license}
{phone}
{email}


Patient Information:
Name: {TextBox_Name.Text}
Date of Birth: {TextBox_DOB.Text}
Gender: {TextBox_Gender.Text}
Patient ID: {TextBox_PID.Text}

Medical Information:
Date of Examination: {TextBox_DOE.Text}
Diagnosis: {TextBox_Diagnosis.Text}

Details of Illness/Injury:
{richTextBox_illnessdetails.Text}

Treatment Given:
{richTextBox_Treatment.Text}

Fitness for Work:
    {TextBox_Name.Text} is advised to refrain from strenuous activities and is recommended to rest for {textBox_RestDuration.Text} days.He will be fit to return to work on {TextBox_ReturnDate.Text}, provided his condition stabilizes as expected.

Doctor’s Declaration:
    I, Dr. {textBox_DoctorNAme.Text}, hereby certify that the above information is true and accurate to the best of my knowledge. This certificate is issued for the purpose of [Purpose of Certificate, e.g., medical leave, assistance, etc.].

Doctor’s Signature:
______________________
Dr. {textBox_DoctorNAme.Text}
{textBox_DoctorQua.Text}

Seal/Stamp:
[Clinic Seal]
";

            // Set the content to the RichTextBox
            RichTextBox_CertificateContent.Text = certificateContent;

            // Apply formatting to the clinic name (Larger and Bold)
            int startIndex = RichTextBox_CertificateContent.Text.IndexOf(clinicName);
            if (startIndex >= 0)
            {
                RichTextBox_CertificateContent.Select(startIndex, clinicName.Length);
                RichTextBox_CertificateContent.SelectionFont = new Font(RichTextBox_CertificateContent.Font.FontFamily, 14, FontStyle.Bold);
                RichTextBox_CertificateContent.SelectionAlignment = HorizontalAlignment.Center;
            }

            // Apply formatting to the other clinic details (Smaller and Centered)
            string[] clinicDetails = { address, license, phone, email };
            foreach (string detail in clinicDetails)
            {
                startIndex = RichTextBox_CertificateContent.Text.IndexOf(detail);
                if (startIndex >= 0)
                {
                    RichTextBox_CertificateContent.Select(startIndex, detail.Length);
                    RichTextBox_CertificateContent.SelectionFont = new Font(RichTextBox_CertificateContent.Font.FontFamily, 10, FontStyle.Regular);
                    RichTextBox_CertificateContent.SelectionAlignment = HorizontalAlignment.Center;
                }
            }

            // Apply formatting to the "Medical Certificate" title (Bold and Centered with lines)
            startIndex = RichTextBox_CertificateContent.Text.IndexOf(medicalCertificateTitle);
            if (startIndex >= 0)
            {
                RichTextBox_CertificateContent.Select(startIndex - 2, medicalCertificateTitle.Length + 4); // Select with padding to include the lines
                RichTextBox_CertificateContent.SelectionFont = new Font(RichTextBox_CertificateContent.Font.FontFamily, 16, FontStyle.Bold);
                RichTextBox_CertificateContent.SelectionAlignment = HorizontalAlignment.Center;
            }

            // Apply formatting to "Fitness for Work" and "Doctor's Declaration" (Indented)
            string[] sectionsToIndent = { "Fitness for Work:", "Doctor’s Declaration:" };
            foreach (string section in sectionsToIndent)
            {
                startIndex = RichTextBox_CertificateContent.Text.IndexOf(section);
                if (startIndex >= 0)
                {
                    RichTextBox_CertificateContent.Select(startIndex + section.Length, RichTextBox_CertificateContent.Text.Length - (startIndex + section.Length));
                    RichTextBox_CertificateContent.SelectionIndent = 40; // Indent the message content
                }
            }

            MessageBox.Show("Medical Certificate Generated!");
        }







        private void richTextBox_Treatment_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox_illnessdetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void Button_Print_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };
            previewDialog.ShowDialog(); // Show print preview
        }

        private void FormatCertificateContent()
        {
            // Define margins
            float marginLeft = 35;
            float marginTop = 10;
            float marginRight = 35;
            float marginBottom = 20;

            // Apply formatting to the "Medical Certificate" title
            int startIndex = RichTextBox_CertificateContent.Text.IndexOf("Medical Certificate");
            if (startIndex >= 0)
            {
                RichTextBox_CertificateContent.Select(startIndex, "Medical Certificate".Length);
                RichTextBox_CertificateContent.SelectionFont = new Font(RichTextBox_CertificateContent.Font.FontFamily, 16, FontStyle.Bold);
                RichTextBox_CertificateContent.SelectionAlignment = HorizontalAlignment.Center;
            }

            // Additional formatting or positioning for other elements in the RichTextBox
            // For example, you can set the margins here or any other specific formatting
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Define margins
            float marginLeft = 50;
            float marginTop = 50;
            float marginRight = 50;
            float marginBottom = 50;

            // Calculate print area with margins
            RectangleF printArea = new RectangleF(
                marginLeft,
                marginTop,
                e.PageBounds.Width - marginLeft - marginRight,
                e.PageBounds.Height - marginTop - marginBottom
            );

            // Initialize variables for text drawing
            float currentY = marginTop;
            StringFormat centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Draw the logo on the left side
            Image logoImage = Image.FromFile("C:\\Users\\tala\\Downloads\\medical logo.jpg"); // Update the path to your logo image
            float logoWidth = 115; // Set the desired width of the logo
            float logoHeight = (logoWidth / logoImage.Width) * logoImage.Height;
            e.Graphics.DrawImage(logoImage, marginLeft, currentY, logoWidth, logoHeight);

            // Adjust the clinic details position
            float clinicDetailsX = marginLeft + logoWidth + 20; // Adjust the X position to the right of the logo
            float clinicDetailsY = marginTop;

            // Draw the "LCC1 MEDICAL AND DIAGNOSTIC CENTER" title
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            string clinicTitle = "LCC1 MEDICAL AND DIAGNOSTIC CENTER";
            SizeF clinicTitleSize = e.Graphics.MeasureString(clinicTitle, titleFont);
            float clinicTitleX = printArea.Left + (printArea.Width - clinicTitleSize.Width) / 2;
            e.Graphics.DrawString(clinicTitle, titleFont, Brushes.Black, clinicTitleX, clinicDetailsY);
            clinicDetailsY += clinicTitleSize.Height;

            // Draw the rest of the clinic details centered below the title
            string[] clinicDetails = { "312 GALAS ST. BIGNAY VALENZUELA CITY", "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2", "Phone: 09566481052", "Email: staffclinic27@gmail.com" };
            Font clinicFont = new Font("Arial", 10, FontStyle.Regular); // Smaller font for details
            foreach (string detail in clinicDetails)
            {
                SizeF detailSize = e.Graphics.MeasureString(detail, clinicFont);
                float detailX = printArea.Left + (printArea.Width - detailSize.Width) / 2;
                e.Graphics.DrawString(detail, clinicFont, Brushes.Black, detailX, clinicDetailsY);
                clinicDetailsY += detailSize.Height;
            }

            // Draw the line below the clinic details
            clinicDetailsY += 10; // Extra space before the line
            e.Graphics.DrawLine(Pens.Black, marginLeft, clinicDetailsY, e.PageBounds.Width - marginRight, clinicDetailsY);
            clinicDetailsY += 20; // Extra space after the line

            // Draw the "Medical Certificate" title
            string title = "Medical Certificate";
            SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
            float titleX = printArea.Left + (printArea.Width - titleSize.Width) / 2;
            e.Graphics.DrawString(title, titleFont, Brushes.Black, titleX, clinicDetailsY);
            clinicDetailsY += titleSize.Height;

            // Draw the line below the "Medical Certificate" title
            clinicDetailsY += 10; // Extra space before the line
            e.Graphics.DrawLine(Pens.Black, marginLeft, clinicDetailsY, e.PageBounds.Width - marginRight, clinicDetailsY);
            clinicDetailsY += 20; // Extra space after the line

            // Draw the remaining content from the RichTextBox
            Font bodyFont = new Font("Arial", 10, FontStyle.Regular);
            string[] lines = RichTextBox_CertificateContent.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                // Skip the duplicate clinic details and "Medical Certificate" if they are present in the RichTextBox content
                if (line.Trim() == "LCC1 MEDICAL AND DIAGNOSTIC CENTER" || line.Trim() == "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2" || line.Trim() == "312 GALAS ST. BIGNAY VALENZUELA CITY" || line.Trim() == "Phone: 09566481052" || line.Trim() == "Email: staffclinic27@gmail.com" || line.Trim() == title)
                {
                    continue;
                }

                // Add extra space before the signature if this is the signature line
                if (line.Trim().StartsWith("Doctor’s Signature"))
                {
                    clinicDetailsY += 50; // Add desired space before the signature
                }

                // Measure the size of the line within the specified width
                SizeF lineSize = e.Graphics.MeasureString(line, bodyFont);

                // Create a rectangle that represents the bounding area for the text
                RectangleF lineRect = new RectangleF(marginLeft, clinicDetailsY, printArea.Width, lineSize.Height);

                // Draw the string within the rectangle, which will automatically handle wrapping
                e.Graphics.DrawString(line, bodyFont, Brushes.Black, lineRect);

                // Update the current Y position based on the height of the text that was drawn
                clinicDetailsY += lineSize.Height + 10; // Add space between lines
            }

            e.HasMorePages = false; // No more pages to print
        }






        private void Button_SendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate the certificate image
                string certificateImagePath = GenerateCertificateImage();

                // Upload the image to Google Drive and get the link
                GoogleDriveHelper driveHelper = new GoogleDriveHelper();
                string GoogleDriveLink = driveHelper.UploadFileToGoogleDrive(certificateImagePath);

                // Use googleDriveLink here
                SendCertificateEmail(GoogleDriveLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message);
            }
        }


        private string UploadFileToGoogleDrive(string filePath)
        {
            var service = GoogleDriveHelper.GetDriveService();

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                MimeType = "image/png"
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/png");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            // Generate a view link to the uploaded file
            string fileId = file.Id;
            string link = $"https://drive.google.com/file/d/{fileId}/view?usp=sharing";

            return link;

        }


        private string UploadToGoogleDrive(string filePath)
        {
            GoogleDriveHelper driveHelper = new GoogleDriveHelper();
            return driveHelper.UploadFileToGoogleDrive(filePath); // Adjust according to your helper class implementation
        }

        private void SendCertificateEmail(string GoogleDriveLink)
        {
            try
            {
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
                smtpServer.Credentials = new System.Net.NetworkCredential("staffclinic27@gmail.com", "cqap urjw lwuy dihw");
                smtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("staffclinic27@gmail.com");
                mail.To.Add(textBox_Email.Text.Trim());
                mail.Subject = "Your Medical Certificate";
                mail.Body = $"Please find your medical certificate at the following link:\n{GoogleDriveLink}";

                smtpServer.Send(mail);
                MessageBox.Show("Medical certificate sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message);
            }
        }


        private string GenerateCertificateImage()
        {
            // Define the dimensions of the image
            int width = 800; // Adjust as needed
            int height = 1000; // Adjust as needed

            // Create a Bitmap with the specified dimensions
            Bitmap bitmap = new Bitmap(width, height);

            // Create a Graphics object from the Bitmap
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White); // Set background color

                // Define margins
                float marginLeft = 50;
                float marginTop = 50;
                float marginRight = 50;
                float marginBottom = 50;

                // Calculate print area with margins
                RectangleF printArea = new RectangleF(
                    marginLeft,
                    marginTop,
                    width - marginLeft - marginRight,
                    height - marginTop - marginBottom
                );

                // Initialize variables for text drawing
                float currentY = marginTop;
                StringFormat centerFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                // Draw the logo on the left side
                Image logoImage = Image.FromFile("C:\\Users\\tala\\Downloads\\medical logo.jpg");
                float logoWidth = 115;
                float logoHeight = (logoWidth / logoImage.Width) * logoImage.Height;
                graphics.DrawImage(logoImage, marginLeft, currentY, logoWidth, logoHeight);

                // Adjust the clinic details position
                float clinicDetailsX = marginLeft + logoWidth + 20; // Adjust the X position to the right of the logo
                float clinicDetailsY = marginTop;

                // Draw the "LCC1 MEDICAL AND DIAGNOSTIC CENTER" title
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                string clinicTitle = "LCC1 MEDICAL AND DIAGNOSTIC CENTER";
                SizeF clinicTitleSize = graphics.MeasureString(clinicTitle, titleFont);
                float clinicTitleX = printArea.Left + (printArea.Width - clinicTitleSize.Width) / 2;
                graphics.DrawString(clinicTitle, titleFont, Brushes.Black, clinicTitleX, clinicDetailsY);
                clinicDetailsY += clinicTitleSize.Height;

                // Draw the rest of the clinic details centered below the title
                string[] clinicDetails = { "312 GALAS ST. BIGNAY VALENZUELA CITY", "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2", "Phone: 09566481052", "Email: staffclinic27@gmail.com" };
                Font clinicFont = new Font("Arial", 10, FontStyle.Regular);
                foreach (string detail in clinicDetails)
                {
                    SizeF detailSize = graphics.MeasureString(detail, clinicFont);
                    float detailX = printArea.Left + (printArea.Width - detailSize.Width) / 2;
                    graphics.DrawString(detail, clinicFont, Brushes.Black, detailX, clinicDetailsY);
                    clinicDetailsY += detailSize.Height;
                }

                // Draw the line below the clinic details
                clinicDetailsY += 10;
                graphics.DrawLine(Pens.Black, marginLeft, clinicDetailsY, width - marginRight, clinicDetailsY);
                clinicDetailsY += 20;

                // Draw the "Medical Certificate" title
                string title = "Medical Certificate";
                SizeF titleSize = graphics.MeasureString(title, titleFont);
                float titleX = printArea.Left + (printArea.Width - titleSize.Width) / 2;
                graphics.DrawString(title, titleFont, Brushes.Black, titleX, clinicDetailsY);
                clinicDetailsY += titleSize.Height;

                // Draw the line below the "Medical Certificate" title
                clinicDetailsY += 10;
                graphics.DrawLine(Pens.Black, marginLeft, clinicDetailsY, width - marginRight, clinicDetailsY);
                clinicDetailsY += 20;

                // Draw the remaining content from the RichTextBox
                Font bodyFont = new Font("Arial", 10, FontStyle.Regular);
                string[] lines = RichTextBox_CertificateContent.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (line.Trim() == "LCC1 MEDICAL AND DIAGNOSTIC CENTER" || line.Trim() == "DOH ACCREDITED LABORATORY with License #13-0679-19-CL-2" || line.Trim() == "312 GALAS ST. BIGNAY VALENZUELA CITY" || line.Trim() == "Phone: 09566481052" || line.Trim() == "Email: staffclinic27@gmail.com" || line.Trim() == title)
                    {
                        continue;
                    }

                    if (line.Trim().StartsWith("Doctor’s Signature"))
                    {
                        clinicDetailsY += 50;
                    }

                    SizeF lineSize = graphics.MeasureString(line, bodyFont);
                    RectangleF lineRect = new RectangleF(marginLeft, clinicDetailsY, printArea.Width, lineSize.Height);
                    graphics.DrawString(line, bodyFont, Brushes.Black, lineRect);

                    clinicDetailsY += lineSize.Height + 10;
                }
            }

            // Define the path where the image will be saved
            string imagePath = Path.Combine(Path.GetTempPath(), "certificate_image.png");

            // Save the bitmap as a PNG file
            bitmap.Save(imagePath, ImageFormat.Png);

            // Dispose of the bitmap to free resources
            bitmap.Dispose();

            // Return the path to the saved image file
            return imagePath;
        }

        // Optional: Generate a PDF version of the certificate (example placeholder method)
        // private string GenerateCertificatePdf(string certificateContent)
        // {
        //     // Implement PDF generation logic here, return the path to the generated PDF file
        //     return "path-to-generated-pdf.pdf";
        // }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void combo11_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void DateTimePickerBirthdate_ValueChanged(object sender, EventArgs e)
        {
            // Get the selected birthdate
            DateTime birthdate = dateTimePickerBirthdate.Value;

            // Calculate the age
            int age = CalculateAge(birthdate);

            // Display the age in the Age TextBox
            textBoxAge.Text = age.ToString();
        }

        private int CalculateAge(DateTime birthdate)
        {
            // Get today's date
            DateTime today = DateTime.Today;

            // Calculate the age
            int age = today.Year - birthdate.Year;

            // Adjust if the birthday has not occurred yet this year
            if (birthdate.Date > today.AddYears(-age)) age--;

            return age;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxSymptoms_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxMedicine.Items.Clear();
            string selectedSymptom = comboBoxSymptoms.SelectedItem.ToString();

            if (symptomMedicineMap.ContainsKey(selectedSymptom))
            {
                comboBoxMedicine.Items.AddRange(symptomMedicineMap[selectedSymptom].ToArray());
            }
            else
            {
                MessageBox.Show("No medicines found for the selected symptom.");
            }

            // Now, let's also update the diagnostic test ComboBox
            UpdateDiagnosticTests(selectedSymptom);
        }


        private void comboBoxMedicine_SelectedIndexChanged(object sender, EventArgs e)
        {
            int patientAge = int.Parse(textBox5.Text);

            comboBoxMedicineType.Items.Clear();

            // Define medicine types based on age groups
            if (patientAge <= 1)
            {
                // Baby (0-1 year)
                comboBoxMedicineType.Items.AddRange(new string[] { "Liquid", "Drops", "Suppository" });
            }
            else if (patientAge <= 3)
            {
                // Toddler (1-3 years)
                comboBoxMedicineType.Items.AddRange(new string[] { "Syrup", "Chewable", "Liquid" });
            }
            else if (patientAge <= 12)
            {
                // Child (4-12 years)
                comboBoxMedicineType.Items.AddRange(new string[] { "Syrup", "Chewable", "Tablet" });
            }
            else if (patientAge <= 17)
            {
                // Teenager (13-17 years)
                comboBoxMedicineType.Items.AddRange(new string[] { "Tablet", "Capsule", "Chewable" });
            }
            else if (patientAge <= 64)
            {
                // Adult (18-64 years)
                comboBoxMedicineType.Items.AddRange(new string[] { "Capsule", "Tablet" });
            }
            else
            {
                // Senior (65+ years)
                comboBoxMedicineType.Items.AddRange(new string[] { "Tablet", "Capsule", "Syrup" });
            }
        }



        private void UpdateDiagnosticTests(string selectedSymptom)
        {
            comboBox8.Items.Clear();  // Clear previous items in Diagnostic Test ComboBox

            // Define diagnostic tests for each symptom
            Dictionary<string, List<string>> symptomDiagnosticMap = new Dictionary<string, List<string>>()
    {
        { "Headache", new List<string> { "CT Scan", "MRI", "X-Ray" } },
        { "Stomachache", new List<string> { "Abdominal Ultrasound", "Endoscopy", "Blood Test" } },
        { "Cough", new List<string> { "Chest X-Ray", "Sputum Test", "Pulmonary Function Test" } }
    };

            // Add tests based on the selected symptom
            if (symptomDiagnosticMap.ContainsKey(selectedSymptom))
            {
                comboBox8.Items.AddRange(symptomDiagnosticMap[selectedSymptom].ToArray());
            }
            else
            {
                comboBox8.Items.Add("No diagnostic test available for the selected symptom.");
            }
        }



        private void comboBoxMedicine_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            DiagnosticReqForm diagnosticReqForm = new DiagnosticReqForm();
            diagnosticReqForm.Show();
        }

        private void RichTextBox_CertificateContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelPID_Click(object sender, EventArgs e)
        {

        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

