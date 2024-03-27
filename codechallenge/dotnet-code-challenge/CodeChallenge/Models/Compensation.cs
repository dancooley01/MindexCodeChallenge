using System;

namespace CodeChallenge.Models
{
    /// <summary>
    /// Class to represent an employee and a record of their salary
    /// </summary>
    public class Compensation
    {
        public String CompensationId { get; set; }

        public Employee Employee { get; set; }

        public decimal Salary { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}
