using ContradoDataHelper;
using ContradoModel.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContradoChallengeAPI.DataAccessLayer
{
    public class DLProductAttributeRepository : DataConnectionRepository, IDisposable
    {
        public async Task<List<ProductAttribute>> GetAllProductAttribute(SqlParameter[] parameter)
        {
            return (await this.ExecuteAsync<ProductAttribute>("uspGetProductAttribute", parameter)).ToList();
        }
        public void Dispose()
        {
            base.DisposeRepository();
        }
    }
}
