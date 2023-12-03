using Microsoft.Extensions.Configuration;
using SkiServiceApp.Common;
using SkiServiceApp.Interfaces.API;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Services.API
{
    public class StateAPIService : BaseAPIService<CreateStateRequest, UpdateStateRequest, StateResponse>, IStateAPIService
    {
        public StateAPIService(IConfiguration configuration) : base(configuration, "states")
        {
        }
    }
}
