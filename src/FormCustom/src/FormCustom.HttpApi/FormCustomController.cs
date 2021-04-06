using FormCustom.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FormCustom
{
    public abstract class FormCustomController : AbpController
    {
        protected FormCustomController()
        {
            LocalizationResource = typeof(FormCustomResource);
        }
    }
}
