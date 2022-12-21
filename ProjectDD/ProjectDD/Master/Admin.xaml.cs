using System;
using System.Data.OracleClient;
using System.Windows;

namespace ProjectDD
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        OracleConnection conn;
        public Admin(OracleConnection c)
        {
            InitializeComponent();
            conn = c;
            init();
        }

        string[] listview = { "View Tools", "View Sparepart" };
        String[] CRUD = { "Insert", "Update", "Delete" };
        string[] tablename = { "Sparepart Category", "Tools Category" };

        private void init()
        {
            showcabang();
            for (int i = 0; i < listview.Length; i++)
            {
                view_cb.Items.Add(listview[i]);
            }
            view_cb.SelectedItem = view_cb.Items[0];

            for (int i = 0; i < CRUD.Length; i++)
            {
                CRUD_CB.Items.Add(CRUD[i]);
            }
            CRUD_CB.SelectedItem = CRUD_CB.Items[0];

            for (int i = 0; i < tablename.Length; i++)
            {
                TableCB.Items.Add(tablename[i]);
            }
            TableCB.SelectedItem = TableCB.Items[0];
        }

        private void showcabang()
        {
            label_cabang.Content = "Welcome Admin of " + connection.cabangnow;
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

        private void btnCheck_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnHistory_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnProduct_Clicked(object sender, RoutedEventArgs e)
        {
            Master.Product mprod = new Master.Product();
            mprod.Show();
        }

        private void btnStok_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnKategori_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void go_button_Click(object sender, RoutedEventArgs e)
        {
            switch (TableCB.SelectedItem.ToString())
            {
                case "Sparepart Category":
                    sparepart_category();
                    break;
                case "Tools Category":
                    tools_category();
                    break;
                default:
                    break;
            }
        }

        private void sparepart_category()
        {
            switch (CRUD_CB.SelectedItem.ToString())
            {
                case "Insert":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Insert sk = new Master.Kategori_Sparepart.Kategori_Sparepart_Insert();
                    sk.Show();
                    break;
                case "Update":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Update usk = new Master.Kategori_Sparepart.Kategori_Sparepart_Update();
                    usk.Show();
                    break;
                case "Delete":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Delete dsk = new Master.Kategori_Sparepart.Kategori_Sparepart_Delete();
                    dsk.Show();
                    break;
                default:
                    break;
            }
        }

        private void tools_category()
        {
            switch (CRUD_CB.SelectedItem.ToString())
            {
                case "Insert":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Insert sk = new Master.Kategori_Sparepart.Kategori_Sparepart_Insert();
                    sk.Show();
                    break;
                case "Update":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Update usk = new Master.Kategori_Sparepart.Kategori_Sparepart_Update();
                    usk.Show();
                    break;
                case "Delete":
                    Master.Kategori_Sparepart.Kategori_Sparepart_Delete dsk = new Master.Kategori_Sparepart.Kategori_Sparepart_Delete();
                    dsk.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
