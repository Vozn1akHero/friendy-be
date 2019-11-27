using System;
using System.Collections.Generic;
using System.Data.Common;

namespace BE.Services.Global.Interfaces
{
    public interface IRowSqlQueryService
    {
        List<T> Execute<T>(string query, Func<DbDataReader, T> map);
    }
}