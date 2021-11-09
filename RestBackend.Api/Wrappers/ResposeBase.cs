namespace RestBackend.Api.Wrappers
{
    public abstract class ResposeBase
    {
        public ResposeBase()
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public string Message { get; set; }
    }
}
