using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab_Mid_Second_Attempt
{
    public partial class Form4 : Form
    {
        
        private DataTable dtRecords;
        private int currentIndex = 0;
        private string connectionString = "Data Source=SEXYMINT;Initial Catalog=Testing;Integrated Security=True";

        public Form4()
        {
            InitializeComponent();
        }

        private void LoadRecords()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mid ORDER BY sr";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        dtRecords = new DataTable();
                        adapter.Fill(dtRecords);
                    }
                }
            }
        }


        private void Form4_Load(object sender, EventArgs e)
        {
            
            LoadRecords();

            
            if (dtRecords.Rows.Count > 0)
            {
                currentIndex = 0;
                DisplayRecord(currentIndex);
            }
            else
            {
                MessageBox.Show("No records found in the database.");
            }
        }

        
       

        
        private void DisplayRecord(int index)
        {
            if (index < 0 || index >= dtRecords.Rows.Count)
                return;

            DataRow row = dtRecords.Rows[index];

            
            textBox1.Text = row["id"].ToString();
            textBox2.Text = row["name"].ToString();
            textBox3.Text = row["father"].ToString();
            textBox4.Text = row["email"].ToString();

            
            string base64Photo = row["photoURL"].ToString();
            if (!string.IsNullOrWhiteSpace(base64Photo))
            {
                byte[] imageBytes = Convert.FromBase64String(base64Photo);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayRecord(currentIndex);
            }
            else
            {
                MessageBox.Show("You are at the first record.");
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentIndex < dtRecords.Rows.Count - 1)
            {
                currentIndex++;
                DisplayRecord(currentIndex);
            }
            else
            {
                MessageBox.Show("You are at the last record.");
            }
        }

       
        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
