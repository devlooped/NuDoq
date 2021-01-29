using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>seealso</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/xhd7ehkk(v=vs.80).aspx.
    /// </remarks>
    public class SeeAlso : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeeAlso"/> class.
        /// </summary>
        /// <param name="cref">The member id of the referenced member.</param>
        /// <param name="content">The link's text label, if any.</param>
        /// <param name="elements">The child elements.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public SeeAlso(string cref, string content, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
        {
            Cref = cref;
            Content = content;
        }

        internal SeeAlso(string content, IEnumerable<Element> elements, IDictionary<string, string> attributes)
            : base(elements, attributes)
            => Content = content;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitSeeAlso(this);
            return visitor;
        }

        /// <summary>
        /// Gets the reference's text.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets the member id of the referenced member.
        /// </summary>
        public string Cref
        {
            get => Attributes.TryGetValue("cref", out var value) ? value : "";
            set => Attributes.SetOrRemove("cref", value);
        }

        /// <summary>
        /// Gets the hyperlink, if present.
        /// </summary>
        public string? Href
        {
            get => Attributes.TryGetValue("href", out var value) ? value : "";
            set => Attributes.SetOrRemove("href", value);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<seealso>" + base.ToString();
    }
}