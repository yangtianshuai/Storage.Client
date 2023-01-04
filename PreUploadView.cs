using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Client
{
    public class PreUploadView
    {
        public string file_id { get; set; }

        public string mirror_id { get; set; }

        public List<StorageConfig> configs { get; set; }
    }
}
