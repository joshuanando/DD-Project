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
    /// Interaction logic for Kategori_Tools_Insert.xaml
    /// </summary>
    public partial class Kategori_Tools_Insert : Window
    {
        DataTable dt;

        public Kategori_Tools_Insert()
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

        private void tools_cat_insert_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(nama_txt.Text))
            {
                MessageBox.Show("Nama tidak boleh kosong");
            }
            else
            {
                insert_category();
                load_tools_cat();
                MessageBox.Show("Insert Berhasil");
            }
        }

        private void insert_category()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "BEGIN Insert_Cat_Tools(:name); END;";
            cmd.Parameters.Add(":name", nama_txt.Text);
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }
    }
}
