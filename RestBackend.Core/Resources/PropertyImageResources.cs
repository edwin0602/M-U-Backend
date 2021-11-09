
using Microsoft.AspNetCore.Http;

namespace RestBackend.Core.Resources
{

    public class PropertyImageResource
    {
        public int IdProperyImage { get; set; }

        public string File { get; set; }

        public bool Enabled { get; set; }
    }

    public class CreatePropertyImageResource
    {
        public IFormFile Image { get; set; }

        public bool Enabled { get; set; }
    }


}
