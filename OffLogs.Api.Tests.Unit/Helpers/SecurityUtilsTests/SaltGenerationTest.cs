using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Helpers.SecurityUtilsTests
{
    public class SaltGenerationTest
    {
        [Theory]
        [InlineData(12, 12)]
        [InlineData(256, 256)]
        public void ShouldGenerateSalt(int size, int expectedSize)
        {
            var bytes = SecurityUtil.GenerateSalt(size);
            Assert.Equal(expectedSize, bytes.Length);
        }
        
        [Theory]
        [InlineData(12)]
        [InlineData(256)]
        [InlineData(512)]
        public void ShouldReturnRandomBytes(int size)
        {
            var result1 = SecurityUtil.GenerateSalt(size);
            var result2 = SecurityUtil.GenerateSalt(size);
            Assert.False(result1.CompareTo(result2));
        }
    }
}