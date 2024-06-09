using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Windows.Forms;

namespace test2
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Employee;Integrated Security=True;TrustServerCertificate=False;Encrypt=False");
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetEmpList();
            dataGridView1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int empId;
            double empAge;
            DateTime joiningDate;

            if (!int.TryParse(textBox1.Text, out empId) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                !double.TryParse(textBox4.Text, out empAge) ||
                !DateTime.TryParse(dateTimePicker1.Text, out joiningDate) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter valid data.");
                return;
            }

            string empName = textBox3.Text;
            string empCity = comboBox1.Text;
            string contact = textBox6.Text;
            string sex = radioButton1.Checked ? "MALE" : "FEMALE";

            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("insertEmp_SP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", empId);
                    cmd.Parameters.AddWithValue("@Emp_name", empName);
                    cmd.Parameters.AddWithValue("@Emp_city", empCity);
                    cmd.Parameters.AddWithValue("@Sex", sex);
                    cmd.Parameters.AddWithValue("@Emp_age", empAge);
                    cmd.Parameters.AddWithValue("@JoiningDate", joiningDate);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.ExecuteNonQuery();
                }
                

                MessageBox.Show("Successfully inserted");
                GetEmpList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private void GetEmpList()
        {
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("listEmp_SP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sd.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }

        private void button3_Click(object sender, EventArgs e)
        {
            int empId;
            if (!int.TryParse(textBox1.Text, out empId))
            {
                MessageBox.Show("Please enter a valid Employee ID.");
                return;
            }

            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Load1Emp_SP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Emp_id", empId));

                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sd.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dt;
                            MessageBox.Show("Successfully Loaded.");
                        }
                        else
                        {
                            MessageBox.Show("No records found for the provided Employee ID.");
                            dataGridView1.DataSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to delete row?", "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes) {
                int empId = int.Parse(textBox1.Text);
                conn.Open();
                SqlCommand cmd = new SqlCommand("exec DeleteEmp_SP '" + empId + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Deleted..");
                GetEmpList();
            }
            else if (dr == DialogResult.No)
            {
                //Nothing to do
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            int empId;
            double empAge;
            DateTime joiningDate;

            if (!int.TryParse(textBox1.Text, out empId) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                !double.TryParse(textBox4.Text, out empAge) ||
                !DateTime.TryParse(dateTimePicker1.Text, out joiningDate) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter valid data.");
                return;
            }

            string empName = textBox3.Text;
            string empCity = comboBox1.Text;
            string contact = textBox6.Text;
            string sex = radioButton1.Checked ? "MALE" : "FEMALE";

            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("udateEmp_SP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Emp_id", empId);
                    cmd.Parameters.AddWithValue("@Emp_name", empName);
                    cmd.Parameters.AddWithValue("@Emp_city", empCity);
                    cmd.Parameters.AddWithValue("@Sex", sex);
                    cmd.Parameters.AddWithValue("@Emp_age", empAge);
                    cmd.Parameters.AddWithValue("@JoiningDate", joiningDate);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.ExecuteNonQuery();
                }


                MessageBox.Show("Successfully updatedted");
                GetEmpList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            userControl11.Hide();
            dataGridView1.Show();
            dataGridView1.BringToFront();
        }
    }
}
