using CodeChallenge.Models;
using System;
using System.Collections.Generic;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(string id);
        IEnumerable<Compensation> GetByEmployeeId(String employeeId);
        Compensation Create(Compensation compensation);
    }
}
