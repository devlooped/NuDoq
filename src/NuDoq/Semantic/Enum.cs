using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Semantically augmented <see cref="TypeDeclaration" /> for
    /// enumerations, available when using an <see cref="System.Reflection.Assembly" />
    /// with the <see cref="DocReader" />.
    /// </summary>
    public class Enum : TypeDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enum"/> class.
        /// </summary>
        /// <param name="memberId">The member id as specified in the documentation XML.</param>
        /// <param name="elements">The contained documentation elements.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public Enum(string memberId, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(memberId, elements, attributes)
        {
        }

        /// <summary>
        /// Gets the kind of member, which contains both the <see cref="MemberKinds.Type" /> and 
        /// <see cref="MemberKinds.Enum"/> flags.
        /// </summary>
        public override MemberKinds Kind => MemberKinds.Type | MemberKinds.Enum;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitEnum(this);
            return visitor;
        }
    }
}