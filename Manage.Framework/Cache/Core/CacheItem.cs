using System;
using System.ComponentModel;
using System.IO;

namespace Manage.Framework.Cache.Core
{
    /// <summary>
    /// 内存缓存对象
    /// </summary>
    class CacheItem : IDisposable
    {
        /// <summary>
        /// 缓存时间戳
        /// </summary>
        public DateTime timestamp { get; set; }
        /// <summary>
        /// 缓存时效
        /// </summary>
        public DateTime expiryDate { get; set; }
        /// <summary>
        /// 缓存对象值
        /// </summary>
        public byte[] value { get; set; }

        public CacheItem(DateTime timestamp, byte[] value)
        {
            Initialize(timestamp, value, DateTime.Now);
        }

        public CacheItem(DateTime timestamp, byte[] value, DateTime expiryDate)
        {
            Initialize(timestamp, value, expiryDate);
        }

        internal void Initialize(DateTime timestamp, byte[] value, DateTime expiryDate)
        {
            this.timestamp = timestamp;
            this.value = value;
            this.expiryDate = expiryDate;
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                this.value = null;

                // Note disposing has been done.
                disposed = true;
            }
        }
    }
}
