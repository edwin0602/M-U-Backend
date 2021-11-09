namespace RestBackend.Api.Wrappers
{
    public class Response<T> : ResposeBase
    {
        public Response() : base()
        {
        }

        public Response(T data) : base()
        {
            Data = data;
        }

        public T Data { get; set; }

        public static Response<T> BadResponse(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message
            };
        }
    }
}
