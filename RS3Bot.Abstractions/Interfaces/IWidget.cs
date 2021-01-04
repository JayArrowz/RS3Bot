using System.IO;
using System.Threading.Tasks;

namespace RS3Bot.Abstractions.Interfaces
{
    public interface IWidget<TArgs>
    {
        Task<Stream> GetWidgetAsync(TArgs args);
    }

    public interface IWidget
    {
        Task<Stream> GetWidgetAsync();
    }
}
