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
    /// Represents the <c>typeparam</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/ms173191(v=vs.80).aspx.
    /// </remarks>
    public class TypeParam : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeParam"/> class.
        /// </summary>
        /// <param name="name">The name of the type parameter.</param>
        /// <param name="elements">The elements that make up the documentation.</param>
        public TypeParam(string name, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Name = name;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitTypeParam(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the type parameter.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<typeparam>" + base.ToString();
        }
    }
}