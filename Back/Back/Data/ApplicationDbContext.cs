using Back.Models;

using Microsoft.EntityFrameworkCore;

namespace Back.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<StudentSubject> StudentSubjects => Set<StudentSubject>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentSubject>()
                .HasKey(ss => new { ss.StudentId, ss.SubjectId });

            // Sembrado inicial (Data Seeding): 5 profesores y 10 materias
            var teacherIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToArray();

            // IDs fijos para sembrar Profesores
            var teacher1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var teacher2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var teacher3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var teacher4Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var teacher5Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = teacher1Id, Name = "Profesor Carlos Pérez" },
                new Teacher { Id = teacher2Id, Name = "Profesora Ana Gómez" },
                new Teacher { Id = teacher3Id, Name = "Profesor Luis Martínez" },
                new Teacher { Id = teacher4Id, Name = "Profesora Marta Rodríguez" },
                new Teacher { Id = teacher5Id, Name = "Profesor Jorge Hernández" }
            );

            modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = Guid.Parse("01000000-0000-0000-0000-000000000001"), Name = "Matemáticas I", Credits = 3, TeacherId = teacher1Id },
            new Subject { Id = Guid.Parse("01000000-0000-0000-0000-000000000002"), Name = "Álgebra Lineal", Credits = 3, TeacherId = teacher1Id },

            new Subject { Id = Guid.Parse("02000000-0000-0000-0000-000000000001"), Name = "Física General", Credits = 3, TeacherId = teacher2Id },
            new Subject { Id = Guid.Parse("02000000-0000-0000-0000-000000000002"), Name = "Mecánica", Credits = 3, TeacherId = teacher2Id },

            new Subject { Id = Guid.Parse("03000000-0000-0000-0000-000000000001"), Name = "Programación I", Credits = 3, TeacherId = teacher3Id },
            new Subject { Id = Guid.Parse("03000000-0000-0000-0000-000000000002"), Name = "Bases de Datos", Credits = 3, TeacherId = teacher3Id },

            new Subject { Id = Guid.Parse("04000000-0000-0000-0000-000000000001"), Name = "Estructura de Datos", Credits = 3, TeacherId = teacher4Id },
            new Subject { Id = Guid.Parse("04000000-0000-0000-0000-000000000002"), Name = "Redes de Computadores", Credits = 3, TeacherId = teacher4Id },

            new Subject { Id = Guid.Parse("05000000-0000-0000-0000-000000000001"), Name = "Sistemas Operativos", Credits = 3, TeacherId = teacher5Id },
            new Subject { Id = Guid.Parse("05000000-0000-0000-0000-000000000002"), Name = "Ingeniería de Software", Credits = 3, TeacherId = teacher5Id }
        );
        }
    }
}
