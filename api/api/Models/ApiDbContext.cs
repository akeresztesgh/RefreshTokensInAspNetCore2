using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApiDbContext()
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public void InsertNew(RefreshToken token)
        {
            var tokenModel = RefreshTokens.SingleOrDefault(i => i.UserId == token.UserId);
            if (tokenModel != null)
            {
                RefreshTokens.Remove(tokenModel);
                SaveChanges();
            }
            RefreshTokens.Add(token);
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.UserId)
                .HasName("refreshToken_UserId");
            modelBuilder.Entity<RefreshToken>()
                .HasAlternateKey(c => c.Token)
                .HasName("refreshToken_Token");

            base.OnModelCreating(modelBuilder);
        }
    }

}
