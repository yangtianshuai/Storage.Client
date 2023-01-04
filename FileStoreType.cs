using System.ComponentModel;

namespace Storage.Client
{
    public enum FileStoreType
    {
        /// <summary>
        /// FTP
        /// </summary>
        [Description("FTP")]
        Ftp = 1,
        /// <summary>
        /// MongoDB
        /// </summary>
        [Description("MongoDB")]
        MongoDB = 2,
        /// <summary>
        /// Haddoop
        /// </summary>
        [Description("Haddoop")]
        Haddoop = 3
    }
}
