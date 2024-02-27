using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;
public class Role
{
    [Required]
    public int? Id { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$")]
    public string Title { get; set; }
}