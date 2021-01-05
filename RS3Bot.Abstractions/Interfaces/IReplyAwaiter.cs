using Discord.WebSocket;
using RS3Bot.Abstractions.Model;
using System;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface IReplyAwaiter
    {
        Task Add(CurrentTaskReply currentTaskReply);

        Task<CurrentTaskReply> IncomingReply(ApplicationUser id, string message);

        CurrentTaskReply CreateReply(CurrentTask currentTask);
    }
}
