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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }
        private void logoutbttn_Click(object sender, EventArgs e)
        {
            LogIn login = new LogIn();
            login.Show();
            this.Hide();
        }

        private void patientbttn_Click(object sender, EventArgs e)  
        {
            patient patientbttn = new patient();
            patientbttn.Show();
            this.Hide();
        }

        private void diagnosisbttn_Click(object sender, EventArgs e)
        {
            diagnosis diagnosisbttn = new diagnosis();
            diagnosisbttn.Show();
            this.Hide();
        }

        private void xbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
