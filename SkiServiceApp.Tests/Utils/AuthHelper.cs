using Newtonsoft.Json;
using SkiServiceApp.Common;
using SkiServiceModels.DTOs.Requests;
using SkiServiceModels.DTOs.Responses;
using System.Text;

namespace SkiServiceApp.Tests.Util
{
    public class AuthHelper
    {
        public static void Login()
        {
            Task.Run(async () =>
            {
                var result = new HttpClient().PostAsync("http://localhost:9000/api/users/login", new StringContent(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = "SuperAdmin",
                    Password = "super",
                    RememberMe = true
                }), Encoding.UTF8, "application/json"));

                if (result.Result.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<LoginResponse>(await result.Result.Content.ReadAsStringAsync());
                    AuthManager.Login(response.Auth.Token, response.Auth.RefreshToken, response.Id);
                }

            }).GetAwaiter().GetResult();
        }
    }
}
