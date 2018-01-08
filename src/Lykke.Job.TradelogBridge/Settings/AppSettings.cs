namespace Lykke.Job.TradelogBridge.Settings
{
    public class AppSettings
    {
        public TradelogBridgeSettings TradelogBridgeJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }

    public class TradelogBridgeSettings
    {
        public string LogsConnString { get; set; }

        public string SqlConnString { get; set; }

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
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }
    }
}
