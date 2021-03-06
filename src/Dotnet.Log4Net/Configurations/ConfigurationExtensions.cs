﻿using Dotnet.Dependency;
using Dotnet.Extensions;
using Dotnet.Logging;

namespace Dotnet.Configurations
{
    public static class ConfigurationExtensions
    {
        /// <summary>使用Log4Net日志记录
        /// </summary>
        public static Configuration UseLog4Net(this Configuration configuration, string configFile = "")
        {
            var container = IocManager.GetContainer();
            if (configFile.IsNullOrEmpty())
            {
                configFile = "log4net.config";
            }
            container.Register<ILoggerFactory, Log4NetLoggerFactory>(new Log4NetLoggerFactory(configFile));
            container.Register<ILogger, Log4NetLogger>(DependencyLifeStyle.Transient);
            return configuration;
        }
    }
}
