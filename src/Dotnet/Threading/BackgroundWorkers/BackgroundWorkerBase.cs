﻿using Dotnet.Dependency;
using Dotnet.Logging;
using System.Reflection;

namespace Dotnet.Threading.BackgroundWorkers
{
    public abstract class BackgroundWorkerBase : RunnableBase, IBackgroundWorker
    {
        protected ILogger Logger { get;  }
        protected BackgroundWorkerBase()
        {
            Logger = IocManager.GetContainer().Resolve<ILoggerFactory>().Create(DotnetConsts.LoggerName);
        }

        public override void Start()
        {
            base.Start();
            Logger.Debug("Start background worker: " + ToString());
        }

        public override void Stop()
        {
            base.Stop();
            Logger.Debug("Stop background worker: " + ToString());
        }

        public override void WaitToStop()
        {
            base.WaitToStop();
            Logger.Debug("WaitToStop background worker: " + ToString());
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}
