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
    /// <summary>
    /// Represents the <c>paramref</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/wb7x2fhw(v=vs.80).aspx.
    /// </remarks>
    public class ParamRef : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamRef"/> class.
        /// </summary>
        /// <param name="name">The name of the referenced parameter.</param>
        public ParamRef(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitParamRef(this);
            return visitor;
        }

        /// <summary>
        /// Gets the name of the referenced parameter.
        /// </summary>
        public string Name { get; private set; }
    }
}