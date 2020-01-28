using System;
using System.IO;
using System.Windows;
using System.Net;
using Microsoft.Win32;
using VideoLibrary;

namespace MyYoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private YouTube youtubeService;
        private string DownloadFolder = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString() + "\\";
   
        public MainWindow()
        {
            InitializeComponent();
            youtubeService = YouTube.Default;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = VideoUrl.Text;
            if (CheckURL(url))
            {
                try
                {
                    var video = youtubeService.GetVideo(url);
                    File.WriteAllBytes(DownloadFolder + video.FullName, video.GetBytes());
                    MessageBox.Show("FINISH : " + DownloadFolder + video.FullName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
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
