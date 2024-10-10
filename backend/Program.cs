using Api.Quote;
using Microsoft.AspNetCore.Mvc;
using ms_learn_api_management.Data;
using Microsoft.EntityFrameworkCore;
using Api.User;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "Policy",
                            policy =>
                            {
                                policy.AllowAnyOrigin();
                                policy.AllowAnyHeader();
                                policy.AllowAnyMethod();
                            });
        });
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.MapPost("/quote-of-the-day", ([FromBody] QuoteRequest request) =>
{
    var quotes = new Quote().QuotesList();
    var quoteId = Random.Shared.Next(1, 10);
    var selectedQuote = quotes.FirstOrDefault(x => x.Id == quoteId);
    var newQuote = $"Hi, {request.Name}. {selectedQuote?.Description}";
    return newQuote;
})
.WithName("GetQuoteOfTheDay")
.WithOpenApi();

app.MapPost("/create-user", async (
    [FromBody] CreateUserRequest request,
    IValidator<CreateUserRequest> validator,
    DataContext context
) =>
{
    var validationResult = validator.Validate(request);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        return Results.BadRequest(errors);
    }

    var newUser = new User
    {
        FullName = request.FullName,
        Email = request.Email
    };

    try
    {
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    catch (System.Exception)
    {
        return Results.BadRequest();
    }
});

app.MapGet("/get-users", async (
    DataContext context
) =>
{
    var users = await context.Users.ToListAsync();
    return Results.Ok(users);
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    
    if (!context.Users.Any())
    {
        context.SeedUsers();
    }
}

app.UseCors("Policy");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


namespace Api.Quote
{
    public class Quote
    {
        public string Description { get; set; } = string.Empty;
        public int Id { get; set; }
        public List<Quote> QuotesList()
        {
            return new List<Quote>()
            {
                new Quote { Id = 1, Description = "The best way to predict the future is to create it." },
                new Quote { Id = 2, Description = "Believe you can and you're halfway there." },
                new Quote { Id = 3, Description = "Do not wait for the perfect moment, take the moment and make it perfect." },
                new Quote { Id = 4, Description = "Success is not final, failure is not fatal: It is the courage to continue that counts." },
                new Quote { Id = 5, Description = "Your time is limited, don’t waste it living someone else’s life." },
                new Quote { Id = 6, Description = "In the end, we only regret the chances we didn’t take." },
                new Quote { Id = 7, Description = "It’s never too late to be what you might have been." },
                new Quote { Id = 8, Description = "The only way to do great work is to love what you do." },
                new Quote { Id = 9, Description = "The harder you work for something, the greater you’ll feel when you achieve it." },
                new Quote { Id = 10, Description = "Don’t watch the clock; do what it does. Keep going." }
            };
        }
    }
    public record QuoteRequest(string Name);
}

namespace Api.User
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public record CreateUserRequest(string FullName, string Email);
    public record UserDto(string FullName, string Email);

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FullName)
            .Length(3, 100).WithMessage("FullName must be between 3 and 100 characters")
            .NotNull().WithMessage("FullName can't be null");


            RuleFor(x => x.Email).EmailAddress().WithMessage("Please, provide a valid email");
        }
    }
}