using System;
using System.Collections.Generic;

namespace PhenixGenerator
{
    public class TransactionList
    {
        public DateTime DateTime { get; set; }

        public List<Transaction> Txs { get; set; }

        public string GetFileName() => string.Format("transactions_{0:yyyyMMdd}.data", (object) this.DateTime);

        public IEnumerable<string> GetContent()
        {
            foreach (Transaction transaction in this.Txs)
                yield return transaction.GetContent();
        }
    }
}