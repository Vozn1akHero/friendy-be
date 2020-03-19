using System;
using System.Collections.Generic;
using System.Text;
using BE.Models;

namespace BE.FakeData
{
    public static class FakeChatMessages
    {
        public static ChatMessages Create(int id,
            int chatId,
            int msgId,
            int firstUserId,
            int secondUserId)
        {
            var firstParticipant = FakeUserData.CreateById(firstUserId);
            var secondParticipant = FakeUserData.CreateById(secondUserId);
            return new ChatMessages()
            {
                Id = id,
                ChatId = chatId,
                MessageId = msgId,
                Chat = new Chat()
                {
                    Id = chatId,
                    FirstParticipantId = firstUserId,
                    SecondParticipantId = secondUserId,
                    FirstParticipant = firstParticipant,
                    SecondParticipant = secondParticipant
                },
                Message = new ChatMessage()
                {
                    Id = msgId,
                    Content = new Guid().ToString(),
                    ImagePath = null,
                    Date = DateTime.Now,
                    UserId = firstUserId,
                    ReceiverId = secondUserId,
                    Read = false,
                    User = firstParticipant,
                    Receiver = secondParticipant
                }
            };
        }

        public static IEnumerable<ChatMessages> CreateList(int length,
            int chatId,
            int firstMsgId,
            int firstUserId,
            int secondUserId)
        {
            for (int i = 0; i < length; i++)
            {
                yield return Create(i+1,
                    chatId,
                    firstMsgId,
                    firstUserId, 
                    secondUserId);
                firstMsgId++;
            }
        }
    }
}
