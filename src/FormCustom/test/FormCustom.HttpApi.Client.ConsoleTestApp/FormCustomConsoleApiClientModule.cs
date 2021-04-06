using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace FormCustom
{
    [DependsOn(
        typeof(FormCustomHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class FormCustomConsoleApiClientModule : AbpModule
    {
        
    }
}
