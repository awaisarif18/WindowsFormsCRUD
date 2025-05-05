//Muhammad Awais Arif
//SP22-BCS-113


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace Lab_Mid_Second_Attempt
{
    public partial class Form1 : Form

    {
        // Global Variable to store photoURL
        private string photoURL = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            string connectionString = "Data Source=SEXYMINT;Initial Catalog=Testing;Integrated Security=True";

            // SQL insert query
            string query = "INSERT INTO mid (id, name,photoURL, father, email) VALUES (@id, @name, @photoURL, @father, @email)";

           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    

                    
                    cmd.Parameters.AddWithValue("@id", textBox1.Text);
                    cmd.Parameters.AddWithValue("@name", textBox2.Text);
                    cmd.Parameters.AddWithValue("@photoURL", photoURL );
                    cmd.Parameters.AddWithValue("@father", textBox3.Text);
                    cmd.Parameters.AddWithValue("@email", textBox4.Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error inserting data: " + ex.Message);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                pictureBox1.ImageLocation = filePath;
                pictureBox1.Load();
            }

            if (pictureBox1.Image != null)
            {

                Bitmap bmp = new Bitmap(pictureBox1.Image);
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        Color col = bmp.GetPixel(i, j);
                        int red = col.R;
                        int green = col.G;
                        int blue = col.B;
                        int avg = (red + green + blue) / 3;
                        bmp.SetPixel(i, j, Color.FromArgb(red, avg, avg));
                    }
                }
                pictureBox1.Image = bmp;
            }

            else
            {
                MessageBox.Show("Please upload an image first.");
            }


            if (pictureBox1.Image != null)
            {
                 
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                     photoURL = Convert.ToBase64String(imageBytes);
                   
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();
            newForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 newForm = new Form4();
            newForm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}