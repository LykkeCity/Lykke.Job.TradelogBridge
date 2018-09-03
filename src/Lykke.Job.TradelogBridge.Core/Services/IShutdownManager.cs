using System.Threading.Tasks;

namespace Lykke.Job.TradelogBridge.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
