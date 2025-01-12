using Routey.Domain.Models;
using Routey.Domain.SQLiteDatabases.Entities;

namespace Routey.Domain.Tests
{
    [TestClass]
    public sealed class ModelTests
    {
        private Route testRoute;

        [TestInitialize]
        public void Setup()
        {
            // Setup items for the unit tests
            testRoute = new Route("Test Route", new DateTime(2010, 11, 15, 10, 0,0));
            testRoute.TotalDistance = 100;
            testRoute.TotalDuration = "01:00:00";
            testRoute.SumOfSpeeds = 100;
            testRoute.AmountOfRoutePoints = 10;
        }

        #region AverageSpeedTests
        [TestMethod]
        // happy flow
        public void RouteAverageSpeed_IsCorrectlyCalcated_WhenGivenACorrectDivision()
        {
            // Arrange
            Route route = testRoute;
            double expectedAverageSpeed = 10;
            Console.WriteLine($"Expected result: {expectedAverageSpeed}"); // Write the expected result

            // Act
            double actualAverageSpeed = route.GetAverageSpeed();
            Console.WriteLine($"Actual result: {actualAverageSpeed}"); // Write the actual result

            // Assert
            Console.WriteLine($"Asserting result: {expectedAverageSpeed}, {actualAverageSpeed}"); // Write the actual result
            Assert.AreEqual(expectedAverageSpeed, actualAverageSpeed); // Compare the expected result with the actual result
        }

        [TestMethod]
        // unhappy flow: Dividing by zero
        public void RouteAverageSpeed_IsCorrectlyHandled_WhenGivenAZeroToDivideBy()
        {
            // Arrange
            Route route = testRoute;
            route.AmountOfRoutePoints = 0;

            double expectedAverageSpeed = 0;
            Console.WriteLine($"Expected result: {expectedAverageSpeed}"); // Write the expected result

            // Act
            double actualAverageSpeed = route.GetAverageSpeed();
            Console.WriteLine($"Actual result: {actualAverageSpeed}"); // Write the actual result

            // Assert
            Console.WriteLine($"Asserting result: {expectedAverageSpeed}, {actualAverageSpeed}"); // Write the actual result
            Assert.AreEqual(expectedAverageSpeed, actualAverageSpeed); // Compare the expected result with the actual result
        }

        [TestMethod]
        // unhappy flow: Sumspeeds is null
        public void RouteAverageSpeed_IsCorrectlyHandled_WhenSumOfSpeedsIsNull()
        {
            // Arrange
            Route route = testRoute;
            route.SumOfSpeeds = null;
            double expectedAverageSpeed = 0;
            Console.WriteLine($"Expected result: {expectedAverageSpeed}"); // Write the expected result

            // Act
            double actualAverageSpeed = route.GetAverageSpeed();
            Console.WriteLine($"Actual result: {actualAverageSpeed}"); // Write the actual result

            // Assert
            Console.WriteLine($"Asserting result: {expectedAverageSpeed}, {actualAverageSpeed}"); // Write the actual result
            Assert.AreEqual(expectedAverageSpeed, actualAverageSpeed); // Compare the expected result with the actual result
        }
        #endregion

        #region ConvertToRouteEntityTests

        [TestMethod]
        public void RouteEntity_IsCorrectlyCreated_WhenGivenAValidRoute()
        {
            // Arrange
            Route route = testRoute;
            RouteEntity expectedRouteEntity = new RouteEntity
            {
                RouteDateTime = new DateTime(2010, 11, 15, 10, 0, 0),
                RouteName = "Test Route",
                AverageSpeed = 10,
                TotalDistance = 100,
                RouteDuration = "01:00:00"
            };
            Console.WriteLine($"Expected result: {expectedRouteEntity.RouteName} + " +
                $" {expectedRouteEntity.RouteDateTime} + {expectedRouteEntity.AverageSpeed} + " +
                $"{expectedRouteEntity.TotalDistance} + {expectedRouteEntity.RouteDuration}"); // Write the expected result

            // Act
            RouteEntity actualRouteEntity = route.ConvertToRouteEntity();
            Console.WriteLine($"Actual result: {actualRouteEntity.RouteName} + " +
                $" {actualRouteEntity.RouteDateTime} + {actualRouteEntity.AverageSpeed} + " +
                $"{actualRouteEntity.TotalDistance} + {actualRouteEntity.RouteDuration}"); // Write the expected result

            // Assert
            Console.WriteLine($"Asserting result of objects: {expectedRouteEntity.RouteName}, {actualRouteEntity.RouteName}"); // Write the actual result
            Assert.AreEqual(expectedRouteEntity.RouteDateTime, actualRouteEntity.RouteDateTime); // Compare the expected result with the actual result
            Assert.AreEqual(expectedRouteEntity.RouteName, actualRouteEntity.RouteName); // Compare the expected result with the actual result
            Assert.AreEqual(expectedRouteEntity.AverageSpeed, actualRouteEntity.AverageSpeed); // Compare the expected result with the actual result
            Assert.AreEqual(expectedRouteEntity.TotalDistance, actualRouteEntity.TotalDistance); // Compare the expected result with the actual result
            Assert.AreEqual(expectedRouteEntity.RouteDuration, actualRouteEntity.RouteDuration); // Compare the expected result with the actual result
        } 
        #endregion
    }
}