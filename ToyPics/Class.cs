using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;

namespace ToyPics
{
    /// <summary>
    /// ToyPics Downloader Class
    /// </summary>
    public class Class : IDisposable
    {

        private static Double ClassVersion = 0.1;

        private HtmlWeb hw = new HtmlWeb();
        private UIDGen uid = new UIDGen();
        private HtmlDocument doc;
        private string saveLocation;
        private string pageURL;
        private WebClient dl = new WebClient();

        public delegate void ProgressUpdate(object sender, DownloadProgressChangedEventArgs e);
        public event ProgressUpdate OnProgressUpdate;

        public delegate void ProgressFinished(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
        public event ProgressFinished OnProgressFinished;

        private Boolean singleVideo = false;

        public Class(string videoPage, string saveLocation, Boolean userPage)
        {
            this.init();

            this.pageURL = videoPage;
            this.doc = hw.Load(videoPage);
            this.singleVideo = !userPage;
            this.saveLocation = saveLocation;
        }

        public void Dispose()
        {

            this.dl.Dispose();

            this.hw = null;
            this.doc = null;
            this.uid = null;
            this.dl = null;
        }

        public void stopDownload() {
            if (dl.IsBusy) {
                dl.CancelAsync();
            } else {
                Console.WriteLine("WebClient isn\'t currently busy.");
            }
        }

        private void init()
        {
            this.hw.UserAgent = String.Format("ToyPisDownloader [task=pageDownload] [version={0}] [uid={1}] [github/teh-random-name/toypicsDL]", ClassVersion, this.uid.userUID());
            this.hw.UsingCache = false;
        }

        /// <summary>
        /// Class Version
        /// </summary>
        public static Double version { get { return ClassVersion; } }

        /// <summary>
        /// Generate video uri
        /// </summary>
        /// <param name="link">Single video page</param>
        /// <returns></returns>
        private Uri getVidUri(HtmlNode link)
        {
            // get javascript source
            string jsSorce = link.InnerText.ToString();

            // make sure js code has what we need
            //Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4");
            if (jsSorce.Contains(@".toypics.net/flvideo/") && jsSorce.Contains(".mp4"))
            {
                // string triming
                int Start = jsSorce.IndexOf(@".toypics.net/flvideo/", 0) + @".toypics.net/flvideo/".Length;
                int End = jsSorce.IndexOf(".mp4", Start);

                string video = jsSorce.Substring(Start, End - Start);// this `should` be the video link, or atlest the name 
                /*
                 * example //static1.toypics.net/flvideo/4ca4238a0b/14112522021748591567.mp4
                 * we would get back the video file name w/out extension and the directory - [4ca4238a0b/14112522021748591567]
                 * I'm unsure if the directory is important
                 * the javascript player does have some other options that could be deemed useful, unfortunately a proper title isnt one of them
                */
                return new Uri(String.Format(@"http://static1.toypics.net/flvideo/{0}.mp4", video));// create a uri from the found string
                // I normally force HTTPS but static1 isnt setup for that
            }
            else
            {
                return null;
            }
        }

        //Get videoLinks for user page
        public List<string> getVideoLinks() {
            List<string> toReturn = new List<string>();
            if (Regex.IsMatch(this.pageURL, "(/public/)")) // /public/ is unique to user profiles
                {
                foreach (HtmlNode videolink in doc.DocumentNode.SelectNodes("//p[@class='video-entry-title']/a")) //loop through every <a> tag page link
                {
                    if (videolink.Attributes["href"].Value != String.Empty && Regex.IsMatch(videolink.Attributes["href"].Value, "(?i)(https://videos.toypics.net/view/)")) // filter only video pages
                    {
                        toReturn.Add(videolink.Attributes["href"].Value); // send whole single video page to downloader
                    }
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Download Video
        /// </summary>
        public void downloadVid()
        {
            foreach (HtmlNode link in this.doc.DocumentNode.SelectNodes("//script"))//loop every <script> tag
            {
                if (Regex.IsMatch(link.InnerText, "(?i)(static1.toypics.net)"))// find javascript video player init code
                {
                    try
                    {
                        Uri video = this.getVidUri(link);// find video source\
                        {
                            dl.Headers.Add(HttpRequestHeader.UserAgent, String.Format("ToyPisDownloader [task=videoDownload] [version={0}] [uid={1}] [github/teh-random-name/toypicsDL]", ClassVersion, this.uid.userUID()));
                            dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnProgressUpdate);
                            dl.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(OnProgressFinished);

                            dl.DownloadFileAsync(video, String.Concat(this.saveLocation, this.getTitle(), ".mp4"));
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get Clean Title of page
        /// </summary>
        /// <returns>String</returns>
        public string getTitle() {
            HtmlWeb hw = new HtmlWeb();// HtmlAgilityPack
            HtmlDocument doc = hw.Load(pageURL);// load uri
            string link = doc.DocumentNode.SelectSingleNode("//*[@id=\"view-video-content\"]/div[1]/div/div").InnerText;
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(link.Trim(), invalidRegStr, "_");
        }
    }
}
