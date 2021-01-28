using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NuDoq;
using Xunit;
using Xunit.Abstractions;

namespace NuDoq
{
    public class ElementTests
    {
        readonly ITestOutputHelper output;

        public ElementTests(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void when_enumerating_elements_then_can_list_twice_enumerates_once()
        {
            var enumerations = 0;

            IEnumerable<Element> GetElements()
            {
                yield return new Text("foo");
                yield return new Text("bar");
                enumerations++;
            };

            var element = new Summary(GetElements(), new Dictionary<string, string>());

            var first = element.Elements.Count();
            var second = element.Elements.Count();

            Assert.Contains(element.Elements.OfType<Text>(), e => e.Content == "foo");
            Assert.Contains(element.Elements.OfType<Text>(), e => e.Content == "foo");
            Assert.Contains(element.Elements.OfType<Text>(), e => e.Content == "bar");
            Assert.Contains(element.Elements.OfType<Text>(), e => e.Content == "bar");
        }

        /// <remarks>
        /// <list type="bullet">
        /// <item>foo</item>
        /// <item>bar</item>
        /// <item>baz</item>
        /// </list>
        /// </remarks>
        [Fact]
        public void when_enumerating_elements_then_visit_twice()
        {
            var member = DocReader.Read(Assembly.GetExecutingAssembly());
            var method = member.Elements.OfType<Method>()
                .Where(x => x.Elements.OfType<TypeParam>().Count() > 2)
                .FirstOrDefault();

            Assert.NotNull(method);

            List? currentList = null;
            var count = 0;

            var visitor = new DelegateVisitor(new VisitorDelegates
            {
                VisitList = (List list) => currentList = list,
                VisitItem = (Item item) => { output.WriteLine(currentList.Elements.Count().ToString()); count++; },
            });

            member.Accept(visitor);

            Assert.Equal(3, count);
        }
    }
}