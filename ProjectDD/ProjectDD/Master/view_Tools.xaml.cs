using System;
using System.Collections.Generic;
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

namespace ProjectDD.Master
{
    /// <summary>
    /// Interaction logic for view_Tools.xaml
    /// </summary>
    public partial class view_Tools : Window
    {
        List<string> listcabang = new List<string>(new string[] { "local", "dave", "jonathan", "bryan", "nando" });
        public view_Tools()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            //MessageBox.Show(connection.cabangnow.Substring(3).ToLower());
            listcabang.Remove(connection.cabangnow.Substring(3).ToLower());
            cabang_cb.Items.Clear();
            foreach (var cabang in listcabang)
            {
                cabang_cb.Items.Add(cabang);
            }
            cabang_cb.SelectedItem = cabang_cb.Items[0];
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(cabang_cb.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
