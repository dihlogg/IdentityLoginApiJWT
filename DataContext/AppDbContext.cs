using IdentityWebApiSample.Server.AppSettings;
using IdentityWebApiSample.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace IdentityWebApiSample.Server.DataContext
{
    public class AppDbContext : IdentityDbContext<UserSystem>
    {
        private readonly PostgreSettings _postgreSetting;

        public AppDbContext(PostgreSettings postgreSetting, DbContextOptions<AppDbContext> options) : base(options)
        {
            _postgreSetting = postgreSetting;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_postgreSetting.ConnectionString ?? "");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        public virtual DbSet<LoginRequest> UserSystems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(iul => new { iul.LoginProvider, iul.ProviderKey });

            modelBuilder.Entity<LoginRequest>().HasNoKey();
        }

        public override int SaveChanges()
        {
            var dateNow = DateTime.UtcNow;
            var errorList = new List<ValidationResult>();

            var entries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified)
                .ToList();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    if (entity is BaseEntities itemBase)
                    {
                        itemBase.CreateDate = itemBase.UpdateDate = dateNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entity is BaseEntities itemBase)
                    {
                        itemBase.UpdateDate = dateNow;
                    }
                }

                Validator.TryValidateObject(entity, new ValidationContext(entity), errorList);
            }

            if (errorList.Count != 0)
            {
                throw new Exception(string.Join(", ", errorList.Select(p => p.ErrorMessage)).Trim());
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var dateNow = DateTime.UtcNow;
            var errorList = new List<ValidationResult>();

            var entries = ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified).ToList();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    if (entity is BaseEntities itemBase)
                    {
                        itemBase.CreateDate = itemBase.UpdateDate = dateNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entity is BaseEntities itemBase)
                    {
                        itemBase.UpdateDate = dateNow;
                    }
                }

                Validator.TryValidateObject(entity, new ValidationContext(entity), errorList);
            }

            if (errorList.Count != 0)
            {
                throw new Exception(string.Join(", ", errorList.Select(p => p.ErrorMessage)).Trim());
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
