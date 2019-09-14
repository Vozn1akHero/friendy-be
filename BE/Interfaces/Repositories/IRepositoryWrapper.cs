using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ISessionRepository Session { get; }
        IEntryRepository Entry { get; }
        IUserEntryRepository UserEntry { get; }
        void Save();
    }
}
