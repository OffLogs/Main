using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Helpers.FormatUtilTests
{
    public class ClearUserNameTest
    {
        [Theory]
        [InlineData("Some !@#$%*()-  User name", "some-username")]
        [InlineData("^&*()?<>,.", "")]
        [InlineData("ИмяПользователя", "")]
        public void ShouldClearUserName(string userName, string expectedUserName)
        {
            var actual = FormatUtil.ClearUserName(userName);
            Assert.Equal(actual, expectedUserName);
        }
    }
}