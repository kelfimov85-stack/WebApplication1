using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class AssignCourierDto
    {
        [Required] public int CourierId { get; set; }
    }
}
