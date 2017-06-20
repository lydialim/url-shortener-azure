
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.NUnit3;

namespace UrlShortener.Services.Tests.Framework
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(new Fixture()
                                                    .Customize(new GreedyConstructorCustomization())
                                                    .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
