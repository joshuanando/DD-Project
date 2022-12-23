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

namespace ProjectDD.Master.History
{
    /// <summary>
    /// Interaction logic for history.xaml
    /// </summary>
    public partial class history : Window
    {
        string[] listTampil = { "Stok", "Transaksi" };
        DataTable dt;
        public history()
        {
            InitializeComponent();
            for (int i = 0; i < listTampil.Length; i++)
            {
                cbHistory.Items.Add(listTampil[i]);
            }
            cbHistory.SelectedIndex = 0;
        }
        
        private void btnBackHistory_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnTampilkanHistory_Clicked(object sender, RoutedEventArgs e)
        {
            if (cbHistory.Text.ToLower().Equals("stok"))
            {
                loadHistoryStok();
            }
            else
            {
                loadHistoryHtrans();
            }
        }

        private void loadHistoryStok()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM history_stok";
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgHistory.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void loadHistoryHtrans()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM htrans";
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgHistory.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void dgHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataGridRow row in dgHistory.SelectedItems)
            {
                
            }
        }
    }
}
