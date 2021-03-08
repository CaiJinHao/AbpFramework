using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Jh.Abp.MenuManagement.EntityFrameworkCore
{
    public static class MenuManagementEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            MenuManagementModuleExtensionConfigurator.Configure();

            OneTimeRunner.Run(() =>
            {
                /* You can configure entity extension properties for the
                 * entities defined in the used modules.
                 *
                 * The properties defined here becomes table fields.
                 * If you want to use the ExtraProperties dictionary of the entity
                 * instead of creating a new field, then define the property in the
                 * BookStoreDomainObjectExtensions class.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .MapEfCoreProperty<IdentityUser, string>(
                 *        "MyProperty",
                 *        b => b.HasMaxLength(128)
                 *    );
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */

                ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, int>(
                    nameof(MyUsers.PlatformType),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.IsRequired();
                    }
                )
                .MapEfCoreProperty<IdentityUser, string>(
                    nameof(MyUsers.Avatar),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(500);
                    }
                )
                .MapEfCoreProperty<IdentityUser, string>(
                    nameof(MyUsers.Introduction),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(500);
                    }
                );
            });
        }
    }
}
