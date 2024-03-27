
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [DataTestMethod]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f", "John", "Lennon", 4)]
        [DataRow("c0c2293d-16bd-4603-8e08-638a9d18b22c", "George", "Harrison", 0)]
        public void GetReportingStructureById_Returns_Ok(String employeeId, String expectedFirstName, String expectedLastName, int expectedNumberOfReports)
        {
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructure_Returns_NotFound()
        {
            // Arrange
            var employeeId = "INVALID";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
