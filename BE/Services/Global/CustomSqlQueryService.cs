using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BE.Services
{
    public class CustomSqlQueryService : ICustomSqlQueryService
    {
        private IConfiguration _configuration;

        public CustomSqlQueryService(IConfiguration configuration)
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