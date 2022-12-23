using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectDD.Master.Sparepart
{
    /// <summary>
    /// Interaction logic for insertSparepart.xaml
    /// </summary>
    public partial class insertSparepart : Window
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

        private static readonly Regex _regex = new Regex("^[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public insertSparepart(OracleConnection c)
        {
            InitializeComponent();
            conn = c;
            loadKategori();
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
                cbCategorySpare.SelectedIndex= 0;
                //conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void txtStok_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);
        private void txtHarga_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);

        private void btnInsertSparepart_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(rtbDesc.Document.ContentStart, rtbDesc.Document.ContentEnd).Text;
            if (txtName.Text.Equals("")){
                MessageBox.Show("Field Nama Harap Diisi Terlebih Dahulu!");
            }else if (txtStok.Text.Equals("")){
                MessageBox.Show("Field Stok Harap Diisi Terlebih Dahulu!");
            }else if (txtHarga.Text.Equals("")){
                MessageBox.Show("Field Harga Harap Diisi Terlebih Dahulu!");
            }else if (richText == "")
            {
                MessageBox.Show("Field Deskripsi Harap Diisi Terlebih Dahulu!");
            }
            else
            {
                OracleTransaction trans;
                trans = connection.conn.BeginTransaction();
                try
                {
                    string tempCate = cbCategorySpare.SelectedItem.ToString();
                    string tempId = "";
                    string tempDesc = new TextRange(rtbDesc.Document.ContentStart, rtbDesc.Document.ContentEnd).Text;
                    
                    for (int i = 0; i < listkat.Count; i++)
                    {
                        if (tempCate.Equals(listkat[i].ToString()))
                        {
                            //MessageBox.Show("asuk pak dave");
                            tempId = listkat[i].id.ToString();
                        }
                    }
                    //MessageBox.Show(tempId);
                    //MessageBox.Show(txtName.ToString() + " " + tempId + " " + Convert.ToInt16(txtStok.ToString()) + " " + Convert.ToInt16(txtHarga.ToString()) + " " + rtbDesc.ToString());
                    string qry = "";
                    qry = "Insert into admin.sparepart values('',:name,:idCate,:stok,:harga,:deskripsi)";
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection.conn;
                    cmd.CommandText = qry;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(":name", txtName.Text);
                    cmd.Parameters.Add(":idCate", tempId);
                    cmd.Parameters.Add(":stok", Convert.ToInt32(txtStok.Text));
                    cmd.Parameters.Add(":harga", Convert.ToInt32(txtHarga.Text));
                    cmd.Parameters.Add(":deskripsi", tempDesc);
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    MessageBox.Show("Berhasil");
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    trans.Rollback();
                }
            }
        }
    }
}
