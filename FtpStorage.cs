using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Client
{
    /// <summary>
    /// FTP存储
    /// </summary>
    public class FtpStorage : AbstractStorage
    {       
        public async override Task<bool> SaveAsync(StorageTask task)
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
            var directory = store["remote_dir"]?.ToString();
            bool has_dir = false;
            if (!ftpClient.CheckFileExist(directory))
            {
                try
                {
                    ftpClient.MakeDirectory(directory);
                }
                catch
                {
                    has_dir = true;
                }            
            }
            ftpClient.RemotePath = directory;
            directory = store["store_dir"]?.ToString();
            if(!has_dir && !string.IsNullOrEmpty(directory) && !ftpClient.CheckFileExist(directory))
            {
                try
                {
                    ftpClient.MakeDirectory(directory);
                }
                catch
                { }
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

        private byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }

        public override async Task<List<FileSegment>> LoadAsync(string file_id)
        {
            var files = new ConcurrentBag<FileSegment>();

            var list = await Service.GetWorkListAsync(file_id);

            Parallel.ForEach(list, item =>
            {
                //FTP上传
                var ftpClient = new FtpClient
                {
                    Host = item["ip"]?.ToString(),
                    UserId = item["user_name"]?.ToString(),
                    Password = item["password"]?.ToString(),
                    Port = int.Parse(item["port"]?.ToString())
                };             
                ftpClient.RemotePath = item["path"]?.ToString();

                var file_name = item["name"]?.ToString();
                var local_file_name = Path.Combine(Directory.GetCurrentDirectory(), item["id"]?.ToString());
                if (ftpClient.Download(file_name, local_file_name))
                {
                    var file = new FileSegment();
                    file.file_id = file_id;
                    file.file_no = int.Parse(item["file_no"]?.ToString());

                    //file.content = FileToByteArray(local_file_name);
                    file.content = File.ReadAllBytes(local_file_name);

                    files.Add(file);
                    //删除本地下载的临时文件
                    File.Delete(local_file_name);
                }
            });

            return files.OrderBy(t=>t.file_no).ToList();
        }
    }
}
