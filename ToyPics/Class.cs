using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Text.RegularExpressions;
using HtmlAgilityPack;
using CommandLine;
using CommandLine.Text;
using System.Net;

namespace ToyPics
{
    class Class
    {

        private static Uri getVidUri(HtmlNode link, string strStart, string strEnd)
        {
            string strSource = link.InnerText.ToString();

            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                int End = strSource.IndexOf(strEnd, Start);
                string uservid = strSource.Substring(Start, End - Start);
                return new Uri(String.Format(@"http://static1.toypics.net/flvideo/{0}.mp4", uservid));
            } else {
                return null;
            }
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Download Video
        /// </summary>
        /// <param name="page">page url in string form</param>
        /// <param name="videoname">string to save the video as</param>
        public static void downloadVid(string page, string videoname)
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
                            DownloadProgressChangedEventHandler callback = new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                            dl.DownloadProgressChanged += callback;
                            dl.DownloadFileAsync(video, String.Format("{0}.mp4", videoname));
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
