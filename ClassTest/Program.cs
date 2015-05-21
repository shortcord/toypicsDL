using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using CommandLine;
using CommandLine.Text;

namespace Something
{
    class Program
    {
        static void Main(string[] args)
        {

//            const string url = @"https://videos.toypics.net/view/3300/butt-bouncing-on-chance~/";
            const string url = @"https://videos.toypics.net/Junee/public/";

            try
            {

                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(url);

                #region video download
                if (Regex.IsMatch(url, "(https://videos.toypics.net/view/)"))
                {
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//script"))
                    {
                        if (Regex.IsMatch(link.InnerText, "(static1.toypics.net)"))
                        {
                            try
                            {
                                Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4");
                                using (WebClient dl = new WebClient())
                                {
                                    dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                                    dl.DownloadFileAsync(video, "video.mp4");
                                    Console.WriteLine("Done");
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            break;
                        }
                    }
                }
                #endregion

                if (Regex.IsMatch(url, "(/public/)"))
                {
                    foreach (HtmlNode videolink in doc.DocumentNode.SelectNodes("//p[@class='video-entry-title']/a"))
                    {
                        if (videolink.Attributes["href"].Value != String.Empty && Regex.IsMatch(videolink.Attributes["href"].Value, "(https://videos.toypics.net/view/)"))
                        {
                            Console.WriteLine(
                                    videolink.Attributes["href"].Value
                                );
                            downloadVid(videolink.Attributes["href"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error happened and shit | {0}", ex);
            }

            Console.ReadKey();
        }

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

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Title = String.Format("{0} % complete...", e.ProgressPercentage);
        }


        private static void downloadVid(string page)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(page);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//script"))
            {
                if (Regex.IsMatch(link.InnerText, "(static1.toypics.net)"))
                {
                    try
                    {
                        Uri video = getVidUri(link, @".toypics.net/flvideo/", ".mp4");
                        using (WebClient dl = new WebClient())
                        {
                            dl.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                            dl.DownloadFileAsync(video, "video.mp4");
                            Console.WriteLine("Done");
                            Console.ReadKey();
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                }
            }
        }
    }
}
