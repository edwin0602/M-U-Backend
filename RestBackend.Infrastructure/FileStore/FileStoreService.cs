using Microsoft.AspNetCore.Http;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RestBackend.Infrastructure.FileStore
{
    public class FileStoreService : IFileStoreService
    {
        public async Task<string> SaveFile(IFormFile file, string fileName = "")
        {
            if (file == null || file.Length == 0)
                throw new Exception("File not uploaded");

            fileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName;

            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Storage");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var path = Path.Combine(directory, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return path;
        }
    }
}
