using System.Collections.Generic;
using BE.Dtos;

namespace BE.Repositories.RepositoryServices.Interfaces.User
{
    public interface IUserSearchingService
    {
        List<UserLookUpModelDto> Filter(List<UserLookUpModelDto> users,
            UsersLookUpCriteriaDto usersLookUpCriteria);
    }
}