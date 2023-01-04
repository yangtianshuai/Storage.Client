using System;
using System.Collections.Generic;

namespace Storage.Client
{
    public class StorageConfig : IComparable
    {
        public string store_id { get; set; }
        public int slice_no { get; set; }
        /// <summary>
        /// 每片大小
        /// </summary>
        public long size { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is StorageConfig)
            {
                var p = obj as StorageConfig;
                return this.slice_no.CompareTo(p.slice_no);
            }
            return 0;
        }
    }
}
