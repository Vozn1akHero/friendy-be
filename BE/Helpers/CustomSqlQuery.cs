using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace BE.Helpers
{
    public interface ICustomSqlQuery
    {
        IQueryable<object> ExecuteQuery(string sqlQuery, List<object> parameters);
    }

    public class CustomSqlQuery : ICustomSqlQuery
    {
        private readonly IConfiguration _configuration;

        public CustomSqlQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IQueryable<object> ExecuteQuery(string sqlQuery, List<object> parameters)
        {
            var connectionString = _configuration.GetConnectionString("connectionString");

            using (var connection =
                new SqlConnection(connectionString))
            {
                var command = new SqlCommand(sqlQuery, connection);

                foreach (var parameter in parameters) command.Parameters.Add(parameter);

                connection.Open();
                using (var reader = command.ExecuteReader(CommandBehavior.SingleResult))
                {
                    var resultList = new List<object>();
                    while (reader.Read()) resultList.Add(reader);
                    connection.Dispose();
                    return resultList.AsQueryable();
                }
            }
        }
    }
}