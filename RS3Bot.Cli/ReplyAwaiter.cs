using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;
using RS3Bot.Abstractions.Interfaces;
using RS3Bot.Abstractions.Model;
using RS3Bot.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RS3Bot.Cli
{
    public class ReplyAwaiter : IReplyAwaiter
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";

        public ReplyAwaiter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public Task Add(CurrentTaskReply currentTaskReply)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            // Save data in cache.
            _memoryCache.Set($"{currentTaskReply.Task.UserId}-{currentTaskReply.Task.MessageId}", currentTaskReply, cacheEntryOptions);
            return Task.CompletedTask;
        }

        public Task<CurrentTaskReply> IncomingReply(ApplicationUser user, string message)
        {
            if(_memoryCache.TryGetValue<CurrentTaskReply>($"{user.Id}-{user?.CurrentTask?.MessageId}", out var reply))
            {
                if(reply.ConfirmChar.ToString().Equals(message, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Task.FromResult(reply);
                }
            }
            return Task.FromResult<CurrentTaskReply>(null);
        }

        public CurrentTaskReply CreateReply(CurrentTask currentTask)
        {
            Random rand = new Random();
            int num = rand.Next(0, _chars.Length - 1);
            return new CurrentTaskReply { ConfirmChar = _chars[num], Task = currentTask };
        }
    }
}
