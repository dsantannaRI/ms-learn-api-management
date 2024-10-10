using Api.User;
using Microsoft.EntityFrameworkCore;

namespace ms_learn_api_management.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public void SeedUsers()
    {
        Random random = new Random();

        for (int i = 1; i <= 5; i++)
        {
            string name = "User" + random.Next(100, 1000);
            string email = $"user{i}@example.com";

            User user = new User
            {
                Id = i,
                FullName = name,
                Email = email
            };

            Users.Add(user);
        }

        SaveChanges(); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}