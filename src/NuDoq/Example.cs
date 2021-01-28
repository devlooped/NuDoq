using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>example</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/9w4cf933(v=vs.80).aspx.
    /// </remarks>
    public class Example : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Example"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        public Example(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitExample(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<example>" + base.ToString();
    }
}