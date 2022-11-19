using Beam.Extensions.AspNetCore.Sample.Data.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Beam.Extensions.AspNetCore.Sample.Data.Context;

public class SampleContext : IdentityDbContext<ApplicationUser, Role, Guid>
{

    public SampleContext(DbContextOptions<SampleContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    public new DbSet<ApplicationUser> Users { get; set; }

    public new DbSet<Role> Roles { get; set; }

}
