using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Client
{
    public class FileListInfo
    {
        public string id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string file_ext { get; set; }   

        public List<string> mirrors { get; set; }

        public byte[] content { get; set; }
        public string content_type { get; set; }

        public string file_path { get; set; }

    }
}
