using System;
using System.Linq;
using AutoMapper;
using BE.Shared.Dtos;
using BE.Models;

namespace BE.Mapping.Resolvers
{
    public class AreFriendsResolver : IValueResolver<User, UserDto, bool>
    {
        public bool Resolve(User source, UserDto destination, bool
            member, ResolutionContext context)
        {
            var issuerId = Convert.ToInt32(context.Items["issuerId"]);
            return source.UserFriendshipFirstFriend.Any(d => d.FirstFriendId == issuerId
                                                             && d.SecondFriendId == source.Id
                                                             || d.SecondFriendId == issuerId &&
                                                             d.FirstFriendId == source.Id) || source
                       .UserFriendshipSecondFriend.Any(d => d.FirstFriendId == issuerId
                                                                 && d.SecondFriendId == source.Id || 
                                                                 d.SecondFriendId == issuerId
                                                            && d.FirstFriendId == source.Id);
        }
    }
}