using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BE.Dtos;

namespace BE.RepositoryServices.User
{
    public interface IUserSearching
    {
        List<Models.User> Sort(UsersLookUpCriteriaDto usersLookUpCriteria);
    }

    public class UserSearching : IUserSearching
    {
        public UserSearching(List<Models.User> users)
        {
            Users = users;
        }

        public List<Models.User> Users { get; set; }

        public List<Models.User> Sort(UsersLookUpCriteriaDto usersLookUpCriteria)
        {
/*            if (usersLookUpCriteria.Name != null)
            {
                SortByName(usersLookUpCriteria.Name);
            }*/

            foreach (PropertyInfo propertyInfo in usersLookUpCriteria.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    ExecuteIfNotNull(propertyInfo.GetValue(usersLookUpCriteria, null), );
                }
            }

            
            
            return Users;
        }

        private void ExecuteIfNotNull(string parameter, Action expression)
        {
            if (parameter != null)
            {
                expression();
            }
        }
        
        private void SortByString(string value, string columnName)
        {
            PropertyInfo prop = typeof(UsersLookUpCriteriaDto)
                .GetProperty(columnName);
            var param = Expression.Parameter(typeof(UsersLookUpCriteriaDto));
            var condition =
                Expression.Lambda<Func<UsersLookUpCriteriaDto>>(
                    Expression.Equal(
                        Expression.Property(param, columnName),
                        Expression.Constant(value, typeof(string))
                    ),
                    param
                ).Compile();
            
            Users.RemoveAll(condition); 
        }
        
        private void SortByName(string name)
        {
            Users.RemoveAll(e =>
                e.Name.ToLower().StartsWith(name.ToLower()));
        }
    }
}