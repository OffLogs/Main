using System;
using OffLogs.Business.Extensions;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Extensions.TimeSpans
{
    public class ToReadableStringTests
    {

        [Fact]
        public void ShouldReturnStringFromSeconds()
        {
            Assert.Equal(
                "1 second",
                TimeSpan.Parse("0.0:0:1").ToReadableString()
            );
            
            Assert.Equal(
                "12 seconds",
                TimeSpan.Parse("0.0:0:12").ToReadableString()
            );
        }
        
        [Fact]
        public void ShouldReturnStringForMultyTimeSpan()
        {
            Assert.Equal(
                "8 days, 7 hours, 6 minutes, 5 seconds",
                TimeSpan.Parse("8.7:6:5").ToReadableString()
            );
        }
    }
}
