using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lykke.Service.DataBridge.Data.Abstractions;
using Lykke.Job.TradelogBridge.Sql.Models;

namespace Lykke.Job.TradelogBridge.Sql
{
    public class TradesProcessor : INotIdentifiableItemsProcessor
    {
        private readonly Dictionary<string, List<TradeLogItem>> _dict = new Dictionary<string, List<TradeLogItem>>();
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(10);
        private DateTime _cacheDate = DateTime.MinValue;

        public async Task AddToContextAsync(object item, DbContextExt context)
        {
            if (context is DataContext dbContext && item is TradeLogItem newObject)
                await dbContext.Trades.AddAsync(newObject);
            else
                await context.AddAsync(item);
        }

        public Task<object> FindCopyInDbAsync(object newObject, DbContextExt context)
        {
            if (!(newObject is TradeLogItem item) || !(context is DataContext dbContext))
                return Task.FromResult((object)null);

            if (_dict.Count == 0 && item.DateTime.Subtract(_cacheDate) >= _cacheTimeout
                || _dict.Count > 0 && item.DateTime.Subtract(_dict.First().Value.First().DateTime) >= _cacheTimeout)
            {
                _dict.Clear();
                _cacheDate = item.DateTime.Date;
            }

            if (!_dict.ContainsKey(item.TradeId))
            {
                var query = $"SELECT * FROM dbo.{DataContext.TradesTable} WHERE TradeId = '{item.TradeId}'";
                var items = dbContext.Trades.FromSql(query).AsNoTracking().ToList();
                _dict[item.TradeId] = items;
            }
            var fromDb = _dict[item.TradeId].FirstOrDefault(c =>
                c.WalletId == item.WalletId
                && c.Asset == item.Asset);
            if (fromDb == null)
            {
                _dict[item.TradeId].Add(item);
                return Task.FromResult((object)null);
            }
            return Task.FromResult((object)fromDb);
        }

        public bool UpdateInContext(object oldVersion, object newVersion, DbContextExt context)
        {
            return false;
        }
    }
}
