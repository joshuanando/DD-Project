using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectDD
{
    /// <summary>
    /// Interaction logic for Kasir.xaml
    /// </summary>
    /// 
    public partial class Kasir : Window
    {
        OracleConnection conn;
        DataTable dt;
        bool isupdating = false;
        string[] listview = { "View Tools", "View Sparepart" };
        List<Item> listitem = new List<Item>();
        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "-", nama_db = ""},
            new db_cab() { nama_cabang = "cabdave", nama_db = ""},
            new db_cab() { nama_cabang = "cabbry", nama_db = ""},
            new db_cab() { nama_cabang = "cabnando", nama_db = ""},
            new db_cab() { nama_cabang = "cabjon", nama_db = ""},
        };
        DataTable dt_listbuyitem = new DataTable();
        List<String> deletedDtransId = new List<String>();

        string active_cab = "";
        private class DTrans
        {
            public String id_htrans { set; get; }
            public String id_item { set; get; }
            public String nama_item { set; get; }
            public long harga_item { set; get; }
            public long jumlah { set; get; }
            public DTrans(string id_htrans, string id_item, string nama_item, long harga_item, long jumlah)
            {
                this.id_htrans = id_htrans;
                this.id_item = id_item;
                this.nama_item = nama_item;
                this.harga_item = harga_item;
                this.jumlah = jumlah;
            }
        }
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
            connection.openConn();
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
            label_ID.Content = "";
            //settingDataGridListBuy()
            dt_listbuyitem.Clear();
            loadListBuyItems();
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
                int ctr = 0;
                foreach (DataGridRow r in rows)
                {
                    ctr++;
                    if (ctr==idx)
                    {
                        foreach (DataGridColumn column in dg_history.Columns)
                        {
                            if (column.GetCellContent(r) is TextBlock)
                            {
                                TextBlock cellContent = column.GetCellContent(r) as TextBlock;
                                rowdata.Add(cellContent.Text);
                            }
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
                tb_npwp.Text = rowdata[5];
                tb_no_polisi.Text = rowdata[6];
                active_cab = rowdata[10];
                lb_harga.Content = "Rp. " + rowdata[8];
                cb_trans_cab.SelectedValue = rowdata[9];
                cb_trans_cab.IsEnabled = false;
                chk_status.IsChecked = Convert.ToBoolean(Convert.ToInt32(rowdata[10]));
                loadListBuyItems();
                deletedDtransId.Clear();
            }
            // get Dtrans and fill the list
            // listbuyitem
        }

        private void clearALL()
        {
            isupdating = false;
            label_action.Content = "Add";
            label_ID.Content = "";
            tb_alamat.Text = "";
            tb_deskripsi.Text = "";
            tb_nama.Text = "";
            tb_no_ktp.Text = "";
            tb_no_polisi.Text = "";
            tb_npwp.Text = "";
            chk_status.IsChecked = false;
            cb_trans_cab.IsEnabled = true;
            active_cab = "";
            cb_trans_cab.SelectedIndex = 0;
            listitem = new List<Item>();
            dt_listbuyitem.Clear();
            updateTotal();
            deletedDtransId.Clear();
        }
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            clearALL();
        }

        private void btn_additem_Click(object sender, RoutedEventArgs e)
        {
            connection.openConn();
            //MessageBox.Show(dt_listbuyitem.Rows.Count+"");
            // checking
            int qty = Convert.ToInt32(tb_qty.Text);
            String id_htrans = label_ID.Content.ToString();

            if (qty<1)
            {
                MessageBox.Show("Jumlah minimal 1");
                return;
            }
            if (dt_listbuyitem.Rows.Count==0) // jika masih kosong tambah
            {
                active_cab = cb_trans_cab.SelectedValue as String;
                cb_trans_cab.IsEnabled = false;
            }
            else if(active_cab != cb_trans_cab.SelectedValue as String) // jika salah/ cabang tidak sama
            {
                MessageBox.Show("Tidak bisa menambah pesanan yang bersangkutan dengan cabang/sumber berbeda");
                return;
            }
            // add item to list items
            // check qty first if adding new transaction
            // get item details
            try
            {
                String id_item = cb_item.SelectedValue.ToString();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = $"SELECT status FROM admin.ITEMS_"+cb_trans_cab.SelectedValue+ $" where id='{id_item}'";
                String status = cmd.ExecuteScalar() as String;
                if (status == "Not Available")
                {
                    MessageBox.Show("Item/Tools sedang tidak dapat digunakan");
                    return;
                }else if (status == "Available") { /*allow*/ }
                else if (Convert.ToInt32(status.Substring(1)) < qty)
                {
                    MessageBox.Show("Stok item < jumlah permintaan");
                    return;
                }
                cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = $"SELECT nama FROM admin.ITEMS_" + cb_trans_cab.SelectedValue + $" where id='{id_item}'";
                String nama = cmd.ExecuteScalar() as String;
                cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = $"SELECT harga FROM admin.ITEMS_" + cb_trans_cab.SelectedValue + $" where id='{id_item}'";
                long harga = Convert.ToInt64(cmd.ExecuteScalar());
                if (!isupdating)
                {
                    id_htrans = getNextHtrans();
                }
                if (cb_item.SelectedValue != null)
                {
                    //DTrans item = new DTrans(id_htrans, id_item, nama, harga, qty);
                    DataRow dr = dt_listbuyitem.NewRow();
                    dr[0] = "";
                    dr[1] = id_htrans;
                    dr[2] = id_item;
                    dr[3] = nama;
                    dr[4] = harga;
                    dr[5] = qty;
                    dt_listbuyitem.Rows.Add(dr);
                    dg_listbuy.ItemsSource = dt_listbuyitem.DefaultView;
                    updateTotal();
                    settingDataGridListBuy();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // untuk LISTBUYITEM
        OracleDataAdapter da_listitem;
        OracleCommandBuilder builder;
        private void loadListBuyItems()
        {
            String idHtrans = label_ID.Content.ToString();
            if (idHtrans.Length>0)
            {
                //load listbuyitem to dg_listbuy
                da_listitem = new OracleDataAdapter($"select * from admin.dtrans where id_htrans='{idHtrans}'", conn);
                builder = new OracleCommandBuilder(da_listitem);
                dt_listbuyitem = new DataTable();
                da_listitem.Fill(dt_listbuyitem);
                dg_listbuy.ItemsSource = dt_listbuyitem.DefaultView;
            }
            else
            {
                dg_listbuy.ItemsSource = dt_listbuyitem.DefaultView;
            }
            settingDataGridListBuy();
            updateTotal();
        }

        private long updateTotal()
        {
            long sum = 0;
            foreach (DataRowView row in dg_listbuy.ItemsSource)
            {
                sum += Convert.ToInt64(row["HARGA_ITEM"]);
            }
            lb_harga.Content = "Rp." + sum;
            return sum;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (dt_listbuyitem.Rows.Count==0)
            {
                MessageBox.Show("Harus ada item yang ditambahkan");
                return;
            }
            connection.openConn();
            // submit current, add or update
            //drop deletedDtransId
            if (isupdating)
            {
                using (OracleTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = conn;
                        /*cmd.CommandText = "update dtrans set id_item=:a,nama_item=:b,harga_item=:c,jumlah=:d, where id:=e";
                        cmd.Parameters.Add(":a", id_item);
                        cmd.Parameters.Add(":b", nama_item);
                        cmd.Parameters.Add(":c", harga_item);
                        cmd.Parameters.Add(":d", jumlah);
                        cmd.Parameters.Add(":e", id_dtrans);
                        cmd.ExecuteNonQuery();
                        da_listitem.Update(dt);*/
                        trans.Commit();
                        MessageBox.Show("Berhasil Update");
                        deletedDtransId.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        trans.Rollback();
                    }
                }
            }
            else {
                //htrans
                using (OracleTransaction trans = connection.conn.BeginTransaction())
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection.conn;
                        cmd.CommandText = "INSERT INTO ADMIN.HTRANS(ID_Transaksi,Tanggal,NAMA_PEMILIK,ALAMAT_PEMILIK,NO_KTP,NPWP,NO_POLISI,DESKRIPSI_KENDARAAN,TOTAL,DARI_CABANG,STATUS) VALUES('', CURRENT_DATE, :pemilik, :alamat, :noktp, :npwp, :nopolisi, :desk, :total, :cab, :status)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(":pemilik", tb_nama.Text);
                        cmd.Parameters.Add(":alamat", tb_alamat.Text);
                        cmd.Parameters.Add(":noktp", tb_no_ktp.Text);
                        cmd.Parameters.Add(":npwp", tb_npwp.Text);
                        cmd.Parameters.Add(":nopolisi", tb_no_polisi.Text);
                        cmd.Parameters.Add(":desk", tb_deskripsi.Text);
                        cmd.Parameters.Add(":total", updateTotal());
                        cmd.Parameters.Add(":cab", cb_trans_cab.SelectedValue as String);
                        cmd.Parameters.Add(":status", chk_status.IsChecked == true ? 1 : 0);
                        cmd.Transaction = trans;
                        cmd.ExecuteNonQuery(); ///
                        //MessageBox.Show(cmd.CommandText);
                        trans.Commit();
                        MessageBox.Show("Berhasil");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        trans.Rollback();
                    }
                }
                //dtrans
                foreach (DataRow r in dt_listbuyitem.Rows)
                {
                    using (OracleTransaction trans = connection.conn.BeginTransaction())
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection.conn;
                            cmd.CommandText = "INSERT INTO ADMIN.DTRANS(ID_HTRANS,ID_ITEM,NAMA_ITEM,HARGA_ITEM,JUMLAH) " +
                                "VALUES(:id_htrans, :id_item, :nama, :harga, :jumlah)";
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(":id_htrans", r[1].ToString());
                            cmd.Parameters.Add(":id_item", r[2].ToString());
                            cmd.Parameters.Add(":nama", r[3].ToString());
                            cmd.Parameters.Add(":harga", r[4].ToString());
                            cmd.Parameters.Add(":jumlah", r[5].ToString());
                            cmd.Transaction = trans;
                            cmd.ExecuteNonQuery(); ///
                            //MessageBox.Show(cmd.CommandText);
                            //da.Update(dt);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            trans.Rollback();
                        }
                    }
                }
                //update stok 
                if (chk_status.IsChecked == true)
                {
                    foreach (DataRow r in dt_listbuyitem.Rows)
                    {
                        bool valid = true;
                        List<int> liststok = new List<int>();

                        // get status
                        String id_htrans = label_ID.Content.ToString();
                        try
                        {;
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection.conn;
                            cmd.CommandText = $"SELECT status FROM admin.ITEMS where id='{r[2].ToString()}'";
                            String status = cmd.ExecuteScalar() as String;
                            if (status == "Not Available")
                            {
                                MessageBox.Show("Item/Tools sedang tidak dapat digunakan");
                            }
                            else if (status == "Available") { 
                                // if TOOLS / status not changed (change in admin)
                            }
                            else if (Convert.ToInt32(status.Substring(1)) < Convert.ToInt32(r[5].ToString()))
                            {
                                MessageBox.Show("Stok item < jumlah permintaan untuk "+ r[3].ToString()); 
                            }
                            else // IF SPAREPART
                            {
                                int stok = Convert.ToInt32(status.Substring(1));
                                int newstok = stok - Convert.ToInt32(r[5].ToString());
                                
                                using (OracleTransaction trans = connection.conn.BeginTransaction())
                                {
                                    try
                                    {
                                        cmd = new OracleCommand();
                                        cmd.Connection = connection.conn;
                                        cmd.CommandText = "UPDATE ADMIN.SPAREPART SET STOK=:newstok WHERE ID_SPARE=:id_spare";
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add(":newstok", newstok);
                                        cmd.Parameters.Add(":id_spare", r[2].ToString());
                                        cmd.Transaction = trans;
                                        cmd.ExecuteNonQuery(); ///
                                        //MessageBox.Show(cmd.CommandText);
                                        //da.Update(dt);
                                        trans.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        trans.Rollback();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                clearALL();
                load_history();
            }
            loadcb(cb_trans_cab.SelectedValue.ToString());
        }

        private void cb_trans_cab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedcabangtrans = cb_trans_cab.SelectedValue.ToString();
            loadcb(selectedcabangtrans);
        }
        private void loadcb(string cabang)
        {
            listitem.Clear();
            connection.openConn();
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
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String id = reader["ID"].ToString();
                    String nama = reader["NAMA"].ToString();
                    String kategori = reader["KATEGORI"].ToString();
                    String status = reader["STATUS"].ToString();
                    listitem.Add(new Item(id, $"{nama} - {status} - {kategori}") { status = status });
                }
                cb_item.ItemsSource = listitem;
                cb_item.DisplayMemberPath = "nama";
                cb_item.SelectedValuePath = "id";
                cb_item.SelectedItem = cb_item.Items[0];
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message); 
                cb_trans_cab.SelectedItem = cb_item.Items[0];
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

        public string getNextHtrans()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "select admin.nexthtransid from dual";
            return cmd.ExecuteScalar().ToString();
        }

        public void settingDataGridListBuy()
        {
            if (dt_listbuyitem.Columns.Count==0)
            {
                dt_listbuyitem.Columns.Add(new DataColumn());
                dt_listbuyitem.Columns.Add(new DataColumn());
                dt_listbuyitem.Columns.Add(new DataColumn());
                dt_listbuyitem.Columns.Add(new DataColumn());
                dt_listbuyitem.Columns.Add(new DataColumn());
                dt_listbuyitem.Columns.Add(new DataColumn());
            }
            dt_listbuyitem.Columns[0].ColumnName = "ID";
            dt_listbuyitem.Columns[0].ReadOnly = true;
            dt_listbuyitem.Columns[0].ColumnMapping=MappingType.Hidden;
            dt_listbuyitem.Columns[1].ColumnName = "ID_HTRANS";
            dt_listbuyitem.Columns[1].ReadOnly = true;
            dt_listbuyitem.Columns[1].ColumnMapping = MappingType.Hidden;
            dt_listbuyitem.Columns[2].ColumnName = "ID_ITEM";
            dt_listbuyitem.Columns[2].ReadOnly = true;
            dt_listbuyitem.Columns[3].ColumnName = "NAMA_ITEM";
            dt_listbuyitem.Columns[3].ReadOnly = true;
            dt_listbuyitem.Columns[4].ColumnName = "HARGA_ITEM";
            dt_listbuyitem.Columns[4].ReadOnly = true;
            dt_listbuyitem.Columns[5].ColumnName = "JUMLAH";
            dt_listbuyitem.Columns[4].ReadOnly = false;
            //dt_listbuyitem.AcceptChanges();
        }

        private void dg_listbuy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is DataGridCell)
                {
                    var grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        var rowview = grid.SelectedItem as DataRowView;
                        if (rowview != null)
                        {
                            DataRow row = rowview.Row;
                            deletedDtransId.Add(row[0] as String);
                            MessageBox.Show("deleted dtrans id array length" + deletedDtransId.Count);
                            dt_listbuyitem.Rows.Remove(row);
                            updateTotal();
                        }
                    }
                }
            }
        }
    }
}
