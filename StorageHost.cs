using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Storage.Client
{
    public class StorageHost : IDisposable
    {
        public static AbstractStorage Storage { get; private set; }

        public static ConcurrentQueue<StorageTask> tasks = new ConcurrentQueue<StorageTask>();
        private static bool flag = true;
        private static Thread thread;
        private static int step = 5000;
        private static System.Timers.Timer timer;

        public static async Task Run(AbstractStorage storage)
        {
            //向服务节点注册
            Storage = storage;
            if(await storage.RegisterAsync())
            {
                timer = new System.Timers.Timer();
                timer.Interval = 5000;
                timer.Elapsed += new ElapsedEventHandler(Elapsed);
            }            

            if (thread == null)
            {
                thread = new Thread(async () =>
                {
                    while (flag)
                    {
                        if (tasks.Count > 0)
                        {
                            if(tasks.TryDequeue(out StorageTask task))
                            {
                                Deal(task);
                            }                            
                        }
                        else
                        {
                            Thread.Sleep(step);
                        }
                    }
                });
            }
            if (!thread.IsAlive)
            {
                thread.Start();
            }
        }

        private static void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Storage == null) return;

            timer.Enabled = false;
            //发送心跳                                                                  
            Storage.Service.HeartBeatAsync(Storage.Service.Client.app_id);

            timer.Enabled = true;
        }

        public static void AddTask(FileSegment file, StorageConfig config)
        {
            if (Storage == null)
            {
                return;
            }
            //加入任务队列           
            tasks.Enqueue(new StorageTask(file, config));
        }

        public async static void Deal(StorageTask task)
        {
            //保存至对应存储
            if (await Storage.SaveAsync(task))
            {
                //关闭任务
                task.Close();
            }
        }
        public void Dispose()
        {
            flag = false;
            if (thread != null)
            {
                thread = null;
            }
        }
    }
}
