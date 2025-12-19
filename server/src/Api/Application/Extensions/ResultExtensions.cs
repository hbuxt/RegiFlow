using System;
using Api.Application.Abstractions;

namespace Api.Application.Extensions
{
    public static class ResultExtensions
    {
        public static TOut Match<TOut>(this Result result,
            Func<TOut> onSuccess,
            Func<Result, TOut> onFailure)
        {
            return result.IsFailure ? onFailure(result) : onSuccess();
        }
        
        public static TOut Match<TIn, TOut>(this Result<TIn> result,
            Func<TIn, TOut> onSuccess,
            Func<Result<TIn>, TOut> onFailure)
        {
            return result.IsFailure ? onFailure(result) : onSuccess(result.Value);
        }
    }
}