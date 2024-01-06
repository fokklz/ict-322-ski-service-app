using SkiServiceApp.Common;
using SkiServiceApp.Interfaces;
using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services;
using SkiServiceApp.Services.API;
using SkiServiceApp.Tests.Util.Helper;
using SkiServiceApp.ViewModels;
using SkiServiceModels.DTOs.Responses;

namespace SkiServiceApp.Tests.ViewModels
{
    [Collection("Docker Collection")]
    public class AppLoginViewModelTests : IDisposable
    {
        private Environment _environment;

        private UserResponse _sampleUser;
        private string _sampleUserPassword = "super";

        public AppLoginViewModelTests() {
            _environment = new Environment();

            _environment.AddService<IUserAPIService>(new UserAPIService(_environment.ConfigurationMock.Object));
            _environment.AddService<IAuthService>(new AuthService(ServiceLocator.GetService<IUserAPIService>(), ServiceLocator.GetService<IStorageService>()));

            GetSample().GetAwaiter().GetResult();
        }

        private async Task GetSample()
        {
            var userApi = ServiceLocator.GetService<IUserAPIService>();
            var res = await userApi.GetAsync(1);

            if (res.IsSuccess)
            {
                var user = await res.ParseSuccess();
                _sampleUser = user!;
            }
        }

        private AppLoginViewModel CreateViewModel()
        {
            return new AppLoginViewModel();
        }

        [Fact]
        public async Task LoginModifiesAuthManager()
        {
            var model = CreateViewModel();
            model.Username = _sampleUser.Username;
            model.Password = _sampleUserPassword;

            await model.Login();

            Assert.True(AuthManager.IsLoggedIn);
            Assert.Equal(_sampleUser.Id, AuthManager.UserId);
        }

        [Fact]
        public async Task LoginFailsWithWrongPassword()
        {
            var model = CreateViewModel();
            model.Username = _sampleUser.Username;
            model.Password = "wrong";

            await model.Login();

            Assert.False(AuthManager.IsLoggedIn);
            Assert.Null(AuthManager.UserId);
        }

        [Fact]
        public async Task LoginFailsWithWrongUsername()
        {
            var model = CreateViewModel();
            model.Username = "wrong";
            model.Password = _sampleUserPassword;

            await model.Login();

            Assert.False(AuthManager.IsLoggedIn);
            Assert.Null(AuthManager.UserId);
        }

        public void Dispose()
        {
            _environment.Dispose();
        }
    }
}
