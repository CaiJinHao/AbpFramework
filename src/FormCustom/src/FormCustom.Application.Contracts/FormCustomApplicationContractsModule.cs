using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace FormCustom
{
    [DependsOn(
        typeof(FormCustomDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class FormCustomApplicationContractsModule : AbpModule
    {

    }
}
