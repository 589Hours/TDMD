using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using HueApp.Infrastructure.PhilipsHueApi;
using Moq;

namespace HueApp.DomainTests
{
    [TestClass]
    public sealed class AppLightTest
    {

        [TestMethod]
        public async void AppLight_ShouldBeCorrectlySavedInLightClass_WhenReceievedFromApi()
        {
            var httpMock = new Mock<IPhilipsHueApiClient>();
            var testLight = new Light() { name = "Test", state = new State()
            {
                on = false,
                hue = 5000,
                sat = 254,
                bri = 254
            } };

            var testResult = new Dictionary<string, Light>()
            {
                { "1", testLight }
            };
            httpMock.Setup(mock => mock.GetLightsAsync(It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await httpMock.Object.GetLightsAsync("");
            
            Assert.AreEqual(testLight, result["1"]);
        }

        [TestMethod]
        public async void AppLight_ShouldTurnOnSuccesfully_WhenPutCommandIsSet()
        {
            var httpMock = new Mock<IPhilipsHueApiClient>();
            var testLight = new Light()
            {
                name = "Test",
                state = new State()
                {
                    on = false,
                    hue = 5000,
                    sat = 254,
                    bri = 254
                }
            };

            
        }
    }
}
