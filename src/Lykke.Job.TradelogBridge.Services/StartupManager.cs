using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using JetBrains.Annotations;
using Lykke.Job.TradelogBridge.Core.Services;

namespace Lykke.Job.TradelogBridge.Services
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly List<IStartable> _startables = new List<IStartable>();

        public Task StartAsync()
        {
            foreach (var startable in _startables)
            {
                startable.Start();
            }

            return Task.CompletedTask;
        }

        public void Register(IStartable startable)
        {
            _startables.Add(startable);
        }
    }
}
