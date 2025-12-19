using System;
using Api.Domain.ValueObjects;

namespace Api.Application.Abstractions
{
    public class Result<TValue> : Result
    {
        protected readonly TValue? value;
        
        public Result(bool isSuccess, TValue? value, Error? error) : base(isSuccess, error)
        {
            this.value = value;
        }
        
        public TValue Value
        {
            get
            {
                if (!isSuccess)
                {
                    throw new InvalidOperationException("This result failed, " +
                        "but you attempted to access the value of the result. " +
                        "If the result fails, access the error of the result. " +
                        "If the result succeeds, access the value of the result.");
                }

                return value!;
            }
        }
    }
}