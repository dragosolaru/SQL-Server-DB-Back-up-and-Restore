using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace SQL_Server_DB_Back_up_and_Restore
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader reader;
        string sql = "";
        string connectionString = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
              
                //"Server= localhost; Database= employeedetails; Integrated Security=True;"
                connectionString = @"Data Source=LAPTOP-MJRNSH14\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
                   // "Data Source = " + txtDataSource.Text + "; User Id = " + txtUserId.Text + "; Password = " + txtPassword.Text + "";
                conn = new SqlConnection(connectionString);
                conn.Open();
                //sql = "EXEC sp_databases";
                sql = "select * from sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                comboDatabases.Items.Clear();
                
                while (reader.Read())
                {

                    comboDatabases.Items.Add(reader[0].ToString());
                }
                reader.Dispose();
                conn.Close();
                conn.Dispose();

                txtDataSource.Enabled = false;
                txtUserId.Enabled = false;
                txtPassword.Enabled = false;
                BtnConnect.Enabled = false;
                BtnDisconnect.Enabled = true;
                comboDatabases.Enabled = true;
                BtnBackup.Enabled = true;
                BtnRestore.Enabled = true;
             

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            txtDataSource.Enabled = true;
            txtUserId.Enabled = true;
            txtPassword.Enabled = true;
            comboDatabases.Enabled = false;
            BtnBackup.Enabled = false;
            BtnRestore.Enabled = false;
            BtnConnect.Enabled = true;
            BtnDisconnect.Enabled = false;
            btnUseCred.Enabled = true;
    
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BtnDisconnect.Enabled = false;
            comboDatabases.Enabled = false;
            BtnBackup.Enabled = false;
            BtnRestore.Enabled = false;
        }

        private void BtnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboDatabases.Text.CompareTo("")==0)
                {
                    MessageBox.Show("Te rog selecteaza o baza de date");
                    return;
                }
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "BACKUP DATABASE "+comboDatabases.Text+" TO DISK ='" +txtBackupFileLoc.Text+ "\\" +comboDatabases.Text+  ".bak'";
                command = new SqlCommand(sql,conn);
                command.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                MessageBox.Show("Backup realizat cu succes!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog()==DialogResult.OK)
            {
                txtBackupFileLoc.Text = dlg.SelectedPath;
            }
        }

        private void BtnDBFileBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Backup Files(*.bak)|*bak|All Files(*.*)|*.*";
            dlg.FilterIndex = 0;
            if (dlg.ShowDialog()==DialogResult.OK)
            {
                txtRestoreFileLoc.Text = dlg.FileName;
            }
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboDatabases.Text.CompareTo("")==0)
                {
                    MessageBox.Show("te rog selecteaza o baza de date");
                    return;
                }
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "ALTER DATABASE " + comboDatabases.Text + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                sql += "Restore database " + comboDatabases.Text + " FROM DISK = '" + txtRestoreFileLoc.Text + "' WITH REPLACE;";
                command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                MessageBox.Show("baza de date restaurata cu succes");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connectionString = @"Data Source=LAPTOP-MJRNSH14\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
                conn = new SqlConnection(connectionString);
                conn.Open();
                sql = "select * from sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                comboDatabases.Items.Clear();

                while (reader.Read())
                {

                    comboDatabases.Items.Add(reader[0].ToString());
                }
                reader.Dispose();
                conn.Close();
                conn.Dispose();

                txtDataSource.Enabled = false;
                txtUserId.Enabled = false;
                txtPassword.Enabled = false;
                BtnConnect.Enabled = false;
                BtnDisconnect.Enabled = true;
                comboDatabases.Enabled = true;
                BtnBackup.Enabled = true;
                BtnRestore.Enabled = true;
                btnUseCred.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
