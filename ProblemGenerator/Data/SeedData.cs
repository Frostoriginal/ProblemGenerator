namespace ProblemGenerator.Data;

public static class SeedData
{
    public static void Initialize(ProblemContext db)
    {
        var problems = new Problem[]
        {
             new Problem()
            {
                Id = 1,
                What = "Create the database",
                Where = "On the server",
                DetailedDescription = "Create database with basic sample data",
                DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                DateSolved = DateTime.Now,
                IsSolved = false,
                IsChecked= false,
            },

             new Problem()
            {
                Id = 2,
                What = "Test the functionality",
                Where = "In the App",
                DetailedDescription = "Detailed desc",
                DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                DateSolved = DateTime.Now,
                IsSolved = false,
                IsChecked= false,
            },
              new Problem()
            {
                Id = 3,
                What = "Solved problem",
                Where = "yeah?",
                DetailedDescription = "Detailed desc",
                DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                DateSolved = DateTime.Now,
                IsSolved = true,
                IsChecked= false,
            },


        };
        db.Problems.AddRange(problems);
        db.SaveChanges();
    }
}




