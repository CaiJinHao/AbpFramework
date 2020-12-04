﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Jh.Abp.MenuManagement
{
    [DependsOn(
        typeof(MenuManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class MenuManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "MenuManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MenuManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
