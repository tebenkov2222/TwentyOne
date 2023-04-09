using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Npgsql;

namespace Repository
{
    public class PostreSQL
    {
        private readonly NpgsqlConnection _db;
        public PostreSQL(Configuration configuration)
        {
            _db = new NpgsqlConnection(
                $"Host={configuration.Host};" +
                $"Port={configuration.Port};" +
                $"Database={configuration.DataBaseName};" +
                $"Username={configuration.UserName};" +
                $"Password={configuration.Password}");
        }

        public async Task<int> ExecuteQuery(string str)
        {
            using var query = new QueryTool(_db);
            return await query.QueryWithoutTable(str);
        }
        
        public async Task<T> GetModel<T>(string str) where T : IModel, new()
        {
            var result = await GetModels<T>(str);
            return result == null ? default : result[0];
        }

        public async Task<T[]> GetModels<T>(string str) where T : IModel, new()
        {
            using var query = new QueryTool(_db);
            return GetModels<T>(await query.QueryWithTable(str));
        }

        private T GetModel<T>(NpgsqlDataReader reader) where T : IModel, new()
        {
            var k = 0;
            var packet = new T();
            var fieldCount = reader.FieldCount;
            var dict = Enumerable.Range(1, fieldCount).ToDictionary(_ => reader.GetName(k), _ => reader.GetValue(k++));
            foreach (var x in packet.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var key = x.GetCustomAttributes(typeof(DbKey)).Cast<DbKey>().First().Value;
                if (!dict.ContainsKey(key)) continue;
                var value = dict[key];
                x.SetValue(packet, value is DBNull ? null : value );
            }

            return packet;
        }
        
        private T[] GetModels<T>(NpgsqlDataReader reader) where T : IModel, new()
        {
            if (!reader.HasRows) return null;
            var result = new List<T>();
            while (reader.Read())
                result.Add(GetModel<T>(reader));
            
            return result.ToArray();
        }
    }
}