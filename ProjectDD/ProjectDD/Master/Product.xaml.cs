using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using WPFCustomMessageBox;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace ProjectDD.Master
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        DataTable dt;
        OracleConnection conn;
        List<Tools_Category> listkat = new List<Tools_Category>();

        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "dave", nama_db = "sparepart_cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "sparepart_cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "sparepart_cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "sparepart_cabjon"},
        };

        public void hideUpdate(bool flag){
            if (flag)
            {
                gbUpSpare.Visibility = Visibility.Hidden;
                lblUpID.Visibility = Visibility.Hidden;
                txtUpIdSpare.Visibility = Visibility.Hidden;
                lblUpNama.Visibility = Visibility.Hidden;
                txtUpNama.Visibility = Visibility.Hidden;
                lblUpCat.Visibility = Visibility.Hidden;
                cbCategorySpare.Visibility = Visibility.Hidden;
                lblUpStok.Visibility = Visibility.Hidden;
                txtUpStok.Visibility = Visibility.Hidden;
                lblUpHarga.Visibility = Visibility.Hidden;
                txtUpHarga.Visibility = Visibility.Hidden;
                btnUpSparepart.Visibility = Visibility.Hidden;
                lblUpDesc.Visibility = Visibility.Hidden;
                rtbUpDesc.Visibility = Visibility.Hidden;
            }
            else
            {
                gbUpSpare.Visibility = Visibility.Visible;
                lblUpID.Visibility = Visibility.Visible;
                txtUpIdSpare.Visibility = Visibility.Visible;
                lblUpNama.Visibility = Visibility.Visible;
                txtUpNama.Visibility = Visibility.Visible;
                lblUpCat.Visibility = Visibility.Visible;
                cbCategorySpare.Visibility = Visibility.Visible;
                lblUpStok.Visibility = Visibility.Visible;
                txtUpStok.Visibility = Visibility.Visible;
                lblUpHarga.Visibility = Visibility.Visible;
                txtUpHarga.Visibility = Visibility.Visible;
                btnUpSparepart.Visibility = Visibility.Visible;
                lblUpDesc.Visibility = Visibility.Visible;
                rtbUpDesc.Visibility = Visibility.Visible;
            }
        }

        public Product(OracleConnection c)
        {
            InitializeComponent();
            c = conn;
            init();
            hideUpdate(true);
            loadKategori();
        }

        private static readonly Regex _regex = new Regex("^[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public void init()
        {
            //listcabang.RemoveAll(x => x.nama_cabang == connection.cabangnow.Substring(3).ToLower());
            //connection.openConn();
            int giliran = 0;

            for (int i = 0; i < listcabang.Count; i++)
            {
                if (listcabang[i].nama_cabang == connection.cabangnow.Substring(3).ToLower())
                {
                    giliran = i;
                }
            }

            cbCabang.Items.Clear();
            cbCabang.ItemsSource = listcabang;
            cbCabang.DisplayMemberPath = "nama_cabang";
            cbCabang.SelectedValuePath = "nama_db";
            cbCabang.SelectedItem = cbCabang.Items[giliran];
        }

        private void load_sparepart()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM " + cbCabang.SelectedValue.ToString() + " Order by id_spare asc";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgProduct.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }
        private void loadKategori()
        {
            connection.openConn();
            try
            {
                listkat = new List<Tools_Category>();
                cbCategorySpare.Items.Clear();
                //OracleCommand cmd = new OracleCommand("select ID, NAMA from tools_category", conn.cabangnow);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = "select ID_CATEGORY, CATEGORY_NAME from admin.sparepart_category";
                //conn.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listkat.Add(new Tools_Category(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                }
                reader.Close();
                cbCategorySpare.ItemsSource = listkat;
                cbCategorySpare.DisplayMemberPath = "nama";
                cbCategorySpare.SelectedValuePath = "id";
                cbCategorySpare.SelectedIndex = 0;
                //conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void btnTampil_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                load_sparepart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            int idx = dgProduct.SelectedIndex + 1;
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                if (CustomMessageBox.ShowOKCancel(
                    "ID Sparepart: "+ dataRowView[0].ToString()+"\n"+
                    "=================================================="+
                    "Nama Sparepart: " + dataRowView[1].ToString() + "\n" +
                    "Kategori: " + dataRowView[2].ToString() + "\n" +
                    "Stok: " + dataRowView[3].ToString() + "\n",
                    "Update / Delete Sparepart",
                    "Update!",
                    "Cancel!") == MessageBoxResult.OK){
                    //doUpdate
                    hideUpdate(false);
                    txtUpIdSpare.Text = dataRowView[0].ToString();
                    txtUpNama.Text = dataRowView[1].ToString();
                    txtUpStok.Text = dataRowView[3].ToString();
                    txtUpHarga.Text = dataRowView[4].ToString();
                    rtbUpDesc.Document.Blocks.Add(new Paragraph(new Run(dataRowView[5].ToString())));
                }
                else{
                    //doDelete
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Master.Sparepart.insertSparepart ins_sparepart = new Master.Sparepart.insertSparepart(conn);
            ins_sparepart.Show();
        }

        private void btnRefreshSparepart1_Clicked(object sender, RoutedEventArgs e)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            //cmd.CommandText = "BEGIN dbms_mview.refresh('" + cabang_cb.SelectedValue.ToString() + "',method=>'C'); END;";
            cmd.CommandText = "BEGIN REFRESH('" + cbCabang.SelectedValue.ToString() + "'); END;";
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void txtUpStok_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);

        private void btnUpSparepart_Click(object sender, RoutedEventArgs e)
        {
            connection.openConn();
            string tempCate = cbCategorySpare.SelectedItem.ToString();
            string tempId = "";
            for (int i = 0; i < listkat.Count; i++)
            {
                if (tempCate.Equals(listkat[i].ToString()))
                {
                    tempId = listkat[i].id.ToString();
                }
            }
            string richDesc = new TextRange(rtbUpDesc.Document.ContentStart, rtbUpDesc.Document.ContentEnd).Text;

            using (OracleTransaction trans = connection.conn.BeginTransaction())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection.conn;
                    cmd.CommandText = "UPDATE ADMIN.SPAREPART SET NAME=:name, ID_CATEGORY=:id_category, STOK=:stok, HARGA=:harga, DESCRIPTION=:desc where ID_SPARE=:id_spare";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(":name", txtUpNama.Text);
                    cmd.Parameters.Add(":id_category", tempId);
                    cmd.Parameters.Add(":stok", Convert.ToInt32(txtUpStok.Text));
                    cmd.Parameters.Add(":harga", Convert.ToInt32(txtUpHarga.Text));
                    cmd.Parameters.Add(":desc", richDesc);
                    cmd.Parameters.Add(":id_spare", txtUpIdSpare.Text);
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    MessageBox.Show("Berhasil Update!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    trans.Rollback();
                }
            }
            connection.closeConn();
        }
    }
}
