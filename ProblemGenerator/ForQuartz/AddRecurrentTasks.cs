
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProblemGenerator;
using ProblemGenerator.Controllers;
using ProblemGenerator.Data;

namespace ProblemGenerator.ForQuartz
{
    public class AddRecurrentTasks : IAddRecurrentTasks
    {
        private readonly ProblemContext _dbcontext;        
        private MyLogger _logger;

        public AddRecurrentTasks(ProblemContext problemContext, MyLogger logger)
        {
            _dbcontext = problemContext;
            _logger = logger;
        }
        public async Task AddTasks(DateTime date)
        {
            await addReccurentTask(_dbcontext, _logger);            
            await writeIt(_logger);                    
        }

        public static Task addReccurentTask(ProblemContext db, MyLogger logger)
        {
            
            List<Problem> problemsTasks = db.Problems.Where(s => s.isRecurrentTask == true).ToList();

            foreach (var problemTask in problemsTasks)
            {
                
                List<Problem> problemsActive = db.Problems.Where(s => s.IsSolved == false && s.isRecurrentTask == false).ToList();
                if (problemTask.isPausedTask) return Task.CompletedTask; //check if task is not paused
                if (problemTask.lastTimeAdded == DateTime.Now.Date) return Task.CompletedTask; //quick duplicates check

                if (problemsActive.Exists(x => x.What == problemTask.What) == false)//check if it doesn;t exist already              
                {
                    if (problemTask.startFromDate.Date == DateTime.Now.Date || problemTask.repeatOnDate.Date == DateTime.Now.Date)  //if it should start from selected date
                    {
                        if (shouldIAddItMethod(problemTask)) addTaskToCurrentProblems(problemTask, db, logger);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private static void addTaskToCurrentProblems(Problem problemTask, ProblemContext db, MyLogger logger)
        {
            problemTask.lastTimeAdded = DateTime.Now;
            Problem copy = new Problem()
            {
                What = problemTask.What,
                Where = problemTask.Where,
                DetailedDescription = problemTask.DetailedDescription,
                DateCreated = DateTime.Now.Date,                
                IsSolved = false,
                IsArchived = false,
                problemPriority = problemTask.problemPriority,
                isRecurrentTask = false,
            };

            db.Problems.Add(copy);
            db.SaveChanges();
            logger.addLog($"Added problem from recurrent task: Id: {copy.Id}, what: {copy.What}, where {copy.Where}, desription: {copy.DetailedDescription}");

        }
        private static bool shouldIAddItMethod(Problem problemTask)
        {
            bool shouldIAddIt = false;

            //repeat on daily or every x days                            
            if (problemTask.repeatedDaily && problemTask.daysBeforeRepetition == 0) shouldIAddIt = true; //if its daily add task on every method call
            if (problemTask.repeatedDaily && problemTask.daysBeforeRepetition > 0)
            {
                TimeSpan timeSinceLastAdded = DateTime.Now.Date - problemTask.lastTimeAdded.Date;
                int timer = timeSinceLastAdded.Days;
                if (timer >= problemTask.daysBeforeRepetition) shouldIAddIt = true;
            }

            //repeat on selected days every week or on slected day every x weeks    
            if (problemTask.repeatedWeekly)
            {
                if (problemTask.daysBeforeRepetition == 0) //every week on selected days
                {
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Monday && problemTask.repeatOnMonday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Tuesday && problemTask.repeatOnTuesday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Wednesday && problemTask.repeatOnWednesday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Thursday && problemTask.repeatOnThursday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Friday && problemTask.repeatOnFriday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Saturday && problemTask.repeatOnSaturday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Sunday && problemTask.repeatOnSunday == true) shouldIAddIt = true;
                }

                if (problemTask.daysBeforeRepetition > 0) //every x weeks on selected day
                {
                    TimeSpan timeSinceLastAdded = DateTime.Now - problemTask.lastTimeAdded;
                    int timer = timeSinceLastAdded.Days;
                    if (timer > problemTask.daysBeforeRepetition)
                    {
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Monday && problemTask.repeatOnMonday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Tuesday && problemTask.repeatOnTuesday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Wednesday && problemTask.repeatOnWednesday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Thursday && problemTask.repeatOnThursday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Friday && problemTask.repeatOnFriday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Saturday && problemTask.repeatOnSaturday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek + 1 == DayOfWeek.Sunday && problemTask.repeatOnSunday == true) shouldIAddIt = true;
                    }
                }
            }
            //repeat on selected date every month or every x months
            if (problemTask.repeatedMonthly)
            {
                //'february' switch for any month with less days than selected date
                if (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) < problemTask.repeatOnDate.Date.Day)
                {
                    int dayschange = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - problemTask.repeatOnDate.Date.Day;
                    problemTask.repeatOnDate = problemTask.repeatOnDate.AddDays(dayschange);
                }

                if (problemTask.daysBeforeRepetition == 0)
                {
                    if (problemTask.repeatOnDate.Date.Day == DateTime.Now.Date.Day) shouldIAddIt = true;
                }
                if (problemTask.daysBeforeRepetition > 0)
                {
                    TimeSpan timeSinceLastAdded = DateTime.Now - problemTask.lastTimeAdded;
                    int timer = timeSinceLastAdded.Days;
                    if (timer > problemTask.daysBeforeRepetition)
                    {
                        if (problemTask.repeatOnDate.Date.Day == DateTime.Now.Date.Day) shouldIAddIt = true;
                    }
                }
            }
            //repeat on selected date every year or every x years
            if (problemTask.repeatedYearly)
            {
                if (problemTask.daysBeforeRepetition == 0)
                {
                    if (problemTask.repeatOnDate.Date.Day == DateTime.Now.Date.Day && problemTask.repeatOnDate.Date.Month == DateTime.Now.Date.Month) shouldIAddIt = true;
                }
                if (problemTask.daysBeforeRepetition > 0)
                {
                    TimeSpan timeSinceLastAdded = DateTime.Now - problemTask.lastTimeAdded;
                    int timer = timeSinceLastAdded.Days;
                    if (timer > problemTask.daysBeforeRepetition)
                    {
                        if (problemTask.repeatOnDate.Date.Day == DateTime.Now.Date.Day && problemTask.repeatOnDate.Date.Month == DateTime.Now.Date.Month) shouldIAddIt = true;
                    }
                }

            }
            return shouldIAddIt;

        }

        public static Task writeIt(MyLogger logger)
        {
            string docPath = "C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\logs"; //update for production!
                                                                                      //ensure log folder exists and if the filename is not used

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{DateTime.Now.AddDays(-1).Date.ToString("yyyy.MM.dd")}_Log.txt")))
            {
                foreach (string item in logger.Logs)
                {
                    outputFile.WriteLine(item);
                }
            }
            //logger.Logs.Clear(); //uncomment on production!
            return Task.CompletedTask;
        }





       

        
 
 

    }
}