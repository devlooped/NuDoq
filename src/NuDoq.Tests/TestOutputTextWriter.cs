using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace NuDoq
{
    class TestOutputTextWriter : TextWriter
    {
        StringBuilder buffer = new StringBuilder();
        readonly ITestOutputHelper output;

        public TestOutputTextWriter(ITestOutputHelper output) => this.output = output;

        public override Encoding Encoding => Encoding.Unicode;

        public override void Write(char value) => buffer.Append(value);

        public override void WriteLine(string value)
        {
            buffer.Append(value);
            Flush();
        }

        public override void Flush()
        {
            var sb = buffer;
            if (sb.Length > 0)
            {
                buffer = new StringBuilder();
                output.WriteLine(sb.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            Flush();
            base.Dispose(disposing);
        }
    }
}
