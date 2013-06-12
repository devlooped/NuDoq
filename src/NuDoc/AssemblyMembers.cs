#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

namespace ClariusLabs.NuDoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// Composite of all lazy-read members from an assembly 
    /// passed to <see cref="Reader.Read(Assembly)"/>.
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
            this.Assembly = assembly;
            this.IdMap = idMap;
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
    }
}