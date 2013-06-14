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
    /// An unsupported or unknown XML documentation element.
    /// </summary>
    public class UnknownElement : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownElement"/> class.
        /// </summary>
        /// <param name="xml">The <see cref="XElement"/> containing the entire element markup.</param>
        /// <param name="content">The child content.</param>
        public UnknownElement(XElement xml, IEnumerable<Element> content)
            : base(content)
        {
            this.Xml = xml;
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> containing the entire element markup.
        /// </summary>
        public XElement Xml { get; private set; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitUnknownElement(this);
            return visitor;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "unknown:<" + this.Xml.Name.LocalName + ">" + base.ToString();
        }
    }
}