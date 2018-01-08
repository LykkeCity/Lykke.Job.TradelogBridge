using Microsoft.EntityFrameworkCore;
using Lykke.Service.DataBridge.Data.Abstractions;
using Lykke.Job.TradelogBridge.Sql.Models;
using System;
using System.Collections.Generic;

namespace Lykke.Job.TradelogBridge.Sql
{
    public class DataContext : DbContextExt
    {
        private const string _tradesTableName = "Trades";

        public virtual DbSet<TradeLogItem> Trades { get; set; }

        public DataContext()
            : base(new DbContextOptionsBuilder<DataContext>().Options)
        {
        }

        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public override List<string> GetTableNames(Type type)
        {
            return new List<string>(1) { _tradesTableName };
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TradeLogItem>(entity =>
            {
                entity.Property(e => e.Id).UseSqlServerIdentityColumn().HasColumnType("bigint");
                entity.Property(e => e.TradeId).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.UserId).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.WalletId).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.OrderId).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.OrderType).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.Direction).IsRequired().HasColumnType($"varchar({ TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.Asset).IsRequired().HasColumnType($"varchar({TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.Volume).IsRequired().HasColumnType("decimal(18,8)");
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,8)");
                entity.Property(e => e.DateTime).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.OppositeOrderId).IsRequired().HasColumnType($"varchar({TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.OppositeAsset).HasColumnType($"varchar({TradeLogItem.MaxStringFieldsLength})");
                entity.Property(e => e.OppositeVolume).HasColumnType("decimal(18,8)");
                entity.Property(e => e.IsHidden).HasColumnType("bit");
                entity.HasOne(e => e.FeeInstruction).WithOne().HasForeignKey<TradeLogItemFee>(i => i.TradeLogItemId);
                entity.ToTable(_tradesTableName);
            });

            modelBuilder.Entity<TradeLogItemFee>(entity =>
            {
                entity.Property(e => e.Id).UseSqlServerIdentityColumn().HasColumnType("bigint");
                entity.Property(e => e.SourceClientId).HasColumnType($"varchar({ TradeLogItemFee.MaxStringFieldsLength})");
                entity.Property(e => e.TargetClientId).HasColumnType($"varchar({ TradeLogItemFee.MaxStringFieldsLength})");
                entity.Property(e => e.Type).IsRequired().HasColumnType($"varchar({ TradeLogItemFee.MaxStringFieldsLength})");
                entity.Property(e => e.SizeType).HasColumnType($"varchar({ TradeLogItemFee.MaxStringFieldsLength})");
                entity.Property(e => e.TradeLogItemId).IsRequired().HasColumnType("bigint");
                entity.Property(e => e.Size).HasColumnType("float");
                entity.ToTable("TradeLogItemFee");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
