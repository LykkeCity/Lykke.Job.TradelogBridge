using System;
using Autofac;
using AzureStorage.Blob;
using Common;
using Common.Log;
using Lykke.Common;
using Lykke.Job.TradelogBridge.Core.Services;
using Lykke.Job.TradelogBridge.Services;
using Lykke.Job.TradelogBridge.Settings;
using Lykke.Job.TradelogBridge.Sql;
using Lykke.Job.TradelogBridge.Sql.Models;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.DataBridge.Data;
using Lykke.SettingsReader;
using Microsoft.EntityFrameworkCore;

namespace Lykke.Job.TradelogBridge.Modules
{
    public class JobModule : Module
    {
        private readonly AppSettings _appSettings;
        private readonly IReloadingManager<AppSettings> _settingsManager;
        private readonly IConsole _console;
        private readonly ILog _log;

        public JobModule(
            IReloadingManager<AppSettings> settingsManager,
            IConsole console,
            ILog log)
        {
            _appSettings = settingsManager.CurrentValue;
            _settingsManager = settingsManager;
            _console = console;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbContextFactory = new DbContextExtFactory();
            using (var context = dbContextFactory.CreateInstance(_appSettings.TradelogBridgeJob.SqlConnString))
            {
                context.Database.Migrate();
            }

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterInstance(_console)
                .As<IConsole>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterResourcesMonitoring(_log);

            builder.RegisterLykkeServiceClient(_appSettings.ClientAccountServiceClient.ServiceUrl);

            var blobStorage = AzureBlobStorage.Create(_settingsManager.ConnectionString(i => i.TradelogBridgeJob.BlobStorageConnString));

            var settings = _appSettings.TradelogBridgeJob;

            var tradesRepository = new DataRepository<TradesConverter.Contract.TradeLogItem, DataContext>(
                settings.SqlConnString,
                settings.MaxBatchCount,
                blobStorage,
                blobsCheckPeriodInSeconds: 3,
                warningPeriodInMinutes: 10,
                warningSqlTableSizeInGigabytes: settings.WarningSqlTableSizeInGigabytes,
                batchPeriodInMilliseconds: settings.BatchPeriodInSeconds * 1000,
                log: _log,
                dbContextFatory: new DbContextExtFactory(),
                entityMapper: new DbEntityMapper(),
                notIdentifiableItemsProcessor: new TradesProcessor());
            builder
                .RegisterInstance(tradesRepository)
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();

            var walletsRepository = new DataRepository<Wallet, DataContext>(
                settings.SqlConnString,
                settings.MaxBatchCount,
                blobStorage,
                blobsCheckPeriodInSeconds: 3,
                warningPeriodInMinutes: 10,
                warningSqlTableSizeInGigabytes: settings.WarningSqlTableSizeInGigabytes,
                batchPeriodInMilliseconds: settings.BatchPeriodInSeconds * 1000,
                log: _log,
                dbContextFatory: new DbContextExtFactory());
            builder
                .RegisterInstance(walletsRepository)
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();

            builder.RegisterType<TradelogSubscriber>()
                .As<IStartStop>()
                .SingleInstance()
                .WithParameter("connectionString", settings.Rabbit.ConnectionString)
                .WithParameter("exchangeName", settings.Rabbit.ExchangeName)
                .WithParameter("tradesRepository", tradesRepository)
                .WithParameter("walletsRepository", walletsRepository);
        }
    }
}
