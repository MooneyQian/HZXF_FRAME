using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Manage.Framework
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 配置文件管理器实例[单例]
        /// </summary>
        public static readonly ConfigManager Instance = new ConfigManager();
        /// <summary>
        /// 配置文件扩展名
        /// </summary>
        public string FileExt { get; set; }

        public ConfigManager()
        {
            this.FileExt = ".config";
        }
        /// <summary>
        /// 多线程锁
        /// </summary>
        private object _lockhelper = new object();
        /// <summary>
        /// 存放配置字典
        /// </summary>
        private Dictionary<Type, object> _configDictionary = new Dictionary<Type, object>();
        /// <summary>
        /// 监视配置文件
        /// </summary>
        private Dictionary<string, FileSystemWatcher> _fileMoniter = new Dictionary<string, FileSystemWatcher>();
        /// <summary>
        /// 配置实体和配置文件匹配
        /// </summary>
        private Dictionary<string, Type> _configFiles = new Dictionary<string, Type>();
        /// <summary>
        /// 获取一个配置实体
        /// </summary>
        /// <typeparam name="T">配置实体泛型</typeparam>
        /// <param name="filename">配置文件地址</param>
        /// <param name="errorfunction">读取配置发生错误的处理方法</param>
        /// <returns></returns>
        public T Single<T>(string filename) where T : class,new()
        {
            lock (_lockhelper)
            {
                Type t = typeof(T);
                if (!this._configDictionary.ContainsKey(t))
                {
                    if (!string.IsNullOrEmpty(filename))
                    {
                        LoadConfig<T>(filename);
                    }
                    else
                    {
                        LoadConfig<T>(GetDefaultFilePath<T>());
                    }

                }
                return this._configDictionary[t] as T;
            }
        }

        public T Single<T>() where T : class,new()
        {
            return Single<T>("");
        }

        /// <summary>
        /// 加载配置实体
        /// </summary>
        /// <typeparam name="T">配置实体泛型</typeparam>
        /// <param name="filename">配置文件地址</param>
        /// <param name="errorfunction">读取配置发生错误的处理方法</param>
        /// <returns></returns>
        public T LoadConfig<T>(string filename) where T : class, new()
        {
            if (!File.Exists(filename))
            {
                throw new Exception("未能找到配置文件：" + filename);
            }
            try
            {
                T entity = null;
                Type entityType = typeof(T);
                using (StreamReader sr = new StreamReader(filename, Encoding.UTF8, false))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    var obj = ser.Deserialize(sr);
                    if (obj != null)
                    {
                        entity = obj as T;
                        //判断是否有空值
                        PropertyInfo[] propertys = entity.GetType().GetProperties();
                        bool flag = false;
                        foreach (PropertyInfo p in propertys)
                        {
                            if (entity.GetValue(p) == null)
                            {
                                entity.SetValue(p, string.Empty);
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            sr.Close();
                            SaveConfig<T>(entity, filename);
                            LoadConfig<T>(filename);
                        }
                    }
                    else
                    {
                        throw new Exception("未能把配置文件序列化为类型" + entityType.FullName + ";有可能配置文件内容不正确");
                    }
                }

                //当不存在配置文件和配置的匹配时，保存这种匹配
                if (!_configFiles.ContainsKey(filename))
                {
                    _configFiles.Add(filename, entityType);
                }
                string dir = Path.GetDirectoryName(filename);
                if (!this._fileMoniter.ContainsKey(dir))    //创建文件监视器
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(dir, "*" + this.FileExt);
                    watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                    this._fileMoniter.Add(dir, watcher);
                    watcher.EnableRaisingEvents = true;
                }
                lock (_lockhelper)
                {
                    //保存配置到配置字典
                    if (!this._configDictionary.ContainsKey(entityType))
                    {
                        this._configDictionary.Add(entityType, entity);
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 如果文件发生编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var watcher = (FileSystemWatcher)sender;
            lock (_lockhelper)
            {
                watcher.EnableRaisingEvents = false;
                if (_configFiles.ContainsKey(e.FullPath))
                {
                    Type t = _configFiles[e.FullPath];
                    switch (e.ChangeType)
                    {
                        case WatcherChangeTypes.Changed:
                            Thread td = new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(1000);
                                if (!this._configDictionary.ContainsKey(t))
                                {
                                    this._configDictionary.Remove(t);
                                }
                                reLoadConfig(e.FullPath, t);
                            }));
                            td.Start();
                            break;
                        case WatcherChangeTypes.Deleted:
                            Thread td1 = new Thread(new ThreadStart(() =>
                            {
                                Thread.Sleep(1000);
                                SaveConfig(this._configDictionary[t], e.FullPath);
                            }
                            ));
                            td1.Start();
                            break;
                        case WatcherChangeTypes.Renamed:
                            break;
                    }
                }
                watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// 重新读取配置
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="entityType">配置实体类型</param>
        /// <returns></returns>
        private void reLoadConfig(string filename, Type entityType)
        {

            try
            {
                object entity;
                using (StreamReader sr = new StreamReader(filename, Encoding.UTF8, false))
                {
                    XmlSerializer ser = new XmlSerializer(entityType);
                    var obj = ser.Deserialize(sr);
                    if (obj != null)
                    {
                        entity = obj;
                        sr.Close();
                    }
                    else
                    {
                        sr.Close();
                        throw new Exception("未能把配置文件序列化为类型" + entityType.FullName + ";有可能配置文件内容不正确");
                    }

                }
                lock (_lockhelper)
                {
                    if (this._configDictionary.ContainsKey(entityType))
                    {
                        this._configDictionary.Remove(entityType);
                    }
                    this._configDictionary.Add(entityType, entity);
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 保存配置：把配置实体转化为配置文件
        /// </summary>
        /// <typeparam name="T">配置实体泛型</typeparam>
        /// <param name="entity">配置实体</param>
        /// <param name="filename">配置文件地址</param>
        public void SaveConfig<T>(T entity, string filename = "") where T : class, new()
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    filename = GetDefaultFilePath<T>();
                lock (_lockhelper)
                {
                    Type t = typeof(T);
                    if (this._configDictionary.ContainsKey(t))
                    {
                        this._configDictionary[t] = entity;
                    }
                }
                string dir = Path.GetDirectoryName(filename);
                if (this._fileMoniter.ContainsKey(dir))
                {
                    this._fileMoniter[dir].EnableRaisingEvents = false;
                }
                using (StreamWriter wr = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    ser.Serialize(wr, entity);
                    wr.Flush();
                    wr.Close();
                }
                if (this._fileMoniter.ContainsKey(dir))
                {
                    this._fileMoniter[dir].EnableRaisingEvents = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得默认文件路径
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetDefaultFilePath<T>()
        {
            return RequestHelper.MapPath("/Config/" + typeof(T).ToString().Substring(typeof(T).ToString().LastIndexOf('.') + 1) + FileExt);
        }
    }
}
