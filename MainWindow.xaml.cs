using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Configuration;
using System.Collections;
using Microsoft.Win32;
using VideoLibrary;

// https://github.com/mrklintscher/YouTubeSearch/blob/master/ExampleApplication/Program.cs

namespace MyYoutubeDownloader
{
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
                var audio = getAudio(url);
                var video = getVideo(url);
                File.WriteAllBytes(DownloadFolder + "audio_ " + audio.FullName, audio.GetBytes());
                File.WriteAllBytes(DownloadFolder + "video_ " + video.FullName, video.GetBytes());
                MessageBox.Show("FINISH : " + DownloadFolder + audio.FullName);
            }
            catch (Exception exception)
            {
                MessageBox.Show("An error occured : " + exception.ToString());
            }
        }

        private void basicDownload(string url)
        {
            var video = youtubeService.GetVideo(url); 
            File.WriteAllBytes(DownloadFolder + video.FullName, video.GetBytes());
        }

        private YouTubeVideo getVideo(string url)
        {
            var videos = youtubeService.GetAllVideos(url);
            ListBoxItem selectedRes = (ListBoxItem)resolutionBox.SelectedItem;
            string resolution = selectedRes.Content.ToString();
            bool highest = resolution == "highest" ? true : false;
            YouTubeVideo selectVideo = null;
            YouTubeVideo highestVideo = null;
            foreach (var v in videos)
            {
                string currentFormat = v.Format.ToString().ToLower();
                if (currentFormat == defaultFormat && highestVideo == null)
                    highestVideo = v;
                else if (currentFormat == defaultFormat && highestVideo.Resolution < v.Resolution)
                {
                    highestVideo = v;
                    if (highest)
                        selectVideo = highestVideo;
                }
                if (currentFormat == defaultFormat && v.Resolution.ToString() == resolution)
                    selectVideo = v;
            }
            if (selectVideo == null)
            {
                MessageBox.Show("No video found with the selected resolution. Use resolution " + highestVideo.Resolution.ToString() + " instead.");
                selectVideo = highestVideo;
            }
            return (selectVideo);
        }

        private YouTubeVideo getAudio(string url)
        {
            var videoInfos = youtubeService.GetAllVideos(url);
            YouTubeVideo toDownload = null;
            foreach (var video in videoInfos)
            {
                if (video.AdaptiveKind == AdaptiveKind.Audio)
                {
                    toDownload = video;
                    break;
                }
            }
            return toDownload;
        }
    }
}
