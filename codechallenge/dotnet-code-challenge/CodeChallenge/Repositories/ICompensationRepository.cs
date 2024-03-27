using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(string id);
        IEnumerable<Compensation> GetByEmployeeId(String employeeId);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}