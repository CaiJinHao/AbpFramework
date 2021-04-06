using FormCustom.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace FormCustom.Permissions
{
    public class FormCustomPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FormCustomPermissions.GroupName, L("Permission:FormCustom"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FormCustomResource>(name);
        }
    }
}