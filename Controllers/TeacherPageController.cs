using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cumilative3_CRUD.Models;
using System.Threading.Tasks;
using System;
using System.Linq;


namespace Cumilative3_CRUD.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext _db;
        public TeacherPageController(SchoolDbContext db) => _db = db;


        public async Task<IActionResult> Show(int id)
        {
            var t = await _db.Teachers.FindAsync(id);
            if (t == null) return NotFound();
            return View(t);
        }

        // --- Part 2: GET /TeacherPage/New ---
        [HttpGet]
        public IActionResult New()
        {
            return View(new Teacher());
        }

        // --- Part 2: POST /TeacherPage/New ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Teacher teacher)
        {
            if (!ModelState.IsValid)
                return View(teacher);

            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        // --- Part 2: GET /TeacherPage/DeleteConfirm/{id} ---
        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var t = await _db.Teachers.FindAsync(id);
            if (t == null) return NotFound();
            return View(t);
        }

        // --- Part 2: POST /TeacherPage/DeleteConfirm/{id} ---
        [HttpPost, ActionName("DeleteConfirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = await _db.Teachers.FindAsync(id);
            if (t == null) return NotFound();

            _db.Teachers.Remove(t);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        /// <summary>
        /// GET: /TeacherPage/Edit/{id}
        /// Shows a form to edit a teacher’s details.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _db.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        /// <summary>
        /// POST: /TeacherPage/Edit/{id}
        /// Processes the update of a teacher’s information.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher updatedTeacher)
        {
            // URL ID must match form ID
            if (id != updatedTeacher.TeacherId)
                return BadRequest();

            // Model validation
            if (!ModelState.IsValid)
                return View(updatedTeacher);

            var teacher = await _db.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            // Server‑side validations
            if (string.IsNullOrWhiteSpace(updatedTeacher.TeacherFName) ||
                string.IsNullOrWhiteSpace(updatedTeacher.TeacherLName))
            {
                ModelState.AddModelError("", "First and last name are required.");
                return View(updatedTeacher);
            }
            if (updatedTeacher.HireDate > DateTime.Today)
            {
                ModelState.AddModelError("", "Hire date cannot be in the future.");
                return View(updatedTeacher);
            }
            if (updatedTeacher.Salary < 0)
            {
                ModelState.AddModelError("", "Salary cannot be negative.");
                return View(updatedTeacher);
            }

            // Apply updates
            teacher.TeacherFName = updatedTeacher.TeacherFName;
            teacher.TeacherLName = updatedTeacher.TeacherLName;
            teacher.EmployeeNumber = updatedTeacher.EmployeeNumber;
            teacher.HireDate = updatedTeacher.HireDate;
            teacher.Salary = updatedTeacher.Salary;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        /// <summary>
        /// GET: /TeacherPage/List
        /// Displays all teachers, optionally filtered by hire‑date range.
        /// </summary>
        public async Task<IActionResult> List(DateTime? from, DateTime? to)
        {
            var q = _db.Teachers.AsQueryable();
            if (from.HasValue)
                q = q.Where(t => t.HireDate >= from.Value);
            if (to.HasValue)
                q = q.Where(t => t.HireDate <= to.Value);

            ViewData["From"] = from?.ToString("yyyy-MM-dd");
            ViewData["To"] = to?.ToString("yyyy-MM-dd");

            var teachers = await q.ToListAsync();
            return View(teachers);
        }
    }
}