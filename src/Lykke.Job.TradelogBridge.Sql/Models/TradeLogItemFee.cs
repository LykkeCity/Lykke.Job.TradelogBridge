using System;

namespace Lykke.Job.TradelogBridge.Sql.Models
{
    public class TradeLogItemFee
    {
        public static int MaxStringFieldsLength => 255;

        public long Id { get; set; }

        public long TradeLogItemId { get; set; }

        public string FromClientId { get; set; }

        public string ToClientId { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Volume { get; set; }

        public string Asset { get; set; }

        public string Type { get; set; }

        public string SizeType { get; set; }

        public decimal? Size { get; set; }
    }
}
