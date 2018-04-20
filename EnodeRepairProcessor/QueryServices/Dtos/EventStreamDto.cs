using System;

namespace EnodeRepairProcessor.QueryServices.Dtos
{
    public class EventStreamDto
    {
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
        public int Version { get; set; }
    }
}
