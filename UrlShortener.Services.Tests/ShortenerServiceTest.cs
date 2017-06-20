
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using System.Threading.Tasks;
using UrlShortener.Services.Tests.Framework;

namespace UrlShortener.Services.Tests
{
    public class ShortenerServiceTest
    {
        [Test, AutoMoqData]
        public async Task CreateShortUrlAsync_ValidLongUrl_ReturnsNewShortCode(
            [Frozen] Mock<IShortenUrlRepository> mockShortUrlRepository,
            ShortenerService sut)
        {
            // Given
            mockShortUrlRepository.Setup(m => m.FindShortCode(It.IsAny<string>()))
                                  .Returns(string.Empty);

            // When
            var result = await sut.CreateShortUrlAsync("http://www.google.com/");

            // Then
            mockShortUrlRepository.Verify(m => m.SaveAsync(result.Item1, It.IsAny<string>()));

            Assert.IsNotEmpty(result.Item1); // shortcode result
            Assert.IsNull(result.Item2); // error should be null
        }

        [Test, AutoMoqData]
        public async Task CreateShortUrlAsync_ExistingLongUrl_ReturnsSameShortCode(
            string fakeShortCode,
            [Frozen] Mock<IShortenUrlRepository> mockShortUrlRepository,
            ShortenerService sut)
        {
            // Given
            mockShortUrlRepository.Setup(m => m.FindShortCode(It.IsAny<string>()))
                                  .Returns(fakeShortCode);

            // When
            var result = await sut.CreateShortUrlAsync("http://www.google.com/");

            // Then
            Assert.AreEqual(result.Item1, fakeShortCode); // shortcode result
            Assert.IsNull(result.Item2); // error should be null
        }

        [Test, AutoMoqData]
        public async Task CreateShortUrlAsync_NonHttpUrl_ReturnsError(ShortenerService sut)
        {
            // When
            var result = await sut.CreateShortUrlAsync("ftp://ftp.shouldfail.com/");

            // Then
            Assert.IsNull(result.Item1);
            Assert.IsNotEmpty(result.Item2);
        }
    }
}
