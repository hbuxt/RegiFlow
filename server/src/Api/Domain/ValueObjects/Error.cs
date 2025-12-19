using System.Collections.Generic;
using Api.Domain.Enums;

namespace Api.Domain.ValueObjects
{
    public class Error
    {
        public Error(ErrorStatus status)
        {
            Status = status;
        }
        
        public Error(ErrorStatus status, string code, string message)
        {
            Status = status;
            Errors = new List<ErrorMessage>()
            {
                new ErrorMessage(code, message)
            };
        }
        
        public Error(ErrorStatus status, IDictionary<string, string[]> errors)
        {
            Status = status;
            Errors = ToErrorMessages(errors);
        }
        
        public ErrorStatus Status { get; private set; }
        public List<ErrorMessage>? Errors { get; private set; }
        
        private List<ErrorMessage> ToErrorMessages(IDictionary<string, string[]> errors)
        {
            var errorMessages = new List<ErrorMessage>();

            foreach (var error in errors)
            {
                foreach (var message in error.Value)
                {
                    errorMessages.Add(new ErrorMessage(error.Key, message));
                }
            }

            return errorMessages;
        }
    }
}