﻿// Copyright 2012-2016 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace GreenPipes.Routers
{
    using System;
    using System.Linq;
    using Contracts;
    using Filters;
    using Internals.Extensions;


    public class CommandContextConverterFactory :
        IPipeContextConverterFactory<CommandContext>
    {
        IPipeContextConverter<CommandContext, TOutput> IPipeContextConverterFactory<CommandContext>.GetConverter<TOutput>()
        {
            var innerType = typeof(TOutput).GetClosingArguments(typeof(CommandContext<>)).Single();

            return (IPipeContextConverter<CommandContext, TOutput>)Activator.CreateInstance(typeof(Converter<>).MakeGenericType(innerType));
        }


        class Converter<T> :
            IPipeContextConverter<CommandContext, CommandContext<T>>
            where T : class
        {
            bool IPipeContextConverter<CommandContext, CommandContext<T>>.TryConvert(CommandContext input, out CommandContext<T> output)
            {
                var outputContext = input as CommandContext<T>;

                if (outputContext != null)
                {
                    output = new Context<T>(outputContext);
                    return true;
                }

                output = null;
                return false;
            }
        }


        class Context<T> :
            BasePipeContext,
            CommandContext<T>
            where T : class
        {
            readonly CommandContext<T> _context;

            public Context(CommandContext<T> context)
                : base(context)
            {
                _context = context;
            }

            public DateTime Timestamp => _context.Timestamp;

            public T Command => _context.Command;
        }
    }
}