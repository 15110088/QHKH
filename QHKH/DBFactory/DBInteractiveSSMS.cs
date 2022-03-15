using Dapper;
using Dapper.Contrib.Extensions;
using KHQH.DBFactory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QHKH.DBFactory
{
    public class DBInteractiveSSMS
    {
        private static string ConnectionString = "";

        public DBInteractiveSSMS()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSSMS"].ConnectionString;
        }

        /// <summary>
        /// Insert by query or storeProcedure
        /// </summary>
        /// <param name="sql">string</param>
        /// <param name="parameters">DynamicParameters  q</param>
        /// <returns></returns>
        public async Task<int> Post(string sql, params DynamicParameters[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.ExecuteAsync(sql, parameters, commandType: System.Data.CommandType.StoredProcedure).Result;
            }
        }

        /// <summary>
        /// Insert 1 or multi entity
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        public int EPost<Entity>(params Entity[] entites) where Entity : class
        {
            var connection = new SqlConnection(ConnectionString);
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            return connection.InsertAsync<Entity>(entites[0]).Result;
        }

        /// <summary>
        /// Get data by procedure name 
        /// </summary>
        /// <typeparam name="Entity">Entity</typeparam>
        /// <param name="sql">string</param>
        /// <param name="parameters">DynamicParameters[]</param>
        /// <returns>Task<IList<dynamic>></returns>
        public IEnumerable<Entity> Get<Entity>(string sql, DynamicParameters parameters = null) where Entity : class
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var dataout = connection.QueryAsync<Entity>(sql, parameters, commandTimeout: 60, commandType: System.Data.CommandType.StoredProcedure).Result;
                return dataout;
            }
        }
        

       
        public System.Data.DataTable GetSQL(string sql, DynamicParameters parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var result = connection.ExecuteReader(sql, parameters, commandTimeout: 120, commandType: System.Data.CommandType.Text);
                var dt = new System.Data.DataTable();
                dt.Load(result);
                return dt;
            }
        }
        /// <summary>
        /// Get 1 entity by ProductDetailID
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Entity</returns>
        public Entity EGetSingleByID<Entity>(int id) where Entity : class
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Get<Entity>(id);
            }
        }

        /// <summary>
        /// Get all entity
        /// </summary>
        /// <returns>IList<Entity></returns>
        public IEnumerable<Entity> EGetAll<Entity>() where Entity : class
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.GetAll<Entity>();
            }
        }

        /// <summary>
        /// Get all entity
        /// </summary>
        /// <returns>IList<Entity></returns>
        public IEnumerable<Entity> EGetByWhere<Entity>(Expression<Func<Entity, object>> expression) where Entity : class
        {
            DBGetByPredicate dBGetByPredicate = new DBGetByPredicate();
            return dBGetByPredicate.GetByPredicate<Entity>(expression, ConnectionString);
        }


        /// <summary>
        /// Delete by query or storeProcedure 
        /// </summary>
        /// <param name="sql">string</param>
        /// <param name="parameters">DynamicParameters</param>
        /// <returns>int</returns>
        public int Delete(string sql)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return connection.Execute(sql, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Delete 1 or multi entity
        /// </summary>
        /// <param name="entites">params Entity[]</param>
        /// <returns>bool</returns>
        public bool EDelete<Entity>(params Entity[] entites) where Entity : class
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Delete(entites);
            }
        }

        /// <summary>
        /// Update by query or storeProcedure
        /// </summary>
        /// <param name="sql">string</param>
        /// <param name="parameters">DynamicParameters</param>
        /// <returns>int</returns>
        public int Put(string sql)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                return connection.Execute(sql, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Update 1 or multi entity
        /// </summary>
        /// <param name="entites">params Entity[]</param>
        /// <returns>bool</returns>
        public bool EPut<Entity>(params Entity[] entites) where Entity : class
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Update(entites);
            }
        }
    }
}