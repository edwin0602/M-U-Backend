using System.Threading.Tasks;

namespace RestBackend.Core.Services
{
    public interface ITaxService
    {
        Task<decimal> GetCurrentTax();
    }
}
