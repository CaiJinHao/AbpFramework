﻿using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace FormCustom.EntityFrameworkCore
{
    public class FormCustomModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public FormCustomModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}