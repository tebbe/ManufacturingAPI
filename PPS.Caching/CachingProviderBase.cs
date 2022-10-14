using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Caching
{
    public abstract class CachingProviderBase
    {
        public CachingProviderBase()
        {
            DeleteLog();
        }

        protected MemoryCache cache = new MemoryCache("CachingProvider");

        static readonly object padlock = new object();

        protected virtual void AddItem(string key, object value)
        {
            lock (padlock)
            {
                if (!cache.Contains(key))
                {
                    //cache.Add(key, value, DateTimeOffset.Now.AddDays(3));
                }
            }
        }

        //protected virtual void RemoveItem(string key)
        //{
        //    lock (padlock)
        //    {
        //        cache.Remove(key);
        //    }
        //}

        protected virtual object GetItem(string key)
        {
            lock (padlock)
            {
                var res = cache[key];
                return res;
            }
        }

        protected virtual object InvalidateItem(string key)
        {
            lock (padlock)
            {
                var res = cache[key];
                if (cache.Contains(key))
                {
                    cache.Remove(key);
                }
                return res;
            }
        }

        #region Error Logs

        string LogPath = System.Environment.GetEnvironmentVariable("TEMP");

        protected void DeleteLog()
        {
            System.IO.File.Delete(string.Format("{0}\\CachingProvider_Errors.txt", LogPath));
        }

        protected void WriteToLog(string text)
        {
            using (System.IO.TextWriter tw = System.IO.File.AppendText(string.Format("{0}\\CachingProvider_Errors.txt", LogPath)))
            {
                tw.WriteLine(text);
                tw.Close();
            }
        }

        #endregion
    }
}
