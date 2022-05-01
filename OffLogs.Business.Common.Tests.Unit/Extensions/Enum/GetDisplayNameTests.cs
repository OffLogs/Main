using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Extensions.Enum
{
    enum TestEnum
    {
        WithoutDescription = 10,
        
        [Description("Car Name")]
        Car = 12,
        
        [
            StringLength(12),
            Description("MyValueName"),
            MaxLength(512)
        ]
        MyValue1 = 11,
    }

    public class GetDisplayName
    {

        [Fact]
        public void ShouldReceiveDisplayNameWithoutDesc()
        {
            Assert.Equal(
                "WithoutDescription",
                TestEnum.WithoutDescription.GetDisplayName()
            );
        }
        
        [Fact]
        public void ShouldReceiveDisplayNameWithDesc()
        {
            Assert.Equal(
                "Car Name",
                TestEnum.Car.GetDisplayName()
            );
        }
        
        [Fact]
        public void ShouldReceiveDisplayNameIfSeveralAttributes()
        {
            Assert.Equal(
                "MyValueName",
                TestEnum.MyValue1.GetDisplayName()
            );
        }
    }
}
