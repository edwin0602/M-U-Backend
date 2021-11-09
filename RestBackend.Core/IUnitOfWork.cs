using System;
using System.Threading.Tasks;
using RestBackend.Core.Repositories;

namespace RestBackend.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IAuditRepository Audit { get; }

        IPropertyRepository Properties { get; }

        IPropertyImageRepository PropertiesImages { get; }

        IPropertyTraceRepository PropertiesTraces { get; }

        IOwnerRepository Owners { get; }

        ITaxRepository Taxes { get; }

        Task<int> CommitAsync();
    }
}