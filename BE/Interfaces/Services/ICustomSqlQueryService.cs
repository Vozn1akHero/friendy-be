using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BE.Interfaces
{
    public interface ICustomSqlQueryService
    {
        IQueryable<object> ExecuteQuery(string sqlQuery, List<object> parameters);
    }
}