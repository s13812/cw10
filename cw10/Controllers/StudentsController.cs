using cw10.DTOs;
using cw10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Cw10Context _context;

        public StudentsController(Cw10Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var res = _context.Students.ToList();
            return Ok(res);
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(string index)
        {
            var student = _context.Students.FirstOrDefault(s => s.IndexNumber == index);

            if (student != null)
            {
                return Ok(student);
            }
            return NotFound("Nie ma takiego studenta");
        }

        [HttpPut("{index}")]
        public IActionResult UpdateStudent(string index, UpdateStudentRequest request)
        {
            var student = _context.Students.FirstOrDefault(s => s.IndexNumber == index);

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
            if (request.BirthDate != null)
            {
                student.BirthDate = request.BirthDate;
            }
            if (request.IdEnrollment != null)
            {
                student.IdEnrollment = (int)request.IdEnrollment;
            }

            _context.SaveChanges();

            return Ok(student);
        }

        [HttpDelete("{index}")]
        public IActionResult RemoveStudent(string index)
        {
            var student = _context.Students.FirstOrDefault(s => s.IndexNumber == index);

            if (student == null)
            {
                return NotFound("Nie ma takiego studenta");
            }

            _context.Remove(student);

            _context.SaveChanges();

            return Ok("Usuwanie ukonczone");
        }
    }
}
