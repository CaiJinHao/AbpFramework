using Volo.Abp.Modularity;

namespace FormCustom
{
    [DependsOn(
        typeof(FormCustomApplicationModule),
        typeof(FormCustomDomainTestModule)
        )]
    public class FormCustomApplicationTestModule : AbpModule
    {

    }
}
