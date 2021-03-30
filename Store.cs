using System;
using System.Collections.Generic;

namespace PhenixGenerator
{
    public class Store
    {
        public Guid StoreId { get; set; }

        public DateTime DateTime { get; set; }

        public List<Reference> References { get; set; }

        public string GetFileName() => string.Format("reference_prod-{0}_{1:yyyyMMdd}.data", (object) this.StoreId, (object) this.DateTime);

        public IEnumerable<string> GetContent()
        {
            foreach (Reference reference in this.References)
                yield return reference.GetContent();
        }
    }
}