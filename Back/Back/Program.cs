using Back.Common.Behaviors;
using Back.Data;
using Back.Middlewares;
using Back.Models;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Back
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("StudentRegistrationDb"));

            // MediatR & FluentValidation
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200") // Puerto por defecto de Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseCors("AllowAngular");
            // Middleware Global de Excepciones
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Registro Estudiantes v1");
                    // Opcional: Establece Swagger UI en la raíz del sitio (http://localhost:<puerto>/)
                    // c.RoutePrefix = string.Empty; 
                });
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Garantiza que la base de datos sea creada con los datos del HasData
                context.Database.EnsureCreated();

                // Cargar un estudiante de prueba con materias si la DB está vacía
                if (!context.Students.Any())
                {
                    var studentSample = new Student
                    {
                        Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                        Name = "Juan Pérez",
                        Email = "juan.perez@email.com"
                    };

                    context.Students.Add(studentSample);

                    // Inscribirlo en 3 materias con profesores distintos (cumpliendo reglas de negocio)
                    context.StudentSubjects.AddRange(
                        new StudentSubject { StudentId = studentSample.Id, SubjectId = Guid.Parse("01000000-0000-0000-0000-000000000001") }, // Profesor 1
                        new StudentSubject { StudentId = studentSample.Id, SubjectId = Guid.Parse("02000000-0000-0000-0000-000000000001") }, // Profesor 2
                        new StudentSubject { StudentId = studentSample.Id, SubjectId = Guid.Parse("03000000-0000-0000-0000-000000000001") }  // Profesor 3
                    );

                    context.SaveChanges();
                }
            }
            app.Run();
        }
    }
}
