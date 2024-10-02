using Api.Quote;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
    var quoteId = Random.Shared.Next(1,10);
    var selectedQuote = quotes.FirstOrDefault(x=>x.Id == quoteId);
    var newQuote = $"Hi, {request.Name}. {selectedQuote?.Description}";
    return newQuote;
})
.WithName("GetQuoteOfTheDay")
.WithOpenApi();

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
}
public record QuoteRequest(string Name);