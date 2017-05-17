using System.IO;
using System.Threading.Tasks;

namespace AsyncPluggableProtocol
{
    public interface IProtocol
    {
        string Name { get; }

        Stream GetStream(string url);
    }
}
