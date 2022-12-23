using System;
using System.Data;
using System.Data.OracleClient;
using System.Windows;
using System.Windows.Input;

namespace ProjectDD.Master.Kategori_Sparepart
{
    /// <summary>
    /// Interaction logic for Kategori_Sparepart_Delete.xaml
    /// </summary>
    public partial class Kategori_Sparepart_Delete : Window
    {
        DataTable dt;
        public Kategori_Sparepart_Delete()
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
            cmd.CommandText = "SELECT * FROM SPAREPART_CATEGORY ORDER BY ID_CATEGORY ASC";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            DG_Kat_spare.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void DG_Kat_spare_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow dr = dt.Rows[DG_Kat_spare.SelectedIndex];
            id_txt.Text = dr[0].ToString();
            nama_txt.Text = dr[1].ToString();
        }

        private void delete_spare_cat(string Id)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN Delete_Cat_Sparepart(:id); END;";
            cmd.Parameters.Add(":id", Id);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void spare_cat_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                delete_spare_cat(id_txt.Text);
                load_spare_cat();
                MessageBox.Show("Delete Sukses");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
