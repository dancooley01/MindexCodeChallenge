
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var employeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            var effectiveDate = new DateTime(2024, 1, 1);
            decimal salary = 5.5M;

            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;
            var employee = response.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                Employee = employee,
                EffectiveDate = effectiveDate,
                Salary = salary
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
              new StringContent(requestContent, Encoding.UTF8, "application/json"));
            response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.IsNotNull(newCompensation.Employee.EmployeeId);
            Assert.AreEqual(newCompensation.Employee.EmployeeId, employee.EmployeeId);
            Assert.AreEqual(newCompensation.EffectiveDate, effectiveDate);
            Assert.AreEqual(newCompensation.Salary, salary);
        }

        [TestMethod]
        public void GetCompensationRecordsByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeIdJohn = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var employeeIdGeorge = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            var effectiveDate1 = new DateTime(2024, 1, 1);
            var effectiveDate2 = new DateTime(2024, 2, 1);
            var salary1 = 5.5M;
            var salary2 = 5.6M;
            CreateCompensation(employeeIdJohn, effectiveDate1, salary1);
            CreateCompensation(employeeIdJohn, effectiveDate2, salary2);
            CreateCompensation(employeeIdGeorge, effectiveDate2, salary2);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeIdJohn}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensationRecords = response.DeserializeContent<Compensation[]>();
            Assert.IsTrue(compensationRecords.All(c => c.Employee.EmployeeId == employeeIdJohn));
            Assert.AreEqual(compensationRecords.Length, 2);
            Assert.IsTrue(compensationRecords.Count(c =>
                c.Employee.EmployeeId == employeeIdJohn
                && c.EffectiveDate == effectiveDate1
                && c.Salary == salary1) == 1);

            Assert.IsTrue(compensationRecords.Count(c =>
                c.Employee.EmployeeId == employeeIdJohn
                && c.EffectiveDate == effectiveDate2
                && c.Salary == salary2) == 1);
        }

        #region Helper Methods

        private void CreateCompensation(String employeeId, DateTime effectiveDate, decimal salary)
        {
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;
            var employee = response.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                Employee = employee,
                EffectiveDate = effectiveDate,
                Salary = salary
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
              new StringContent(requestContent, Encoding.UTF8, "application/json"));
            response = postRequestTask.Result;
        }

        #endregion
    }
}
