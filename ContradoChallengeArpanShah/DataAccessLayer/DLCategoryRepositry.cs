using ContradoDataHelper;
using ContradoModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.DataAccessLayer
{
    public class DLCategoryRepositry : DataConnectionRepository, IDisposable
    {
        public async Task<List<ProductCategory>> GetAllCategories()
        {
            return (await this.ExecuteAsync<ProductCategory>("uspProductCategory")).ToList();
        }
        public void Dispose()
        {
            base.DisposeRepository();
        }
    }
}
