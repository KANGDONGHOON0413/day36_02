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

namespace day36_02
{
    public partial class Form1 : Form
    {
        string connString = "server = .\\SQLEXPRESS; database = test; uid = sa; password = alencia;";
        SqlConnection conn;
        SqlDataAdapter DA;
        DataSet Dset;
        Boolean Male = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            conn.ConnectionString = connString;
            DA = new SqlDataAdapter("SELECT * FROM testTable02", conn);
            Dset = new DataSet();
            DA.Fill(Dset, "testTable02");
            dataGridView1.DataSource = Dset.Tables["testTable02"];
        }

        private void INPUT_SEX_Click(object sender, EventArgs e)
        {
            if (Male) { Male = false; INPUT_SEX.Text = "여"; INPUT_SEX.BackColor = Color.Violet; }
            else { Male = true; INPUT_SEX.Text = "남"; INPUT_SEX.BackColor = Color.CornflowerBlue; }
        }

        private void BTN_Enter_Click(object sender, EventArgs e)
        {
            //update를 활용한 insert문
            conn.Open();
            DA.InsertCommand = new SqlCommand("INSERT INTO testTable02 (Nid, name) VALUES (@Nid, @name)");
            DA.InsertCommand.Connection = conn; //매우중요 여기서 안하면 connection property has not been initialized 가 뜨더라
            DA.InsertCommand.Parameters.Add("@Nid", SqlDbType.Int, 0, "Nid");
            DA.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 16, "name");

            DataRow NewRow = Dset.Tables["testTable02"].NewRow();
            NewRow["Nid"] = int.Parse(INPUT_Nid.Text);
            NewRow["name"] = INPUT_Name.Text;
            Dset.Tables["testTable02"].Rows.Add(NewRow);
            DA.Update(Dset, "testTable02");

            Dset.Clear();
            DA.Fill(Dset, "testTable02");
            dataGridView1.DataSource = Dset.Tables["testTable02"];
            conn.Close();
            DA.InsertCommand.Dispose();
        }
    }
}
