using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Client
{
    /// <summary>
    /// ResponseResult
    /// </summary>
    internal class ResponseResult
    {
        /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }
        private object data;
        /// <summary>
        /// Data
        /// </summary>
        public object Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
                if (value != null)
                {
                    Code = 1;
                }
            }
        }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// ResponseResult
        /// </summary>
        public ResponseResult()
        {
            Code = 0;
            data = null;
            Message = "";
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message">提示消息</param>
        public void Sucess(string message)
        {
            Code = 1;
            if (message != null)
            {
                Message = message;
            }
        }
        /// <summary>
        /// IsSuccess
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return Code == 1;
        }
    }
    internal class HttpStorageService : StorageService
    {
        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="id"></param>
        public async override Task<bool> HeartBeatAsync(string id)
        {
            var url = $"{Client.service_url}/server/ping?id={id}";
            var res = JsonConvert.DeserializeObject<ResponseResult>(await HttpHelper.GetAsync(url));
            return res.IsSuccess();
        }

        /// <summary>
        /// 注册节点
        /// </summary>
        /// <param name="storeType"></param>
        /// <returns></returns>
        public async override Task<Dictionary<string, JToken>> RegisterAsync(FileStoreType storeType)
        {
            var url = $"{Client.service_url}/server/register?id={Client.app_id}&&type={storeType.ToString("d")}";
            var res = JsonConvert.DeserializeObject<ResponseResult>(await HttpHelper.PostAsync(url,null));
            if (res == null || !res.IsSuccess())
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Dictionary<string, JToken>>(res.Data.ToString());
        }

        public async override Task<bool> ReportAsync(FtpReportInfo report)
        {
            var url = $"{Client.service_url}/ftp/report";
            var json = JsonConvert.SerializeObject(report);
            var res = JsonConvert.DeserializeObject<ResponseResult>(
                await HttpHelper.PostAsync(url, new StringContent(json, Encoding.UTF8, "text/json")));

            return res.IsSuccess();
        }
       

        public override async Task<PreUploadView> UploadAsync(PreStoreSubmit submit)
        {
            var url = $"{Client.service_url}/upload/plan";
            var json = JsonConvert.SerializeObject(submit);
            var res = JsonConvert.DeserializeObject<ResponseResult>(
                await HttpHelper.PostAsync(url, new StringContent(json, Encoding.UTF8, "text/json")));
            if (res == null || !res.IsSuccess())
            {
                return null;
            }
            var json_obj = JsonConvert.DeserializeObject<JObject>(res.Data.ToString());
            return new PreUploadView
            {
                file_id = json_obj["file_id"]?.ToString(),
                mirror_id = json_obj["mirror_id"]?.ToString(),
                configs = json_obj["configs"]?.ToObject<List<StorageConfig>>()
            };
        }

        public override async Task<JObject> GetFtpAsync(string id)
        {
            var url = $"{Client.service_url}/ftp/GetStore?id={id}";
            var res = JsonConvert.DeserializeObject<ResponseResult>(await HttpHelper.GetAsync(url));
            if (res == null || !res.IsSuccess())
            {
                return null;
            }
            return JsonConvert.DeserializeObject<JObject>(res.Data.ToString());
        }
    }
}
