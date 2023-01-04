using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Storage.Client
{
    public class StorageOptions
    {
        /// <summary>
        /// 宿主应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 存储实例
        /// </summary>
        public AbstractStorage Storage { get; set; } = new FtpStorage();
        /// <summary>
        /// 基本URL
        /// </summary>
        internal string base_url { get; set; }

        /// <summary>
        /// 主机域名/IP
        /// </summary>
        internal string host { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        internal int port { get; set; }
        /// <summary>
        /// 通讯方式
        /// </summary>

        private CommType comm_type { get; set; } = CommType.Http;


        //向 服务节点 注册客户端（分片处理）节点
        //分配存储服务器（FTP、MongoDB、Hadoop）
        //定时向 服务节点 发送心跳
        //拦截分片处理请求
        //本地保存分片
        //请求 服务节点 进行记录
        //将任务加入处理队列
        //存储命令执行成功，回调->向 服务节点 发送报告

        /// <summary>
        /// 采用Http实现远程调用
        /// </summary>
        /// <param name="base_url"></param>
        /// <returns></returns>
        public StorageOptions SetBaseUrl(string base_url)
        {            
            this.base_url = base_url;
            return this;
        }

        public string BaseUrl
        {
            get
            {
                return base_url;
            }
        }

        /// <summary>
        /// 采用Http实现远程调用
        /// </summary>
        /// <param name="base_url"></param>
        /// <returns></returns>
        public StorageOptions UserHttp()
        {
            comm_type = CommType.Http;
            return this;
        }

        /// <summary>
        /// 采用TCP实现远程调用
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        [Description("暂未实现该功能")]
        public StorageOptions UserTcp(string host, int port)
        {
            comm_type = CommType.Tcp;

            this.host = host;
            this.port = port;
            return this;
        }

        public StorageService GetService()
        {
            if (comm_type == CommType.Tcp)
            {
                return null;
            }
            return new HttpStorageService();
        }

    }
}
