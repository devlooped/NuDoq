using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>term</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/y3ww3c7e(v=vs.80).aspx.
    /// </remarks>
    public class Term : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Term"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public Term(IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitTerm(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<term>" + base.ToString();
    }
}