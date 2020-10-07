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

        //GET: api/Employee/5
        [HttpPost]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Post(EmployeeDTO employeeRecord)
        {
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            getEmployeeList.Add(employeeRecord);

            var result = await _dataHandler.AddJsonRecordInFile("Employees.json", getEmployeeList, employeeRecord.EmployeeId);

            if (result != string.Empty)
            {
                var response = new ResponseModel<string> { Result = result };
                return Ok(response);
            }

            return StatusCode(StatusCodes.Status304NotModified, new ResponseModel<string> { Message = "Employee not added" });
        }

        [HttpPut]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Put(EmployeeDTO employeeRecord)
        {
            //get actual list
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            //remove element of actual list
            var getEmployeeToUpdate = getEmployeeList.RemoveAll((x) => x.EmployeeId == employeeRecord.EmployeeId);
            
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

        [HttpDelete]
        public async Task<ActionResult<ResponseModel<EmployeeDTO>>> Delete(string id)
        {
            //get actual list
            var getEmployeeList = await _dataHandler.LoadJsonFile<EmployeeDTO>("Employees.json");
            //remove element of the list
            var getEmployeeToDelete = getEmployeeList.Where(x => x.EmployeeId == id).FirstOrDefault();

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
