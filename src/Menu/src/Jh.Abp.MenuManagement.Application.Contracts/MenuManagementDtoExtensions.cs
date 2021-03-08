using Jh.Abp.MenuManagement.ObjectExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Jh.Abp.MenuManagement
{
    public static class MenuManagementDtoExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can add extension properties to DTOs
                 * defined in the depended modules.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */

                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<int>(new[] { typeof(IdentityUserDto),typeof(IdentityUserCreateDto),typeof(IdentityUserUpdateDto) },IdentityUserExtension.PlatformType)
                    .AddOrUpdateProperty<string>(new[] { typeof(IdentityUserDto), typeof(IdentityUserCreateDto), typeof(IdentityUserUpdateDto) }, IdentityUserExtension.Avatar)
                    .AddOrUpdateProperty<string>(new[] { typeof(IdentityUserDto), typeof(IdentityUserCreateDto), typeof(IdentityUserUpdateDto) }, IdentityUserExtension.Introduction);
            });
        }
    }
}
