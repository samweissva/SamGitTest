using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntNull
{
    class Program
    {
        static void Main(string[] args)
        {
            int? g = 5;
            var model = new HH()
            {
                num = g == 2 ? (int?)null : 3,
            };
        }
    }

    public class HH
    {
        public int? num { get; set; }
    }
}
