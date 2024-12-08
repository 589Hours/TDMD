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
            // Initialize Mock Client, Test light, and State
            mockClient = new Mock<IPhilipsHueApiClient>();
            testState = new State { on = true, bri = 255, hue = 50000, sat = 200 };
            testLight = new Light { name = "Test Light", state = testState };
        }

        // Happy path instrumented test
        [TestMethod] 
        public async Task AppLight_ShouldBeCorrectlySavedInLightClass_WhenReceievedFromApi()
        {
            Console.WriteLine("Started test: AppLight_ShouldBeCorrectlySavedInLightClass_WhenReceievedFromApi"); // Log Start of Test
            // Create Test Light to use for comparing with the result
            var testResult = new Dictionary<string, Light>()
            {
                { "1", testLight }
            };

            Console.WriteLine("-Expected Results-"); // Log Header of Expected Results
            for (int i = 1; i < testResult.Count + 1; i++)
            {
                Console.WriteLine($"Expected Light Name: {testResult[i.ToString()].name}"); // Log a Single Expected Result
            }
            
            // Setup Mock
            mockClient.Setup(mock => mock.GetLightsAsync(It.IsAny<string>())).ReturnsAsync(testResult);

            // Use Function
            var result = await mockClient.Object.GetLightsAsync("");

            Console.WriteLine("-Results-"); // Log Header of Results
            for (var i = 1; i < result.Count + 1; i++)
            {
                Console.WriteLine($"Name: {result[i.ToString()].name}"); // Log a Single Result
            }
            // Assert Result and compare to expected data
            Assert.IsNotNull(result);
            Assert.AreEqual(testLight, result["1"]);
            Console.WriteLine("Finished test: AppLight_ShouldBeCorrectlySavedInLightClass_WhenReceievedFromApi"); // Log End of Test
        }

        // Happy path instrumented test
        [TestMethod]
        public async Task Api_ShouldReturnUsername_WhenLinkIsSuccesful()
        {
            Console.WriteLine("Started test: Api_ShouldReturnUsername_WhenLinkIsSuccesful"); // Log Start of Test
            string testOutput = "username";

            // Setup Mock
            mockClient.Setup(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(testOutput);
            Console.WriteLine($"Expected Name From Link: {testOutput}"); // Log Expected Name

            // Use Function
            var usernameFromLink = await mockClient.Object.Link("Url", "peter", "windowsPC");

            // Assert Result and Verify Method Call
            Console.WriteLine($"Username From Link: {usernameFromLink}"); // Log Gotten Name
            Assert.AreEqual(testOutput, usernameFromLink);
            mockClient.Verify(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Console.WriteLine("Finished test: Api_ShouldReturnUsername_WhenLinkIsSuccesful"); // Log End of Test
        }

        // Unhappy path instrumented test
        [TestMethod]
        public async Task Api_ShouldNotReturnUsername_WhenBridgeIpIsWrong()
        {
            Console.WriteLine("Started test: Api_ShouldNotConnect_WhenBridgeIpIsWrong"); // Log Start of Test
            string testOutput = "bridge request error";

            // Setup Mock
            mockClient.Setup(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(testOutput);
            Console.WriteLine($"Expected Name From Link: bridge request error"); // Log Expected Name

            // Use Function
            var usernameFromLink = await mockClient.Object.Link("Wrong Url", "peter", "windowsPC");

            // Assert Result and Verify Method Call
            Console.WriteLine($"Username From Link: {usernameFromLink}"); // Log Gotten Name
            Assert.AreEqual(testOutput, usernameFromLink);
            mockClient.Verify(client => client.Link(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Console.WriteLine("Finished test: Api_ShouldNotConnect_WhenBridgeIpIsWrong"); // Log End of Test
        }
    }
}
