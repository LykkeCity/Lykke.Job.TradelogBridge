using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Lykke.Job.TradelogBridge.Core.Services;

namespace Lykke.Job.TradelogBridge.Services
{
    public class ShutdownManager : IShutdownManager
    {
        private readonly List<IStartStop> _startStops = new List<IStartStop>();
        private readonly List<IStopable> _stopables = new List<IStopable>();

        public ShutdownManager(IEnumerable<IStartStop> startStops, IEnumerable<IStopable> stopables)
        {
            _startStops.AddRange(startStops);
            _stopables.AddRange(stopables);
        }

        public Task StopAsync()
        {
            Parallel.ForEach(_startStops, i => i.Stop());
            Parallel.ForEach(_stopables, i => i.Stop());
            return Task.CompletedTask;
        }
    }
}
