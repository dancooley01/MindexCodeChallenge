using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);

        //This will load the direct reports of the employee when it gets it
        Employee GetByIdWithDirectReports(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Employee GetManager(String id);
        Task SaveAsync();
    }
}