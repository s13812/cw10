using System.ComponentModel.DataAnnotations;

namespace cw10.DTOs
{
    public class PromoteStudentsRequest
    {
        [Required]
        public string Studies { get; set; }

        [Required]
        public int Semester { get; set; }
    }
}
