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

namespace ProjectDD.Master.Kategori_Tools
{
    /// <summary>
    /// Interaction logic for Kategori_Tools_Delete.xaml
    /// </summary>
    public partial class Kategori_Tools_Delete : Window
    {
        DataTable dt;
        public Kategori_Tools_Delete()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            load_tools_cat();
        }

        private void load_tools_cat()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM TOOLS_CATEGORY ORDER BY ID_CATEGORY ASC";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            DG_Kat_tools.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void DG_Kat_tools_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow dr = dt.Rows[DG_Kat_tools.SelectedIndex];
            id_txt.Text = dr[0].ToString();
            nama_txt.Text = dr[1].ToString();
        }

        private void delete_tools_cat(string Id)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN Delete_Cat_Tools(:id); END;";
            cmd.Parameters.Add(":id", Id);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void tools_cat_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                delete_tools_cat(id_txt.Text);
                load_tools_cat();
                MessageBox.Show("Delete Sukses");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
