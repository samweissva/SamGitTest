using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sycn
{
    class Program
    {
        static void Main(string[] args)
        {
            GetTest();
        }

        private static StringBuilder sb = new StringBuilder();
        public static dynamic GetTest()
        {
            sb.AppendLine("Thread.CurrentThread.ManagedThreadId1:" + Thread.CurrentThread.ManagedThreadId);
            //var result = Task.Run(Test4).Result.Content.ReadAsStringAsync().Result;
            var result = Test3().Result;
            sb.AppendLine("Thread.CurrentThread.ManagedThreadId4:" + Thread.CurrentThread.ManagedThreadId);
            return result;
        }

        public static async Task<string> Test3()
        {
            sb.AppendLine("Thread.CurrentThread.ManagedThreadId2:" + Thread.CurrentThread.ManagedThreadId);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://www.baidu.com");
                sb.AppendLine("Thread.CurrentThread.ManagedThreadId3:" + Thread.CurrentThread.ManagedThreadId);
                return await response.Content.ReadAsStringAsync();
                //return await client.GetStringAsync("http://www.baidu.com");
            }
        }
    }
}
