using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;
public class Role
{
    [Required]
    public int? Id { get; set; }

    [Required]
    public string Title { get; set; }
}