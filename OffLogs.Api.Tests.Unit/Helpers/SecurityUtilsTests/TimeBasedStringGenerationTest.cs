using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Mapping;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Helpers.SecurityUtilsTests
{
    public class TimeBasedStringGenerationTest
    {
        [Fact]
        public void ShouldGenerateHash()
        {
            var randomString = SecurityUtil.GetTimeBasedRandomString();
            Assert.NotEmpty(randomString);
            Assert.Equal(44, randomString.Length);
        }
        
        [Fact]
        public void ShouldGenerateUniqHashes()
        {
            var hashes = new List<string>();
            Parallel.For(0, 10000, index =>
            {
                hashes.Add(
                    SecurityUtil.GetTimeBasedRandomString()   
                ); 
            });
            var count1 = hashes.Distinct().Count();
            var count2 = hashes.Count();
            Assert.True(
                hashes.Distinct().Count() == hashes.Count()  
            );
        }
    }
}