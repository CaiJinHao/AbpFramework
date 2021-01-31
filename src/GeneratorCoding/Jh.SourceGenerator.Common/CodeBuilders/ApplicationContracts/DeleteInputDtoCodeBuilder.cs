﻿using Jh.SourceGenerator.Common.GeneratorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jh.SourceGenerator.Common.CodeBuilders
{
    public class DeleteInputDtoCodeBuilder : CodeBuilderAbs
    {
        public DeleteInputDtoCodeBuilder(TableDto tableDto) : base(tableDto)
        {
            this.FileName = $"{table.Name}DeleteInputDto";
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {table.Namespace}");
            builder.AppendLine("{");
            {
                builder.AppendLine($"\tpublic class {FileName} : {table.Name}RetrieveInputDto");
                builder.AppendLine("\t{");
                builder.AppendLine("\t}");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
