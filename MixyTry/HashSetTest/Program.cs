using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashSetTest
{
    class Program
    {
        static void Main(string[] args)
        {

            test1();
            test2();
        }

        static void test2()
        {
            Dictionary<DateTime, bool> dic = new Dictionary<DateTime, bool>();
            dic.Add(new DateTime(2016, 1, 1),true);
            dic.Add(new DateTime(2016, 1, 1), true);

        }


        static void test1()
        {
            HashSet<string> test = new HashSet<string>();
            test.Add("1");
            test.Add("1");
        }
    }
}
