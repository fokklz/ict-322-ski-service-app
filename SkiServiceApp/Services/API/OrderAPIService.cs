using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SkiServiceApp.Services.API
{
    public class OrderAPIService : BaseAPIService<CreateOrderRequest, UpdateOrderRequest, OrderResponseAdmin>, IOrderAPIService
    {
        public OrderAPIService(IConfiguration configuration) : base(configuration, "orders")
        {
        }

        /// <summary>
        /// Get all orders by user id
        /// </summary>
        /// <param name="userId">The user to get the orders for</param>
        /// <returns>a list of orders assigned to that user</returns>
        public async Task<HTTPResponse<List<OrderResponse>>> GetAllByUserIdAsync(int userId)
        {
            var res = await _sendRequest(HttpMethod.Get, _url("user", userId.ToString()));
            return new HTTPResponse<List<OrderResponse>>(res);
        }

        /// <summary>
        /// Get all orders by priority id
        /// </summary>
        /// <param name="priorityId">The priority to get the orders for</param>
        /// <returns>a list of orders with that priority</returns>
        public async Task<HTTPResponse<List<OrderResponse>>> GetAllByPriorityIdAsync(int priorityId)
        {
            var res = await _sendRequest(HttpMethod.Get, _url("priority", priorityId.ToString()));
            return new HTTPResponse<List<OrderResponse>>(res);
        }

        /// <summary>
        /// Get all orders by status id
        /// </summary>
        /// <param name="statusId">The status to get the orders for</param>
        /// <returns>a list of orders with that status</returns>
        public async Task<HTTPResponse<List<OrderResponse>>> GetAllByStatusIdAsync(int statusId)
        {
            var res = await _sendRequest(HttpMethod.Get, _url("status", statusId.ToString()));
            return new HTTPResponse<List<OrderResponse>>(res);
        }

        /// <summary>
        /// Get all orders by service id
        /// </summary>
        /// <param name="serviceId">The service to get the orders for</param>
        /// <returns>a list of orders with that service</returns>
        public async Task<HTTPResponse<List<OrderResponse>>> GetAllByServiceIdAsync(int serviceId)
        {
            var res = await _sendRequest(HttpMethod.Get, _url("service", serviceId.ToString()));
            return new HTTPResponse<List<OrderResponse>>(res);
        }
    }
}
