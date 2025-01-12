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

            // Act
            double actualAverageSpeed = route.GetAverageSpeed();

            // Assert
            Assert.AreEqual(expectedAverageSpeed, actualAverageSpeed);
        }

        [TestMethod]
        // unhappy flow
        public void RouteAverageSpeed_IsCorrectlyHandled_WhenGivenAZeroToDivideBy()
        {
            // Arrange
            Route route = testRoute;
            route.AmountOfRoutePoints = 0;

            double expectedAverageSpeed = 0;

            // Act
            double actualAverageSpeed = route.GetAverageSpeed();

            // Assert
            Assert.AreEqual(expectedAverageSpeed, actualAverageSpeed);
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

            // Act
            RouteEntity actualRouteEntity = route.ConvertToRouteEntity();

            // Assert
            Assert.AreEqual(expectedRouteEntity.RouteDateTime, actualRouteEntity.RouteDateTime);
            Assert.AreEqual(expectedRouteEntity.RouteName, actualRouteEntity.RouteName);
            Assert.AreEqual(expectedRouteEntity.AverageSpeed, actualRouteEntity.AverageSpeed);
            Assert.AreEqual(expectedRouteEntity.TotalDistance, actualRouteEntity.TotalDistance);
            Assert.AreEqual(expectedRouteEntity.RouteDuration, actualRouteEntity.RouteDuration);
        }
        #endregion
    }
}
