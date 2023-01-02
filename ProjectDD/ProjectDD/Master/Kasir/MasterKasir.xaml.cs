using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectDD.Master.Kasir
{
    /// <summary>
    /// Interaction logic for MasterKasir.xaml
    /// </summary>
    public partial class MasterKasir : Window
    {

        string[] action = { "insert", "update", "delete" };
        DataTable dt;

        public MasterKasir()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            for (int i = 0; i < action.Length; i++)
            {
                cb_action.Items.Add(action[i]);
            }
            cb_action.SelectedItem = cb_action.Items[0];
            loadkasir();
        }

        private void loadkasir()
        {
            try
            {
                connection.openConn();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = "SELECT GRANTEE AS USERNAME from dba_role_privs where granted_role='KASIR' AND GRANTEE != 'ADMIN'";
                //MessageBox.Show(cmd.CommandText);
                dt = new DataTable();
                cmd.ExecuteNonQuery();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                DG_kasir.ItemsSource = dt.DefaultView;
                connection.closeConn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cb_action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
            string choice = cb_action.SelectedItem.ToString();
            action_btn.Content = choice;

            if (choice.ToLower() == "update" || choice.ToLower() == "delete")
            {
                username_txtbox.IsEnabled = false;
                if (choice.ToLower() == "delete")
                {
                    password_label.Visibility = Visibility.Hidden;
                    password_txtbox.Visibility = Visibility.Hidden;
                }
            }
        }

        private void refresh()
        {
            username_txtbox.IsEnabled = true;
            password_label.Visibility = Visibility.Visible;
            password_txtbox.Visibility = Visibility.Visible;
            username_txtbox.Text = "";
        }

        private void DG_kasir_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (cb_action.SelectedItem.ToString().ToLower() != "insert")
            {
                DataRow dr = dt.Rows[DG_kasir.SelectedIndex];
                username_txtbox.Text = dr[0].ToString();
            }
        }

        private void add_kasir(string username, string password)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN CREATE_PEGAWAI(:username,:password); END;";
            cmd.Parameters.Add(":username", username);
            cmd.Parameters.Add(":password", password);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void update_kasir(string username, string password)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN CHANGE_PASS_PEGAWAI(:username,:password); END;";
            cmd.Parameters.Add(":username", username);
            cmd.Parameters.Add(":password", password);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void delete_kasir(string username)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN DELETE_PEGAWAI(:username); END;";
            cmd.Parameters.Add(":username", username);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void action_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (cb_action.SelectedItem.ToString().ToLower())
                {
                    case "insert":
                        int check = Checkganda(username_txtbox.Text);
                        if (username_txtbox.Text.Trim().Length > 0 && password_txtbox.Text.Trim().Length > 0)
                        {
                            if (check == 0 && username_txtbox.Text.ToLower() != "admin")
                            {
                                add_kasir(username_txtbox.Text, password_txtbox.Text);
                                MessageBox.Show("Create kasir berhasil");
                            }
                            else
                            {
                                MessageBox.Show("Username sudah terdaftar / tidak boleh username admin");
                            }
                        }
                        else
                        {
                            MessageBox.Show("username atau password tidak boleh kosong");
                        }
                        break;
                    case "update":
                        if (username_txtbox.Text.Trim().Length > 0 && password_txtbox.Text.Trim().Length > 0)
                        {
                            update_kasir(username_txtbox.Text, password_txtbox.Text);
                            MessageBox.Show("Update password kasir berhasil");
                        }
                        else
                        {
                            MessageBox.Show("username atau password tidak boleh kosong");
                        }
                        break;
                    case "delete":
                        if (username_txtbox.Text.Trim().Length > 0)
                        {
                            delete_kasir(username_txtbox.Text);
                            MessageBox.Show("kasir berhasil terhapus");
                        }
                        else
                        {
                            MessageBox.Show("username tidak boleh kosong");
                        }
                        break;
                    default:
                        break;
                }
                loadkasir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int Checkganda(string username)
        {
            bool a = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() == username)
                {
                    a = true;
                }
            }
            if (a)
            {
                return 1;
            }
            return 0;
        }
    }
}
