using System;
using System.Collections.Generic;

namespace RestBackend.Core.Models.Business
{
    public class Owner
    {
        public int IdOwner { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; }

        public DateTime Birthday { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }

    public static class OwnerExtensions
    {
        /// <summary>
        /// Update default properties
        /// </summary>
        /// <param name="me">Object to update</param>
        /// <param name="source">Source of changes</param>
        public static void SetForUpdate(this Owner me, Owner source)
        {
            me.Name = source.Name;
            me.Address = source.Address;
            me.Birthday = source.Birthday;
            me.Photo = source.Photo;
        }
    }
}
