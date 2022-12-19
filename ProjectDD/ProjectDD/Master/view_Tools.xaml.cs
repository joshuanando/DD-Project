using System;
using System.Collections.Generic;
using System.Data;
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
using Oracle.DataAccess.Client;

namespace ProjectDD.Master
{
    /// <summary>
    /// Interaction logic for view_Tools.xaml
    /// </summary>
    public partial class view_Tools : Window
    {

        DataTable dt;

        List<db_cab> listcabang = new List<db_cab>() 
        {
            new db_cab() { nama_cabang = "local", nama_db = "LOCAL_TOOLS"},
            new db_cab() { nama_cabang = "dave", nama_db = "LOCAL_TOOLS@cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "LOCAL_TOOLS@cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "LOCAL_TOOLS@cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "LOCAL_TOOLS@cabjon"},
        };

        public view_Tools()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            listcabang.RemoveAll(x => x.nama_cabang == connection.cabangnow.Substring(3).ToLower());
            cabang_cb.Items.Clear();
            cabang_cb.ItemsSource = listcabang;
            cabang_cb.DisplayMemberPath = "nama_cabang";
            cabang_cb.SelectedValuePath = "nama_db";
            cabang_cb.SelectedItem = cabang_cb.Items[0];
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                load_tools();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void load_tools()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM " + cabang_cb.SelectedValue.ToString();
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            Tools_DG.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh_view_tools();
                load_tools();
                MessageBox.Show("Database berhasil diperbarui");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void refresh_view_tools()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN dbms_mview.refresh('" + cabang_cb.SelectedValue.ToString() + "',method=>'C'); END;";
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
