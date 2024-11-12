using Employee_APP.Data;
using Employee_APP.Dtos;
using Employee_APP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDBContext _context;
        public EmployeeController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            if (_context.Employees == null)
                return NotFound();
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            if (!_context.Employees.Any())
                return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if(employee ==null)
                return NotFound();
            return employee;
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDto employee)
        {
            var employeeModel = new Employee();
            employeeModel.Name = employee.Name;
            employeeModel.Age = employee.Age;
            employeeModel.Active = employee.Active;
              
            _context.Employees.Add(employeeModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployeeById), new {id = employeeModel.Id}, employeeModel);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id, [FromBody]EmployeeDto updateEmployee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            employee.Name = updateEmployee.Name;
            employee.Age = updateEmployee.Age;
            employee.Active = updateEmployee.Active;

            _context.Employees.Entry(employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(employee);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (!_context.Employees.Any())
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);

            if(employee == null) return NotFound(); 

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok(employee);
        }
    }
    
}
