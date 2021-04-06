using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace FormCustom.MongoDB
{
    public class FormCustomMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public FormCustomMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}