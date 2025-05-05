using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;


namespace Lab_Mid_Second_Attempt
{
    public partial class Form3: Form
    {
        private string connectionString = "Data Source=SEXYMINT;Initial Catalog=Testing;Integrated Security=True";
        // Global Variable to store photoURL
        private string photoURL;
        public Form3()
        {
            InitializeComponent();
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


            //if (pictureBox1.Image != null)
            //{

            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        byte[] imageBytes = ms.ToArray();
            //        photoURL = Convert.ToBase64String(imageBytes);

            //    }
            //}
        }

        private void button3_Click(object sender, EventArgs e)

        {
   
            string searchId = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(searchId))
            {
                MessageBox.Show("Please enter a valid ID.");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM mid WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", searchId);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                textBox1.Text = reader["id"].ToString();
                                textBox2.Text = reader["name"].ToString();
                                textBox3.Text = reader["father"].ToString();
                                textBox4.Text = reader["email"].ToString();

                                string base64Photo = reader["photoURL"].ToString();
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
                            else
                            {
                                MessageBox.Show("Record not found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading record: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string query = "UPDATE mid SET name = @name, photoURL = @photoURL, father = @father, email = @email WHERE id = @id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", textBox1.Text);
                cmd.Parameters.AddWithValue("@name", textBox2.Text);
               
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        photoURL = Convert.ToBase64String(imageBytes);
                        cmd.Parameters.AddWithValue("@photoURL", photoURL);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@photoURL", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@father", textBox3.Text);
                cmd.Parameters.AddWithValue("@email", textBox4.Text);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Record updated successfully!");
                    else
                        MessageBox.Show("Update failed. Record may not exist.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }
    }
    }

