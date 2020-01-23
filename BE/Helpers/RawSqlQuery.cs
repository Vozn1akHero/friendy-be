using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Global
{
    public interface IRawSqlQuery
    {
        List<T> Execute<T>(string query, Func<DbDataReader, T> map);
    }
    
    public class RawSqlQuery : IRawSqlQuery
    {
        public RawSqlQuery(FriendyContext friendyContext)
        {
            FriendyContext = friendyContext;
        }

        private FriendyContext FriendyContext { get; }

        public List<T> Execute<T>(string query, Func<DbDataReader, T> map)
        {
            using (var command = FriendyContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                FriendyContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read()) entities.Add(map(result));

                    return entities;
                }
            }
        }
        
        public List<T> ExecuteAsync<T>(string query, Func<DbDataReader, T> map)
        {
            using (var command = FriendyContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                FriendyContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read()) entities.Add(map(result));

                    return entities;
                }
            }
        }
    }
}