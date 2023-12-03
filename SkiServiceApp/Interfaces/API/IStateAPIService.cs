using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces.API
{
    public interface IStateAPIService : IBaseAPIService<CreateStateRequest, UpdateStateRequest, StateResponse>
    {
    }
}
