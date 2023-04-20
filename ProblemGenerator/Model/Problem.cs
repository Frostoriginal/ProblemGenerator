using ProblemGenerator.Data;
using System.ComponentModel.DataAnnotations;

namespace ProblemGenerator;


public class Problem
{
    public int Id { get; set; }
    [Required]
    [MinLength(2, ErrorMessage ="Must be at least 2 characters long")] 
    [StringLength(20, ErrorMessage = "Be more concise, write additional information in detailed description, maximum field length is 20 characters")]
    public string What { get; set; } = "";
    [Required]
    [MinLength(2, ErrorMessage = "Must be at least 2 characters long")]
    [StringLength(20, ErrorMessage = "Be more concise, write additional information in detailed description, maximum field length is 20 characters")]
    public string Where { get; set; } = "";
    [StringLength(500, ErrorMessage = "Be more concise, maximum field length is 500 characters")]
    public string DetailedDescription { get; set; } = "";

    public DateTime DateCreated { get; set; }
    public DateTime DateSolved { get; set; }
    public bool IsSolved { get; set; }
    public bool IsArchived { get; set; }
    [Range(1,5, ErrorMessage = "Priority must be between 1 and 5")]
    public int problemPriority { get; set; } = 1;
    public TimeSpan TimeElapsed => DateCreated - DateTime.Now;

    public string ImgPath { get; set; } = "";


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
                IsArchived= false,
                problemPriority = 1,
            },
    };
        db.Problems.AddRange(problems);
        db.SaveChanges();
    }
}
