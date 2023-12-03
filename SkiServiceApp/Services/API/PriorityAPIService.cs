using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Services.API
{
    public class PriorityAPIService : BaseAPIService<CreatePriorityRequest, UpdatePriorityRequest, PriorityResponse>, IPriorityAPIService
    {
        public PriorityAPIService(IConfiguration configuration) : base(configuration, "priorities")
        {
        }
    }
}
