using Api.Domain.Constants;
using Api.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Roles.ListByScope
{
    public sealed record Request
    {
        public Request()
        {
        }
        
        [FromQuery(Name = FieldNames.RoleScope)]
        public RoleScope? Scope { get; init; }
    }
}