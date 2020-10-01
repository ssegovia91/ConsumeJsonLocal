using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumeJsonLocal.Models
{
    public class EmployeeDTO
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CompanyPositionDTO CompanyPosition { get; set; }
    }
}
