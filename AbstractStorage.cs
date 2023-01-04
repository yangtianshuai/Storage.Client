using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Client
{
    /// <summary>
    /// 存储抽象类
    /// </summary>
    public abstract class AbstractStorage
    {        
        internal Dictionary<string, JToken> Configs { get; set; }

        protected readonly string url = "/storage";
        /// <summary>
        /// 存储服务
        /// </summary>
        public StorageService Service { get; set; }
        /// <summary>
        /// 设备存储
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> Save(StorageTask task);
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
