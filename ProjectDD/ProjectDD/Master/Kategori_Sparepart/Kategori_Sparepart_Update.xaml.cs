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

namespace ProjectDD.Master.Kategori_Sparepart
{
    /// <summary>
    /// Interaction logic for Kategori_Sparepart_Update.xaml
    /// </summary>
    public partial class Kategori_Sparepart_Update : Window
    {
        DataTable dt;

        public Kategori_Sparepart_Update()
        {
            InitializeComponent();
            init();
        }

        private void DG_Kat_spare_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow dr = dt.Rows[DG_Kat_spare.SelectedIndex];
            id_txt.Text = dr[0].ToString();
            nama_txt.Text = dr[1].ToString();
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

        private void update_spare_cat(string Id, string name)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN Update_Cat_Sparepart(:name,:id); END;";
            cmd.Parameters.Add(":id", Id);
            cmd.Parameters.Add(":name", name);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void spare_cat_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                update_spare_cat(id_txt.Text,nama_txt.Text);
                load_spare_cat();
                MessageBox.Show("Update Sukses");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
