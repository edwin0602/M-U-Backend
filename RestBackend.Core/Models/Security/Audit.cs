using System;

namespace RestBackend.Core.Models.Security
{
    public class Audit
    {
        public int IdAudit { get; set; }

        public string Action { get; set; }

        public string Resource { get; set; }

        public string Target { get; set; }

        public string Entity { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
