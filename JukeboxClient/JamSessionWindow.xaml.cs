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

namespace JukeboxClient
{
    /// <summary>
    /// Interaction logic for JamSessionWindow.xaml
    /// </summary>
    public partial class JamSessionWindow : Window
    {
        public JamSessionWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string hbt = HostTextBox.Text;
            string ptb = HostTextBox_Copy1.Text;
            string address = "";

            if (hbt.CompareTo("") == 0 || ptb.CompareTo("") == 0)
            {

                address = "http://localhost:5078/";
                MessageBox.Show(address);
            }
            else
            {
                address = "http://" + hbt + ":" + ptb + "/";
                MessageBox.Show(address);
            }

            Network.setHostUrl(address);
            //Network.GetPlaylist();
            //Network.GetSongs();
        }
            
    }
}
