using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the method kind of documentation member, denoted by the starting "M:" 
    /// prefix in the member <see cref="Member.Id"/>.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public class Method : Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="memberId">The member id as specified in the documentation XML.</param>
        /// <param name="elements">The contained documentation elements.</param>
        public Method(string memberId, IEnumerable<Element> elements)
            : base(memberId, elements)
        {
        }

        /// <summary>
        /// Gets the kind of member, which equals to <see cref="MemberKinds.Method"/>.
        /// </summary>
        public override MemberKinds Kind => MemberKinds.Method;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitMethod(this);
            return visitor;
        }
    }
}