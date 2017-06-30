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
    /// <summary>
    /// Represents the <c>code</c> documentation tag.
    /// </summary>
    /// <remarks>
    /// See http://msdn.microsoft.com/en-US/library/f8hahtxf(v=vs.80).aspx.
    /// </remarks>
    public class Code : Element
    {
        /// <summary>
        /// Gets or sets the code source, if any.
        /// </summary>
        /// 
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the #pragma region of the <see cref="Source"/>, if any.
        /// </summary>
        /// 
        public string Region { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code"/> class 
        /// with the given content.
        /// </summary>
        public Code(string content, string source, string region)
        {
            this.Content = content;
            this.Source = source;
            this.Region = region;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        public override TVisitor Accept<TVisitor>(TVisitor visitor)
        {
            visitor.VisitCode(this);
            return visitor;
        }

        /// <summary>
        /// Gets the code content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            if (Source != null)
                return string.Format("<code source=\"{0}>\"", Source) + base.ToString();
            return "<code>" + base.ToString();
        }
    }
}