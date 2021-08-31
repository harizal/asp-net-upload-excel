using System.IO;

namespace BTPNS.Contracts
{
    public interface IFileManagement
    {
        string Save(MemoryStream stream, string extension);
        Stream GetStream(string path);
        byte[] GetByte(string fileName);
    }
}
