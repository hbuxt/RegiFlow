using System;

namespace Api.Domain.Entities
{
    public interface IEntity : IBaseEntity
    {
        Guid Id { get; }
    }
}