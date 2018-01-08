using System.Threading.Tasks;

namespace Lykke.Job.TradelogBridge.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}