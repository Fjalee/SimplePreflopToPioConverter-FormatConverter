using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace FormatConverter.UnitTests
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public static IFixture FixtureFactory()
        {
            var f = new Fixture();
            f.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return f;
        }


        public AutoDomainDataAttribute() : base(FixtureFactory) { }
    }

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoDomainDataAttribute(), objects) { }
    }
}
