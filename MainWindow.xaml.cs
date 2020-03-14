using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Configuration;
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
        private VideoClient client;
        private string defaultFormat = ConfigurationSettings.AppSettings["default_format"];
        private string defaultQuality = ConfigurationSettings.AppSettings["default_quality"];

        public MainWindow()
        {
            InitializeComponent();
            youtubeService = YouTube.Default;
            string disp = "";
            foreach (ListBoxItem elem in resolutionBox.Items)
            {
                if (elem.Content.ToString() == defaultQuality)
                {
                    resolutionBox.SelectedItem = elem;
                    break;
                }
            }
            resolutionBox.SelectedItem = defaultQuality;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = VideoUrl.Text;
            switch(CheckURL(url))
            {
                case HttpStatusCode.OK:
                    DownloadVideo(url);
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    DisplayError("erreur");
                    break;
            }
        }

        private void DisplayError(string msg)
        {
            MessageBox.Show(msg);
        }

        private HttpStatusCode CheckURL(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return (response.StatusCode);
            }
            catch
            {
                return (HttpStatusCode)1;
            }
        }

        private void DownloadVideo(string url)
        {
            try
            {
                var videos = youtubeService.GetAllVideos(url);
                string resolution = resolutionBox.SelectedItem.ToString();
                bool highest = resolution == "highest" ? true : false;
                YouTubeVideo selectVideo = null;
                YouTubeVideo highestVideo = null;
                foreach (var video in videos)
                {
                    if (highestVideo == null)
                        highestVideo = video;
                    else if (highestVideo.Resolution < video.Resolution && video.Format.ToString() == defaultFormat)
                    {
                        highestVideo = video;
                        if (highest)
                            selectVideo = highestVideo;
                    }
                    if (video.Resolution.ToString() == resolution && video.Format.ToString() == defaultFormat)
                        selectVideo = video;
                }
                if (selectVideo == null)
                {
                    MessageBox.Show("No video found with the selected resolution. Use resolution " + highestVideo.Resolution.ToString() + " instead.");
                    selectVideo = highestVideo;
                }
                //                    MessageBox.Show((videos.Resolution).ToString());
                //                File.WriteAllBytes(DownloadFolder + video.FullName, video.GetBytes());
                //                MessageBox.Show("FINISH : " + DownloadFolder + video.FullName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
