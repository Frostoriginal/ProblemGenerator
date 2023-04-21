using Quartz;

namespace ProblemGenerator.ForQuartz
{
    public class AddRecurrentTasksJob : IJob

    {
        private readonly IAddRecurrentTasks _addRecurrentTasks;
        private readonly int _amountOfDays;

        public AddRecurrentTasksJob(IAddRecurrentTasks addRecurrentTasks)
        {
            _addRecurrentTasks = addRecurrentTasks;
            _amountOfDays = 180; //fixed value for test
           
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var date = DateTime.Now.AddDays(-_amountOfDays);
           await _addRecurrentTasks.AddTasks(date);

        }
    }
}
