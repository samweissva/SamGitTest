using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test123
{
    class Program
    {
        static void Main(string[] args)
        {
            //var date = DateTime.Now;
            //var fgg = date.Hour;
            //Dictionary<string,string> dic=new Dictionary<string, string>() { {"dd","ghgh" } };
            //var fg = dic["dd"];
            //var fg3 = dic["dd3"];

            var now = DateTime.Now;
            var dd = now.ToString("dd");

            GG hhh = (GG)4;
            var f = hhh == GG.qq;
            var f2 = hhh == GG.ww;

        }
    }

    public enum GG
    {
        qq = 1,
        ww = 2,


    }
}
