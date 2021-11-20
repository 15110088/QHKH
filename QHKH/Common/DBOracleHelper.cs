using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using Oracle.ManagedDataAccess.Client;

namespace KHQH.Common
{
    public  class DBOracleHelper
    {
        private static string defaultConnectionString = "";
        public DBOracleHelper()
        {
            defaultConnectionString = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.169.3.13)(PORT = 1521))(CONNECT_DATA = (SID = reis))); User Id = quyhoach2021; Password = abc123@;";

        }

        public static DataTable ExecuteProcedure(string PROC_NAME, List<OracleParameter> parameters)
        {
            try
            {
                //if (parameters.Length % 2 != 0)
                //    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                DataTable a = new DataTable();
                string query = PROC_NAME;
               
                a = Query(query, parameters);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ExecuteQuery(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                DataTable a = new DataTable();
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));

                //a = Query(query, filters);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ExecuteNonQuery(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                return NonQuery(query, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ExecuteScalar(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to query. Expected an even number.");
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                return Scalar(query, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        private static DataTable Query(String consulta, IList<OracleParameter> parametros)
        {
            try
            {
                DataTable dt = new DataTable();
                OracleConnection connection = new OracleConnection();
                connection.ConnectionString = defaultConnectionString;
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                OracleCommand command = new OracleCommand();
                OracleDataAdapter da;
                try
                {
                    command.Connection = connection;
                    command.CommandText = consulta;
                    command.CommandType = CommandType.StoredProcedure;
                    // command.Parameters.Add("IDKYQH", OracleDbType.Decimal).Value = 3;

                    if (parametros != null)
                    {
                        command.Parameters.AddRange(parametros.ToArray());
                    }
                    command.Parameters.Add("preloop", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                    da = new OracleDataAdapter(command);
                    da.Fill(dt);
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private static int NonQuery(string query, IList<SqlParameter> parametros)
        {
            try
            {
                DataSet dt = new DataSet();
                Oracle.ManagedDataAccess.Client.OracleConnection connection = new Oracle.ManagedDataAccess.Client.OracleConnection(defaultConnectionString);
                OracleCommand command = new OracleCommand();

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteNonQuery();

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static object Scalar(string query, List<SqlParameter> parametros)
        {
            try
            {
                DataSet dt = new DataSet();
                Oracle.ManagedDataAccess.Client.OracleConnection connection = new Oracle.ManagedDataAccess.Client.OracleConnection(defaultConnectionString);
                OracleCommand command = new OracleCommand();

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteScalar();

                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }



}