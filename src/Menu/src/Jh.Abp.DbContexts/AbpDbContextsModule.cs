using Jh.Abp.DbContexts.DbName;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Dapper;
using Volo.Abp.Modularity;

namespace Jh.Abp.DbContexts
{
    [DependsOn(
        typeof(AbpDapperModule)
    )]
    public class AbpDbContextsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DbNameDbContext>(options =>
            {
               
            });
        }
    }
}
