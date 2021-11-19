using Dapper;
using Dapper.Contrib.Extensions;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KHQH.DBFactory
{
    public class DBInteractive
    {
        private static string ConnectionString = "";
        private static string connString = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.159.128)(PORT = 1521))(CONNECT_DATA = (SID = reis))); User Id = quyhoach2021; Password = abc123;";

        public DBInteractive()
        {

            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Insert by query or storeProcedure
        /// </summary>
        /// <param name="sql">string</param>
        /// <param name="parameters">DynamicParameters  q</param>
        /// <returns></returns>
        public async Task<int> Post(string sql, params DynamicParameters[] parameters)
        {
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString);
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
            {
                connection.Open();
                var dataout = connection.QueryAsync<Entity>(sql, parameters, commandTimeout: 60, commandType: System.Data.CommandType.StoredProcedure).Result;
                return dataout;
            }
        }
        //public TugboatReportModel Get2(string sql, DynamicParameters parameters = null)
        //{
        //    using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
        //    {
        //        connection.Open();
        //        var grid = connection.QueryMultiple(sql, parameters, commandTimeout: 60, commandType: System.Data.CommandType.StoredProcedure);
        //        TugboatReportModel report = new TugboatReportModel();
        //        report.MonthlyReports = grid.Read<MonthlyReportModel>();
        //        report.VoyageReports = grid.Read<VoyageReportModel>();
        //        report.ServiceReports = grid.Read<ServiceReportModel>();
        //        return report;
        //    }
        //}

        //public TooltipModel GetCargoTooltip(string sql, DynamicParameters parameters = null)
        //{
        //    using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
        //    {
        //        connection.Open();
        //        var grid = connection.QueryMultiple(sql, parameters, commandTimeout: 60, commandType: System.Data.CommandType.StoredProcedure);
        //        TooltipModel tt = new TooltipModel();
        //        tt.ListTooltip = grid.Read<TooltipGrid>();
        //        tt.Tooltip = grid.Read<TooltipGrid>();
        //        return tt;
        //    }
        //}
        //public ExploitReportModel GetExploitReport(string sql, DynamicParameters parameters = null)
        //{
        //    using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
        //    {
        //        connection.Open();
        //        var grid = connection.QueryMultiple(sql, parameters, commandTimeout: 120, commandType: System.Data.CommandType.StoredProcedure);
        //        ExploitReportModel report = new ExploitReportModel();
        //        report.CargoReports = grid.Read<CargoReportModel>().ToList();
        //        report.InOutReports = grid.Read<InOutReportModel>().ToList();
        //        report.ServiceReports = grid.Read<ServiceReportModel>().ToList();
        //        return report;
        //    }
        //}
        public System.Data.DataTable GetExploitReportSummary(string sql, DynamicParameters parameters = null)
        {
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
            {
                connection.Open();
                var result = connection.ExecuteReader(sql, parameters, commandTimeout: 120, commandType: System.Data.CommandType.StoredProcedure);
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
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
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
            {
                connection.Open();
                return connection.Update(entites);
            }
        }
    }
}