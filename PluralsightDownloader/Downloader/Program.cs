using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
           // DownloadFile("http://vid.pluralsight.com/expiretime=1411183653/570eeba9e723386d3f2cc03f099d1c71/clip-videos/lukas-ruebbelke/angularjs-in-depth-m2/angularjs-in-depth-m2-05/1024x768mp4/20140213133009.mp4");
            Console.ReadKey();
        }

        private static void DownloadFile(string url) 
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileAsync(new Uri(url),@"D:\d\1.mp4");
        }

        static void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage);
        }

        static void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Done!");
        }



        private static void PostToGetUrl()
        {
            HttpWebRequest webRequest = WebRequest.Create("http://pluralsight.com/training/Player/ViewClip") as HttpWebRequest;
            //NetworkCredential networkCredential = new NetworkCredential("hqq", "dthappy76");
            //webRequest.Credentials = networkCredential;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            webRequest.Headers["Origin"] = "http://pluralsight.com";
            //webRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36");
            //webRequest.Headers.Add("Host", "pluralsight.com");
            //webRequest.Headers.Add("Referer", "http://pluralsight.com/training/courses/TableOfContents?courseName=angularjs-in-depth&highlight=lukas-ruebbelke_angularjs-in-depth-m4!lukas-ruebbelke_angularjs-in-depth-m6*0");
            webRequest.Headers.Add("X-NewRelic-ID", "VwUGVl5VGwIJVFVXAAc=");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //webRequest.Headers["Cookie"] = "optimizelyEndUserId=oeu1410269039678r0.41727527789771557; betaToggledOff=true; __uvt=; _ga=GA1.2.1500543602.1410269079; PSM=AEFE02355B67042C971EB19D49EE7EA57EEB5B3BD51F8E1AF55D2CFBC8CF0D7B6D16A018159CB247177CB6B9C94807E4EBF881FC9BCE7D473E22B17F7FBAE28D001F4D40DA068F8305F0193B305DA146D8B0F9B289636ED888600A33A6EB874E7968A07A3737E22B0BE3ACC8C2B9E92DD9504402806EE1A807DBF573911F869AF7FF55B40A61AF9ED3A0527E78CCC5F8B0DDA0495BAF699117CFEC76C80A778CE0E731F45F8C914740E5585FC7325CA9B3AAAA98; mp_super_properties=%7B%22all%22%3A%20%7B%22%24initial_referrer%22%3A%20%22http%3A//pluralsight.com/training/courses/TableOfContents%3FcourseName%3Dbuilding-apps-durandal-2-mvc5-breeze%26highlight%3D%22%2C%22%24initial_referring_domain%22%3A%20%22pluralsight.com%22%7D%2C%22events%22%3A%20%7B%7D%2C%22funnels%22%3A%20%7B%7D%7D; __RequestVerificationToken_L3RyYWluaW5n=RQsBQBzWbhJYJ5pQtph095pTSjV14pVho4Bo6PrTCxgUVdClytMIFmrBSLm78GAFcwpVNVBEBfYsuceliWt6SKOSytqbxyUw+bA64LNjmgJ6GDxKKCDAD1R2EK9tsapetfcnFNQi82O7vPkeoi5gZHT61Xo=; optimizelySegments=%7B%221227392893%22%3A%22referral%22%2C%221248401246%22%3A%22gc%22%2C%221258181237%22%3A%22false%22%7D; optimizelyBuckets=%7B%221523120776%22%3A%221532130772%22%7D; __utma=195666797.1500543602.1410269079.1411175310.1411177466.10; __utmb=195666797.4.9.1411177674604; __utmc=195666797; __utmz=195666797.1410270742.1.1.utmcsr=beta.pluralsight.com|utmccn=(referral)|utmcmd=referral|utmcct=/courses/aspdotnet-mvc5-fundamentals; psPlayer=%7B%22videoScaling%22%3A%22Scaled%22%2C%22videoQuality%22%3A%22High%22%2C%22playerHintsDismissalCount%22%3A1%7D; __ar_v4=DOMT5ESRMRH2PDG3LFNDCP%3A20140909%3A20%7CBFLWHRV7W5FLTIZIQ4OSO6%3A20140909%3A27%7CNPTOMQSYYZABNNUIQDRAKL%3A20140909%3A27%7C4YCMENXFKFBQLNQCLOV3GS%3A20140909%3A7; _bizo_bzid=e87c2ef0-7198-4dd7-b94e-488faf22ef79; _bizo_cksm=0069C2A760B519F1; _bizo_np_stats=14%3D1414%2C; visitor_id36882=37483928; uvts=23iN0ahsiafRb8rW";
            byte[] buffer = Encoding.UTF8.GetBytes("a=lukas-ruebbelke&m=angularjs-in-depth-m1&course=angularjs-in-dept&cn=0&mt=mp4&q=1024x768&cap=false&lc=en");
            Stream PostData = webRequest.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            HttpWebResponse WebResp = (HttpWebResponse)webRequest.GetResponse();
            Stream Answer = WebResp.GetResponseStream();
            StreamReader sr = new StreamReader(Answer);
            string r = sr.ReadToEnd();
            Console.Write(r);
        }
    }
}
