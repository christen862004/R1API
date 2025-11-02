using Microsoft.AspNetCore.Mvc;
using R1API.DTOS;
using R1API.Models;

namespace R1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ITIContext context) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public  IActionResult  GetEmp(int id)
        {
            EmpWithdeptDTO? emp= context.Employees
                .Where(e => e.Id== id)
                .Select(e=>new EmpWithdeptDTO()
                {
                    EmpId=e.Id,
                    EmpName=e.Name,
                    DeptName=e.Department.Name
                }).FirstOrDefault();

            return Ok(emp);
        }
    }
}
