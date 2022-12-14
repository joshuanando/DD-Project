using System;
using System.Data.OracleClient;
using System.Windows;

namespace ProjectDD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static String cabang, username, password;
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            username = txtbox_username.Text;
            password = txtbox_password.Text;
            cabang = cb_cabang.SelectionBoxItem.ToString();
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Please input Username and Password");
                return;
            }
            var connectionString = "Data Source=" + cabang + "; User ID=" + username + "; Password=" + password;

            //MessageBox.Show(connectionString);

            OracleConnection conn = new OracleConnection(connectionString);
            connection.addConn(conn);
            connection.cabangnow = cabang;
            connection.usernamenow = username;

            try
            {
                connection.openConn();
                connection.closeConn();
                if (username.ToLower() == "admin")
                {
                    Admin w = new Admin(conn);
                    this.Hide();
                    w.ShowDialog();
                }
                else
                {
                    Kasir w = new Kasir(conn);
                    this.Hide();
                    w.ShowDialog();
                }
                this.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
