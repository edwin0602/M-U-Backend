namespace RestBackend.Core.Models.Business
{
    public class Tax
    {
        public int IdTax { get; set; }

        public decimal Value { get; set; }

        public bool Enabled { get; set; }
    }
}
