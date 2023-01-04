using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Client
{
    public class ClientInfo
    {        
        /// <summary>
        /// 宿主应用ID
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// 服务接口URL
        /// </summary>
        public string service_url { get; set; }
        /// <summary>
        /// 存储文件夹
        /// </summary>
        public string storage_dir { get; set; }
    }
}
