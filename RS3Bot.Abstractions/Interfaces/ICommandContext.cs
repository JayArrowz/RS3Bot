using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface ICommandContext
    {
        public IDiscordBot Bot { get; set; }
    }
}
