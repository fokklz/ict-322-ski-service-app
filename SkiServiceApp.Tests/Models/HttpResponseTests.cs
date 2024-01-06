using Newtonsoft.Json;
using SkiServiceApp.Models;
using SkiServiceModels.DTOs;
using SkiServiceModels.DTOs.Responses;
using System.Net;
using System.Text;

namespace SkiServiceApp.Tests.Models
{
    public class HttpResponseTests
    {
        [Fact]
        public async Task HttpResponse_Success()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var httpResponse = new HTTPResponse<string>(response);

            Assert.True(httpResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [Fact]
        public async Task HttpResponse_Failure()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpResponse = new HTTPResponse<string>(response);

            Assert.False(httpResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, httpResponse.StatusCode);
        }

        [Fact]
        public async Task HttpResponse_ParseSuccess()
        {
            var expectedResponse = new StateResponse
            {
                Id = 1,
                Name = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponse), Encoding.UTF8, "application/json");
            var httpResponse = new HTTPResponse<StateResponse>(response);

            var parsed = await httpResponse.ParseSuccess();

            Assert.True(httpResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(expectedResponse.Name, parsed.Name);
        }

        [Fact]
        public async Task HttpResponse_ParseError()
        {
            var expectedError = new ErrorData
            {
                MessageCode = "test",
                Breaking = false,
            };
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(JsonConvert.SerializeObject(expectedError), Encoding.UTF8, "application/json");
            var httpResponse = new HTTPResponse<OrderResponse>(response);

            var parsed = await httpResponse.ParseError();

            Assert.False(httpResponse.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, httpResponse.StatusCode);
            Assert.Equal(expectedError.MessageCode, parsed.MessageCode) ;
        }

    }
}
