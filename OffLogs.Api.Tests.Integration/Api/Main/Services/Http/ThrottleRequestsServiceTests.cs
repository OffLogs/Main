using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Exceptions;
using OffLogs.Business.Common.Exceptions.Common;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Redis.Clients;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Http
{
    public class ThrottleRequestsServiceTests : MyApiIntegrationTest
    {
        private static long _itemId = 0;
        private readonly RequestItemType _itemType = RequestItemType.Application;

        public ThrottleRequestsServiceTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            
        }

        [Fact]
        public async Task ShouldCreateNewItemAndIncreaseCounter()
        {
            var itemId = ++_itemId;
            for (int i = 0; i <= 5 + 1; i++)
            {
                var actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                    _itemType,
                    itemId,
                    TimeSpan.FromSeconds(1)
                );
                Assert.Equal(i + 1, actualCounter);
            }
        }

        [Fact]
        public async Task ShouldResetCounterIfPeriodCameOut()
        {
            var itemId = ++_itemId;
            var maxItemsCounter = 20;
            var period = TimeSpan.FromSeconds(5);
            var actualCounter = 0;
            for (int i = 0; i <= 3; i++)
            {
                actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                    _itemType, 
                    itemId, 
                    period, 
                    maxItemsCounter
                );
                Assert.Equal(i + 1, actualCounter);
            }
            Thread.Sleep(period);

            actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                _itemType, 
                itemId,
                period, 
                maxItemsCounter
            );
            Assert.Equal(0, actualCounter);
        }

        [Fact]
        public async Task ShouldThrowExceptionIfCounterTooBig()
        {
            var itemId = ++_itemId;
            var actualCounter = 0;
            var maxItemsCounter = 20;

            await Assert.ThrowsAsync<TooManyRequestsException>(async () => {
                for (int i = 0; i <= maxItemsCounter + 1; i++)
                {
                    actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                        _itemType, 
                        itemId, 
                        TimeSpan.FromSeconds(5), 
                        maxItemsCounter
                    );
                    Assert.Equal(i + 1, actualCounter);
                }    
            });
            Assert.Equal(maxItemsCounter + 1, actualCounter);
        }

        [Fact]
        public async Task ShouldCleanExistsRecords()
        {
            var itemId = ++_itemId;
            var maxItemsCounter = 20;
            var period = TimeSpan.FromSeconds(5);
            var actualCounter = 0;
            for (int i = 0; i <= 3; i++)
            {
                actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                    _itemType, 
                    itemId, 
                    period, 
                    maxItemsCounter
                );
            }
            Assert.Equal(4, actualCounter);
            ThrottleRequestsService.Clean();

            actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                _itemType, 
                itemId, 
                period, 
                maxItemsCounter
            );
            Assert.Equal(1, actualCounter);
        }

        [Fact]
        public async Task ShouldNotIncrementCounterForOtherType()
        {
            var itemId = ++_itemId;
            var maxItemsCounter = 20;
            var period = TimeSpan.FromSeconds(5);
            var actualCounter = 0;
            for (int i = 0; i <= 3; i++)
            {
                actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                    RequestItemType.Application,
                    itemId,
                    period,
                    maxItemsCounter
                );
            }
            Assert.Equal(4, actualCounter);
            
            actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionAsync(
                RequestItemType.User,
                itemId,
                period,
                maxItemsCounter
            );
            Assert.Equal(1, actualCounter);
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic)]
        [InlineData(PaymentPackageType.Standart)]
        [InlineData(PaymentPackageType.Pro)]
        public async Task ShouldReceivePackageTypeFromRedisAndThrowExceptionForStandartPackage(PaymentPackageType expectedPackage)
        {
            var userModel = (DataSeeder.CreateActivatedUser().Result);
            var user = userModel.Original;
            var userInfoRedisClient = _factory.Services.GetRequiredService<IUserInfoRedisClient>();

            if (expectedPackage != PaymentPackageType.Basic)
            {
                await PaymentPackageService.ExtendOrChangePackage(user, expectedPackage, 30);    
            }
            await CommitDbChanges();
            // Fill Redis DB
            await userInfoRedisClient.SeedUsersPackages();
            
            var maxItemsCounter = expectedPackage.GetRestrictions().MaxApiRequests;
            var actualCounter = 0;

            await Assert.ThrowsAsync<TooManyRequestsException>(async () => {
                for (int i = 0; i <= maxItemsCounter + 1; i++)
                {
                    actualCounter = await ThrottleRequestsService.CheckOrThrowExceptionByApplicationIdAsync(
                        userModel.Application.Id,
                        user.Id
                    );
                    Assert.Equal(i + 1, actualCounter);
                }    
            });
            Assert.Equal(maxItemsCounter + 1, actualCounter);
        }
    }
}
