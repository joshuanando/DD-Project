using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Windows;

namespace ProjectDD.Master
{
    /// <summary>
    /// Interaction logic for view_Spareparts.xaml
    /// </summary>
    public partial class view_Spareparts : Window
    {

        DataTable dt;

        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "local", nama_db = "LOCAL_SPAREPART"},
            new db_cab() { nama_cabang = "dave", nama_db = "LOCAL_SPAREPART@cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "LOCAL_SPAREPART@cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "LOCAL_SPAREPART@cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "LOCAL_SPAREPART@cabjon"},
        };
        public view_Spareparts()
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
                load_sparepart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void load_sparepart()
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
            spareparts_DG.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh_view_tools();
                load_sparepart();
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
            MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
