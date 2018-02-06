using System.Threading.Tasks;
using Lykke.Service.DataBridge.Data.Abstractions;
using Lykke.Job.TradelogBridge.Sql.Models;

namespace Lykke.Job.TradelogBridge.Services
{
    public class DbEntityMapper : IDbEntityMapper
    {
        public Task<object> MapEntityAsync(object item)
        {
            if (item is TradesConverter.Contract.TradeLogItem model)
            {
                var converted = new TradeLogItem
                {
                    TradeId = model.TradeId,
                    UserId = model.UserId, //model.HashedUserId,
                    WalletId = model.WalletId,
                    Direction = model.Direction.ToString(),
                    OrderType = model.OrderType,
                    OrderId = model.OrderId,
                    Asset = model.Asset,
                    Volume = model.Volume,
                    Price = model.Price,
                    DateTime = model.DateTime,
                    OppositeOrderId = model.OppositeOrderId,
                    OppositeAsset = model.OppositeAsset,
                    OppositeVolume = model.OppositeVolume,
                    IsHidden = model.IsHidden,
                };
                if (model.WalletType == "Trading")
                    converted.WalletId = converted.UserId;
                if (model.Fee != null)
                    converted.FeeInstruction = new TradeLogItemFee
                    {
                        Type = model.Fee.Type,
                        SourceClientId = model.Fee.SourceClientId,
                        TargetClientId = model.Fee.TargetClientId,
                        SizeType = model.Fee.SizeType,
                        Size = model.Fee.Size,
                    };
                item = converted;
            }
            return Task.FromResult(item);
        }
    }
}
