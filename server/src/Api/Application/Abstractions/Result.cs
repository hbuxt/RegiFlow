using System;
using System.Collections.Generic;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;

namespace Api.Application.Abstractions
{
    public class Result
    {
        protected readonly bool isSuccess;
        private readonly Error? error;
        
        protected Result(bool isSuccess, Error? error)
        {
            this.isSuccess = isSuccess;
            this.error = error;
        }
        
        public bool IsFailure => !isSuccess;

        public Error? Error
        {
            get
            {
                if (isSuccess)
                {
                    throw new InvalidOperationException("This result succeeded, " +
                        "but you attempted to access the error of the result. " +
                        "If the result fails, access the error of the result.");
                }

                return error;
            }
        }
        
        public static Result Success()
        {
            return new Result(true, null);
        }
        
        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(true, value, null);
        }
        
        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }
        
        public static Result<TValue> Failure<TValue>(Error error)
        {
            return new Result<TValue>(false, default, error);
        }
        
        public static Result Failure(IDictionary<string, string[]> errors)
        {
            return new Result(false, new Error(ErrorStatus.BadRequest, errors));
        }
        
        public static Result<TValue> Failure<TValue>(IDictionary<string, string[]> errors)
        {
            return new Result<TValue>(false, default, new Error(ErrorStatus.BadRequest, errors));
        }
    }
}