using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Client
{
    /// <summary>
    /// 存储抽象类
    /// </summary>
    public abstract class AbstractStorage
    {
        private ConcurrentDictionary<string, string> storage = new ConcurrentDictionary<string, string>();

        public bool Exists(string file_id)
        {            
            return storage.ContainsKey(file_id);
        }

        public void Add(string file_id, string directory)
        {
            storage.TryAdd(file_id, directory);
        }

        public void Clear(string file_id)
        {
            if (storage.ContainsKey(file_id))
            {
                storage.TryRemove(file_id,out string value);
            }
        }

        internal Dictionary<string, JToken> Configs { get; set; }

        protected readonly string url = "/storage";
        /// <summary>
        /// 存储服务
        /// </summary>
        public StorageService Service { get; set; }



        /// <summary>
        /// 列出文件
        /// </summary>
        /// <returns></returns>
        public void Clear(FileListInfo storage)
        {
            if(File.Exists(storage.file_path))
            {
                File.Delete(storage.file_path);
            }            
        }

        /// <summary>
        /// 列出文件
        /// </summary>
        /// <returns></returns>
        public FileListInfo Save(FileListInfo storage)
        {
            storage.file_path = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            if (!Directory.Exists(storage.file_path))
            {
                Directory.CreateDirectory(storage.file_path);
            }
            storage.file_path = Path.Combine(storage.file_path, $"{storage.id}.{storage.file_ext}");
            File.WriteAllBytes(storage.file_path, storage.content);
            return storage;
        }

        /// <summary>
        /// 列出文件
        /// </summary>
        /// <returns></returns>
        public async Task<FileListInfo> ListAsync(string file_id)
        {
            var file = await Service.FileAsync(file_id);
            if (file != null)
            {
                var file_list = new FileListInfo
                {
                    name = file["file"]["name"]?.ToString(),
                    file_ext = file["file"]["file_ext"]?.ToString()
                };
                if (file["mirrors"] != null)
                {
                    var mirrors = JArray.FromObject(file["mirrors"]);
                    file_list.mirrors = mirrors.Select(t => t["mirror_url"]?.ToString()).ToList();
                }                
                return file_list;
            }
            return null;
        }
        /// <summary>
        /// 设备存储
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> SaveAsync(StorageTask task);

        public abstract Task<List<FileSegment>> LoadAsync(string file_id);

        /// <summary>
        /// 获取存储方式
        /// </summary>
        /// <returns></returns>
        public abstract FileStoreType GetMode();

        /// <summary>
        /// 获取上传URL
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            return url + "/upload";
        }
        /// <summary>
        /// 获取分片上传URL
        /// </summary>
        /// <returns></returns>
        public string GetSliceUrl()
        {
            return url + "/upload/slice";
        }

        public async Task<bool> RegisterAsync()
        {
            try
            {
                Configs = await Service.RegisterAsync(GetMode());
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
