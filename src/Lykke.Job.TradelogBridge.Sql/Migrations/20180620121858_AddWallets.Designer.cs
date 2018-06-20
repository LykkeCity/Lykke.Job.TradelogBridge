﻿// <auto-generated />
using System;
using Lykke.Job.TradelogBridge.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lykke.Job.TradelogBridge.Sql.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180620121858_AddWallets")]
    partial class AddWallets
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lykke.Job.TradelogBridge.Sql.Models.TradeLogItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("OppositeAsset")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("OppositeOrderId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal?>("OppositeVolume")
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("TradeId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("WalletId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("Lykke.Job.TradelogBridge.Sql.Models.TradeLogItemFee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("FromClientId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal?>("Size")
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("SizeType")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ToClientId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<long>("TradeLogItemId")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,8)");

                    b.HasKey("Id");

                    b.HasIndex("TradeLogItemId")
                        .IsUnique();

                    b.ToTable("FeeTradeLogItem");
                });

            modelBuilder.Entity("Lykke.Job.TradelogBridge.Sql.Models.Wallet", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Lykke.Job.TradelogBridge.Sql.Models.TradeLogItemFee", b =>
                {
                    b.HasOne("Lykke.Job.TradelogBridge.Sql.Models.TradeLogItem")
                        .WithOne("Fee")
                        .HasForeignKey("Lykke.Job.TradelogBridge.Sql.Models.TradeLogItemFee", "TradeLogItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
