using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Client
{
    /// <summary>
    /// FTP存储
    /// </summary>
    public class FtpStorage : AbstractStorage
    {
        public async override Task<bool> Save(StorageTask task)
        {   
            var store = await Service.GetFtpAsync(task.store_config.store_id);            
            //FTP上传
            var ftpClient = new FtpClient
            {
                Host = store["ip"]?.ToString(),
                UserId = store["user_name"]?.ToString(),
                Password = store["password"]?.ToString(),
                Port = int.Parse(store["port"]?.ToString())
            };
            ftpClient.RemotePath = store["remote_dir"]?.ToString();
            var directory = store["store_dir"]?.ToString();
            if(!string.IsNullOrEmpty(directory) && !ftpClient.CheckFileExist(directory))
            {
                ftpClient.MakeDirectory(directory);                
            }
            ftpClient.RemotePath = ftpClient.RemotePath + directory;
            if (ftpClient.Upload(new FileInfo(task.file_path),task.id))
            {
                var report = new FtpReportInfo
                {
                    file_id = task.file_id,
                    file_no = task.file_no,
                    name = task.id,
                    store_ftp_id = task.store_config.store_id,
                    path = ftpClient.RemotePath
                };
                //发送报告
                if (await Service.ReportAsync(report))
                {
                    //发送报告成功
                    return true;
                }
            }            
            return false;
        }

        public override FileStoreType GetMode()
        {
            //1代表FTP存储
            return FileStoreType.Ftp;
        }        
    }
}
