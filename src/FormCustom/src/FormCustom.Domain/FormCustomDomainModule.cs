using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace FormCustom
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(FormCustomDomainSharedModule)
    )]
    public class FormCustomDomainModule : AbpModule
    {

    }
}
