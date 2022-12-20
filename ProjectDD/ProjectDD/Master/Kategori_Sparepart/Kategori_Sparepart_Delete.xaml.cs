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
            cmd.CommandText = "SELECT * FROM SPAREPART_CATEGORY";
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
            cmd.CommandText = "DELETE FROM SPAREPART_CATEGORY WHERE ID_CATEGORY=:id";
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
