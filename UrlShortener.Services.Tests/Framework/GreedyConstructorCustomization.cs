
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace UrlShortener.Services.Tests.Framework
{
    internal class GreedyConstructorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ShortenerService>(c => c.FromFactory(
                                                        new MethodInvoker(
                                                            new GreedyConstructorQuery())));
        }
    }
}
