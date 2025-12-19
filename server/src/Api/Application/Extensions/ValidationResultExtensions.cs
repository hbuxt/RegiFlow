using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Api.Application.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IDictionary<string, string[]> ToFormattedDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(e => e.FormattedMessagePlaceholderValues["PropertyName"].ToString() ?? e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}