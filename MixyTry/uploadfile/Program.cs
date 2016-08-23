using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace uploadfile
{
    class Program
    {
        static void Main(string[] args)
        {
            //var url1 = @"D:\test\BodyPart_7763b104-d68f-4d88-883b-8375a50dc6ca.7z";
            //var url2 = @"D:\test\IMG_1498.JPG";
            var url3 = @"D:\Sam Weiss D\heap\Chrome cannary.zip";

            using (HttpClient client = new HttpClient())
            {
                using (var fs = File.OpenRead(url3))
                {
                     var httpContent = new MultipartFormDataContent();
                    httpContent.Add(new StreamContent(fs), "\"file\"", "\"dfhgfjhj.7z\"");
                    //httpContent.Add(new StringContent(@"6VA8orNe7W%2BEZjeHlRvzTUN9a%2FtpuDXInVr6G02pkFFA%2FJMkqi%2B%2Fm9iFnADnTM7YSuVTcmS2vj%2BG1BEIXxXsYnS%2FCx8sYMNNUsFMPbBQbQEp8WPhdv9rh3qK3WUkjaVX"), "token");
                    var result = client.PostAsync(@"http://localhost:33334/BD/TuCao/TuCaoUploadAttachments?token=6VA8orNe7W%2BEZjeHlRvzTUN9a%2FtpuDXInVr6G02pkFFA%2FJMkqi%2B%2Fm9iFnADnTM7YSuVTcmS2vj%2BG1BEIXxXsYnS%2FCx8sYMNNUsFMPbBQbQEp8WPhdv9rh3qK3WUkjaVX", httpContent).Result;
                }
            }
        }
    }
}
