using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using CommandLine; //its not used in this test
using CommandLine.Text; // ^^

namespace Something
{
    class Program
    {
        static void Main(string[] args)
        {

            const string url = @"https://videos.toypics.net/view/3300/butt-bouncing-on-chance~/"; // dev
//            const string url = @"https://videos.toypics.net/Junee/public/"; // dev

            try
            {

                HtmlWeb hw = new HtmlWeb(); // HtmlAgilityPack
                HtmlDocument doc = hw.Load(url); // load uri

                // single video download
                #region video download
                if (Regex.IsMatch(url, "(https://videos.toypics.net/view/)")) // filter single video pages
                {
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//script")) // for each <script></script> tag on page
                    {
                        if (Regex.IsMatch(link.InnerText, "(static1.toypics.net)")) //find the tag that contains javascript player init code
                        {
                            try
                            {
                                Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4"); //get raw video link
                                using (WebClient dl = new WebClient())
                                {
                                    //dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback); //dev
                                    //dl.DownloadFileAsync(video, @"video.mp4"); //dev
                                    Console.WriteLine("Done"); //dev
                                    Console.WriteLine(getTitle(url));
                                }

                            }
                            catch (WebException)
                            {
                                throw;
                            }
                            break; //break out of the foreach loop, only one javascript player on each page but there are multiple <script> tags , so its useless to continue once we find the player
                        }
                    }
                }
                #endregion

                // user profile download
                #region user profile download
                if (Regex.IsMatch(url, "(/public/)")) // /public/ is unique to user profiles
                {
                    foreach (HtmlNode videolink in doc.DocumentNode.SelectNodes("//p[@class='video-entry-title']/a")) //loop through every <a> tag page link
                    {
                        if (videolink.Attributes["href"].Value != String.Empty && Regex.IsMatch(videolink.Attributes["href"].Value, "(https://videos.toypics.net/view/)")) // filter only video pages
                        {
                            Console.WriteLine( videolink.Attributes["href"].Value ); // dev
                            downloadVid(videolink.Attributes["href"].Value); // send whole single video page to downloader
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error somewhere; {0}", ex.Message);
            }

            Console.ReadKey();
        }
        
        // dev
        public static Uri getVidUri(HtmlNode link, string strStart, string strEnd)
        {
            string strSource = link.InnerText.ToString();

            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                int End = strSource.IndexOf(strEnd, Start);
                string uservid = strSource.Substring(Start, End - Start);
                return new Uri(String.Format(@"http://static1.toypics.net/flvideo/{0}.mp4", uservid));
            }
            else
            {
                return null;
            }
        }       

        // dev
        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Title = String.Format("{0} % complete...", e.ProgressPercentage);
        }

        // dev
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
                            dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);// dev
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
