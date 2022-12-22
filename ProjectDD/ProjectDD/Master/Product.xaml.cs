using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using WPFCustomMessageBox;
using System.Windows;
using System.Windows.Controls;

namespace ProjectDD.Master
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        DataTable dt;

        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "dave", nama_db = "sparepart_cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "sparepart_cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "sparepart_cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "sparepart_cabjon"},
        };

        public Product()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            //listcabang.RemoveAll(x => x.nama_cabang == connection.cabangnow.Substring(3).ToLower());

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

        private void load_tools()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM " + cbCabang.SelectedValue.ToString();
            MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgProduct.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void btnTampil_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                load_tools();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //for (int i = 0; i < dgProduct.Items.Count; i++)
                //{
                //    DataGridRow row = (DataGridRow)dgProduct.ItemContainerGenerator.ContainerFromIndex(i);

                //    TextBlock t_name = dgProduct.Columns[0].GetCellContent(row) as TextBlock;
                //    TextBlock t_type = dgProduct.Columns[1].GetCellContent(row) as TextBlock;
                //}
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                if (CustomMessageBox.ShowOKCancel(
                    "ID Sparepart: "+ dataRowView[0].ToString()+"\n"+
                    "=================================================="+
                    "Nama Sparepart: " + dataRowView[1].ToString() + "\n" +
                    "Kategori: " + dataRowView[2].ToString() + "\n" +
                    "Stok: " + dataRowView[3].ToString() + "\n",
                    "Update / Delete Sparepart",
                    "Update!",
                    "Delete!") == MessageBoxResult.OK){
                    MessageBox.Show("OK");
                }else{
                    if (MessageBox.Show("Are you sure want to DELETE this item?",
                    "Delete Sparepart",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Deleted!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }
    }
}
