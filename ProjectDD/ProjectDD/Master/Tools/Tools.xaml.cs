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
using WPFCustomMessageBox;

namespace ProjectDD.Master.Tools
{
    /// <summary>
    /// Interaction logic for Tools.xaml
    /// </summary>
    public partial class Tools : Window
    {
        DataTable dt;
        OracleConnection conn;
        List<Tools_Category> listkat = new List<Tools_Category>();

        List<db_cab> listcabang = new List<db_cab>()
        {
            new db_cab() { nama_cabang = "dave", nama_db = "tools_cabdave"},
            new db_cab() { nama_cabang = "bryan", nama_db = "tools_cabbry"},
            new db_cab() { nama_cabang = "nando", nama_db = "tools_cabnando"},
            new db_cab() { nama_cabang = "jon", nama_db = "tools_cabjon"},
        };

        public void hideUpdate(bool flag)
        {
            if (flag)
            {
                gbUpTools.Visibility = Visibility.Hidden;
                lblUpId.Visibility = Visibility.Hidden;
                txtUpIdTools.Visibility = Visibility.Hidden;
                lblUpNama.Visibility = Visibility.Hidden;
                txtUpNamaTools.Visibility = Visibility.Hidden;
                lblUpCat.Visibility = Visibility.Hidden;
                cbCategoryTools.Visibility = Visibility.Hidden;
                lblUpStat.Visibility = Visibility.Hidden;
                txtUpStat.Visibility = Visibility.Hidden;
                lblUpKet.Visibility = Visibility.Hidden;
                btnUpTools.Visibility = Visibility.Hidden;
            }
            else
            {
                gbUpTools.Visibility = Visibility.Visible;
                lblUpId.Visibility = Visibility.Visible;
                txtUpIdTools.Visibility = Visibility.Visible;
                lblUpNama.Visibility = Visibility.Visible;
                txtUpNamaTools.Visibility = Visibility.Visible;
                lblUpCat.Visibility = Visibility.Visible;
                cbCategoryTools.Visibility = Visibility.Visible;
                lblUpStat.Visibility = Visibility.Visible;
                txtUpStat.Visibility = Visibility.Visible;
                lblUpKet.Visibility = Visibility.Visible;
                btnUpTools.Visibility = Visibility.Visible;
            }
        }
        public Tools()
        {
            InitializeComponent();
            int giliran = 0;

            for (int i = 0; i < listcabang.Count; i++)
            {
                if (listcabang[i].nama_cabang == connection.cabangnow.Substring(3).ToLower())
                {
                    giliran = i;
                }
            }

            cbCabangTools.Items.Clear();
            cbCabangTools.ItemsSource = listcabang;
            cbCabangTools.DisplayMemberPath = "nama_cabang";
            cbCabangTools.SelectedValuePath = "nama_db";
            cbCabangTools.SelectedIndex = 0;
            hideUpdate(true);
            loadKategori();
        }

        private static readonly Regex _regex = new Regex("^[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
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
                cmd.CommandText = "select ID_CATEGORY, NAMA from admin.tools_category";
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

        private void btnBackTools_Clicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnInsertTools_Clicked(object sender, RoutedEventArgs e)
        {
            Master.Tools.insert_tools pInsertTools = new Master.Tools.insert_tools();
            pInsertTools.Show();
        }

        private void btnTampilTools_Clicked(object sender, RoutedEventArgs e)
        {
            loadTools();
        }

        private void loadTools()
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            cmd.CommandText = "SELECT * FROM " + cbCabangTools.SelectedValue.ToString() + " Order by id_tools asc";
            //MessageBox.Show(cmd.CommandText);
            dt = new DataTable();
            cmd.ExecuteNonQuery();
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            oda.Fill(dt);
            dgTools.ItemsSource = dt.DefaultView;
            connection.closeConn();
        }

        private void btnRefreshTools_Clicked(object sender, RoutedEventArgs e)
        {
            connection.openConn();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection.conn;
            //cmd.CommandText = "BEGIN dbms_mview.refresh('" + cabang_cb.SelectedValue.ToString() + "',method=>'C'); END;";
            cmd.CommandText = "BEGIN REFRESH('" + cbCabangTools.SelectedValue.ToString() + "'); END;";
            //MessageBox.Show(cmd.CommandText);
            cmd.ExecuteNonQuery();
            connection.closeConn();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
                txtUpIdTools.Text = dataRowView[0].ToString();
                txtUpNamaTools.Text = dataRowView[1].ToString();
                if (dataRowView[3].ToString() == "Available")
                {
                    txtUpStat.Text = "1";
                }
                else
                {
                    txtUpStat.Text = "0";
                }
                if (CustomMessageBox.ShowOKCancel(
                    "ID Tools: " + dataRowView[0].ToString() + "\n" +
                    "==================================================" +
                    "Nama Tools: " + dataRowView[1].ToString() + "\n" +
                    "Kategori: " + dataRowView[2].ToString() + "\n" +
                    "Status: " + dataRowView[3].ToString() + "\n",
                    "Update / Delete Tools",
                    "Update!",
                    "Delete!") == MessageBoxResult.OK)
                {
                    //doUpdate
                    hideUpdate(false);
                }
                else
                {
                    //doDeletetry
                    connection.openConn();
                    using (OracleTransaction trans = connection.conn.BeginTransaction())
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection.conn;
                            cmd.CommandText = "DELETE FROM ADMIN.TOOLS WHERE ID_TOOLS=:id_toolsT";
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add(":id_toolsT", txtUpIdTools.Text);
                            cmd.Transaction = trans;
                            cmd.ExecuteNonQuery();
                            trans.Commit();
                            MessageBox.Show("Berhasil Delete!");
                            hideUpdate(true);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.closeConn();
            }
        }

        private void btnUpTools_Click(object sender, RoutedEventArgs e)
        {
            string tempCate = cbCategoryTools.SelectedItem.ToString();
            string tempId = "";
            for (int i = 0; i < listkat.Count; i++)
            {
                if (tempCate.Equals(listkat[i].ToString()))
                {
                    tempId = listkat[i].id.ToString();
                }
            }

            connection.openConn();
            using (OracleTransaction trans = connection.conn.BeginTransaction())
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection.conn;
                    cmd.CommandText = "UPDATE ADMIN.TOOLS SET NAMA=:namaT, ID_CATEGORY=:id_categoryT, STATUS=:statusT where ID_TOOLS=:id_toolsT";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(":namaT", txtUpNamaTools.Text);
                    cmd.Parameters.Add(":id_categoryT", tempId);
                    cmd.Parameters.Add(":statusT", Convert.ToInt64(txtUpStat.Text));
                    cmd.Parameters.Add(":id_toolsT", txtUpIdTools.Text);
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    MessageBox.Show("Berhasil Update!");
                    hideUpdate(true);
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
