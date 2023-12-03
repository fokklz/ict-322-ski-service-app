using SkiServiceModels.DTOs;
using System.Net;

namespace SkiServiceApp.Interfaces
{
    public interface IHTTPResponse<TResponse>
    {
        bool IsSuccess { get; }
        HttpStatusCode StatusCode { get; }

        Task<ErrorData?> ParseError();
        Task<TResponse?> ParseSuccess();
    }
}