using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.DataBridge.Data.Abstractions;
using Lykke.Job.TradelogBridge.Core.Services;
using Lykke.Job.TradelogBridge.Sql.Models;
using TradeLogItem = Lykke.Job.TradesConverter.Contract.TradeLogItem;

namespace Lykke.Job.TradelogBridge.Services
{
    public class TradelogSubscriber : IStartStop
    {
        private readonly ILog _log;
        private readonly IConsole _console;
        private readonly IClientAccountClient _clientAccountClient;
        private readonly IDataRepository _tradesRepository;
        private readonly IDataRepository _walletsRepository;
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private RabbitMqSubscriber<List<TradeLogItem>> _subscriber;

        public TradelogSubscriber(
            string connectionString,
            string exchangeName,
            IDataRepository tradesRepository,
            IDataRepository walletsRepository,
            IClientAccountClient clientAccountClient,
            IConsole console,
            ILog log)
        {
            _connectionString = connectionString;
            _exchangeName = exchangeName;
            _tradesRepository = tradesRepository;
            _walletsRepository = walletsRepository;
            _clientAccountClient = clientAccountClient;
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
                await _tradesRepository.AddDataItemsAsync(arg);

                var walletsToUpdate = arg
                    .Where(i => i.UserId != i.WalletId)
                    .Select(i => i.WalletId)
                    .Distinct()
                    .ToList();

                foreach (var walletId in walletsToUpdate)
                {
                    var wallet = await _clientAccountClient.GetWalletAsync(walletId);
                    if (wallet == null)
                        continue;

                    await _walletsRepository.AddDataItemAsync(
                        new Wallet
                        {
                            Id = wallet.Id,
                            Type = wallet.Type,
                            Name = wallet.Name,
                            Owner = wallet.Owner,
                            UserId = wallet.ClientId,
                        });
                }
            }
            catch (Exception exc)
            {
                _log.WriteError("TradelogSubscriber.ProcessMessageAsync", arg, exc);
            }
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }
    }
}
