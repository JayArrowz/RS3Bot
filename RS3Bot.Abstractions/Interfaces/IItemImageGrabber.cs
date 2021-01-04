using System.IO;
using System.Threading.Tasks;

namespace RS3BotWeb.Shared
{
    public interface IItemImageGrabber
    {
        Task<Stream> GetAsync(int id);
    }
}