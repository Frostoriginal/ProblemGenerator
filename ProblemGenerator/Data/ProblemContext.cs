using Microsoft.EntityFrameworkCore;

namespace ProblemGenerator.Data;

public class ProblemContext : DbContext
{
    public ProblemContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Problem> Problems { get; set; }
}