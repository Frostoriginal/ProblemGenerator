using ProblemGenerator.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

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

    //Recurrt task relevant
    public bool isRecurrentTask { get; set; } = false;
    public DateTime lastTimeAdded { get; set; }

    public bool repeatOnMonday { get; set; }
    public bool repeatOnTuesday { get; set; }
    public bool repeatOnWednesday { get; set; }
    public bool repeatOnThursday { get; set; }
    public bool repeatOnFriday { get; set; }
    public bool repeatOnSaturday { get; set; }
    public bool repeatOnSunday { get; set; }


    public bool repeatedWeekly { get; set; }
    public bool repeatedMonthly { get; set; }
    public bool repeatedYearly { get; set; }

    public DateTime repeatFromDate { get; set; }

    public int daysBeforeRepetition { get; set; }





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

    public static void enumerateAll(ProblemContext db)
    {
        List<Problem> problems = new();
        problems = db.Problems.ToList();
        foreach (var problem in problems)
        {
           // Console.WriteLine($"Writing problems {problem.What}");
            if (problem.Where == "206")
            {
                Console.WriteLine("if statement true");
                Console.WriteLine(DateTime.Today.DayOfWeek);

                Problem copy = new Problem()
                {
                    //Id = 1,
                    What = problem.What,
                    Where = "It's cheesy and delicious. Why wouldn't you want one?",
                    DetailedDescription = "Detailed desc",
                    DateCreated = new DateTime(2023, 5, 1, 8, 30, 52),
                    DateSolved = DateTime.Now,
                    IsSolved = false,
                    IsArchived = false,
                    problemPriority = 1,
                };

                db.Problems.Add(copy);
                db.SaveChanges();
            }
        }
    }

    public static void addReccurentTask(ProblemContext db)
    {
        Console.WriteLine("Timestamp");
        Console.WriteLine(DateTime.Now.ToString());
        List<Problem> problemsActive = new();
        List<Problem> problemsTasks = new();

        problemsActive = db.Problems.Where(s=>s.IsSolved == false && s.isRecurrentTask==false).ToList();
        problemsTasks = db.Problems.Where(s => s.isRecurrentTask == true).ToList();
        foreach (var problemTask in problemsTasks)
        {
            bool comp = problemsActive.Exists(x => x.What == problemTask.What); //true if it's already added
            bool shouldIAddIt = false;
            if (comp == false)
            {
                if(problemTask.repeatFromDate == DateTime.Now) shouldIAddIt = true; //if it should start from selected date


                if (problemTask.lastTimeAdded == DateTime.MinValue) //if it's default it was never added
                {

                Console.WriteLine("its min value"); //minvalue is default.
                problemTask.lastTimeAdded = DateTime.Now; //on first addition
                TimeSpan sinceLastTime = problemTask.lastTimeAdded - DateTime.Now;

                }

                //timespan check here
                if (problemTask.repeatedWeekly == false)
                {
                    
                    TimeSpan timeSinceLastAdded = DateTime.Now - problemTask.lastTimeAdded;
                    int timer = timeSinceLastAdded.Days;
                    if(timer > problemTask.daysBeforeRepetition) shouldIAddIt = true;

                }
                else { 

                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Monday && problemTask.repeatOnMonday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Tuesday && problemTask.repeatOnTuesday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Wednesday && problemTask.repeatOnWednesday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Thursday && problemTask.repeatOnThursday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Friday && problemTask.repeatOnFriday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Saturday && problemTask.repeatOnSaturday == true) shouldIAddIt = true;
                if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Sunday && problemTask.repeatOnSunday == true) shouldIAddIt = true;
                }


                if (shouldIAddIt) 
                                            {
                    problemTask.lastTimeAdded = DateTime.Now;

                                             Problem copy = new Problem()
                                            {
                                                //Id = 1,
                                                What = problemTask.What,
                                                Where = problemTask.Where,
                                                DetailedDescription = problemTask.DetailedDescription,
                                                DateCreated = DateTime.Now,
                    
                                                IsSolved = false,
                                                IsArchived = false,
                                                problemPriority = 1,
                                                isRecurrentTask = false,
                                            };

                                            db.Problems.Add(copy);
                                            db.SaveChanges();
                                            }
            }
        }
    }
}
