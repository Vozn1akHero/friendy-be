using System.Collections.Generic;
using BE.Dtos.UserDtos;

namespace BE.Repositories.RepositoryServices.Interfaces.User
{
    public interface IUserSearchingService
    {
        List<UserLookUpModelDto> Filter(List<UserLookUpModelDto> users,
            UsersLookUpCriteriaDto usersLookUpCriteria);
    }
}