using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Mapping;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Helpers.SecurityUtilsTests
{
    public class TimeBasedStringGenerationTest
    {
        [Fact]
        public void ShouldGenerateToken()
        {
            var token = SecurityUtil.GetTimeBasedToken();
            Assert.True(!string.IsNullOrEmpty(token));
            Assert.True(token.Length >= 10);
        }

        [Fact]
        public void GeneratedTokensShouldBeUniq()
        {
            var concurrentBag = new ConcurrentBag<string>();
            var bagAddTasks = new List<Task>();
            for (int i = 0; i < 1000000; i++)
            {
                bagAddTasks.Add(Task.Run(
                    () => concurrentBag.Add(SecurityUtil.GetTimeBasedToken())
                ));
            }

            // Wait for all tasks to complete
            Task.WaitAll(bagAddTasks.ToArray());

            Assert.Equal(concurrentBag.Count(), concurrentBag.Distinct().Count());
        }

        [Fact]
        public void BigArrayGenerationShouldNotBeLongerThan3Seconds()
        {
            var start = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                SecurityUtil.GetTimeBasedToken();
            }
            var end = DateTime.Now;
            Assert.True(end - start < TimeSpan.FromSeconds(3));
        }
    }
}