using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContradoDataHelper
{
    public class DbContextAsync
    {
        private SqlConnection _connectionAsync;
        private readonly IConnectionFactoryAsync _connectionFactoryAsync;
        SqlCommand _dbCommandAsync;
        SqlDataReader _dataReaderAsync;
        
        public SqlCommand CommandAsync
        {
            get
            {
                return this._dbCommandAsync;
            }
        }
        public SqlDataReader ReaderAsync
        {
            get
            {
                return this._dataReaderAsync;
            }
            set
            {
                this._dataReaderAsync = value;
            }
        }
        public DbContextAsync()
        {
            _connectionFactoryAsync = new DbConnectionFactory();
        }
        public async Task CreateCommandAsync(string strStoreProcedureName, SqlParameter[] parameters = null, int? TimeOut = null)
        {
            _connectionAsync = await _connectionFactoryAsync.CreateAsync();
            _dbCommandAsync = _connectionAsync.CreateCommand();

            _dbCommandAsync.CommandType = CommandType.StoredProcedure;
            _dbCommandAsync.CommandText = strStoreProcedureName;
            if (TimeOut.HasValue && TimeOut.Value > 0)
                _dbCommandAsync.CommandTimeout = TimeOut.Value;

            if (parameters != null)
                foreach (var param in parameters)
                    _dbCommandAsync.Parameters.Add(_dbCommandAsync.CreateParameter(param.ParameterName, param.Value));
        }
        /// <summary>
        /// Initialize a data reader.
        /// </summary>
        /// <param name="_storeProcedureName">Store procedure name</param>
        /// <param name="_storeProcedureParameters">Store procedure parameters</param>
        /// <param name="_commandTimeOut">Command timepout</param>
        public async Task InitializeReaderAsync(string _storeProcedureName, SqlParameter[] _storeProcedureParameters = null, int? _commandTimeOut = null)
        {
            if (this.CommandAsync == null && this.ReaderAsync == null)
            {
                await this.CreateCommandAsync(_storeProcedureName, _storeProcedureParameters, _commandTimeOut);
                this.ReaderAsync = await this._dbCommandAsync.ExecuteReaderAsync();
            }
        }
        public void Dispose(bool _connectionDispose = false)
        {
            if (_dataReaderAsync != null)
            {
                _dataReaderAsync.Dispose();
                _dataReaderAsync = null;
            }
            if (_dbCommandAsync != null)
            {
                _dbCommandAsync.Dispose();
                _dbCommandAsync = null;
            }
                if (_connectionAsync != null)
                    _connectionAsync.Dispose();
        }
    }
}
