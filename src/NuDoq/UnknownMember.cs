using System.Collections.Generic;
using System.Linq;

namespace NuDoq
{
    /// <summary>
    /// An unsupported or unknown member, such as those prefixed 
    /// with "N:" or "!:".
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public class UnknownMember : Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownMember"/> class.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public UnknownMember(string memberId, IDictionary<string, string> attributes)
            : base(memberId, Enumerable.Empty<Element>(), attributes)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitUnknownMember(this);
            return visitor;
        }

        /// <summary>
        /// Gets the kind of member, which equals <see cref="MemberKinds.Unknown"/>.
        /// </summary>
        public override MemberKinds Kind => MemberKinds.Unknown;
    }
}