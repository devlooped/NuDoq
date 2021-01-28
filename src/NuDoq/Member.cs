using System.Collections.Generic;
using System.Reflection;

namespace NuDoq
{
    /// <summary>
    /// Base class for all documentation members: types, 
    /// fields, properties, methods and events.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public abstract class Member : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Member"/> class.
        /// </summary>
        /// <param name="memberId">The member id as specified in the documentation XML.</param>
        /// <param name="elements">The contained documentation elements.</param>
        public Member(string memberId, IEnumerable<Element> elements)
            : base(elements)
            => Id = memberId;

        /// <summary>
        /// Gets the member id as specified in the documentation XML.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the kind of member.
        /// </summary>
        public abstract MemberKinds Kind { get; }

        /// <summary>
        /// Gets the reflection information for this member, 
        /// if the reading process used an assembly.
        /// </summary>
        public MemberInfo? Info { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => Id + " " + base.ToString();
    }
}