using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumeJsonLocal.Models;
using ConsumeJsonLocal.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        /// <summary>
        /// Get a List of Employees
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Get api/Employee
        /// </remarks>
        /// <returns>Employee List</returns>
        /// <response code="200">Returns Employee List</response>
        /// <response code="404">Employee List not found </response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        // GET: api/Employee/00001
        /// <summary>
        /// Get an Employee by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Get api/Employee/
        /// </remarks>
        /// <returns>Employee by Id</returns>
        /// <response code="200">Returns Employee by Id</response>
        /// <response code="404">Employee by Id not found </response>
        [HttpGet("{employeeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> GetEmployeeById([FromRoute] string employeeId)
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

        // POST: api/Employee
        /// <summary>
        /// Create an Employee
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Insert api/Employee
        /// </remarks>
        /// <returns>Created Employee</returns>
        /// <response code="200">Returns when Employee was correctly created</response>
        /// <response code="304">Returned when Employee was not created </response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(304)]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Post([FromBody] EmployeeDTO employeeRecord)
        {
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            getEmployeeList.Add(employeeRecord);

            var result = await _dataHandler.AddJsonRecordInFile("Employees.json", getEmployeeList, employeeRecord.EmployeeId);

            if (result != string.Empty)
            {
                var response = new ResponseModel<string> { Result = result };
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status304NotModified, new ResponseModel<string> { Message = "Employee not created" });
        }

        // PUT: api/Employee/{employeeId}
        /// <summary>
        /// Updates an Employee
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     Update api/Employee/00001
        /// </remarks>
        /// <returns>Updated Employee</returns>
        /// <response code="200">Returns when Employee was correctly updated</response>
        /// <response code="304">Returned when Employee was not updated </response>
        /// <response code="404">Returned when Employee was not found </response>
        [HttpPut("{employeeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]        
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Put([FromBody] EmployeeDTO employeeRecord, [FromRoute] string employeeId)
        {
            //get actual list
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            //remove element of actual list
            var getEmployeeToUpdate = getEmployeeList.RemoveAll((x) => x.EmployeeId == employeeId);
            
            if(getEmployeeToUpdate > 0)
            {
                //add new element to the list
                getEmployeeList.Add(employeeRecord);

                var result = await _dataHandler.UpdateJsonRecordInFile("Employees.json", getEmployeeList, employeeRecord);

                if (result != null)
                {
                    var response = new ResponseModel<EmployeeDTO> { Result = result };
                    return Ok(response);
                }

                return StatusCode(StatusCodes.Status304NotModified, new ResponseModel<string> { Message = "Employee not updated" });
            }

            return StatusCode(StatusCodes.Status404NotFound, new ResponseModel<string> { Message = "Employee not found" });

        }

        // DELETE: api/Employee/{employeeId}
        /// <summary>
        /// Deletes an Employee
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE api/Employee/00001
        /// </remarks>
        /// <returns>Confirmation of deletion</returns>
        /// <response code="200">Returns when Employee was correctly deleted</response>
        /// <response code="304">Returned when Employee was not modified </response>
        [HttpDelete("{employeeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(304)]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Delete([FromRoute] string employeeId)
        {
            //get actual list
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            //remove element of the list
            var getEmployeeToDelete = getEmployeeList.Where(x => x.EmployeeId == employeeId).FirstOrDefault();

            if (getEmployeeToDelete != null)
            {
                getEmployeeList.Remove(getEmployeeToDelete);
            }

            var result = await _dataHandler.DeleteJsonRecordInFile("Employees.json", getEmployeeList);

            if (result)
            {
                var response = new ResponseModel<bool> { Result = result };
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status304NotModified, new ResponseModel<string> { Message = "Employee not deleted" });
        }
    }
}
