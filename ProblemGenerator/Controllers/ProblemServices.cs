using Microsoft.EntityFrameworkCore;
using ProblemGenerator.Data;
using ProblemGenerator.Controllers;


namespace ProblemGenerator.Controllers
{
    public class ProblemServices
    {
        #region Private members
        private ProblemContext dbContext;
       
        
        #endregion

        #region Constructor
        public ProblemServices(ProblemContext dbContext)
        {
            this.dbContext = dbContext;            
        }
        #endregion

        #region Public methods
        /// <summary>
        /// This method returns the list of problems
        /// </summary>
        /// <returns></returns>
        public async Task<List<Problem>> GetProblemsAsync()
        {
            return await dbContext.Problems.ToListAsync();
        }

        /// <summary>
        /// This method add a new problem to the DbContext and saves it
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<Problem> AddNewProblem(Problem problem)
        { 
                        
            try
            {
                dbContext.Problems.Add(problem);
                await dbContext.SaveChangesAsync();              

            }
            catch (Exception)
            {
                throw;
            }
            return problem;
        }

        /// <summary>
        /// This method update and existing problem and saves the changes
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<Problem> UpdateProblemAsync(Problem problem)
        {
            try
            {
                var productExist = dbContext.Problems.FirstOrDefault(p => p.Id == problem.Id);
                if (productExist != null)
                {
                    dbContext.Update(problem);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return problem;
        }

        /// <summary>
        /// This method removes and existing problem from the DbContext and saves it
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task DeleteProblemAsync(Problem problem)
        {
            try
            {
                if(problem.ImgPath != null && problem.ImgPath != "")
                {
                    List<string> imagePaths = new List<string>();
                    if (problem.ImgPath[0] == 1)
                    {
                        imagePaths.Add(problem.ImgPath.Substring(1));
                    }
                    if (problem.ImgPath[0] > 1)
                    {
                        imagePaths = problem.ImgPath.Substring(1).Split("#*#").ToList(); //combine physical path!
                    }
                    foreach (var item in imagePaths)
                    {
                        if (File.Exists(Path.Combine("C:\\Websites\\Usterka\\wwwroot\\", item))) //hardcoded!
                        {
                            File.Delete(Path.Combine("C:\\Websites\\Usterka\\wwwroot\\", item));                            
                        }
                        /*
                        if (File.Exists(Path.Combine("C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\", item))) //hardcoded!
                        {
                            File.Delete(Path.Combine("C:\\Users\\user\\Desktop\\TestowyBuild\\wwwroot\\", item));
                        }
                        */
                    }

                }
                dbContext.Problems.Remove(problem);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
