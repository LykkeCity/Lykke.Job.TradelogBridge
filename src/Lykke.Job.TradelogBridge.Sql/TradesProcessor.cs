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
        private const string _format = "yyyy-MM-dd";
        private readonly Dictionary<string, List<TradeLogItem>> _dict = new Dictionary<string, List<TradeLogItem>>();
        private DateTime _cacheDate = DateTime.MinValue;

        public async Task AddToContextAsync(object item, DbContextExt context)
        {
            var dbContext = context as DataContext;
            if (dbContext != null && item is TradeLogItem newObject)
                await dbContext.Trades.AddAsync(newObject);
            else
                await context.AddAsync(item);
        }

        public async Task<bool> FindCopyInDbAsync(object newObject, DbContextExt context)
        {
            if (!(newObject is TradeLogItem item) || !(context is DataContext dbContext))
                return false;

            if (_dict == null
                || _dict.Count == 0 && item.DateTime.Date != _cacheDate
                || _dict.Count > 0 && _dict.First().Value.First().DateTime.Date != item.DateTime.Date)
            {
                _dict.Clear();
                _cacheDate = item.DateTime.Date;
            }

            if (!_dict.ContainsKey(item.TradeId))
            {
                string query = $"SELECT * FROM dbo.{DataContext.TradesTable} WHERE TradeId = '{item.TradeId}'";
                var items = dbContext.Trades.FromSql(query).ToList();
                _dict[item.TradeId] = items;
            }

            var fromDb = _dict[item.TradeId].FirstOrDefault(c =>
                c.WalletId == item.WalletId
                && c.Asset == item.Asset);
            return fromDb != null;
        }
    }
}
