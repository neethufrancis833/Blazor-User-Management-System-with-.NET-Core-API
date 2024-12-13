
using Microsoft.EntityFrameworkCore;
using PeopleWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS to allow requests from the Blazor app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("https://localhost:7078")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseInMemoryDatabase("UsersDb"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowLocalhost");
// Seed data if the database is empty
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    Console.WriteLine("Sg data...");
    // Check if the database already contains any users
    if (!dbContext.Users.Any())
    {
        Console.WriteLine("Seeding data...");

        // Seed initial data
        dbContext.Users.AddRange(
            new User { FirstName = "David", LastName = "J", Email = "david.j@gmail.com",Phone="2893456789" },
            new User { FirstName = "John", LastName = "D", Email = "john.d@gmail.com", Phone = "4195673456" },
            new User { FirstName = "Cassandra", LastName = "J", Email = "c.j@gmail.com", Phone = "3456789876" },
             new User { FirstName = "Arjun", LastName = "r", Email = "arjun.r@gmail.com", Phone = "2898956789" },
            new User { FirstName = "Peter", LastName = "D", Email = "peter.d@gmail.com", Phone = "4195673567" },
            new User { FirstName = "Paul", LastName = "J", Email = "paul.j@gmail.com", Phone = "4567898767" }
        );
        dbContext.SaveChanges(); // Save the changes to the database

        Console.WriteLine("Data seeded successfully.");

    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
