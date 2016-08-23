using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShuffleList
{
    class Program
    {
        static void Main(string[] args)
        {

            test2();


        }

        static void test2()
        {
            var input = File.ReadAllText("input.txt");
            var worldList = input.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            var rand = new Random();
            worldList = worldList.OrderBy(o => rand.Next()).ToList();
            var content = string.Join("\r\n", worldList.ToArray());
            File.WriteAllText("input.txt", content);
        }

        static void test1()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var r = new Random();
            list = list.OrderBy(o => r.Next()).ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
