using System.Collections.Generic;

namespace RestBackend.Core.Resources
{
    public class PropertyResource
    {
        public int IdProperty { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public int IdOwner { get; set; }

        public IEnumerable<PropertyImageResource> Images { get; set; }
    }

    public class CreatePropertyResource
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public int IdOwner { get; set; }
    }

    public class PropertyFilterResource
    {
        public string LikeName { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string CodeInternal { get; set; }

        public int? MinYear { get; set; }

        public int? MaxYear { get; set; }
    }
}
