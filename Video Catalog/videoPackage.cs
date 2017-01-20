using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Catalog
{
    public class videoPackage
    {
        public string genreName { get; set; }
        public List<FileInfo> videos { get; set; }
    }
}
