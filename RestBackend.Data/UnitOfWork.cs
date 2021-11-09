using System.Threading.Tasks;
using RestBackend.Core;
using RestBackend.Core.Repositories;
using RestBackend.Data.Repositories;

namespace RestBackend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestBackendDbContext _context;

        private PropertyRepository _propertyRepository;

        private PropertiesImagesRepository _propertiesImagesRepository;

        private PropertiesTracesRepository _propertiesTracesRepository;

        private OwnerRepository _ownerRepository;

        private TaxRepository _taxRepository;

        private AuditRepository _auditRepository;

        public UnitOfWork(RestBackendDbContext context)
        {
            this._context = context;
        }

        public IPropertyRepository Properties
            => _propertyRepository ??= new PropertyRepository(_context);

        public IPropertyImageRepository PropertiesImages
            => _propertiesImagesRepository ??= new PropertiesImagesRepository(_context);

        public IPropertyTraceRepository PropertiesTraces
            => _propertiesTracesRepository ??= new PropertiesTracesRepository(_context);

        public IOwnerRepository Owners
            => _ownerRepository ??= new OwnerRepository(_context);

        public ITaxRepository Taxes
            => _taxRepository ??= new TaxRepository(_context);

        public IAuditRepository Audit
            => _auditRepository ??= new AuditRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}