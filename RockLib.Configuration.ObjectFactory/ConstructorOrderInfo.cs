﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RockLib.Configuration.ObjectFactory
{
    internal class ConstructorOrderInfo : IComparable<ConstructorOrderInfo>
    {
        public ConstructorOrderInfo(ConstructorInfo constructor, Dictionary<string, IConfigurationSection> availableMembers)
        {
            Constructor = constructor;
            var parameters = constructor.GetParameters();
            TotalParameters = parameters.Length;
            if (TotalParameters == 0)
            {
                IsInvokableWithoutDefaultParameters = true;
                IsInvokableWithDefaultParameters = true;
                MissingParameterNames = Enumerable.Empty<string>();
            }
            else
            {
                IsInvokableWithoutDefaultParameters = parameters.Count(p => availableMembers.ContainsKey(p.Name)) == TotalParameters;
                IsInvokableWithDefaultParameters = parameters.Count(p => availableMembers.ContainsKey(p.Name) || p.HasDefaultValue) == TotalParameters;
                MissingParameterNames = parameters.Where(p => !availableMembers.ContainsKey(p.Name) && !p.HasDefaultValue).Select(p => p.Name);
            }
        }

        public ConstructorInfo Constructor { get; }
        public bool IsInvokableWithoutDefaultParameters { get;  }
        public bool IsInvokableWithDefaultParameters { get; }
        public int TotalParameters { get; }
        public IEnumerable<string> MissingParameterNames { get; }

        public int CompareTo(ConstructorOrderInfo other)
        {
            if (IsInvokableWithoutDefaultParameters && !other.IsInvokableWithoutDefaultParameters) return -1;
            if (!IsInvokableWithoutDefaultParameters && other.IsInvokableWithoutDefaultParameters) return 1;
            if (IsInvokableWithDefaultParameters && !other.IsInvokableWithDefaultParameters) return -1;
            if (!IsInvokableWithDefaultParameters && other.IsInvokableWithDefaultParameters) return 1;
            if (TotalParameters > other.TotalParameters) return -1;
            if (TotalParameters < other.TotalParameters) return 1;
            return 0;
        }
    }
}
