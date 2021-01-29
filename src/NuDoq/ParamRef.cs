using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents the <c>paramref</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/wb7x2fhw(v=vs.80).aspx.
    /// </remarks>
    public class ParamRef : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamRef"/> class.
        /// </summary>
        /// <param name="name">The name of the referenced parameter.</param>
        /// <param name="attributes">The attributes of the element, if any.</param>
        public ParamRef(string name, IDictionary<string, string> attributes)
            : base(attributes)
            => Name = name;

        internal ParamRef(IDictionary<string, string> attributes)
            : base(attributes) { }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitParamRef(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the referenced parameter.
        /// </summary>
        public string Name
        {
            get => Attributes.TryGetValue("name", out var value) ? value : "";
            private set => Attributes["name"] = value;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<paramref>" + base.ToString();
    }
}