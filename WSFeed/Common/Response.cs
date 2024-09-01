namespace WSFeed.Common
{
    public class Response
    {
        public Response()
        {
           
        }

        public Response(TypeOfResponse typeOfResponse, string message = "")
        {
            this.TypeOfResponse = typeOfResponse;
            this.Message = message;
            this.Data = null;
        }


        public TypeOfResponse TypeOfResponse { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum TypeOfResponse
    {
        OK = 0,
        FailedResponse = 1,
        Exception = 2,
        TimeOut = 3,
        NotFound = 4,
    }
}
