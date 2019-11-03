using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BE.Dtos;
using BE.Repositories.RepositoryServices.Interfaces.User;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace BE.RepositoryServices.User
{
    
    public class UserSearchingService : IUserSearchingService
    {
        public List<UserLookUpModelDto> Filter(List<UserLookUpModelDto> users,
         UsersLookUpCriteriaDto usersLookUpCriteria)
        {
            foreach (PropertyInfo propertyInfo in usersLookUpCriteria.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    string value = propertyInfo.GetValue(usersLookUpCriteria)?.ToString();
                    if (CheckIfNotNull(value))
                    {
                        string columnName = propertyInfo.Name;
                        users = new List<UserLookUpModelDto>(FilterByString(value, columnName, users));
                    }
                }
                else if(propertyInfo.PropertyType == typeof(int))
                {
                    int value = Convert.ToInt32(propertyInfo.GetValue(usersLookUpCriteria));
                    if (value != 0)
                    {
                        string columnName = propertyInfo.Name;
                        users = new List<UserLookUpModelDto>(FilterByInteger(value, columnName, users));
                    }
                } else if (propertyInfo.PropertyType == typeof(IEnumerable<string>))
                {
                    var interests = (IEnumerable<string>) propertyInfo.GetValue(usersLookUpCriteria);
                    FilterByInterests(ref users, interests);
                }
            }
            
            return users;
        }

        private bool CheckIfNotNull(string parameter)
        {
            return parameter != null;
        }
        
        private IEnumerable<UserLookUpModelDto> FilterByString(string value, 
            string columnName,
            IEnumerable<UserLookUpModelDto> users)
        {
            var param = Expression.Parameter(typeof(UserLookUpModelDto), "type");
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] {typeof(string)});
            
/*            var method = typeof(Enumerable)
                .GetRuntimeMethods()
                .Single(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);
            var containsMethod = method.MakeGenericMethod(typeof(string));*/
            
            var condition =
                Expression.Lambda<Func<UserLookUpModelDto, bool>>(
                    Expression.Call(
                        Expression.Call(Expression.Property(param, columnName),
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes)),
                        method,
                        Expression.Call(Expression.Constant(value, typeof(string)),
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes))
                    ),
                    param
                ).Compile();
            
            return users.Where(condition);
        }
        
        private IEnumerable<UserLookUpModelDto> FilterByInteger(int value, 
            string columnName,
            IEnumerable<UserLookUpModelDto> users)
        {
            var param = Expression.Parameter(typeof(UserLookUpModelDto), "type");
            var condition =
                Expression.Lambda<Func<UserLookUpModelDto, bool>>(
                    Expression.Equal(
                        Expression.Property(param, "UserAdditionalInfo."+columnName+"Id"),
                        Expression.Constant(value, typeof(int))
                    ),
                    param
                ).Compile();
            return users.Where(condition);
        }

        private void FilterByInterests(ref List<UserLookUpModelDto> users,
            IEnumerable<string> interests)
        {
            users.RemoveAll(e => e.UserInterests.Any(y => interests.Any(b => b == y)));
        }
    }
}