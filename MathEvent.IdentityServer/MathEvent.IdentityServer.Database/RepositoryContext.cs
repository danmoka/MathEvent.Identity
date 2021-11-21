using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MathEvent.IdentityServer.Database
{
    public class RepositoryContext : IdentityDbContext<MathEventIdentityUser, MathEventIdentityRole, Guid>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<MathEventIdentityUser> MathEventIdentityUsers { get; set; }
    }
}
