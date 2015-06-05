using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;

namespace ToyPics
{
    public class Class : IDisposable
    {

        private Double ClassVersion = 0.1;

        private HtmlWeb hw = new HtmlWeb();
        private UIDGen uid = new UIDGen();
        private HtmlDocument doc;
        private string saveLocation;
        private string pageURL;
        
        private Boolean singleVideo = false;

        /// <summary>
        /// sdahjklf
        /// </summary>
        /// <param name="videoPage">link to page</param>
        /// <param name="saveLocation">video save location</param>
        /// <param name="userPage">is the page a userPage</param>
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
            this.hw = null;
            this.doc = null;
            this.uid = null;
        }

        private void init()
        {
            this.hw.UserAgent = String.Format("ToyPisDownloader [task=pageDownload] [version={0}] [uid={1}] [github/teh-random-name/toypicsDL]", this.ClassVersion, this.uid.userUID());
            this.hw.UsingCache = false;
        }

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
                // I normaly force HTTPS but static1 isnt setup for that
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Download Video
        /// </summary>
        public void downloadVid()
        {
            foreach (HtmlNode link in this.doc.DocumentNode.SelectNodes("//script"))//loop every <script> tag
            {
                if (Regex.IsMatch(link.InnerText, "(static1.toypics.net)"))// find javascript video player init code
                {
                    try
                    {
                        Uri video = this.getVidUri(link);// find video source
                        using (WebClient dl = new WebClient())
                        {
                            var tmp1 = this.ClassVersion; //dev
                            var tmp2 = this.uid.uid; //dev 
                            string tmp3 = this.uid.userUID(); //dev; same as above

                            // this is causing a NullRef exceptionl i have no fucking clue what could be causing this
                            dl.ResponseHeaders.Add("UserAgent", String.Format("ToyPisDownloader [task=videoDownload] [version={0}] [uid={1}] [github/teh-random-name/toypicsDL]", tmp1, tmp2));
                            //dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);// dev
                            dl.DownloadFile(video, String.Concat(this.saveLocation, this.getTitle(this.pageURL), ".mp4"));// dev
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        private string getTitle(string page)
        {
            HtmlWeb hw = new HtmlWeb();// HtmlAgilityPack
            HtmlDocument doc = hw.Load(page);// load uri
            string link = doc.DocumentNode.SelectSingleNode("//*[@id='view-video-content']/*[@class='section bg2']/*[@class='hd']").InnerText; // the true title is stored in a <div> tag with the class of hd-l (HD-L) but i keep getting an exception if i try /*[@class='hd-l']
            //for now im just going to trim the returned string

            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = String.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );
            
            return Regex.Replace(link.Trim(), invalidRegStr, "_"); 
        }
    }
}
