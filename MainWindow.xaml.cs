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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Google.Apis.Auth.OAuth2;


/// allez voir : https://github.com/flagbug/YoutubeExtractor/blob/master/YoutubeExtractor/ExampleApplication/Program.cs
/// https://www.youtube.com/watch?v=TnG3urCD_m0

namespace MyYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _connected;
        private Uri _uri;
        private UserCredential credentials;

        public MainWindow()
        {
/*            InitializeComponent();
            _connected = connectToYT();
            if (_connected) {
                _uri = new Uri("pack://application:,,,/Assets/wifi.png");
            } else {
                _uri = new Uri("pack://application:,,,/Assets/no-wifi.png");
            }
            var image = new BitmapImage(_uri);
            status.Source = image;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No link are precised :(");
        }

        private bool connectToYT()
        {
            return true;
        }
    }
}
