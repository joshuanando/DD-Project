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
        bool isupdating = false;
        string[] listview = { "View Tools", "View Sparepart" };
        List<Item> listitem = new List<Item>();
        List<Item> listbuyitem = new List<Item>();


        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "-", nama_db = ""},
            new db_cab() { nama_cabang = "cabdave", nama_db = ""},
            new db_cab() { nama_cabang = "cabbry", nama_db = ""},
            new db_cab() { nama_cabang = "cabnando", nama_db = ""},
            new db_cab() { nama_cabang = "cabjon", nama_db = ""},
        };

        public Kasir(OracleConnection c)
        {
            InitializeComponent();
            conn = c;
            date_start.SelectedDate = DateTime.ParseExact("2022-01-01", "yyyy-MM-dd",
                                           System.Globalization.CultureInfo.CurrentCulture);
            date_end.SelectedDate = DateTime.Now;
            init();
        }

        private void init()
        {
            load_history();
            label_cabang.Content = "Welcome Kasir of " + connection.cabangnow;
            for (int i = 0; i < listview.Length; i++)
            {
                view_cb.Items.Add(listview[i]);
            }
            view_cb.SelectedItem = view_cb.Items[0];
            listcabang.RemoveAll(x => x.nama_cabang.ToUpper() == connection.cabangnow.ToUpper());
            cb_trans_cab.Items.Clear();
            cb_trans_cab.ItemsSource = listcabang;
            cb_trans_cab.DisplayMemberPath = "nama_cabang";
            cb_trans_cab.SelectedValuePath = "nama_cabang";
            cb_trans_cab.SelectedItem = cb_trans_cab.Items[0];
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
            if (rules.Length > 0 && status.ToUpper() != "ALL") rules += " and";
            if (status.ToUpper() == "UNFINISHED") rules += " status=0";
            else if (status.ToUpper() == "FINISHED") rules += " status=1";
            if (rules.Length > 0)
            {
                cmd.CommandText += " where " + rules + " order by tanggal desc";
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //refresh history
            load_history();
        }

        private void dg_history_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int idx = dg_history.SelectedIndex + 1;
            List<String> rowdata = new List<String>();
            if (idx > 0)
            {
                var rows = GetDataGridRows(dg_history);
                foreach (DataGridRow r in rows)
                {
                    //drv = (DataRowView)r.Item; 
                    foreach (DataGridColumn column in dg_history.Columns)
                    {
                        if (column.GetCellContent(r) is TextBlock)
                        {
                            TextBlock cellContent = column.GetCellContent(r) as TextBlock;
                            rowdata.Add(cellContent.Text);
                        }
                    }
                }
                //update
                isupdating = true;
                label_action.Content = "Update";
                label_ID.Content = rowdata[0];
                tb_alamat.Text = rowdata[3];
                tb_deskripsi.Text = rowdata[7];
                tb_nama.Text = rowdata[2];
                tb_no_ktp.Text = rowdata[4];
                tb_no_polisi.Text = rowdata[6];
                //cb_trans_cab.SelectedItem = ;
                chk_status.IsChecked = Convert.ToBoolean(Convert.ToInt32(rowdata[9]));
            }
            // get Dtrans and fill the list
            // listbuyitem
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            isupdating = false;
            label_action.Content = "Add";
            label_ID.Content = "";
            tb_alamat.Text = "";
            tb_deskripsi.Text = "";
            tb_nama.Text = "";
            tb_no_ktp.Text = "";
            tb_no_polisi.Text = "";
            chk_status.IsChecked = false;
            listitem = new List<Item>();
        }

        private void btn_additem_Click(object sender, RoutedEventArgs e)
        {
            // add item to list items
            // check qty first if adding new transaction

            if (cb_item.SelectedValue != null)
            {
                Item i = listitem[cb_item.SelectedIndex];
                i.status = "@" + tb_qty.Text;
                listbuyitem.Add(i);
            }
            updateListBuyItems();
        }
        private void updateListBuyItems()
        {
            //load listbuyitem to dg_listbuy
            dg_listbuy.ItemsSource = listbuyitem;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            // submit current, add or update
            if (isupdating)
            {

            }
            else {

            }
        }

        private void cb_trans_cab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedcabangtrans = cb_trans_cab.SelectedValue.ToString();
            loadcb(selectedcabangtrans);
        }
        private void loadcb(string cabang)
        {
            //cbParam.Items.Add("Everyone");
            String query = "select * from admin.items";
            if (cabang!="-")
            {
                query += $"@{cabang}";
            }
            try
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader;
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String id = reader["ID"].ToString();
                    String nama = reader["NAMA"].ToString();
                    String kategori = reader["KATEGORI"].ToString();
                    String status = reader["STATUS"].ToString();
                    listitem.Add(new Item(id, $"{nama} - {status} - {kategori}") { status = status });
                }
                cb_item.Items.Clear();
                cb_item.ItemsSource = listitem;
                cb_item.DisplayMemberPath = "nama";
                cb_item.SelectedValuePath = "id";
                cb_item.SelectedItem = cb_item.Items[0];
                reader.Close();
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }            
        }
        private void ComboBox_SelectionUpdate(object sender, KeyEventArgs e)
        {
            var cmb = (ComboBox)sender;
            cmb.IsDropDownOpen = true;
            var textbox = cmb.Template.FindName("PART_EditableTextBox", cmb) as TextBox;
            cmb.ItemsSource = listitem.Where(p => string.IsNullOrEmpty(cmb.Text) || p.nama.ToLower().Contains(textbox.Text.ToLower())).ToList();
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as System.Collections.IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}
