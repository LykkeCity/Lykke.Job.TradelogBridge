using System.Threading.Tasks;
using Common;

namespace Lykke.Job.TradelogBridge.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();

        void AddStopSequence(params IStopable[] stopSequence);
    }
}
