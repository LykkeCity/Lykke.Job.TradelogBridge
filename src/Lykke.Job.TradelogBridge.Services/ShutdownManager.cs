using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Lykke.Job.TradelogBridge.Core.Services;

namespace Lykke.Job.TradelogBridge.Services
{
    public class ShutdownManager : IShutdownManager
    {
        private readonly List<ICollection<IStopable>> _stopSequences = new List<ICollection<IStopable>>();

        public Task StopAsync()
        {
            foreach (var stopSequence in _stopSequences)
            {
                foreach (var item in stopSequence)
                {
                    item.Stop();
                }
            }
            return Task.CompletedTask;
        }

        public void AddStopSequence(params IStopable[] stopSequence)
        {
            _stopSequences.Add(stopSequence);
        }
    }
}
