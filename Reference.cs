namespace PhenixGenerator
{
    public class Reference
    {
        public long ProductId { get; set; }

        public int Price { get; set; }

        public string GetContent() => string.Format("{0}|{1}", (object) this.ProductId, (object) (this.Price / 100).ToString("##.0"));
    }
}