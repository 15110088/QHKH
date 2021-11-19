using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace KHQH.DBFactory
{
    internal class DBGetByPredicate
    {
        /// <summary>
        /// Get all entity
        /// </summary>
        /// <returns>IList<Entity></returns>
        internal IEnumerable<Entity> GetByPredicate<Entity>(Expression<Func<Entity, object>> expression,string ConnectionString) where Entity : class
        {
            using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString))
            {
                connection.Open();
                //var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                //pg.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));
                //pg.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));
                return connection.GetList<Entity>(expression).ToList();
            }
        }
    }
}