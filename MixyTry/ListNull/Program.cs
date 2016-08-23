using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListNull
{
    class Program
    {
        static void Main(string[] args)
        {
            Amodel test = null;
            setValue(out test);
        }

        public static void setValue(out Amodel input)
        {
            input = new Amodel() { gg = "old" };
            input.gg = "new";
            //input = null;
        }
    }

 

    public class Amodel
    {
        public string gg { get; set; }
    }
}
