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
    using System.Linq;

    /// <summary>
    /// Represents the <c>param</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/8cw818w8(v=vs.80).aspx.
    /// </remarks>
    public class Param : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Param"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="elements">The elements that make up the parameter documentation.</param>
        public Param(string name, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Name = name;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitParam(this);
            return visitor;
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<param>" + base.ToString();
        }
    }
}