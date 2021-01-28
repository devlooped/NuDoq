using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NuDoq
{
    /// <summary>
    /// Composite of all lazy-read members in a documentation file 
    /// returned from the <see cref="DocReader.Read(string)"/>.
    /// </summary>
    public class DocumentMembers : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentMembers" /> class.
        /// </summary>
        /// <param name="xml">The source XML document that was used to read the members.</param>
        /// <param name="members">The lazily-read members of the set.</param>
        public DocumentMembers(XDocument xml, IEnumerable<Member> members)
            : base(members, new Dictionary<string, string>())
            => Xml = xml;

        /// <summary>
        /// Gets the source XML document that was used to read the members.
        /// </summary>
        public XDocument Xml { get; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitDocument(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString() => new Uri(Xml.BaseUri).LocalPath;
    }
}