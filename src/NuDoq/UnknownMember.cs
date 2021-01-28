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
        public UnknownMember(string memberId)
            : base(memberId, Enumerable.Empty<Element>())
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
        public override MemberKinds Kind { get { return MemberKinds.Unknown; } }
    }
}