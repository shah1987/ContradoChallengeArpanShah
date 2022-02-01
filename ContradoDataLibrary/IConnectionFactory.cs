using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContradoDataHelper
{
    public interface IConnectionFactoryAsync
    {
        Task<SqlConnection> CreateAsync();
    }
}
