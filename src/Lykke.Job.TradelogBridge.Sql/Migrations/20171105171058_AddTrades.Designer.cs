﻿// <auto-generated />
using Lykke.Job.TradelogBridge.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Lykke.Job.TradelogBridge.Sql.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20171105171058_AddTrades")]
    partial class AddTrades
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
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
#pragma warning restore 612, 618
        }
    }
}
