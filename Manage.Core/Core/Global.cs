using Manage.Core.Logging;
using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

namespace Manage.Core
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // 默认情况下对 Entity Framework 使用 LocalDB
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BootLoger.WriteLog(DateTime.Now, "系统正在启动中", true);

            //配置log4net
            log4net.Config.XmlConfigurator.Configure();

            BootLoger.WriteLog(DateTime.Now, "正在加载日志记录引擎...");
            LoggerFactory.Init(new Dictionary<LoggingType, ILoggerFactory>()
                {
                    {LoggingType.Text,new Log4netLogFactory()},
                    {LoggingType.TraceSource,new TraceSourceLogFactory()},
                    {LoggingType.AppDatabase,new AppDatabaseLogFactory()}
                });
            BootLoger.WriteLog(DateTime.Now, "加载日志记录引擎完成!");

            BootLoger.WriteLog(DateTime.Now, "=====================================================================\r\n系统启动完成!\r\n\r\n\r\n");
            BootLoger.Dispose();

        }
    }
}
