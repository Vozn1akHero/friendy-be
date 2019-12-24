using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Post
{
    public interface IMainCommentRepository : IRepositoryBase<MainComment>
    {
        Task<IEnumerable<TType>> GetAllByPostIdAsync<TType>(int postId,
            Expression<Func<MainComment, TType>> select);
        
        
    }
}