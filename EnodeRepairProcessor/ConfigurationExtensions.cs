using ECommon.Configurations;
using EnodeRepairProcessor.QueryServices;

namespace EnodeRepairProcessor
{
    public static class ConfigurationExtensions
    {
        /// <summary>.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseMyComponents(this Configuration configuration)
        {
            configuration.SetDefault<IAggregateRootQueryService, DapperAggregateRootQueryService>(new DapperAggregateRootQueryService());
            configuration.SetDefault<IGenerator, DapperGenerator>(new DapperGenerator());

            return configuration;
        }
        
    }
}
