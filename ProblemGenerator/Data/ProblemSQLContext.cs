using Microsoft.EntityFrameworkCore;

namespace ProblemGenerator.Data
{
    public class ProblemSQLContext:DbContext
    {
        public ProblemSQLContext(DbContextOptions<ProblemSQLContext> options):base(options)
        {
                
        }

        public DbSet<ProblemSQL> Problems { get; set; }
    }
}
