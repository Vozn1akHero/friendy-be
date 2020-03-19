using BE.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BE.FakeData
{
    public static class FakeChat
    {
        public static Chat Create(User firstParticipant, User secondParticipant, IEnumerable<ChatMessages> chatMessages)
        {
            return new Chat() { 
                FirstParticipant = firstParticipant,
                SecondParticipant = secondParticipant
            };
        }
    }
}
