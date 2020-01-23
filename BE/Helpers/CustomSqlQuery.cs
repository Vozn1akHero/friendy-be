using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BE.Services.Global
{
    public interface ICustomSqlQuery
    {
        IQueryable<object> ExecuteQuery(string sqlQuery, List<object> parameters);
    }
    
    public class CustomSqlQuery : ICustomSqlQuery
    {
        private IConfiguration _configuration;

        public CustomSqlQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IQueryable<object> ExecuteQuery(string sqlQuery, List<object> parameters)
        {
            string connectionString = _configuration.GetConnectionString("connectionString");
            
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var resultList = new List<object>();
                    while (reader.Read())
                    {
                        resultList.Add(reader);
                    }
                    connection.Dispose();
                    return resultList.AsQueryable();
                }
            }
        }
    }
}