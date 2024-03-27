using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var employee = _employeeRepository.GetByIdWithDirectReports(id);
                if (employee != null)
                {
                    var reportingStructure = new ReportingStructure()
                    {
                        Employee = employee,
                        NumberOfReports = GetReportCount(employee)
                    };
                    return reportingStructure;
                }
            }

            return null;
        }

        private int GetReportCount(Employee employee)
        {
            if (employee.DirectReports != null)
            {
                return employee.DirectReports.Count
                    + employee.DirectReports.Sum(dr => GetReportCount(_employeeRepository.GetByIdWithDirectReports(dr.EmployeeId)));
            }
            return 0;
        }
    }
}
