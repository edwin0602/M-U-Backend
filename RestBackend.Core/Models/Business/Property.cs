using System.Collections.Generic;

namespace RestBackend.Core.Models.Business
{
    public class Property
    {
        public int IdProperty { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public int IdOwner { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual ICollection<PropertyTrace> PropertiesTraces { get; set; }

        public virtual ICollection<PropertyImage> PropertiesImages { get; set; }
    }

    /// <summary>
    /// Update default properties
    /// </summary>
    /// <param name="me">Object to update</param>
    /// <param name="source">Source of changes</param>
    public static class PropertyExtensions
    {
        public static void SetForUpdate(this Property me, Property source)
        {
            me.Name = source.Name;
            me.Address = source.Address; 
            me.Year = source.Year;
            me.CodeInternal = source.CodeInternal;
        }
    }
}
