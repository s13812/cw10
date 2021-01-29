using cw10.DTOs;
using cw10.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace cw10.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private readonly Cw10Context _dbContext;

        public EnrollmentsController(Cw10Context context)
        {
            _dbContext = context;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var study = _dbContext.Studies.FirstOrDefault(s => s.Name == request.Studies);

            if (study == null)
            {
                return NotFound("Nie ma takich studiow");
            }

            var enrollment = _dbContext.Enrollments.FirstOrDefault(e => e.Semester == 1 && e.IdStudy == study.IdStudy);

            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    IdEnrollment = _dbContext.Enrollments.Max(e => e.IdEnrollment) + 1,
                    Semester = 1,
                    IdStudy = study.IdStudy,
                    StartDate = DateTime.Now
                };
                _dbContext.Enrollments.Add(enrollment);
            }

            if (_dbContext.Students.Any(s => s.IndexNumber == request.IndexNumber))
            {
                return BadRequest("Index zajety");
            }

            var student = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IdEnrollment = enrollment.IdEnrollment
            };

            _dbContext.Students.Add(student);

            _dbContext.SaveChanges();

            return Ok(new
            {
                index = student.IndexNumber,
                study = study.Name,
                enrollment.IdEnrollment,
                enrollment.Semester,
                enrollment.StartDate
            });
        }
        
        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            var enrollment = _dbContext.Enrollments.FirstOrDefault(e => e.Semester == request.Semester && e.IdStudy == _dbContext.Studies.FirstOrDefault(s => s.Name == request.Studies).IdStudy);
            
            if (enrollment == null)
            {
                return NotFound("Nie ma takiego wpisu");
            }

            var newEnrollment = _dbContext.Enrollments.FirstOrDefault(e => e.IdStudy == enrollment.IdStudy && e.Semester == enrollment.Semester + 1);

            if (newEnrollment == null)
            {
                newEnrollment = new Enrollment
                {
                    IdEnrollment = _dbContext.Enrollments.Max(e => e.IdEnrollment) + 1,
                    Semester = enrollment.Semester + 1,
                    IdStudy = enrollment.IdStudy,
                    StartDate = DateTime.Now
                };
                _dbContext.Enrollments.Add(newEnrollment);
            }

            foreach (var stu in _dbContext.Students.Where(s => s.IdEnrollment == enrollment.IdEnrollment))
            {
                stu.IdEnrollment = newEnrollment.IdEnrollment;
            }

            _dbContext.SaveChanges();

            return Ok(new
            {
                newEnrollment.IdEnrollment,
                request.Studies,
                newEnrollment.Semester,
                newEnrollment.StartDate
            });
        }
    }
}
