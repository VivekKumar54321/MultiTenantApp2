﻿namespace API.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string Response { get; }
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException = null)
            : base($"{message}\n\nStatus: {statusCode}\nResponse: \n{(response == null ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length))}", innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers ?? new Dictionary<string, IEnumerable<string>>();
        }

        public override string ToString()
        {
            return $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";
        }
    }

    public class ApiException<TResult> : ApiException
    {
        public TResult Result { get; }

        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception? innerException = null)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }
}
