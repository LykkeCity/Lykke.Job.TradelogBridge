﻿using System.Threading.Tasks;
using Autofac;

namespace Lykke.Job.TradelogBridge.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();

        void Register(IStartable startable);
    }
}
