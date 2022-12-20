using Oracle.DataAccess.Client;
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

namespace ProjectDD.Master.Kategori_Sparepart
{
    /// <summary>
    /// Interaction logic for Kategori_Sparepart_Insert.xaml
    /// </summary>
    public partial class Kategori_Sparepart_Insert : Window
    {
        DataTable dt;
        public Kategori_Sparepart_Insert()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            load_spare_cat();
        }

        private void load_spare_cat()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM SPAREPART_CATEGORY";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            DG_Kat_spare.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void spare_cat_insert_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(nama_txt.Text))
            {
                MessageBox.Show("Nama tidak boleh kosong");
            }
            else
            {
                insert_category();
                load_spare_cat();
                MessageBox.Show("Insert Berhasil");
            }
        }

        private void insert_category()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "INSERT INTO SPAREPART_CATEGORY VALUES ('',:nama)";
            cmd.Parameters.Add(":nama", nama_txt.Text);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
