namespace RestBackend.Core.Models.Business
{
    public class PropertyImage
    {
        public int IdProperyImage { get; set; }

        public string File { get; set; }

        public bool Enabled { get; set; }

        public int IdProperty { get; set; }

        public virtual Property Property { get; set; }
    }
}
