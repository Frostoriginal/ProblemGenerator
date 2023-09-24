using Microsoft.EntityFrameworkCore;

namespace ProblemGenerator.Data;

public class ProblemContext : DbContext
{
    public ProblemContext(DbContextOptions<ProblemContext> options) : base(options)
    {
    }

    public DbSet<Problem> Problems { get; set; }
}