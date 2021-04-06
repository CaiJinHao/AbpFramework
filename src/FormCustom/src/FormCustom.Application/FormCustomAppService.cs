using FormCustom.Localization;
using Volo.Abp.Application.Services;

namespace FormCustom
{
    public abstract class FormCustomAppService : ApplicationService
    {
        protected FormCustomAppService()
        {
            LocalizationResource = typeof(FormCustomResource);
            ObjectMapperContext = typeof(FormCustomApplicationModule);
        }
    }
}
