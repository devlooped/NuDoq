using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the field kind of documentation member, denoted by the starting "F:" 
    /// prefix in the member <see cref="Member.Id"/>.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public class Field : Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <param name="elements">The contained documentation elements.</param>
        public Field(string memberId, IEnumerable<Element> elements)
            : base(memberId, elements)
        {
        }

        /// <summary>
        /// Gets the kind of member, which equals to <see cref="MemberKinds.Field"/>.
        /// </summary>
        public override MemberKinds Kind { get { return MemberKinds.Field; } }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitField(this);
            return visitor;
        }
    }
}