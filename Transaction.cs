using System;

namespace PhenixGenerator
{
    public class Transaction
    {
        public long TxId { get; set; }

        public DateTime DateTime { get; set; }

        public Guid StoreId { get; set; }

        public long ProductId { get; set; }

        public int Quantity { get; set; }

        public string GetContent() => string.Format("{0}|{1}|{2}|{3}|{4}", (object) this.TxId, (object) this.DateTime, (object) this.StoreId, (object) this.ProductId, (object) this.Quantity);
    }
}