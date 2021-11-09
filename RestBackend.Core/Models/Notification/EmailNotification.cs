namespace RestBackend.Core.Models.Notification
{
    public class EmailNotification
    {
        public string Sender { get; set; }

        public string To { get; set; }

        public string CC { get; set; }

        public string Message { get; set; }

        public string Subject { get; set; }
    }
}
