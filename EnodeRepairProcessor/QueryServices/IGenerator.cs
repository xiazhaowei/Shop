using EnodeRepairProcessor.QueryServices.Dtos;

namespace EnodeRepairProcessor.QueryServices
{
    public interface IGenerator
    {
        int DelEventStream(EventStreamDto eventStream);
        int UpdatePublishedVersion(AggregateRootDto publishedVersion);
    }
}
