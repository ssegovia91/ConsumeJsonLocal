using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumeJsonLocal.Models;
using ConsumeJsonLocal.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ConsumeJsonLocal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        readonly DataHandler _dataHandler;
        public EmployeeController(DataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<EmployeeDTO>>>> GetAllEmployees()
        {
            var employee = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");

            if(employee != null)
            {
                var response = new ResponseModel<List<EmployeeDTO>> { Result = employee };
                return Ok(response);
            }

            return NotFound(new ResponseModel<EmployeeDTO> { Message = "Non employees were found" });

        }

        //GET: api/Employee/5
        [HttpGet]
        [Route("{employeeId}")]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> GetEmployeeById(string employeeId)
        {

            var employeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            var singleEmployee = employeeList.Find(x => x.EmployeeId == employeeId);

            if (singleEmployee != null)
            {
                var response = new ResponseModel<EmployeeDTO> { Result = singleEmployee };
                return Ok(response);
            }

            return NotFound(new ResponseModel<EmployeeDTO> { Message = "Employee not found" });
        }
    }
}
