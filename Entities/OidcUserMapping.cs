using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Entities;

public class OidcUserMapping
{
    // holds the user Guid value (user's id on application DB side)
    [Key]
    public Guid UserId { get; set; } 

    // holds the sub claim (user's id on OIDC provider side)
    [Required]
    public string SubClaim { get; set; } 
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}