using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
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

namespace ProjectDD
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        OracleConnection conn;
        public Admin(OracleConnection c)
        {
            InitializeComponent();
            conn = c;
            init();
        }

        string[] listview = { "View Tools", "View Sparepart" };

        private void init()
        {
            showcabang();
            for (int i = 0; i < listview.Length; i++)
            {
                view_cb.Items.Add(listview[i]);
            }
            view_cb.SelectedItem = view_cb.Items[0];
        }

        private void showcabang()
        {
            label_cabang.Content = "Welcome Admin of " + connection.cabangnow;
        }

        private void loadItems()
        {
            //cbParam.Items.Add("Everyone");
            OracleCommand cmd = new OracleCommand("select * from tools", conn);
            OracleDataReader reader;
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //cbParam.Items.Add(reader.GetString(0));
            }
            reader.Close();
            conn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(view_cb.SelectedItem.ToString());
            switch (view_cb.SelectedItem.ToString())
            {
                case "View Tools":
                    Master.view_Tools vt = new Master.view_Tools();
                    vt.Show();
                    break;
                case "View Sparepart":
                    Master.view_Spareparts vs = new Master.view_Spareparts();
                    vs.Show();
                    break;
                default:
                    break;
            }
        }

        private void btnCheck_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnHistory_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnProduct_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnStok_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnKategori_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
