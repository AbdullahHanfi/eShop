using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Helper
{
    public class PhotoSettings
    {
        public string DestinationFolder { get; set; }
        public long MaxFileSizeBytes { get; set; }
        public List<string> AllowedExtensions { get; set; }
        public List<string> AllowedContentTypes { get; set; }
    }
}
