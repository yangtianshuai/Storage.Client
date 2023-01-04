using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Client
{
    public abstract class StorageService
    {
        public ClientInfo Client { get; set; }
        /// <summary>
        /// 向服务节点注册
        /// </summary>
        public abstract Task<Dictionary<string, JToken>> RegisterAsync(FileStoreType storeType);
        /// <summary>
        /// 向服务节点发送心跳
        /// </summary>
        /// <param name="id">客户端ID</param>
        public abstract Task<bool> HeartBeatAsync(string id);
        
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="submit"></param>
        public abstract Task<PreUploadView> UploadAsync(PreStoreSubmit submit);
        /// <summary>
        /// 获取FTP存储
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Task<JObject> GetFtpAsync(string id);
        /// <summary>
        /// 向服务节点发送报告
        /// </summary>
        public abstract Task<bool> ReportAsync(FtpReportInfo report);
    }
}
