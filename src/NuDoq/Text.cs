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
        {
            Content = content;
        }

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
        public string Content { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<text>" + base.ToString();
        }
    }
}