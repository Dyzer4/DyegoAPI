namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class Response
    {
        private readonly List<string> _messages;

        public bool HasErrors { get { return _messages.Any(); } }

        public string[] Messages
        {
            get { return _messages.ToArray(); }
        }

        public int Data { get; set; }

        public Response()
        {
            _messages = new List<string>();
        }

        public void AddMessage(params string[] messages)
        {
            _messages.AddRange(messages);
        }

        public void AddMessage(IEnumerable<string> messages)
        {
            _messages.AddRange(messages);
        }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
    }

    public class ResponseNotFound<T> : Response<T>
    {
    }

    public class ResponseAccesDenied<T> : Response<T>
    {
    }
}
