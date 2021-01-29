using System;

namespace cw10.DTOs
{
    public class UpdateStudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int? IdEnrollment { get; set; }
    }
}
