using System;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Common.Log;
using AzureStorage.Blob;
using Lykke.Common;
using Lykke.SettingsReader;
using Lykke.Service.DataBridge.Data;
using Lykke.Job.TradesConverter.Contract;
using Lykke.Job.TradelogBridge.Core.Services;
using Lykke.Job.TradelogBridge.Services;
using Lykke.Job.TradelogBridge.Sql;
using Lykke.Job.TradelogBridge.Settings;

namespace Lykke.Job.TradelogBridge.Modules
{
    public class JobModule : Module
    {
        private readonly TradelogBridgeSettings _settings;
        private readonly IReloadingManager<TradelogBridgeSettings> _settingsManager;
        private readonly IConsole _console;
        private readonly ILog _log;

        public JobModule(
            IReloadingManager<TradelogBridgeSettings> settingsManager,
            IConsole console,
            ILog log)
        {
            _settings = settingsManager.CurrentValue;
            _settingsManager = settingsManager;
            _console = console;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var dbContextFactory = new DbContextExtFactory();
            using (var context = dbContextFactory.CreateInstance(_settings.SqlConnString))
            {
                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(15));
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
                .As<IStartupManager>();

            builder.RegisterResourcesMonitoring(_log);

            var shutdownManager = new ShutdownManager(_log);
            builder.RegisterInstance(shutdownManager)
                .As<IShutdownManager>()
                .SingleInstance();

            var blobStorage = AzureBlobStorage.Create(_settingsManager.ConnectionString(i => i.BlobStorageConnString));

            var repository = new DataRepository<TradeLogItem, DataContext>(
                _settings.SqlConnString,
                _settings.Rabbit.ExchangeName,
                _settings.MaxBatchCount,
                blobStorage,
                blobsCheckPeriodInSeconds: 3,
                warningPeriodInMinutes: 10,
                warningSqlTableSizeInGigabytes: _settings.WarningSqlTableSizeInGigabytes,
                batchPeriodInMilliseconds: _settings.BatchPeriodInSeconds * 1000,
                log: _log,
                dbContextFatory: new DbContextExtFactory(),
                entityMapper: new DbEntityMapper(),
                notIdentifiableItemsProcessor: new TradesProcessor());
            builder
                .RegisterInstance(repository)
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            var subscriber = new TradelogSubscriber(
                _settings.Rabbit.ConnectionString,
                _settings.Rabbit.ExchangeName,
                repository,
                _console,
                _log);

            builder.RegisterInstance(subscriber)
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            shutdownManager.AddStopSequence(subscriber, repository);
        }
    }
}
