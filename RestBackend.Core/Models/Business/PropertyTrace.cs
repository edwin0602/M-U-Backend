using System;

namespace RestBackend.Core.Models.Business
{
    public class PropertyTrace
    {
        public int IdPropertyTrace { get; set; }

        public DateTime DateSale { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }

        public int IdProperty { get; set; }

        public virtual Property Property { get; set; }
    }
}
