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

namespace ProjectDD.Master.Tools
{
    /// <summary>
    /// Interaction logic for insert_tools.xaml
    /// </summary>
    public partial class insert_tools : Window
    {
        List<Tools_Category> listkat = new List<Tools_Category>();
        public insert_tools()
        {
            InitializeComponent();
            loadKategori();
        }

        private void loadKategori()
        {
            connection.openConn();
            try
            {
                listkat = new List<Tools_Category>();
                cbCategoryTools.Items.Clear();
                //OracleCommand cmd = new OracleCommand("select ID, NAMA from tools_category", conn.cabangnow);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection.conn;
                cmd.CommandText = "select ID_CATEGORY, nama from admin.tools_category";
                //conn.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listkat.Add(new Tools_Category(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                }
                reader.Close();
                cbCategoryTools.ItemsSource = listkat;
                cbCategoryTools.DisplayMemberPath = "nama";
                cbCategoryTools.SelectedValuePath = "id";
                cbCategoryTools.SelectedIndex = 0;
                //conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void btnInsertTools_Clicked(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Equals(""))
            {
                MessageBox.Show("Field Nama Harap Diisi Terlebih Dahulu!");
            }
            else
            {
                OracleTransaction trans;
                trans = connection.conn.BeginTransaction();
                try
                {
                    string tempCate = cbCategoryTools.SelectedItem.ToString();
                    string tempId = "";
                    int tempStatus = 0;
                    for (int i = 0; i < listkat.Count; i++)
                    {
                        if (tempCate.Equals(listkat[i].ToString()))
                        {
                            //MessageBox.Show("asuk pak dave");
                            tempId = listkat[i].id.ToString();
                        }
                    }
                    if (rbAktif.IsChecked == true)
                    {
                        tempStatus= 1;
                    }
                    else
                    {
                        tempStatus = 0;
                    }
                    //MessageBox.Show(tempId);
                    //MessageBox.Show(txtName.ToString() + " " + tempId + " " + Convert.ToInt16(txtStok.ToString()) + " " + Convert.ToInt16(txtHarga.ToString()) + " " + rtbDesc.ToString());
                    string qry = "";
                    qry = "Insert into admin.tools values('',:name,:idCate,:status)";
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection.conn;
                    cmd.CommandText = qry;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(":name", txtName.Text);
                    cmd.Parameters.Add(":idCate", tempId);
                    cmd.Parameters.Add(":status", tempStatus);
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
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
}
