﻿using Newtonsoft.Json;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceApp.Services.API;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Net.Http.Headers;
using System.Text;

namespace SkiServiceApp.Tests.Models
{
    [Collection("Docker Collection")]
    public class CustomListItemTests : IDisposable
    {
        private readonly Environment _environment;
        private readonly IOrderAPIService _orderAPIService;

        private CustomListItem _sample;

        public CustomListItemTests()
        {
            _environment = new Environment().UseAuth();

            _environment.AddService<IOrderAPIService>(new OrderAPIService(_environment.ConfigurationMock.Object));

            _orderAPIService = ServiceLocator.GetService<IOrderAPIService>();

            GetSample().GetAwaiter().GetResult();
        }

        private async Task GetSample()
        {
            _sample = new CustomListItem(await GetOrder());
        }

        private async Task<OrderResponseAdmin> GetOrder()
        {
            var target = await _orderAPIService.GetAsync(1);
            if (target.IsSuccess)
            {
                var parsed = await target.ParseSuccess();
                return parsed;
            }

            return null;
        }

        /// <summary>
        /// Helper method to create a request message with the correct authorization header and content
        /// </summary>
        /// <param name="method">The Method to use</param>
        /// <param name="url">The url to send it to</param>
        /// <param name="data">The data to send (if any)</param>
        /// <returns></returns>
        public HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, object? data)
        {
            var request = new HttpRequestMessage(method, url);
            if (AuthManager.IsLoggedIn)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthManager.Token);
            }
            else
            {
                request.Headers.Authorization = null;
            }
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            return request;
        }

        [Fact]
        public async Task VerifySample()
        {
            Assert.Equal(1, _sample.Order.Id);
        }

        [Fact]
        public async Task ApplyToSampleWorks()
        {
            // Arrange
            var expected = AuthManager.UserId;

            // Act & Assert
            await _sample.Apply(async () =>
            {
                Assert.Equal(expected, _sample.Order.User?.Id);
                Assert.Equal(expected, (await GetOrder()).User.Id);
            });

            // Reset
            var res = await _orderAPIService.UpdateAsync(_sample.Order.Id, new UpdateOrderRequest
            {
                ServiceId = _sample.Order.Service.Id,
                PriorityId = _sample.Order.Priority.Id,
                StateId = _sample.Order.State.Id,
                UserId = null
            });
            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task NextStateForSampleWorks()
        {
            // Arrange
            await _sample.Apply();
            var expected = _sample.Order.State.Id + 1;

            // Act & Assert
            await _sample.GoNextState(async () =>
            {
                Assert.Equal(expected, _sample.Order.State.Id);
                Assert.Equal(expected, (await GetOrder()).State.Id);
            });

            // Reset
            var res = await _orderAPIService.UpdateAsync(_sample.Order.Id, new UpdateOrderRequest
            {
                ServiceId = _sample.Order.Service.Id,
                PriorityId = _sample.Order.Priority.Id,
                StateId = expected - 1,
                UserId = null
            });
            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task CancelForSampleWorks()
        {
            // Arrange
            await _sample.Apply();

            // Act & Assert
            await _sample.Cancel(async () =>
            {
                Assert.True(_sample.Order.IsDeleted);
                Assert.True((await GetOrder()).IsDeleted);
            });

            // Reset not supported from the backend
        }


        [Fact]
        public async Task UpdateDetailsWorks()
        {
            // Arrange
            var expected = "Test";

            // Act & Assert
            await _sample.UpdateDetails(new UpdateOrderRequest
            {
                Name = expected
            }, async () =>
            {
                Assert.Equal(expected, _sample.Order.Name);
                Assert.Equal(expected, (await GetOrder()).Name);
            });
        }

        public void Dispose()
        {
            _environment.Dispose();
        }
    }
}
