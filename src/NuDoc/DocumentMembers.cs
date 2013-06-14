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
    using System.Xml.Linq;

    /// <summary>
    /// Composite of all lazy-read members in a documentation file 
    /// returned from the <see cref="Reader.Read(string)"/>.
    /// </summary>
    public class DocumentMembers : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentMembers" /> class.
        /// </summary>
        /// <param name="xml">The source XML document that was used to read the members.</param>
        /// <param name="members">The lazily-read members of the set.</param>
        public DocumentMembers(XDocument xml, IEnumerable<Member> members)
            // In .NET 3.5 there's no covariance on IEnumerable.
            : base(members.OfType<Element>())
        {
            this.Xml = xml;
        }

        /// <summary>
        /// Gets the source XML document that was used to read the members.
        /// </summary>
        public XDocument Xml { get; private set; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitDocument(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return new Uri(this.Xml.BaseUri).LocalPath;
        }
    }
}