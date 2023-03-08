using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Storage.Client
{
    public class StorageTask
    {
        public string id { get; set; }
        /// <summary>
        /// 存储ID
        /// </summary>
        public StorageConfig store_config { get; set; }

        /// <summary>
        /// 文件ID
        /// </summary>
        public string file_id { get; set; }
        /// <summary>
        /// 文件序号
        /// </summary>
        public int file_no { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int file_size { get; set; }
        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string file_path { get; set; }

        private string directory;

        public StorageTask(FileSegment file, StorageConfig config)
        {
            this.store_config = config;
                  
            file_id = file.file_id;
            file_no = file.file_no;
            file_path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }
            file_path = Path.Combine(file_path, file_id);
            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }
            directory = file_path;
            file_path = Path.Combine(file_path, file_no.ToString() + ".sto");
            if (file.content != null && file.content.Length > 0)
            {
                File.WriteAllBytes(file_path, file.content);
                file_size = file.content.Length;
                file.content = null;
            }
            
            id = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 清理任务
        /// </summary>
        public void Close()
        {
            if(File.Exists(file_path))
            {
                File.Delete(file_path);
            }            
            if(Directory.Exists(directory) && Directory.GetFiles(directory).Length == 0)
            {
                //本地清理
                Directory.Delete(directory);
            }            
        }

    }
}
