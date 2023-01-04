namespace Storage.Client
{
    /// <summary>
    /// 文件片段
    /// </summary>
    public class FileSegment
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public string file_id { get; set; }
        /// <summary>
        /// 文件序号
        /// </summary>
        public int file_no { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>

        public byte[] content { get; set; }
       
    }
}
