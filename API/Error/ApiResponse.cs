namespace API.Error
{
    public class ApiResponse
    {
        public int SatutsCode { get; set; } 
        public string Message { get; set; }

        public ApiResponse(int statusCode,string? message)
        {
            this.SatutsCode = statusCode;
            this.Message = message??GetDefultMessage(statusCode);
        }

        private string GetDefultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request , you have made",
                401 => "Authorized you are not",
                404 => "Response found it is not",
                500 => "server error occured",
                _ => ""
            };
        }
    }
}
