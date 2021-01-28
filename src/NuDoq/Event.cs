using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the event kind of documentation member, denoted by the starting "E:" 
    /// prefix in the member <see cref="Member.Id"/>.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x(v=vs.80).aspx.
    /// </remarks>
    public class Event : Member
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="memberId">The member id.</param>
        /// <param name="elements">The contained documentation elements.</param>
        public Event(string memberId, IEnumerable<Element> elements)
            : base(memberId, elements)
        {
        }

        /// <summary>
        /// Gets the kind of member, which equals to <see cref="MemberKinds.Event"/>.
        /// </summary>
        public override MemberKinds Kind => MemberKinds.Event;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitEvent(this);
            return visitor;
        }
    }
}