using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentsLab
{
    public partial class Form1 : Form {
        private MySqlConnection myConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SqlFunctions.CloseConnection(myConnection);
        }

        private void button1_Click(object sender, EventArgs e) {
            SqlFunctions.CloseConnection(myConnection);
            myConnection = SqlFunctions.EstablishConnection();
            SqlFunctions.DropAll(myConnection);
            SqlFunctions.RecreateTables(myConnection);
            SqlFunctions.FillTestData(myConnection);

            listBox2.Items.Clear();
            listBox2.Items.AddRange(SqlFunctions.GetStudentsList(myConnection));
            listBox2.SelectedValueChanged += (o, args) =>
            {
                listBox4.Items.Clear();
                listBox4.Items.AddRange(SqlFunctions.SelectPersonal(listBox2.SelectedIndex + 1, myConnection));
            };

            listBox1.Items.Clear();
            listBox1.Items.AddRange(SqlFunctions.GetTeachersList(myConnection));
            listBox1.SelectedValueChanged += (o, args) =>
            {
                listBox5.Items.Clear();
                listBox5.Items.AddRange(SqlFunctions.SelectWork(listBox1.SelectedIndex + 1, myConnection));
            };
        }
    }
}
