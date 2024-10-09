using Api.User;
using Microsoft.EntityFrameworkCore;

namespace ms_learn_api_management.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
}