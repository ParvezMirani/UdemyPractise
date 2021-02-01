using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DLL.Models;
using DLL.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        private const string _isDeletedProperty = "IsDeleted";
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(_isDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    entity.AddProperty(_isDeletedProperty, typeof(bool));
                    modelBuilder.Entity(entity.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }

            modelBuilder.Entity<CourseStudent>()
                .HasKey(bc => new { bc.CourseId, bc.StudentId });
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseStudents)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Student)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(bc => bc.StudentId);

            base.OnModelCreating(modelBuilder);
        }


        private void OnBeforeSavingData()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            foreach (var entry in entries)
            {
                if(entry.Entity is ITrackable trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = DateTimeOffset.Now;
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Deleted:
                            entry.Property(_isDeletedProperty).CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }                
            }
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSavingData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<CourseStudent> CourseStudent { get; set; }
    }
}
