﻿using Microsoft.EntityFrameworkCore;
using ProblemGenerator.Data;

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
        /// This method returns the list of product
        /// </summary>
        /// <returns></returns>
        public async Task<List<Problem>> GetProblemsAsync()
        {
            return await dbContext.Problems.ToListAsync();
        }

        /// <summary>
        /// This method add a new product to the DbContext and saves it
        /// </summary>
        /// <param name="product"></param>
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
        /// This method update and existing product and saves the changes
        /// </summary>
        /// <param name="product"></param>
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
        /// This method removes and existing product from the DbContext and saves it
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task DeleteProblemAsync(Problem problem)
        {
            try
            {
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
