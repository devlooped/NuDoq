using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>exception</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/w1htk11d(v=vs.80).aspx.
    /// </remarks>
    public class Exception : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="cref">The member id of the exception type.</param>
        /// <param name="elements">The elements that explain when this exception is thrown.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public Exception(string cref, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
            Cref = cref;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitException(this);
            return visitor;
        }

        /// <summary>
        /// Gets the member id of the exception type.
        /// </summary>
        public string Cref { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<exception>" + base.ToString();
    }
}