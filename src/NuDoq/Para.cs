using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>para</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/x640hcd2(v=vs.80).aspx.
    /// </remarks>
    public class Para : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Para"/> class.
        /// </summary>
        /// <param name="elements">The contained elements within this instance.</param>
        public Para(IEnumerable<Element> elements)
            : base(elements)
        {
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitPara(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<para>" + base.ToString();
    }
}