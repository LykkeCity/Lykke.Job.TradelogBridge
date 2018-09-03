using Common.Log;
using Lykke.Service.DataBridge.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Job.TradelogBridge.Sql.Models
{
    public class Wallet : IDbIdentity
    {
        public static int MaxStringFieldsLength => 255;

        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public string UserId { get; set; }

        public object DbId => Id;

        public void Add(DbContext context, ILog log)
        {
            context.Add(this);
        }

        public void FillMissingData(DbContext context)
        {
        }

        public bool Update(object newVersion)
        {
            return false;
        }
    }
}
