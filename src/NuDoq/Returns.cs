using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>returns</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/azda5z79(v=vs.80).aspx.
    /// </remarks>
    public class Returns : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Returns"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        public Returns(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitReturns(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<returns>" + base.ToString();
        }
    }
}