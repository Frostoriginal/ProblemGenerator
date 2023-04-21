
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProblemGenerator.Data;


namespace ProblemGenerator.ForQuartz
{
    public class AddRecurrentTasks: IAddRecurrentTasks
    {
        private readonly ProblemContext _dbcontext;
        private List<Problem> problems = new();
       

        public AddRecurrentTasks(ProblemContext problemContext)
        {
            _dbcontext = problemContext;
            
            problems = problemContext.Problems.ToList();
        }

        public async Task AddTasks(DateTime date)
        {
            //await 
            // Problem.AddProblem(_dbcontext);
            Problem.enumerateAll(_dbcontext);
            /*
            foreach (var problem in problems)
            {
                Console.WriteLine($"Writing problems {problem.What}");
            }
            Console.WriteLine("I am writing it every second");
            */
        }

        

    }
}
