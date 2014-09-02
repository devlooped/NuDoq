#region Apache Licensed
/*
 Copyright 2013 Daniel Cazzulino

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
    using System.Collections.Generic;

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
        public SeeAlso(string cref, string content, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Cref = cref;
            this.Content = content;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitSeeAlso(this);
            return visitor;
        }

        /// <summary>
        /// Gets the member id of the referenced member.
        /// </summary>
        public string Cref { get; private set; }

        /// <summary>
        /// Gets the reference's text.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<seealso>" + base.ToString();
        }
    }
}