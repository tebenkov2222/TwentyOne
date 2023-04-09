using System;
using System.Threading.Tasks;
using Npgsql;

namespace Repository
{
    public class QueryTool : IDisposable
    {
        private NpgsqlConnection _dataBase;

        public QueryTool(NpgsqlConnection dataBase)
        {
            _dataBase = dataBase;
            _dataBase.Open();
        }

        public async Task<int> QueryWithoutTable(string query) =>
            await new NpgsqlCommand(query, _dataBase).ExecuteNonQueryAsync();

        public async Task<NpgsqlDataReader> QueryWithTable(string query) =>
            await new NpgsqlCommand(query, _dataBase).ExecuteReaderAsync();
        
        private bool _isDisposed;

        ~QueryTool() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            _dataBase.Close();
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool fromDisposeMethod)
        {
            if (_isDisposed) return;
            if (fromDisposeMethod) _dataBase.Close();
            _isDisposed = true;
        }
    }
}