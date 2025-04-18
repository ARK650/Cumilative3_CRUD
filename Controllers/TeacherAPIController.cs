using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cumilative3_CRUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cumilative3_CRUD.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _db;
        public TeacherAPIController(SchoolDbContext db) => _db = db;

        // Part 1 READ...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetAll()
            => Ok(await _db.Teachers.ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> Get(int id)
        {
            var t = await _db.Teachers.FindAsync(id);
            if (t == null) return NotFound(new { message = "Teacher not found" });
            return Ok(t);
        }

        // --- Part 2: ADD teacher ---
        /// <summary>
        /// POST: api/TeacherAPI
        /// Adds a new teacher.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Teacher>> Post([FromBody] Teacher teacher)
        {
            // Basic server-side validations
            if (string.IsNullOrWhiteSpace(teacher.TeacherFName) ||
                string.IsNullOrWhiteSpace(teacher.TeacherLName))
            {
                return BadRequest(new { message = "First and last name are required." });
            }
            if (teacher.HireDate > System.DateTime.Today)
            {
                return BadRequest(new { message = "Hire date cannot be in the future." });
            }

            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync();

            // Returns 201 Created + the new object
            return CreatedAtAction(nameof(Get), new { id = teacher.TeacherId }, teacher);
        }

        // --- Part 2: DELETE teacher ---
        /// <summary>
        /// DELETE: api/TeacherAPI/{id}
        /// Deletes the teacher with the given ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.Teachers.FindAsync(id);
            if (t == null) return NotFound(new { message = "Teacher not found" });

            _db.Teachers.Remove(t);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// PUT: api/TeacherAPI/{id}
        /// Updates an existing teacher’s information.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Teacher updatedTeacher)
        {
            // ID in URL must match ID in payload
            if (id != updatedTeacher.TeacherId)
                return BadRequest(new { message = "Teacher ID mismatch." });

            // Find existing
            var teacher = await _db.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound(new { message = "Teacher not found." });

            // Server‑side validations
            if (string.IsNullOrWhiteSpace(updatedTeacher.TeacherFName) ||
                string.IsNullOrWhiteSpace(updatedTeacher.TeacherLName))
            {
                return BadRequest(new { message = "First and last name are required." });
            }
            if (updatedTeacher.HireDate > DateTime.Today)
            {
                return BadRequest(new { message = "Hire date cannot be in the future." });
            }
            if (updatedTeacher.Salary < 0)
            {
                return BadRequest(new { message = "Salary cannot be negative." });
            }

            // Apply updates
            teacher.TeacherFName = updatedTeacher.TeacherFName;
            teacher.TeacherLName = updatedTeacher.TeacherLName;
            teacher.EmployeeNumber = updatedTeacher.EmployeeNumber;
            teacher.HireDate = updatedTeacher.HireDate;
            teacher.Salary = updatedTeacher.Salary;

            await _db.SaveChangesAsync();
            return Ok(teacher);
        }
    }
}