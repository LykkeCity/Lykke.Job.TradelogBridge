using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.DataBridge.Data.Abstractions;
using Lykke.Job.TradesConverter.Contract;

namespace Lykke.Job.TradelogBridge.Services
{
    public class TradelogSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly IConsole _console;
        private readonly IDataRepository _dataRepository;
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private RabbitMqSubscriber<List<TradeLogItem>> _subscriber;

        public TradelogSubscriber(
            string connectionString,
            string exchangeName,
            IDataRepository dataRepository,
            IConsole console,
            ILog log)
        {
            _connectionString = connectionString;
            _exchangeName = exchangeName;
            _dataRepository = dataRepository;
            _console = console;
            _log = log;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_connectionString, _exchangeName, "tradelogbridge")
                .MakeDurable();

            _subscriber = new RabbitMqSubscriber<List<TradeLogItem>>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        retryTimeout: TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new MessagePackMessageDeserializer<List<TradeLogItem>>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetConsole(_console)
                .SetLogger(_log)
                .Start();
        }

        private async Task ProcessMessageAsync(List<TradeLogItem> arg)
        {
            try
            {
                await _dataRepository.AddDataItemsAsync(arg);
            }
            catch (Exception exc)
            {
                await _log.WriteErrorAsync("TradelogSubscriber.ProcessMessageAsync", arg.ToJson(), exc);
            }
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }
    }
}
