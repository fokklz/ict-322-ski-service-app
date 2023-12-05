using SkiServiceApp.Models;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Interfaces
{
    public interface IBaseAPIServiceBase
    {
        HttpClient Client { get; }
        void SetAuthorizationHeader(string? token);
    }

    public interface IBaseAPIService<TCreateRequest, TUpdateRequest, TResponse> : IBaseAPIServiceBase
        where TCreateRequest : class
        where TUpdateRequest : class
        where TResponse : class
    {
        Task<HTTPResponse<TResponse>> CreateAsync(TCreateRequest data);
        Task<HTTPResponse<DeleteResponse>> DeleteAsync(int id);
        Task<HTTPResponse<List<TResponse>>> GetAllAsync();
        Task<HTTPResponse<TResponse>> GetAsync(int id);
        Task<HTTPResponse<TResponse>> UpdateAsync(int id, TUpdateRequest data);
    }
}