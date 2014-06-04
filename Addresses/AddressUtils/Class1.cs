using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressUtils
{
    public class ClipboardUtils
    {
        public static void CopyToClipboard(params string[] p)
        {
            string contents = "";
            foreach (String s in p)
                contents = contents + s + "\r\n";

            Clipboard.Clear();
            Clipboard.SetText(contents);
        }
    }
}
