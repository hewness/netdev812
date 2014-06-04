using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUtils
{
    public class Maps
    {
        public static void Lookup(string address, string csz)
        {
            string combined = address + ", " + csz;        // Add+CSZ
            string converted = combined.Replace(' ', '+'); // repl sp with +
            string args = "maps.google.com/maps?q=" + converted;// Full URL

            // Fire off IE as a new process
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = "iexplore.exe";
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
    }
}
