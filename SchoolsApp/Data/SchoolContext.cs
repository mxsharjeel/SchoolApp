using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace SchoolApp.Data
{
    internal class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }
        public string ConnectionString { get; set; }
        public SchoolContext()
        {
            ConnectionString = "Data Source=LENOVO-V14;Initial Catalog=SchoolsApp;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne<Course>(c => c.Course)
                .WithMany(sc => sc.StudentCourse)
                .HasForeignKey(c => c.CourseId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne<Student>(s => s.Student)
                .WithMany(sc => sc.StudentCourse)
                .HasForeignKey(s => s.StudentId);


        }
    }

}
