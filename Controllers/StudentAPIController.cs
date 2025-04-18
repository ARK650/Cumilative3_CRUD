using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cumilative3_CRUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cumilative3_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchoolDbContext _db;
        public StudentAPIController(SchoolDbContext db) => _db = db;

        /// <summary>
        /// GET: api/StudentAPI
        /// Returns all students.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
            => Ok(await _db.Students.ToListAsync());

        /// <summary>
        /// GET: api/StudentAPI/{id}
        /// Returns a single student by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var s = await _db.Students.FindAsync(id);
            if (s == null) return NotFound(new { message = "Student not found." });
            return Ok(s);
        }
    }
}
