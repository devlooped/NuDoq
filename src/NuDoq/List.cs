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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the <c>list</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/y3ww3c7e(v=vs.80).aspx.
    /// </remarks>
    public class List : Container
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="List"/> class.
        /// </summary>
        /// <param name="type">The type of list, which can be "bullet", "number" or "table".</param>
        /// <param name="elements">The elements.</param>
        public List(string type, IEnumerable<Element> elements)
            : base(elements)
        {
            this.Type = ListType.Unknown;
            if (!string.IsNullOrEmpty(type))
            {
                try
                {
                    this.Type = (ListType)System.Enum.Parse(typeof(ListType), System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(type));
                }
                catch (ArgumentException) { }
            }
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitList(this);
            return visitor;
        }

        /// <summary>
        /// Gets the type of list.
        /// </summary>
        public ListType Type { get; private set; }

        /// <summary>
        /// Gets the header from the contained elements, if any.
        /// </summary>
        public ListHeader Header { get { return this.Elements.OfType<ListHeader>().FirstOrDefault(); } }

        /// <summary>
        /// Gets the items from the contained elements.
        /// </summary>
        public IEnumerable<Item> Items { get { return this.Elements.OfType<Item>(); } }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return "<list>" + base.ToString();
        }
    }
}