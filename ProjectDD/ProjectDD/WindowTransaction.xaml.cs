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
    /// Interaction logic for WindowTransaction.xaml
    /// </summary>
    public partial class WindowTransaction : Window
    {
        OracleConnection conn;
        public WindowTransaction(OracleConnection c)
        {
            InitializeComponent();
            conn = c;
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
            Master.view_Tools vt = new Master.view_Tools();
            vt.Show();
        }
    }
}
