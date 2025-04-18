using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cumilative3_CRUD.Models;
using System.Threading.Tasks;

namespace Cumilative3_CRUD.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly SchoolDbContext _db;
        public StudentPageController(SchoolDbContext db) => _db = db;

        /// <summary>
        /// GET: /StudentPage/List
        /// Displays a list of all students.
        /// </summary>
        public async Task<IActionResult> List()
            => View(await _db.Students.ToListAsync());

        /// <summary>
        /// GET: /StudentPage/Show/{id}
        /// Displays details for one student.
        /// </summary>
        public async Task<IActionResult> Show(int id)
        {
            var s = await _db.Students.FindAsync(id);
            if (s == null) return NotFound();
            return View(s);
        }
    }
}
