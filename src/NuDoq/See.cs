using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>see</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/acd0tfbe(v=vs.80).aspx.
    /// </remarks>
    public class See : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="See" /> class.
        /// </summary>
        /// <param name="cref">The member id of the referenced member, if any.</param>
        /// <param name="langword">The element langword, if any.</param>
        /// <param name="content">The link's text label, if any.</param>
        /// <param name="elements">The child elements.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public See(string cref, string langword, string content, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
            Cref = cref;
            Langword = langword;
            Content = content;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitSee(this);
            return visitor;
        }

        /// <summary>
        /// Gets the member id of the referenced member.
        /// </summary>
        public string Cref { get; }

        /// <summary>
        /// Gets the original langword attribute.
        /// </summary>
        public string Langword { get; }

        /// <summary>
        /// Gets the reference's text.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<see>" + base.ToString();
    }
}