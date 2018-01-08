namespace Lykke.Job.TradelogBridge.Sql.Models
{
    public class TradeLogItemFee
    {
        public static int MaxStringFieldsLength { get { return 255; } }

        public long Id { get; set; }

        public long TradeLogItemId { get; set; }

        public string Type { get; set; }

        public string SourceClientId { get; set; }

        public string TargetClientId { get; set; }

        public string SizeType { get; set; }

        public double? Size { get; set; }
    }
}
