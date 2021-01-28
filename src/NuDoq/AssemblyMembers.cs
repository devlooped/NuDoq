using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace NuDoq
{
    /// <summary>
    /// Composite of all lazy-read members from an assembly 
    /// passed to <see cref="DocReader.Read(Assembly)"/>.
    /// </summary>
    public class AssemblyMembers : DocumentMembers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentMembers" /> class.
        /// </summary>
        /// <param name="assembly">The assembly that was used to read the documented members.</param>
        /// <param name="idMap">The id map of reflection members to documentation ids.</param>
        /// <param name="xml">The source XML document that was used to read the members.</param>
        /// <param name="members">The lazily-read members of the set.</param>
        public AssemblyMembers(Assembly assembly, MemberIdMap idMap, XDocument xml, IEnumerable<Member> members)
            : base(xml, members)
        {
            Assembly = assembly;
            IdMap = idMap;
        }

        /// <summary>
        /// Gets the assembly that was used to read the documented members.
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Gets the map of reflection members to documentation ids used 
        /// to augment the <see cref="Member.Info"/> on all documented members 
        /// found in the XML API documentation associated with an assembly.
        /// </summary>
        public MemberIdMap IdMap { get; private set; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitAssembly(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return Assembly.Location + "|" + base.ToString();
        }
    }
}