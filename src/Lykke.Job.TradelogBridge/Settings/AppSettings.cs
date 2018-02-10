using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.TradelogBridge.Settings
{
    public class AppSettings
    {
        public TradelogBridgeSettings TradelogBridgeJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }

    public class TradelogBridgeSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [SqlCheck]
        public string SqlConnString { get; set; }

        [AzureBlobCheck]
        public string BlobStorageConnString { get; set; }

        public int MaxBatchCount { get; set; }

        public int BatchPeriodInSeconds { get; set; }

        public int WarningSqlTableSizeInGigabytes { get; set; }

        public RabbitMqSettings Rabbit { get; set; }
    }

    public class SlackNotificationsSettings
    {
        public AzureQueuePublicationSettings AzureQueue { get; set; }
    }

    public class AzureQueuePublicationSettings
    {
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }
    }

    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }
    }
}
