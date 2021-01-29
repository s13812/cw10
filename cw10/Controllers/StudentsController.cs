using cw10.DTOs;
using cw10.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace cw10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Cw10Context _dbContext;

        public StudentsController(Cw10Context context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var res = _dbContext.Students.ToList();
            return Ok(res);
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(string index)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.IndexNumber == index);

            if (student != null)
            {
                return Ok(student);
            }
            return NotFound("Nie ma takiego studenta");
        }

        [HttpPut("{index}")]
        public IActionResult UpdateStudent(string index, UpdateStudentRequest request)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.IndexNumber == index);

            if (student == null)
            {
                return NotFound("Nie ma takiego studenta");
            }

            if (request.FirstName != null)
            {
                student.FirstName = request.FirstName;
            }
            if (request.LastName != null)
            {
                student.LastName = request.LastName;
            }
            if (request.BirthDate != null && request.BirthDate.Year != 1)
            {
                student.BirthDate = request.BirthDate;
            }
            if (request.IdEnrollment != null)
            {
                student.IdEnrollment = (int)request.IdEnrollment;
            }

            _dbContext.SaveChanges();

            return Ok(student);
        }

        [HttpDelete("{index}")]
        public IActionResult RemoveStudent(string index)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.IndexNumber == index);

            if (student == null)
            {
                return NotFound("Nie ma takiego studenta");
            }

            _dbContext.Remove(student);

            _dbContext.SaveChanges();

            return Ok("Usuwanie ukonczone");
        }
    }
}
