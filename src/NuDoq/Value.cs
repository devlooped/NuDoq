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
        public Value(IEnumerable<Element> elements)
            : base(elements)
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<value>" + base.ToString();
        }
    }
}