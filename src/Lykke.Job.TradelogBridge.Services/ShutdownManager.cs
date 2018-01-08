using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TradelogBridge.Core.Services;

namespace Lykke.Job.TradelogBridge.Services
{
    public class ShutdownManager : IShutdownManager
    {
        private readonly ILog _log;
        private readonly List<ICollection<IStopable>> _stopSequences = new List<ICollection<IStopable>>();

        public ShutdownManager(ILog log)
        {
            _log = log;
        }

        public async Task StopAsync()
        {
            foreach (var stopSequence in _stopSequences)
            {
                foreach (var item in stopSequence)
                {
                    item.Stop();
                }
            }
        }

        public void AddStopSequence(params IStopable[] stopSequence)
        {
            _stopSequences.Add(stopSequence);
        }
    }
}
