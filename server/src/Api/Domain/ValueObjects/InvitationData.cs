using System;
using System.Collections.Generic;

namespace Api.Domain.ValueObjects
{
    public class InvitationData
    {
        public InvitationData()
        {
        }
        
        public List<Guid>? Roles { get; set; }
    }
}