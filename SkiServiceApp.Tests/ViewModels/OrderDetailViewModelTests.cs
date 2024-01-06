using SkiServiceApp.Interfaces.API;
using SkiServiceApp.Services.API;
using SkiServiceApp.ViewModels;

namespace SkiServiceApp.Tests.ViewModels
{
    [Collection("Docker Collection")]
    public class OrderDetailViewModelTests
    {
        private Environment _environment;

        public OrderDetailViewModelTests()
        {
            _environment = new Environment().UseAuth();

            _environment.AddService<IOrderAPIService>(new OrderAPIService(_environment.ConfigurationMock.Object));
        }

        private OrderDetailViewModel CreateViewModel()
        {
            return new OrderDetailViewModel();
        }


        // This class is very hard to test, because it uses a lot of dialogs which need a XMAL context to work.
        // We just assume most things working with dialogs are working, because they are tested while developing.
        // Not sure what the best solution for this would be.


        [Fact]
        public async Task LoadServiceDetails_LoadsEntry()
        {
            // Arrange
            var model = CreateViewModel();
            var expected = 1;

            // Act
            await model.LoadServiceDetails(expected);

            // Assert
            Assert.Equal(expected, model.Entry.Order.Id);
        }

        // all other functions not using dialogs are already tested inside the CustomListItemTests.cs
    }
}
