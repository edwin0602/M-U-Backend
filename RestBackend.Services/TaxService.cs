using RestBackend.Core;
using RestBackend.Core.Models.Exceptions;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace RestBackend.Services
{
    public class TaxService : ITaxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public TaxService(
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        private const string CURRENT_TAX_KEY = "CURRENT_TAX_KEY";

        public async Task<decimal> GetCurrentTax()
        {
            return _cacheService.GetOrCreate(CURRENT_TAX_KEY, () =>
            {
                var CurrentTax = _unitOfWork.Taxes
                                    .FirstOrDefaultAsync(x => x.Enabled == true, o => o.OrderByDescending(p => p.IdTax))
                                    .Result;

                if (CurrentTax == default)
                    throw new BusinessException("Current Tax not configured.");

                return CurrentTax.Value;
            });
        }
    }
}
