using System.Linq;
using Demo;
using Xunit;

namespace NuDoq
{
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