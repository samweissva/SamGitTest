using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IO_Byte_ReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ms = new MemoryStream())
            {
                byte[] bb = new byte[2];
                bb[0] = 1;
                bb[1] = 2;
                ms.Write(bb, 0, bb.Length);

                var sr=new StreamReader(ms);
                ms.Position = 0;


                var q1 = sr.Read();
                var q2 = sr.Read();


                byte a = (byte)'a';
                byte b = (byte)'b';

                var sw = new StreamWriter(ms);
                char[] cc = new char[2];
                cc[0] = 'a';
                cc[1] = 'b';
                sw.Write(cc);
                sw.Flush();

                ms.Position = 0;
                byte[] gg = new byte[4];
                ms.Read(gg, 0, gg.Length);
            }
        }
    }
}
