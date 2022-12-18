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

namespace ProjectDD
{
    /// <summary>
    /// Interaction logic for Kasir.xaml
    /// </summary>
    public partial class Kasir : Window
    {
        OracleConnection conn;
        DataTable dt;

        public Kasir(OracleConnection c)
        {
            InitializeComponent();        
            conn = c;
            date_start.SelectedDate = DateTime.ParseExact("2022-01-01", "yyyy-MM-dd",
                                           System.Globalization.CultureInfo.CurrentCulture);
            date_end.SelectedDate = DateTime.Now;
            init();
        }

        string[] listview = { "View Tools", "View Sparepart" };

        private void init()
        {
            showcabang();
            load_history();
            for (int i = 0; i < listview.Length; i++)
            {
                view_cb.Items.Add(listview[i]);
            }
            view_cb.SelectedItem = view_cb.Items[0];
        }

        private void showcabang()
        {
            label_cabang.Content = "Welcome Kasir of " + connection.cabangnow;
        }

        private void loadcb()
        {
            //cbParam.Items.Add("Everyone");
            OracleCommand cmd = new OracleCommand("select * from items", conn);
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

        private void load_history()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM admin.htrans";
            string rules = "";
            //FILTER DATE 
            DateTime dtstart = date_start.DisplayDate;
            DateTime dtend = date_end.DisplayDate;
            if (date_start.SelectedDate != null && date_end.SelectedDate != null)
            {
                string tgl1 = date_start.SelectedDate.ToString().Split(' ')[0];
                string tgl2 = date_end.SelectedDate.ToString().Split(' ')[0];
                rules += $"to_date(to_char(tanggal,'dd/mm/yyyy'),'dd/mm/yyyy') <= to_date('{tgl2}','dd/mm/yyyy') and " +
                         $"to_date(to_char(tanggal,'dd/mm/yyyy'),'dd/mm/yyyy') >=to_date('{tgl1}','dd/mm/yyyy')";
            }
            //FILTER STATUS
            string status = cb_status.SelectionBoxItem.ToString();
            if (rules.Length > 0 && status.ToUpper()!="ALL") rules += " and";
            if (status.ToUpper() == "UNFINISHED") rules += " status=0";            
            else if (status.ToUpper() == "FINISHED") rules += " status=1";
            if (rules.Length > 0)
            {
                cmd.CommandText += " where "+rules+" order by tanggal desc";
            }
            //MessageBox.Show(cmd.CommandText);
            try
            {
                dt = new DataTable();
                cmd.ExecuteNonQuery();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                dg_history.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            connection.closeConn();
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            load_history();
        }
    }
}
