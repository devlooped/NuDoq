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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Demo;
    using Xunit;

    public class TrimLinesBugRepro
    {
        [Fact]
        public void WhenCodeContainsEmptyLines_ThenToTextShouldNotThrow()
        {
            var members = DocReader.Read(typeof(ProviderType).Assembly);

            var code = members.Elements
                .OfType<Member>()
                .Single(x => x.Id == "T:Demo.SampleTypeWithCodeWithEmptyLines")
                .Traverse()
                .OfType<Code>()
                .Single();

            var text = code.ToText();

            Assert.NotNull(text);
        }

        [Fact]
        public void WhenRemarksContainsInvalidIndentation_ThenToTextShouldNotThrow()
        {
            var members = DocReader.Read(typeof(ProviderType).Assembly);

            var remarks = members.Elements
                .OfType<Member>()
                .Single(x => x.Id == "T:Demo.SampleTypeWithInvalidRemarksIndentation")
                .Traverse()
                .OfType<Remarks>()
                .Single();

            var text = remarks.ToText();

            Assert.NotNull(text);
        }
    }
}