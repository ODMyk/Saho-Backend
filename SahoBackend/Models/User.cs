using System.ComponentModel.DataAnnotations;

namespace SahoBackend.Models;

public class User
{
    [Required]
    [RegularExpression("^(?=.{1,50}$)[\\w\\s\\-_,.&:;()'\"\']+$")]
    public string Nickname { get; set; }
}