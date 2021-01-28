using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>value</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/azda5z79(v=vs.80).aspx.
    /// </remarks>
    public class Value : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Value"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public Value(IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitValue(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<value>" + base.ToString();
    }
}