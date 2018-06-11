using System;

namespace Foundation.Messaging
{
    public interface IEvent
    {
        Guid CandidateId { get; set; }
    }
}