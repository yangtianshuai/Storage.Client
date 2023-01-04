using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Client
{
    public class FtpReportInfo
    {
        public string file_id { get; set; }
        public int file_no { get; set; }
        public string store_ftp_id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string path { get; set; }
    }
}
