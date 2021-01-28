using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>remarks</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/3zw4z1ys(v=vs.80).aspx.
    /// </remarks>
    public class Remarks : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Remarks"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        public Remarks(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitRemarks(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<remarks>" + base.ToString();
    }
}