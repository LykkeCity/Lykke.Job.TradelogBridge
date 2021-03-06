﻿using System;

namespace Lykke.Job.TradelogBridge.Sql.Models
{
    public class TradeLogItem
    {
        public static int MaxStringFieldsLength { get { return 255; } }

        public long Id { get; set; }

        public string TradeId { get; set; }

        public string UserId { get; set; }

        public string WalletId { get; set; }

        public string OrderId { get; set; }

        public string OrderType { get; set; }

        public string Direction { get; set; }

        public string Asset { get; set; }

        public decimal Volume { get; set; }

        public decimal Price { get; set; }

        public DateTime DateTime { get; set; }

        public string OppositeOrderId { get; set; }

        public string OppositeAsset { get; set; }

        public decimal? OppositeVolume { get; set; }

        public bool? IsHidden { get; set; }

        public TradeLogItemFee Fee { get; set; }
    }
}
