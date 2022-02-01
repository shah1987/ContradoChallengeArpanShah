using ContradoDataHelper;
using ContradoModel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.DataAccessLayer
{
    public class DLProductRepository : DataConnectionRepository, IDisposable
    {
        public async Task<List<GetAllProductsResponse>> GetAllProducts(SqlParameter[] parameter)
        {
            return (await this.ExecuteAsync<GetAllProductsResponse>("uspGetAllProducts", parameter)).ToList();
        }
        public async Task<GetAllProductsResponse> GetProductByID(SqlParameter[] parameter)
        {
            return (await this.ExecuteAsync<GetAllProductsResponse>("uspProductByID", parameter)).FirstOrDefault();
        }
        public async Task<long> SaveProduct(SqlParameter[] parameter)
        {
            return (await this.ExecuteAsync<long>("uspAddEditProduct", parameter)).FirstOrDefault();
        }
        public async Task DeleteProduct(SqlParameter[] parameter)
        {
            await this.ExecuteAsync<dynamic>("uspDeleteProduct", parameter);
        }
        /// <summary>
        /// Dispose Connection
        /// </summary>
        public void Dispose()
        {
            base.DisposeRepository();
        }
    }
}
