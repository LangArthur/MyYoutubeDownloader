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
using System.Net;

namespace MyYoutubeDownloader
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckURL(VideoUrl.Text))
            {
                MessageBox.Show("In developpement");
            }
            else
            {
                MessageBox.Show("URL is invalid");
            }
        }

        private bool CheckURL(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
    }
}
