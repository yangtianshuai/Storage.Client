namespace Storage.Client
{
    public class PreStoreSubmit
    {
        public string name { get; set; }
        /// <summary>
        /// 文件字节大小
        /// </summary>
        public long size { get; set; }
        public string file_ext { get; set; }
        public FileStoreType file_type { get; set; }
        public string md5 { get; set; }
        /// <summary>
        /// 文件镜像
        /// </summary>
        public string url { get; set; }
    }
}
