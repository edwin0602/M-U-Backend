using System;

namespace RestBackend.Core.Resources
{
    public class OwnerResource
    {
        public int IdOwner { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; }

        public DateTime Birthday { get; set; }
    }

    public class CreateOwnerResource
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; }

        public DateTime Birthday { get; set; }

    }

}
