using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface IFileStoreService
    {
        Task<string> SaveFile(IFormFile file, string fileName = "");
    }
}
