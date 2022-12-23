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

namespace ProjectDD.Master.Tools
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Window
    {
        DataTable dt;
        OracleConnection conn;

        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "dave", nama_db = "tools_cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "tools_cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "tools_cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "tools_cabjon"},
        };
        public Tools()
        {
            InitializeComponent();
            int giliran = 0;

            for (int i = 0; i < listcabang.Count; i++)
            {
                if (listcabang[i].nama_cabang == connection.cabangnow.Substring(3).ToLower())
                {
                    giliran = i;
                }
            }

            cbCabangTools.Items.Clear();
            cbCabangTools.ItemsSource = listcabang;
            cbCabangTools.DisplayMemberPath = "nama_cabang";
            cbCabangTools.SelectedValuePath = "nama_db";
            cbCabangTools.SelectedIndex = 0;
        }

        private void btnBackTools_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnInsertTools_Clicked(object sender, RoutedEventArgs e)
        {
            Master.Tools.insert_tools pInsertTools = new Master.Tools.insert_tools();
            pInsertTools.Show();
        }

        private void btnTampilTools_Clicked(object sender, RoutedEventArgs e)
        {
            loadTools();
        }

        private void loadTools()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM " + cbCabangTools.SelectedValue.ToString() + " Order by id_tools asc";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgTools.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void btnRefreshTools_Clicked(object sender, RoutedEventArgs e)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            //cmd.CommandText = "BEGIN dbms_mview.refresh('" + cabang_cb.SelectedValue.ToString() + "',method=>'C'); END;";
            cmd.CommandText = "BEGIN REFRESH('" + cbCabangTools.SelectedValue.ToString() + "'); END;";
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
