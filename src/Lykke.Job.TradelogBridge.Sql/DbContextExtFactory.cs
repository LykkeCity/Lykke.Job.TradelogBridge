using Microsoft.EntityFrameworkCore;
using Lykke.Service.DataBridge.Data.Abstractions;

namespace Lykke.Job.TradelogBridge.Sql
{
    public class DbContextExtFactory : IDbContextExtFactory
    {
        public DbContextExt CreateInstance(DbContextOptions options)
        {
            return new DataContext(options);
        }

        public DataContext CreateInstance(string connectionString)
        {
            var optionsBiuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBiuilder.UseSqlServer(connectionString, opts => opts.EnableRetryOnFailure());
            return new DataContext(optionsBiuilder.Options);
        }
    }
}
