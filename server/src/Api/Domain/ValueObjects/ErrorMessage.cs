using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Domain.ValueObjects
{
    public class ErrorMessage
    {
        public ErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
        
        [JsonPropertyName(FieldNames.ErrorCode)]
        [JsonPropertyOrder(0)]
        public string Code { get; private set; }
        
        [JsonPropertyName(FieldNames.ErrorMessage)]
        [JsonPropertyOrder(1)]
        public string Message { get; private set; }
    }
}