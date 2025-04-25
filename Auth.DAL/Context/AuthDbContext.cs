
using Auth.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Auth.DAL.Context
{
    public class AuthDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserToken> UserTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
