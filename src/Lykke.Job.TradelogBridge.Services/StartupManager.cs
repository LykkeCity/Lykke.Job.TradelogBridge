﻿using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Job.TradelogBridge.Core.Services;

namespace Lykke.Job.TradelogBridge.Services
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        public async Task StartAsync()
        {
            // TODO: Implement your startup logic here. Good idea is to log every step

            await Task.CompletedTask;
        }
    }
}
