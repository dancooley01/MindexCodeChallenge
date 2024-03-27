using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    //I decided to create a new repo for this since it is not getting only employee data.
    //If direct reports were being persisted for the other task, I probably would have created a repo for that also
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation GetById(string id)
        {
            return _employeeContext.CompensationRecords.SingleOrDefault(c => c.CompensationId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.CompensationRecords.Add(compensation);
            //don't save this employee, it's not changing, and it's not new
            _employeeContext.Attach(compensation.Employee);
            return compensation;
        }

        public IEnumerable<Compensation> GetByEmployeeId(string employeeId)
        {
            return _employeeContext.CompensationRecords.Include(c => c.Employee).Where(e => e.Employee.EmployeeId == employeeId);
        }
    }
}
