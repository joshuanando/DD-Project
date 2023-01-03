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

namespace ProjectDD.Master.Jasa
{
    /// <summary>
    /// Interaction logic for MasterJasa.xaml
    /// </summary>
    public partial class MasterJasa : Window
    {
        public class status
        {
            public string nama { get; set; }
            public int value { get; set; }
        }

        string[] action = { "insert", "update", "delete" };
        string id = "";

        List<status> liststatus = new List<status>()
        {
            new status() {nama = "available", value = 1},
            new status() {nama = "unavailable", value = 0}
        };

        DataTable dt;

        public MasterJasa()
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
            loadjasa();
            status_cb.ItemsSource = liststatus;
            status_cb.DisplayMemberPath = "nama";
            status_cb.SelectedValuePath = "value";
            status_cb.SelectedItem = status_cb.Items[0];
        }

        private void loadjasa()
        {
            try
            {
                connection.openConn();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = "SELECT * from jasa order by id_jasa desc";
                //MessageBox.Show(cmd.CommandText);
                dt = new DataTable();
                cmd.ExecuteNonQuery();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                DG_jasa.ItemsSource = dt.DefaultView;
                connection.closeConn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DG_jasa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (cb_action.SelectedItem.ToString().ToLower() != "insert")
            {
                DataRow dr = dt.Rows[DG_jasa.SelectedIndex];
                id = dr[0].ToString();
                nama_txtbox.Text = dr[1].ToString();
                Harga_txtbox.Text = dr[2].ToString();
                for (int i = 0; i < liststatus.Count; i++)
                {
                    if (dr[3].ToString() == liststatus[i].value.ToString())
                    {
                        status_cb.SelectedItem = status_cb.Items[i];
                    }
                }
            }
        }

        private void cb_action_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refresh();
            string choice = cb_action.SelectedItem.ToString();
            action_btn.Content = choice;

            if (choice.ToLower() == "delete")
            {
                nama_txtbox.IsEnabled = false;
                Harga_txtbox.IsEnabled = false;
                status_cb.IsEnabled = false;
            }
        }

        private void action_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (cb_action.SelectedItem.ToString().ToLower())
                {
                    case "insert":
                        if (nama_txtbox.Text.Trim().Length > 0 && Harga_txtbox.Text.Trim().Length > 0)
                        {
                            if (Convert.ToInt64(Harga_txtbox.Text) >= 0)
                            {
                                insert_jasa(nama_txtbox.Text, Harga_txtbox.Text);
                                MessageBox.Show("berhasil insert jasa");
                            }
                            else
                            {
                                MessageBox.Show("harga tidak boleh kurang dari 0");
                            }
                        }
                        else
                        {
                            MessageBox.Show("nama dan harga tidak boleh kosong");
                        }
                        break;
                    case "update":
                        if (nama_txtbox.Text.Trim().Length > 0 && Harga_txtbox.Text.Trim().Length > 0)
                        {
                            if (Convert.ToInt64(Harga_txtbox.Text) >= 0)
                            {
                                update_jasa(nama_txtbox.Text, Harga_txtbox.Text);
                                MessageBox.Show("Update jasa berhasil");
                            }
                            else
                            {
                                MessageBox.Show("harga tidak boleh kurang dari 0");
                            }
                        }
                        else
                        {
                            MessageBox.Show("nama dan harga tidak boleh kosong");
                        }
                        break;
                    case "delete":
                        if (id.Trim().Length > 0)
                        {
                            delete_jasa();
                            MessageBox.Show("kasir berhasil terhapus");
                        }
                        else
                        {
                            MessageBox.Show("pilih jasa terlebih dahulu");
                        }
                        break;
                    default:
                        break;
                }
                loadjasa();
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void insert_jasa(string nama, string harga)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "INSERT INTO JASA VALUES('',:nama,:harga,:status)";
            cmd.Parameters.Add(":nama", nama);
            cmd.Parameters.Add(":harga", Convert.ToInt64(harga));
            cmd.Parameters.Add(":status", Convert.ToInt64(status_cb.SelectedValue.ToString()));
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void update_jasa(string nama, string harga)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "UPDATE JASA SET NAMA = :nama, HARGA = :harga, STATUS = :status where ID_JASA = :idjasa";
            cmd.Parameters.Add(":nama", nama);
            cmd.Parameters.Add(":harga", Convert.ToInt64(harga));
            cmd.Parameters.Add(":status", Convert.ToInt64(status_cb.SelectedValue.ToString()));
            cmd.Parameters.Add(":idjasa", id);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void delete_jasa()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "DELETE FROM JASA WHERE ID_JASA = :idjasa";
            cmd.Parameters.Add(":idjasa", id);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void refresh()
        {
            nama_txtbox.IsEnabled = true;
            Harga_txtbox.IsEnabled = true;
            status_cb.IsEnabled = true;
            nama_txtbox.Text = "";
            Harga_txtbox.Text = "";
            id = "";
        }
    }
}
