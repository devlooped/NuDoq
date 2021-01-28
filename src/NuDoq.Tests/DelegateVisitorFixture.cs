using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Demo;
using Xunit;

namespace NuDoq
{
    public class DelegateVisitorFixture
    {
        readonly ConcurrentDictionary<PropertyInfo, int> calls = new ConcurrentDictionary<PropertyInfo, int>();

        [Fact]
        public void when_visiting_root_then_visits_all_delegates()
        {
            var calledMethod = GetType().GetMethod("Called", BindingFlags.Instance | BindingFlags.NonPublic);
            var delegates = new VisitorDelegates();
            foreach (var prop in typeof(VisitorDelegates).GetProperties())
            {
                var actionTypeParam = prop.PropertyType.GetGenericArguments()[0];
                var expression = Expression.Lambda(
                    prop.PropertyType,
                    Expression.Call(Expression.Constant(this), calledMethod, Expression.Constant(prop)),
                    Expression.Parameter(actionTypeParam));

                prop.SetValue(delegates, expression.Compile());
            }

            var members = DocReader.Read(typeof(Sample).Assembly);
            members.Accept(new DelegateVisitor(delegates));

            foreach (var prop in typeof(VisitorDelegates).GetProperties())
            {
                if (prop == Reflect<VisitorDelegates>.GetProperty(x => x.VisitUnknownMember))
                    continue;

                Assert.True(calls.GetOrAdd(prop, 0) > 0, string.Format("Delegate on {0} was not called.", prop.Name));
            }

            when_visiting_root_then_can_validate_all_crefs();
        }

        /// <summary>
        /// Summary seealso <seealso href="https://www.google.com"/> or <see href="https://foo.bar.baz"/>.
        /// </summary>
        /// <remarks>
        /// Remarks seealso <seealso href="https://www.google.com"/> or <see href="https://foo.bar.baz"/>.
        /// </remarks>
        [Fact]
        public void when_visiting_root_then_can_validate_all_crefs()
        {
            var members = DocReader.Read(Assembly.GetExecutingAssembly());
            var http = new HttpClient();

            var validUrls = 0;
            var invalidUrls = 0;

            void ValidateUrl(string? href)
            {
                if (string.IsNullOrEmpty(href))
                    return;

                try
                {
                    if (http.GetAsync(href).Result.IsSuccessStatusCode)
                        validUrls++;
                    else
                        invalidUrls++;
                }
                catch
                {
                    invalidUrls++;
                }
            }

            var visitor = new DelegateVisitor(new VisitorDelegates
            {
                VisitSee = see => ValidateUrl(see.Href),
                VisitSeeAlso = seealso => ValidateUrl(seealso.Href),
            });

            members.Accept(visitor);

            Assert.Equal(2, validUrls);
            Assert.Equal(2, invalidUrls);
        }

        void Called(PropertyInfo property)
        {
            calls[property] = calls.GetOrAdd(property, 0) + 1;
        }
    }
}