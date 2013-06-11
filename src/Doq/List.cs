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

namespace ClariusLabs.Doq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class List : Container
    {
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

        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitList(this);
            return visitor;
        }

        public ListType Type { get; private set; }
        public ListHeader Header { get { return this.Elements.OfType<ListHeader>().FirstOrDefault(); } }
        public IEnumerable<Item> Items { get { return this.Elements.OfType<Item>(); } }
    }
}