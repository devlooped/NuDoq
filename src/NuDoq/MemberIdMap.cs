using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NuDoq
{
    /// <summary>
    /// Provides bidirectional mapping between reflection members and XML documentation ids.
    /// </summary>
    /// <remarks>
    /// This map can be used in conjunction with the result of a <see cref="DocReader.Read(Assembly)"/>
    /// operation to do post-processing on the <see cref="Members"/> by grouping by type, loading inheritance trees, 
    /// etc.
    /// </remarks>
    public class MemberIdMap
    {
        internal readonly Dictionary<string, MemberInfo> idToMemberMap = new Dictionary<string, MemberInfo>();
        internal readonly Dictionary<MemberInfo, string> memberToIdMap = new Dictionary<MemberInfo, string>();
        readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Adds all the members in the specified assembly to the map.
        /// </summary>
        /// <param name="assembly">The assembly to map to member ids.</param>
        public void Add(Assembly assembly) => AddRange(assembly.GetTypes());

        /// <summary>
        /// Adds a set of types to the map.
        /// </summary>
        /// <param name="types">The types to map to member ids.</param>
        public void AddRange(IEnumerable<Type> types)
        {
            foreach (var type in types)
                Add(type);
        }

        /// <summary>
        /// Adds the specified type to the map.
        /// </summary>
        /// <param name="type">The type to map to a member id.</param>
        public void Add(Type type)
        {
            sb.Length = 0;
            AppendType(sb, type);

            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var member in members)
                Add(member);
        }

        /// <summary>
        /// Given a <paramref name="memberId"/> string, tries to find 
        /// the corresponding reflection member.
        /// </summary>
        /// <param name="memberId">The member id to find.</param>
        /// <returns>The <see cref="MemberInfo"/> if found; <see langword="null"/> otherwise.</returns>
        public MemberInfo FindMember(string memberId)
        {
            idToMemberMap.TryGetValue(memberId, out var result);
            return result;
        }

        /// <summary>
        /// Gets the documentation member ids of all members added so far to the map.
        /// </summary>
        public IEnumerable<string> Ids => idToMemberMap.Keys;

        /// <summary>
        /// Gets all the reflection members added so far to the map.
        /// </summary>
        public IEnumerable<MemberInfo> Members => memberToIdMap.Keys;

        /// <summary>
        /// Given a reflection <paramref name="member"/>, tries to find 
        /// the corresponding documentation member id.
        /// </summary>
        /// <param name="member">The member to find the id for.</param>
        /// <returns>The documentation member id if found; <see langword="null"/> otherwise.</returns>
        public string FindId(MemberInfo member)
        {
            memberToIdMap.TryGetValue(member, out var result);
            return result;
        }

        void Add(MemberInfo member)
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
                    Append((MethodInfo)member);
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

        void Append(PropertyInfo property)
        {
            AppendType(sb, property.DeclaringType);
            sb.Append('.').Append(property.Name);
        }

        void Append(MethodInfo method)
        {
            AppendType(sb, method.DeclaringType);
            sb.Append('.').Append(method.Name);

            if (method.IsGenericMethodDefinition)
            {
                // Append arity
                sb.Append("``").Append(method.GetGenericArguments().Length);
            }

            Append(method.GetParameters());
        }

        void Append(ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
            {
                return;
            }
            sb.Append('(');
            for (var i = 0; i < parameters.Length; i++)
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

        void Append(FieldInfo field)
        {
            AppendType(sb, field.DeclaringType);
            sb.Append('.').Append(field.Name);
        }

        void Append(EventInfo @event)
        {
            AppendType(sb, @event.DeclaringType);
            sb.Append('.').Append(@event.Name);
        }

        void Append(ConstructorInfo constructor)
        {
            AppendType(sb, constructor.DeclaringType);
            sb.Append('.').Append("#ctor");
            Append(constructor.GetParameters());
        }

        void AppendType(StringBuilder sb, Type type, bool addTypeToMap = true)
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
                    for (var i = 0; i < args.Length; i++)
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