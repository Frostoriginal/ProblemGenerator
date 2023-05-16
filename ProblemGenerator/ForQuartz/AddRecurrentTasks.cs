
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
        private IConfiguration _config;

        public AddRecurrentTasks(ProblemContext problemContext, MyLogger logger, IConfiguration config)
        {
            _dbcontext = problemContext;
            _logger = logger;
            _config = config;
        }
        public async Task AddTasks(DateTime date)
        {
            await addReccurentTask(_dbcontext, _logger);            
            await writeIt(_logger, _config);                    
        }

        public static Task addReccurentTask(ProblemContext db, MyLogger logger)
        {
            logger.addLog($"Started task evaluation");

            List<Problem> problemsTasks = db.Problems.Where(s => s.isRecurrentTask == true).ToList();

            foreach (var problemTask in problemsTasks)
            {
                logger.addLog($"Evaluating task Id:{problemTask.Id} what: {problemTask.What} detailed desc: {problemTask.DetailedDescription}  ");
                
                List<Problem> problemsActive = db.Problems.Where(s => s.IsSolved == false && s.isRecurrentTask == false).ToList();
                if (problemTask.isPausedTask)
                {
                    logger.addLog($"Task id: {problemTask.Id} is paused");
                    return Task.CompletedTask;
                }//check if task is not paused
               // if (problemTask.lastTimeAdded == DateTime.Now.Date) return Task.CompletedTask; //quick duplicates check

                if (problemsActive.Exists(x => x.What == problemTask.What) == false)//check if it doesn;t exist already              
                {
                    logger.addLog($"Task id: {problemTask.Id} is not added yet");
                    
                    if (problemTask.startFromDate.Date <= DateTime.Now.Date || problemTask.repeatOnDate.Date == DateTime.Now.Date)  //if it should start from selected date
                    {
                        logger.addLog($"Task id: {problemTask.Id} passed date switch");
                        if (shouldIAddItMethod(problemTask, logger)) addTaskToCurrentProblems(problemTask, db, logger);
                    }
                    else
                    {
                        logger.addLog($"Task id: {problemTask.Id} didnt pass date switch");
                    }
                }
                else
                {
                    logger.addLog($"Task id: {problemTask.Id} is already added");
                }
            }
            return Task.CompletedTask;
        }

        private static void addTaskToCurrentProblems(Problem problemTask, ProblemContext db, MyLogger logger)
        {
            logger.addLog($"Task id: {problemTask.Id} passed all shouldIAddIt");
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
        private static bool shouldIAddItMethod(Problem problemTask, MyLogger logger)
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
            logger.addLog($"Task id: {problemTask.Id} passed daily switch, shouldIAddIt = {shouldIAddIt}");
            //repeat on selected days every week or on slected day every x weeks    
            if (problemTask.repeatedWeekly)
            {
                if (problemTask.daysBeforeRepetition == 0) //every week on selected days
                {
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && problemTask.repeatOnMonday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday && problemTask.repeatOnTuesday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday && problemTask.repeatOnWednesday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && problemTask.repeatOnThursday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Friday && problemTask.repeatOnFriday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && problemTask.repeatOnSaturday == true) shouldIAddIt = true;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && problemTask.repeatOnSunday == true) shouldIAddIt = true;
                }

                if (problemTask.daysBeforeRepetition > 0) //every x weeks on selected day
                {
                    TimeSpan timeSinceLastAdded = DateTime.Now - problemTask.lastTimeAdded;
                    int timer = timeSinceLastAdded.Days;
                    if (timer > problemTask.daysBeforeRepetition)
                    {
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && problemTask.repeatOnMonday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday && problemTask.repeatOnTuesday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday && problemTask.repeatOnWednesday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday && problemTask.repeatOnThursday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday && problemTask.repeatOnFriday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday && problemTask.repeatOnSaturday == true) shouldIAddIt = true;
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && problemTask.repeatOnSunday == true) shouldIAddIt = true;
                    }
                }
            }
            logger.addLog($"Task id: {problemTask.Id} passed weekly switch, shouldIAddIt = {shouldIAddIt}");
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
            logger.addLog($"Task id: {problemTask.Id} passed monthly switch, shouldIAddIt = {shouldIAddIt}");
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
            logger.addLog($"Task id: {problemTask.Id} passed yearly switch, shouldIAddIt = {shouldIAddIt}");
            return shouldIAddIt;

        }

        public static Task writeIt(MyLogger logger, IConfiguration _config)
        { 
            if(DateTime.Now.Hour == 23) { 

            string docPath = _config.GetValue<string>("LogStorage");
            Directory.CreateDirectory(docPath);

            //ensure log folder exists and if the filename is not used

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, $"{DateTime.Now.Date.ToString("yyyy.MM.dd")}_Log.txt")))
            {
                foreach (string item in logger.Logs)
                {
                    outputFile.WriteLine(item);
                }
            }
            logger.Logs.Clear();

            }
            return Task.CompletedTask;
        }





       

        
 
 

    }
}