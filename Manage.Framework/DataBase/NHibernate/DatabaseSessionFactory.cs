using NHibernate;
using NHibernate.Cfg;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace Manage.Framework
{
    internal delegate void BuildSessionFactoryHandle(ISessionFactory sessionFactory);

    /// <summary>
    /// NHibernate会话工厂辅助类
    /// </summary>
    internal sealed class DatabaseSessionFactory
    {
        #region Private Fields

        internal event BuildSessionFactoryHandle BuildedSessionFactory;

        private object helpLock = new object();

        private static System.Collections.Generic.Dictionary<string, ISessionFactory> sessionFactoryDict = new System.Collections.Generic.Dictionary<string, ISessionFactory>();

        /// <summary>
        /// 会话工厂实例
        /// </summary>
        private ISessionFactory sessionFactory = null;

        /// <summary>
        /// 会话实例
        /// </summary>
        private ISession session = null;

        private static Dictionary<string, Configuration> _configpool = new Dictionary<string, Configuration>();

        public string FactoryName
        {
            get;
            set;
        }
        #endregion

        #region Constructors

        internal DatabaseSessionFactory(NHibernate.Cfg.Configuration nhibernateConfig, bool reBuildSessionFactory = false)
        {
            string sessionFactoryKey = nhibernateConfig.GetProperty("session_factory_name");
            if (sessionFactoryDict.ContainsKey(sessionFactoryKey) && !reBuildSessionFactory)
            {
                sessionFactory = sessionFactoryDict[sessionFactoryKey];
            }
            else
            {
                //TODO:调试用的
                //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
                
                sessionFactory = nhibernateConfig.BuildSessionFactory();

                if (BuildedSessionFactory != null)
                {
                    BuildedSessionFactory(sessionFactory);
                }

                lock (helpLock)
                {
                    if (sessionFactoryDict.ContainsKey(sessionFactoryKey))
                    {
                        sessionFactoryDict[sessionFactoryKey] = sessionFactory;
                    }
                    else
                    {
                        sessionFactoryDict.Add(sessionFactoryKey, sessionFactory);
                    }
                }
            }
            this.FactoryName = sessionFactoryKey;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFileName">配置文件路径名称</param>
        internal DatabaseSessionFactory(string configFileName)
        {
            //string sessionFactoryKey = configFileName;
            //if (string.IsNullOrEmpty(configFileName) || configFileName.ToLower().EndsWith("hibernate.cfg.xml"))//如果是默认值为空时，给它设置一个固定key值
            //    sessionFactoryKey = "hibernate";
            Configuration nhibernateConfig;
          
            if (!_configpool.ContainsKey(configFileName))
            {
                nhibernateConfig=  new Configuration();
                if (string.IsNullOrEmpty(configFileName))
                {
                    
                    nhibernateConfig.Configure();
                }
                else
                {
                    if (File.Exists(configFileName))
                    {
                        nhibernateConfig.Configure(configFileName);
                    }
                    else
                    {
                        var newConfigFileName = configFileName.Replace("~", "");
                        var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
                        if (basePath.EndsWith("\\"))
                        {
                            basePath = basePath.Substring(0, basePath.LastIndexOf("\\"));
                        }
                        newConfigFileName = basePath + newConfigFileName;
                        nhibernateConfig.Configure(newConfigFileName);
                    }
                }
                _configpool.Add(configFileName, nhibernateConfig);
            }
            else
            {
                nhibernateConfig = _configpool[configFileName];
            }
            
            string sessionFactoryKey = nhibernateConfig.GetProperty("session_factory_name");

            if (sessionFactoryDict.ContainsKey(sessionFactoryKey))
            {
                sessionFactory = sessionFactoryDict[sessionFactoryKey];
            }
            else
            {
                
                //TODO:调试用的
                // HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
                sessionFactory = nhibernateConfig.BuildSessionFactory();
                lock (helpLock)
                {
                    sessionFactoryDict.Add(sessionFactoryKey, sessionFactory);
                }
            }
            this.FactoryName = sessionFactoryKey;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Nhibernate会话实例，如果会话没有打开或创建，那么就会从会话工厂中创建一个新会话的。
        /// </summary>
        public ISession Session
        {
            get
            {
                try
                {
                    ISession result = session;
                    if (result != null && result.IsOpen)
                        return result;
                    return OpenSession();
                }
                catch
                { throw; }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 从会话工厂中创建一个新会话
        /// </summary>
        /// <returns>创建一个新会话</returns>
        public ISession OpenSession()
        {
            this.session = sessionFactory.OpenSession();
            return this.session;
        }
        #endregion

    }
}
