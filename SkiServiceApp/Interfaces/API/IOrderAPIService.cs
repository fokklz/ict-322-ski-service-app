using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces.API
{
    public interface IOrderAPIService : IBaseAPIService<CreateOrderRequest, UpdateOrderRequest, OrderResponseAdmin>
    {
        Task<HTTPResponse<List<OrderResponse>>> GetAllByPriorityIdAsync(int priorityId);
        Task<HTTPResponse<List<OrderResponse>>> GetAllByServiceIdAsync(int serviceId);
        Task<HTTPResponse<List<OrderResponse>>> GetAllByStatusIdAsync(int statusId);
        Task<HTTPResponse<List<OrderResponse>>> GetAllByUserIdAsync(int userId);
    }
}