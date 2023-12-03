using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces.API
{
    public interface IPriorityAPIService : IBaseAPIService<CreatePriorityRequest, UpdatePriorityRequest, PriorityResponse>
    {
    }
}
