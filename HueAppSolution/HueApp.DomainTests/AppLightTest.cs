using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using HueApp.Infrastructure.PhilipsHueApi;
using Moq;

namespace HueApp.DomainTests
{
    [TestClass]
    public sealed class AppLightTest
    {
        private Mock<IPhilipsHueApiClient> mockClient;
        private State testState;
        private Light testLight;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize mock client, test light, and state
            mockClient = new Mock<IPhilipsHueApiClient>();
            testState = new State { on = true, bri = 255, hue = 50000, sat = 200 };
            testLight = new Light { name = "Test Light", state = testState };
        }

        [TestMethod]
        public async void AppLight_ShouldBeCorrectlySavedInLightClass_WhenReceievedFromApi()
        {
            var testResult = new Dictionary<string, Light>()
            {
                { "1", testLight }
            };
            mockClient.Setup(mock => mock.GetLightsAsync(It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await mockClient.Object.GetLightsAsync("");

            Assert.IsNotNull(result);
            Assert.AreEqual(testLight, result["1"]);
        }

        [TestMethod]
        public async void Api_ShouldReturnUsername_WhenLinkIsSuccesful()
        {
            mockClient.Setup(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("username");

            var usernameFromLink = await mockClient.Object.Link("Url", "peter", "windowsPC");

            //check result and verify method call
            Assert.AreEqual("username", usernameFromLink);
            mockClient.Verify(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
