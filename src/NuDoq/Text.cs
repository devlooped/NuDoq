using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Represents basic literal text in the documentation.
    /// </summary>
    public class Text : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="content">The literal text content.</param>
        public Text(string content)
            : base(new Dictionary<string, string>())
            => Content = content;

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitText(this);
            return visitor;
        }

        /// <summary>
        /// Gets the literal text content
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => "<text>" + base.ToString();
    }
}