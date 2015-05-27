using System;
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;

namespace ToyPics
{
    class Class
    {
        /// <summary>
        /// Generate video uri
        /// </summary>
        /// <param name="link">Single video page</param>
        /// <param name="strStart">.toypics.net/flvideo/</param>
        /// <param name="strEnd">.mp4</param>
        /// <returns></returns>
        public static Uri getVidUri(HtmlNode link, string strStart, string strEnd)
        {
            // get javascript source
            string jsSorce = link.InnerText.ToString();

            // make sure js code has what we need
            //Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4");
            if (jsSorce.Contains(strStart) && jsSorce.Contains(strEnd))
            {
                // string triming
                int Start = jsSorce.IndexOf(strStart, 0) + strStart.Length;
                int End = jsSorce.IndexOf(strEnd, Start);
                
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
        /// Generate video uri; override function
        /// </summary>
        /// <param name="link">Single video page</param>
        /// <returns></returns>
        public static Uri getVidUri(HtmlNode link)
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

        private static void downloadVid(string page)
        {
            HtmlWeb hw = new HtmlWeb();// HtmlAgilityPack
            HtmlDocument doc = hw.Load(page);// load uri
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//script"))//loop every <script> tag
            {
                if (Regex.IsMatch(link.InnerText, "(static1.toypics.net)"))// find javascript video player init code
                {
                    try
                    {
                        Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4");// find video source
                        using (WebClient dl = new WebClient())
                        {
                            //dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);// dev
                            dl.DownloadFileAsync(video, "video.mp4");// dev
                            Console.WriteLine("Done");// dev
                            Console.ReadKey();// dev
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        private static string getTitle(string page)
        {
            HtmlWeb hw = new HtmlWeb();// HtmlAgilityPack
            HtmlDocument doc = hw.Load(page);// load uri
            string link = doc.DocumentNode.SelectSingleNode("//*[@id='view-video-content']/*[@class='section bg2']/*[@class='hd']").InnerText; // the true title is stored in a <div> tag with the class of hd-l (HD-L) but i keep getting an exception if i try /*[@class='hd-l']
            //for now im just going to trim the returned string
            return link.Trim();
        }
    }
}
