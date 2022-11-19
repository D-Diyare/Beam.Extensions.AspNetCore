using Microsoft.AspNetCore.Identity;

namespace Beam.Extensions.AspNetCore.Sample.Data.Entities;

/// <summary>
/// Extending identity user
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }
}
