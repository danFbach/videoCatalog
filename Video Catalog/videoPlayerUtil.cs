using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Catalog
{
    public class videoPlayerUtil
    {
        public void play(FileInfo video)
        {
            printUtil _pu = new printUtil();
            _pu.write(_pu.br + _pu.br + " Now playing " + video.Name + ".", _pu.grn);
            _pu.rest(1000);
            Process.Start(video.FullName);
            for (int i = 0;i < 10;i++)
            {
                _pu.rest(50);
                _pu.write(".", _pu.grn);
            }
        }
    }
}
