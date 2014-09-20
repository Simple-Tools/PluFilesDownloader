using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PluralsightDownloader
{
    /// <summary>
    /// Interaction logic for GetDownloadList.xaml
    /// </summary>
    public partial class GetDownloadList : Page
    {
        private const string BasePath = @"D:\Pluralsight\";
        private const int MaxDownloadNum = 5;
        private static string[] _floaders;
        private List<VideoLink> _videoLinkList;
        private List<VideoLink> _toDoList;
        public GetDownloadList()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadTextBox.Text == "") return;
            if (DownloadButton.Content.ToString() == "创建目录") 
            {
                CreateFloder(DownloadTextBox.Text);
            }
            else if (DownloadButton.Content.ToString() == "开始下载")
            {
                InfoTextBlock.Text = "";
                DownloadButton.Content = "返回列表";
                StartDownload(DownloadTextBox.Text);
            }
            else
            {
                _videoLinkList = null;
                _toDoList = null;
                DownloadDataGrid.Visibility = Visibility.Collapsed;
                DownloadTextBox.Visibility = Visibility.Visible;
                DownloadButton.Content = "开始下载";
            }
        }

        private void CreateFloder(string floderText)
        {

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            floderText = Regex.Replace(floderText, @"[^(\s\w,.&;)]", "");
            _floaders = floderText.Split(';');
            Directory.CreateDirectory(BasePath + _floaders[0]);
            for (int i = 1; i < _floaders.Length; i++)
            {
                Directory.CreateDirectory(BasePath + _floaders[0] + "\\" + i + "." + _floaders[i]);
            }
            DownloadTextBox.Text = "";
            DownloadButton.Content = "开始下载";
            InfoTextBlock.Text = "成功创建目录";
        }

       
        private void StartDownload(string downloadText)
        {
            string[] lists = downloadText.Replace("\r\n", "").Split(';');
            _videoLinkList = new List<VideoLink>();
            foreach(string list in lists){
                string[] listinfo = list.Split(',');
                if (listinfo.Length == 2)
                {
                   VideoLink videoLink = new VideoLink{
                       Name = listinfo[0],
                       Link = listinfo[1]
                    };
                    int floderId = Convert.ToInt32(videoLink.Name.Split('-')[0]);
                    videoLink.Floder = floderId + "." + _floaders[floderId];
                    _videoLinkList.Add(videoLink);
                }
            }

            DownloadTextBox.Visibility = Visibility.Collapsed;
            DownloadDataGrid.Visibility = Visibility.Visible;
            RefreshDataGrid();
            StartDownloadTask();
        }

        private void StartDownloadTask()
        {
            if (_toDoList == null)
            {
                _toDoList =  new List<VideoLink>(_videoLinkList.ToArray());
                int downloadNum = _videoLinkList.Count < MaxDownloadNum ? _videoLinkList.Count : MaxDownloadNum;
                for (int i = 0; i < downloadNum; i++)
                {
                    VideoLink videoLink = _toDoList.First();
                    DownloadFile(videoLink);              
                    _toDoList.Remove(videoLink);
                }
            }
            else if (_toDoList.Count > 0)
            {
                VideoLink videoLink = _toDoList.First();
                DownloadFile(videoLink);
                _toDoList.Remove(videoLink);
            }
        }


        private void DownloadFile(VideoLink videoLink)
        {
            videoLink.ProcessTime = DateTime.Now;
            string localName = BasePath + _floaders[0] + "\\" + videoLink.Floder + "\\" + videoLink.Name + ".mp4";
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileAsync(new Uri("http://" + videoLink.Link), localName, videoLink);
        }


        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            VideoLink videoLink = e.UserState as VideoLink;
            videoLink.Process = e.ProgressPercentage + "%";
            videoLink.Size = FileSize.GetFileSizeString(e.TotalBytesToReceive,2);
            double avrageSpeedSize = e.BytesReceived / (DateTime.Now - videoLink.ProcessTime).TotalSeconds;
            videoLink.Speed = FileSize.GetFileSizeString(avrageSpeedSize,2) + "/s";
            RefreshDataGrid();
        }
        private void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            VideoLink videoLink = e.UserState as VideoLink;
            videoLink.Speed = "已完成";
            RefreshDataGrid();
            StartDownloadTask();
        }
        private void RefreshDataGrid()
        {
            DownloadDataGrid.ItemsSource = _videoLinkList.Select(a => new { a.Floder, a.Name, a.Size, a.Process, a.Speed });
        }


    }

    public class VideoLink
    {
        public string Floder { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Size { get; set; }
        public string Speed { get; set; }
        public string Process { get; set; }
        public DateTime ProcessTime { get; set; }
    }

    public class PluralsightInfo
    {
        public string[] floders { get; set; }
        public List<VideoLink> DownloadList { get; set; }
    }


    #region 文件相关操作类分

    /// <summary>
    /// 文件有关的操作类
    /// </summary>
    public class FileSize
    {
        #region 相应单位转换常量

        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        #endregion

        #region 获取适应大小
        /// <summary>
        /// 得到适应大小
        /// </summary>
        /// <param name="size">字节大小</param>
        /// <param name="round">保留小数(位)</param>
        /// <returns></returns>
        public static string GetFileSizeString(double size, int round)
        {
            if (KBCount > size) return Math.Round(size, round) + "B";
            else if (MBCount > size) return Math.Round(size / KBCount, round) + "KB";
            else if (GBCount > size) return Math.Round(size / MBCount, round) + "MB";
            else if (TBCount > size) return Math.Round(size / GBCount, round) + "GB";
            else return Math.Round(size / TBCount, round) + "TB";
        }

        #endregion
    }

    #endregion
}
