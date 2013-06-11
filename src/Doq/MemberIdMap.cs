#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Provides bidirectional mapping between reflection and XML documentation ids.
    /// </summary>
    internal class MemberIdMap
    {
        internal readonly Dictionary<string, MemberInfo> idToMemberMap = new Dictionary<string, MemberInfo>();
        internal readonly Dictionary<MemberInfo, string> memberToIdMap = new Dictionary<MemberInfo, string>();
        private readonly StringBuilder sb = new StringBuilder();

        public void Add(Assembly assembly)
        {
            AddRange(assembly.GetTypes());
        }

        public void AddRange(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void Add(Type type)
        {
            sb.Length = 0;
            AppendType(sb, type);

            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var member in members)
            {
                Add(type, member);
            }
        }

        public MemberInfo FindMember(string memberId)
        {
            MemberInfo result = null;
            idToMemberMap.TryGetValue(memberId, out result);
            return result;
        }

        public string FindId(MemberInfo member)
        {
            string result = null;
            memberToIdMap.TryGetValue(member, out result);
            return result;
        }

        private void Add(Type type, MemberInfo member)
        {
            sb.Length = 0;

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    sb.Append("M:");
                    Append((ConstructorInfo)member);
                    break;
                case MemberTypes.Event:
                    sb.Append("E:");
                    Append((EventInfo)member);
                    break;
                case MemberTypes.Field:
                    sb.Append("F:");
                    Append((FieldInfo)member);
                    break;
                case MemberTypes.Method:
                    sb.Append("M:");
                    Append(type, (MethodInfo)member);
                    break;
                case MemberTypes.NestedType:
                    Add((Type)member);
                    break;
                case MemberTypes.Property:
                    sb.Append("P:");
                    Append((PropertyInfo)member);
                    break;
            }

            if (sb.Length > 0)
            {
                idToMemberMap[sb.ToString()] = member;
                memberToIdMap[member] = sb.ToString();
            }
        }

        private void Append(PropertyInfo property)
        {
            AppendType(sb, property.DeclaringType);
            sb.Append('.').Append(property.Name);
        }

        private void Append(Type owner, MethodInfo method)
        {
            if (method.IsSpecialName || method.IsPrivate)
            {
                sb.Length = 0;
                return;
            }

            AppendType(sb, method.DeclaringType);
            sb.Append('.').Append(method.Name);

            if (method.IsGenericMethodDefinition)
            {
                // Append arity
                sb.Append("``").Append(method.GetGenericArguments().Length);
            }

            Append(owner, method, method.GetParameters());
        }

        private void Append(Type owner, MethodBase method, ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
            {
                return;
            }
            sb.Append('(');
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(',');
                }
                var p = parameters[i];
                AppendType(sb, p.ParameterType);
            }
            sb.Append(')');
        }

        private void Append(FieldInfo field)
        {
            if (field.IsPrivate)
            {
                sb.Length = 0;
                return;
            }

            AppendType(sb, field.DeclaringType);
            sb.Append('.').Append(field.Name);
        }

        private void Append(EventInfo @event)
        {
            AppendType(sb, @event.DeclaringType);
            sb.Append('.').Append(@event.Name);
        }

        private void Append(ConstructorInfo constructor)
        {
            AppendType(sb, constructor.DeclaringType);
            sb.Append('.').Append("#ctor");
            Append(constructor.DeclaringType, constructor, constructor.GetParameters());
        }

        private void AppendType(StringBuilder sb, Type type, bool addTypeToMap = true)
        {
            // Generic parameters will only have the parameter name, i.e. "T".
            if (!type.IsGenericParameter)
            {
                if (type.DeclaringType != null)
                {
                    AppendType(sb, type.DeclaringType);
                    sb.Append('.');
                }
                else if (!string.IsNullOrEmpty(type.Namespace))
                {
                    sb.Append(type.Namespace);
                    sb.Append('.');
                }

                // We only append the name if it's a non-generic parameter.
                sb.Append(type.Name);
            }
            else
            {
                // Never add to map the generic parameters, they 
                // can never be referenced in reflection.
                addTypeToMap = false;

                // If the generic parameter was declared in a method, 
                // the arity is a double ``.
                if (type.DeclaringMethod != null)
                {
                    sb.Append("``");
                    // Get the generic method parameter index.
                    sb.Append(type.DeclaringMethod
                        .GetGenericArguments()
                        .ToList()
                        .IndexOf(type));
                }
                else if (type.DeclaringType != null)
                {
                    sb.Append("`");
                    // Get the generic type parameter index.
                    sb.Append(type.DeclaringType
                        .GetGenericArguments()
                        .ToList()
                        .IndexOf(type));
                }
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                // Remove "`1" suffix from type name
                while (char.IsDigit(sb[sb.Length - 1]))
                    sb.Length--;
                sb.Length--;
                {
                    var args = type.GetGenericArguments();
                    sb.Append('{');
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(',');
                        }
                        AppendType(sb, args[i]);
                    }
                    sb.Append('}');
                }
            }

            if (addTypeToMap)
            {
                // Call back to ourselves but with a blank 
                // builder to get a clean rendering of the 
                // type name.
                var typeBuilder = new StringBuilder("T:");
                AppendType(typeBuilder, type, false);
                idToMemberMap[typeBuilder.ToString()] = type;
                memberToIdMap[type] = typeBuilder.ToString();
            }
        }
    }
}