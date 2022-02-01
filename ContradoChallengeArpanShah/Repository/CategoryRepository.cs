using ContradoChallengeAPI.DataAccessLayer;
using ContradoModel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.Repository
{
    public class CategoryRepository : IDisposable
    {
        private readonly DLCategoryRepositry categoryRepositry;
        public CategoryRepository()
        {
            categoryRepositry = new DLCategoryRepositry();
        }
        public async Task<List<ProductCategory>> GetAllCategories()
        {
            return await categoryRepositry.GetAllCategories();
        }
        public void Dispose()
        {

        }
    }
}
