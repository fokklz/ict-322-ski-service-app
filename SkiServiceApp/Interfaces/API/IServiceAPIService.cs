using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces.API
{
    public interface IServiceAPIService : IBaseAPIService<CreateServiceRequest, UpdateServiceRequest, ServiceResponse>
    {
    }
}
