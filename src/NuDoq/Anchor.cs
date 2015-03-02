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

namespace NuDoq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the HTML (<c>&lt;a&gt;</c>) Element,
    /// which can be used as an extended documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/acd0tfbe(v=vs.80).aspx.
    /// </remarks>
    public class Anchor : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Anchor" /> class.
        /// </summary>
        /// <param name="href">The anchor's destination address.</param>
        /// <param name="content">The link's text label, if any.</param>
        /// <param name="elements">The child elements.</param>
        public Anchor(string href, string content, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Href = href;
            this.Content = content;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitAnchor(this);
            return visitor;
        }

        /// <summary>
        /// Gets the anchor's destination link address.
        /// </summary>
        public string Href { get; private set; }

        /// <summary>
        /// Gets the reference's text.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("<a href=\"{0}\">{1}</a>", Href, Content);
        }
    }
}