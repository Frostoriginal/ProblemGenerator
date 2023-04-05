using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemGenerator.CoreBusiness;

namespace ProblemGenerator.UseCases
{
    public class ViewInventoriesByNameUseCase
    {
        private readonly IInventoryRepositorry inventoryRepositorry;

        public ViewInventoriesByNameUseCase(IInventoryRepositorry inventoryRepositorry)
        {
            this.inventoryRepositorry = inventoryRepositorry;
        }
        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
        {
           return await inventoryRepositorry.GetInventoriesByName(name);

        }
    }
}
