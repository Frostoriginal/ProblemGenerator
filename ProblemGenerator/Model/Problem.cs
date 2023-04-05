using ProblemGenerator.Data;

namespace ProblemGenerator;

public class Problem
{
    public int Id { get; set; }
    public string What { get; set; }

    public string Where { get; set; }

    public string DetailedDescription { get; set; }

    // image!

    public DateTime DateCreated { get; set; }
    public DateTime DateSolved { get; set; }
    public bool IsSolved { get; set; }
    public bool IsChecked { get; set; }

    public TimeSpan TimeElapsed => DateCreated - DateTime.Now;

    public static void AddProblem(ProblemContext db)
    {
        var problems = new Problem[] {


             new Problem()
            {
                //Id = 1,
                What = "Basic Cheese Pizza",
                Where = "It's cheesy and delicious. Why wouldn't you want one?",
                DetailedDescription = "Detailed desc",
                DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                DateSolved = DateTime.Now,
                IsSolved = false,
                IsChecked= false,
            },
    };
        db.Problems.AddRange(problems);
        db.SaveChanges();
    }
}
