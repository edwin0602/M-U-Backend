using System;

namespace RestBackend.Core.Resources
{
    public class PropertyTraceResource
    {
        public DateTime DateSale { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }
    }
}
