using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Users = base.Set<User>();
            Roles = base.Set<Role>();
            Permissions = base.Set<Permission>();
            UserRoles = base.Set<UserRole>();
            RolePermissions = base.Set<RolePermission>();
            Projects = base.Set<Project>();
            ProjectUsers = base.Set<ProjectUser>();
            ProjectUserRoles = base.Set<ProjectUserRole>();
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Users = base.Set<User>();
            Roles = base.Set<Role>();
            Permissions = base.Set<Permission>();
            UserRoles = base.Set<UserRole>();
            RolePermissions = base.Set<RolePermission>();
            Projects = base.Set<Project>();
            ProjectUsers = base.Set<ProjectUser>();
            ProjectUserRoles = base.Set<ProjectUserRole>();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectUserRole> ProjectUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectUserRoleConfiguration());
        }
    }
}