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

namespace Demo
{
    using System;

    /// <summary>
    /// </summary>
    /// <remarks>
    /// Remarks.
    ///   <example>
    ///       Example with code:<code>
    ///           var code = new Code();
    ///           var length = code.Length + 1;
    ///       </code><para>
    ///           With a paragraph with <c>inline code</c>.
    ///       </para>
    ///   </example>
    ///   <list type="table">
    ///       <listheader>
    ///           <term>Term</term>
    ///           <description>Description</description>
    ///       </listheader>
    ///       <item>
    ///           <term>ItemTerm</term>
    ///           <description>ItemDescription</description>
    ///       </item>
    ///   </list>
    /// </remarks>
    /// <seealso cref="Provider"/>
    /// <exception cref="Provider">Exception</exception>
    public class Provider : IProvider
    {
        /// <summary>
        /// Occurs when the provider is connected.
        /// </summary>
        public event EventHandler Connected = (sender, args) => {};
    }
}