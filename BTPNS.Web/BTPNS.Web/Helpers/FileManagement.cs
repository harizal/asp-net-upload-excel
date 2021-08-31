using BTPNS.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BTPNS.Core
{
    public class FileManagement : IFileManagement
    {
        private readonly IConfiguration _configuration;
        public FileManagement(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] GetByte(string fileName)
        {
            var baseDirectory = _configuration.GetValue<string>("BaseDirectory");
            var filePath = Path.Combine(baseDirectory, $"{fileName}");
            return File.ReadAllBytes(filePath);
        }

        public Stream GetStream(string path)
        {
            return File.OpenRead(path);
        }

        public string Save(MemoryStream stream, string extension)
        {
            var baseDirectory = _configuration.GetValue<string>("BaseDirectory");
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            var filePath = Path.Combine(baseDirectory, $"{Guid.NewGuid()}{extension}");

            File.WriteAllBytes(filePath, stream.ToArray());

            return filePath;
        }
    }
}
