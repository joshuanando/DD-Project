using System;
using System.Data;
using System.Data.OracleClient;
using System.Windows;

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
            cmd.CommandText = "BEGIN Insert_Cat_Sparepart(:name); END;";
            cmd.Parameters.Add(":name", nama_txt.Text);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
