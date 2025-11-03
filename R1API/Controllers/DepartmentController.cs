using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R1API.DTOS;
using R1API.Models;

namespace R1API.Controllers
{
    //api/Department
    [Route("api/[controller]")]
    [ApiController]//controller api (binding - validation)
    public class DepartmentController : ControllerBase
    {
        private readonly ITIContext ctx;

        public DepartmentController(ITIContext ctx)
        {
            this.ctx = ctx;
        }
        //CRUD
        [Authorize]
        [HttpGet]//Verb GET : /api/Department
        public IActionResult ShowAllDepartment()
        {
            //ctx.Set<T>().Find(1);
            //ctx.Set<T>().FirstOrDefault(e=>e.id)
            
            List<Department> depts = ctx.Departments.ToList();
            return Ok(depts);//
        }
        
        
        [HttpGet("{id:int}")]//MEtho: GET /api/Departent/10
        public ActionResult<GeneralResponse> GetByID(int id)
        {
            DeptWithEmpNamesListDTO? deptdto = ctx.Departments
                .Include(d=>d.Employees).Where(d=>d.Id==id)
                .Select(d=>new DeptWithEmpNamesListDTO()
                {
                    DeptId=d.Id,
                    DeptName=d.Name,
                    EmpsName=d.Employees.Select(e=>e.Name).ToList(),
                })
                .FirstOrDefault();

            GeneralResponse response = new GeneralResponse();
            if (deptdto == null)
            {
                
                response.IsSuccess = false;
                response.Data = "Invalid Idd  not found";
            }
            else
            {
                response.IsSuccess = true;
                response.Data = deptdto;
            }
            return response;
        }







        [HttpGet("{name:alpha}")]//MEtho: GET /api/Departent/SD
        public IActionResult GetByName(string name)
        {
            Department? dept = ctx.Departments.FirstOrDefault(d => d.Name.Contains(name));
            return Ok(dept);
        }



        [HttpPost]//Verb POST : /api/Department
        public IActionResult New(Department deptFromReq)
        {
            if (ModelState.IsValid)
            {
                ctx.Departments.Add(deptFromReq);
                ctx.SaveChanges();
                //return Created($"http://localhost:10441/api/department/{deptFromReq.Id}",deptFromReq);
                return CreatedAtAction("GetByID", new { id = deptFromReq.Id }, deptFromReq);
            }
            GeneralResponse respo = new GeneralResponse();

            respo.IsSuccess = false;
            respo.Data = ModelState;
            return Ok(respo);
        }
        [HttpPut("{id:int}")]//PUT /api/Department/id
        public IActionResult Edit(int id,Department deptFromRe)
        {
            if (ModelState.IsValid)
            {
                Department deptFromDB = ctx.Departments.FirstOrDefault(d => d.Id == id);
                deptFromDB.Name = deptFromRe.Name;
                deptFromDB.ManagerName = deptFromRe.ManagerName;
                ctx.SaveChanges();
                return NoContent();
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id:int}")]
        public IActionResult remove(int id)
        {
            Department dept = ctx.Departments.FirstOrDefault(d => d.Id == id);
            ctx.Departments.Remove(dept);
            ctx.SaveChanges();
            return NoContent();
        }
    }
}
