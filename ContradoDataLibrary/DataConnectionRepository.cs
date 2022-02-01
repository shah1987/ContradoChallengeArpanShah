using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContradoDataHelper
{
    public abstract class DataConnectionRepository
    {
        /// <summary>
        /// ADO.Net DataContext 
        /// </summary>
        DbContextAsync _contextAsync;

        /// <summary>
        /// An ADO.Net Context Constructor.
        /// </summary>
        /// <param name="IsReadonlyConnection">ReadOnly Connection</param>
        /// <param name="IsReadonlySecondaryConnection">ReadOnly Secondary Connection</param>
        /// <param name="GroupID">SQL Server Group ID</param>
        public DataConnectionRepository()
        {
            _contextAsync = new DbContextAsync();
        }

        /// <summary>
        /// Get ADO.Net Context
        /// </summary>
        protected DbContextAsync ContextAsync
        {
            get
            {
                return this._contextAsync;
            }
        }

        /// <summary>
        /// Executes a store procedure and returns generic class object. Used for multi result sets.
        /// </summary>
        /// <typeparam name="TElement">Generic class object</typeparam>
        /// <param name="_storeProcedureName">Store procedure name</param>
        /// <param name="_storeProcedureParameters">Store procedure parameters</param>
        /// <param name="_nextResultSet">Fetches next result set from the multi result sets.</param>
        /// <param name="_commandTimeOut">Command timepout</param>        
        /// <returns></returns>
        protected async Task<IEnumerable<TElement>> ExecuteAsync<TElement>(string _storeProcedureName, SqlParameter[] _storeProcedureParameters = null, bool _nextResultSet = false, int? _commandTimeOut = null)// where TElement : class
        {
            try
            {
                await this.ContextAsync.InitializeReaderAsync(_storeProcedureName, _storeProcedureParameters, _commandTimeOut);
                return await this.GetResultAsync<TElement>(_nextResultSet);
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                throw;
            }
        }
        private async Task<IEnumerable<TElement>> GetResultAsync<TElement>(bool _nextResultSet = false)
        {
            List<TElement> items = new List<TElement>();
            try
            {
                if (true == _nextResultSet)
                    await this.ContextAsync.ReaderAsync.NextResultAsync();

                while (await this.ContextAsync.ReaderAsync.ReadAsync())
                {
                    items.Add(this.Map<TElement>(this.ContextAsync.ReaderAsync));
                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                items = null;
            }
        }

        /// <summary>
        /// Mapes data reader result sets with generic object.
        /// </summary>
        /// <typeparam name="TEntity">Generic objec type</typeparam>
        /// <param name="record">Data reader record</param>
        /// <returns>Generic object type</returns>
        private TEntity Map<TEntity>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<TEntity>();
            bool blProperties = false;
            try
            {
                foreach (var property in typeof(TEntity).GetProperties())
                {
                    blProperties = true;
                    if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                        property.SetValue(objT, record[property.Name]);

                }
                //if no properties found than return default single value back depends on the type.
                if (false == blProperties)
                {
                    objT = (TEntity)Convert.ChangeType(record[0], objT.GetType());
                }
                return objT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    objT = default(TEntity);
            //}
        }
        public virtual void DisposeRepository()
        {
            if (_contextAsync != null) _contextAsync.Dispose(true);
        }
    }
}
