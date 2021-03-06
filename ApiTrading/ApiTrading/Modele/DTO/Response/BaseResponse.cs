namespace ApiTrading.Modele.DTO.Response
{
    public class BaseResponse<T> where T : new()
    {
        public BaseResponse(string message, T data)
        {
            Message = message;
            Data = data;
        }

        public BaseResponse(T data)
        {
            Data = data;
        }

        public BaseResponse()
        {
            Data = new T();
        }

        public string Message { get; set; }

        public T Data { get; set; }
    }

    public class BaseResponse
    {
        public BaseResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}