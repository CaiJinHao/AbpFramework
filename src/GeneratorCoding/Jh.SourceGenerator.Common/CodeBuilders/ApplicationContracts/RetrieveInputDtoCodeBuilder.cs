﻿using Jh.SourceGenerator.Common.GeneratorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.SourceGenerator.Common.CodeBuilders
{
    public class RetrieveInputDtoCodeBuilder : CodeBuilderAbs
    {
        public RetrieveInputDtoCodeBuilder(TableDto tableDto) : base(tableDto)
        {
            this.FileName = $"{table.Name}RetrieveInputDto";
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(@"using Jh.Abp.Application.Contracts.Dtos;
using Jh.Abp.Application.Contracts.Extensions;
using Volo.Abp.Application.Dtos;");
            builder.AppendLine($"namespace {table.Namespace}");
            builder.AppendLine("{");
            {
                builder.AppendLine($"\tpublic class {FileName}: PagedAndSortedResultRequestDto, IFullRetrieveDto<{table.KeyType}>");
                builder.AppendLine("\t{");
                {
                    foreach (var _field in table.FieldsRetrieve)
                    {
                        builder.AppendLine($"\t\t/// <summary>");
                        builder.AppendLine($"\t\t/// {_field.Description}");
                        builder.AppendLine($"\t\t/// <summary>");
                        builder.AppendLine($"\t\tpublic string {_field.Name} " + "{ get; set; }");
                    }
                    //IFullRetrieveDto
                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine("\t\t/// 是否软删除");
                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine("\t\tpublic string Deleted " + "{ get; set; }");

                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine("\t\t/// 方法参数回调");
                    builder.AppendLine("\t\t/// <summary>");
                    builder.AppendLine("\t\tpublic string MethodInput " + "{ get; set; }");
                }
                builder.AppendLine("\t}");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
