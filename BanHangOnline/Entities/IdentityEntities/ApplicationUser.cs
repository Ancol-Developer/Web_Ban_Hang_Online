using Microsoft.AspNetCore.Identity;

namespace Entities.IdentityEntities;

public class ApplicationUser : IdentityUser<int>
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
}
