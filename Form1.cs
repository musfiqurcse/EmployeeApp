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

namespace EmployeeInformationApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool update = false;
        Employee newEmployeeInfo = new Employee();
        
        private void saveButton_Click(object sender, EventArgs e)
        {
           
   
            if (IsEmailExist(newEmployeeInfo.Email))
            {
                MessageBox.Show("Email Exist");
                ClearAll();
            }
            else
            {
                if (update)
                {
                    insertQuery(newEmployeeInfo);
                    update = false;
                    ClearAll();
                    saveButton.Text = "Save";
                    MessageBox.Show("Updated Successfully");

                }
                else
                {
                    if (!NullChecker())
                    {
                        InitialValueAssign();
                        insertQuery(newEmployeeInfo);
                        ClearAll();
                        MessageBox.Show("Saved Successfully");
                    }
                    else
                    {
                        MessageBox.Show("All Information must be filled up");
                        ClearAll();
                    }

                }
                LoadEmployeeInformation();
            }


        }

        private void InitialValueAssign()
        {
            newEmployeeInfo.Name = employeeNameTextBox.Text;
            newEmployeeInfo.Address = addressTextBox.Text;
            newEmployeeInfo.Email = emailTextBox.Text;
            newEmployeeInfo.salary = Convert.ToDouble(salaryTextBox.Text);
        }

        private void insertQuery(Employee getEmployeeInfo)
        {
            string connectionString = @"SERVER=.\SQLEXPRESS;DATABASE= EmployeeInfoDB;INTEGRATED SECURITY=TRUE";
            SqlConnection con = new SqlConnection(connectionString);
            string query = "INSERT INTO EmployeeInformationTable(Name,Address,Email,Salary) Values('" +
                           getEmployeeInfo.Name + "','" + getEmployeeInfo.Address + "','" + getEmployeeInfo.Email +
                           "','" + getEmployeeInfo.salary + "')";
            SqlCommand queryNew= new SqlCommand(query,con);
            
            con.Open();
            int num = queryNew.ExecuteNonQuery();
            con.Close();

        }

        public void ClearAll()
        {
            employeeNameTextBox.Text = addressTextBox.Text = emailTextBox.Text = salaryTextBox.Text = null;
            newEmployeeInfo=new Employee();
        }

        private bool IsEmailExist(string mailCheck)
        {
            string connectionString = @"SERVER=.\SQLEXPRESS;DATABASE= EmployeeInfoDB;INTEGRATED SECURITY=TRUE";
            SqlConnection con = new SqlConnection(connectionString);
            string checkQuery = "SELECT * FROM EmployeeInformationTable WHERE EMAIL='" + mailCheck + "'";
            SqlCommand cm = new SqlCommand(checkQuery,con);
            con.Open();
            SqlDataReader readerChecker = cm.ExecuteReader();
            bool pass = false;
           while(readerChecker.Read())
            {
                pass = true;
                break;
            }
            readerChecker.Close();
            con.Close();
            return pass;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadEmployeeInformation();
        }

        private bool NullChecker()
        {
            if (employeeNameTextBox.Text == null || addressTextBox.Text == null || emailTextBox.Text == null ||
                salaryTextBox.Text == null)
            {
                return false;
            }
            return true;
        }

        private void LoadEmployeeInformation()
        {
            employeeInformationDataGridView.Rows.Clear();
            string connectionString = @"SERVER=.\SQLEXPRESS;DATABASE= EmployeeInfoDB;INTEGRATED SECURITY=TRUE";
            SqlConnection con = new SqlConnection(connectionString);
            string showQuery = "SELECT * FROM EmployeeInformationTable";
            SqlCommand cmdSelect= new SqlCommand(showQuery,con);
            con.Open();
            SqlDataReader readerRow = cmdSelect.ExecuteReader();

            while (readerRow.Read())
            {
                employeeInformationDataGridView.Rows.Add(readerRow[0].ToString(), readerRow[1].ToString(),
                    readerRow[2].ToString(), readerRow[3].ToString(), readerRow[4].ToString());


            }
            readerRow.Close();
            con.Close();
        }

        private void employeeInformationDataGridView_DoubleClick(object sender, EventArgs e)
        {
            string str = employeeInformationDataGridView.Rows[employeeInformationDataGridView.SelectedRows[0].Index].Cells[0].Value.ToString();
            int id = Convert.ToInt16(str);
            Employee updateEm = GetEmployeeInfo(id);
            employeeNameTextBox.Text = updateEm.Name;
            addressTextBox.Text = updateEm.Address;
            emailTextBox.Text = updateEm.Email;
            salaryTextBox.Text = Convert.ToString(updateEm.salary);
            saveButton.Text = "Update";
            newEmployeeInfo = updateEm;


        }

        private Employee GetEmployeeInfo(int id)
        {
            string connectionString = @"SERVER=.\SQLEXPRESS;DATABASE= EmployeeInfoDB;INTEGRATED SECURITY=TRUE";
            Employee newInfo= new Employee();
            SqlConnection con = new SqlConnection(connectionString);
            string checkQuery = "SELECT * FROM EmployeeInformationTable WHERE ID='" + id+ "'";
            SqlCommand newCmd=new SqlCommand(checkQuery,con);
            con.Open();
            SqlDataReader reader = newCmd.ExecuteReader();
            while (reader.Read())
            {
                newInfo.Name = reader[1].ToString();
                newInfo.Address = reader[2].ToString();
                newInfo.Email = reader[3].ToString();
                newInfo.salary = Convert.ToDouble(reader[4].ToString());

            }
            reader.Close();
            con.Close();
            update = true;
            return newInfo;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
           
                InitialValueAssign();
                DeleteEmployeeInformation();
                
                ClearAll();
                MessageBox.Show("Information Deleted Successfully");
            LoadEmployeeInformation();
            update = false;
            ClearAll();
            saveButton.Text = "Save";



        }

        private void DeleteEmployeeInformation()
        {
            
            string connectionString = @"SERVER=.\SQLEXPRESS;DATABASE= EmployeeInfoDB;INTEGRATED SECURITY=TRUE";
           SqlConnection con = new SqlConnection(connectionString);
            string checkQuery = "DELETE FROM EmployeeInformationTable WHERE Name='" + newEmployeeInfo.Name+ "' AND Address='"+newEmployeeInfo.Address+"' AND Email='"+newEmployeeInfo.Email+"'";
            SqlCommand newCmd = new SqlCommand(checkQuery, con);
            con.Open();
            int i1 = newCmd.ExecuteNonQuery();
            con.Close();

        }

       
    }
}
