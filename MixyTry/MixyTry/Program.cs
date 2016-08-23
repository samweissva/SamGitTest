using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixyTry
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<A>();
            list.Add(new A { name = "ff" });
            list.Add(new A { name = "445" });
            list.Add(new A { name = "6677" });
            foreach (var item in list)
            {
                //item = new A { name = "--995" };
                item.name = "-------==";
            }

        }
    }



    class A
    {
        public string name { get; set; }
    }
}
