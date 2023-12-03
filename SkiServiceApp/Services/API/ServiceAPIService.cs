using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Services.API
{
    public class ServiceAPIService : BaseAPIService<CreateServiceRequest, UpdateServiceRequest, ServiceResponse>, IServiceAPIService
    {
        public ServiceAPIService(IConfiguration configuration) : base(configuration, "services")
        {
        }
    }
}
