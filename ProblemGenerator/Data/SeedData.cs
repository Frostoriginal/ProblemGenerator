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
                IsArchived= false,
                problemPriority = 1,
                ImgPath ="",
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
                IsArchived= false,
                problemPriority = 1,
                ImgPath ="",
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
                IsArchived= false,
                ImgPath ="",
            },
              new Problem()
            {
                Id = 4,
                What = "Recurrent task test",
                Where = "Server?",
                DetailedDescription = "Detailed desc",
                DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                DateSolved = DateTime.Now,
                IsSolved = false,
                IsArchived= false,
                ImgPath ="",
                isRecurrentTask = true,
                repeatOnSaturday= true,
            },


        };
        db.Problems.AddRange(problems);
        db.SaveChanges();
    }
}




