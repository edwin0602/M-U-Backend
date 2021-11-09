using System;
using System.Threading.Tasks;

namespace RestBackend.Core.Services.Infrastructure
{
    public interface ICacheService
    {
        public T GetOrCreate<T>(object key, Func<T> createItem);
    }
}
