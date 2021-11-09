using AutoMapper;
using Microsoft.Extensions.Logging;
using RestBackend.Core;
using RestBackend.Core.Constants;
using RestBackend.Core.Models.Business;
using RestBackend.Core.Models.Exceptions;
using RestBackend.Core.Resources;
using RestBackend.Core.Resources.Pagination;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestBackend.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly ILogger<OwnerService> _logger;

        private readonly IAuditService _auditService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OwnerService(
            ILogger<OwnerService> logger,
            IAuditService auditService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;

            this._auditService = auditService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<OwnerResource> Create(CreateOwnerResource toCreate)
        {
            var owner = _mapper.Map<CreateOwnerResource, Owner>(toCreate);

            await _unitOfWork.Owners.AddAsync(owner);
            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.CREATED, $"{owner.IdOwner}", owner);

            _logger.LogInformation($"Owner created! ({owner.IdOwner})");

            return _mapper.Map<Owner, OwnerResource>(owner);
        }

        public async Task Update(int id, OwnerResource owner)
        {
            var ownerToUpdate = await _unitOfWork.Owners.FirstOrDefaultAsync(x => x.IdOwner == id);
            if (ownerToUpdate == default)
                throw new BusinessException("Owner not found.");

            var ownerSource = _mapper.Map<OwnerResource, Owner>(owner);
            ownerToUpdate.SetForUpdate(ownerSource);

            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.UPDATED, $"{owner.IdOwner}", owner);
            _logger.LogInformation($"Owner updated! ({owner.IdOwner})");
        }

        public async Task<OwnerResource> GetById(int id)
        {
            var owner = await _unitOfWork.Owners.FirstOrDefaultAsync(x => x.IdOwner == id);
            if (owner == default)
                return null;

            return _mapper.Map<Owner, OwnerResource>(owner);
        }

        public async Task<PaginationResource<OwnerResource>> GetAll(PaginationFilter pagination)
        {
            var page = new PaginationResource<OwnerResource>(pagination.PageNumber, pagination.PageSize);

            var ownersPaged = await _unitOfWork.Owners
                .GetPagedAsync(pagination.PageNumber, pagination.PageSize, pagination.OrderBy, pagination.IsAscending);

            page.TotalRecords = await _unitOfWork.Owners.Count();

            page.Data = ownersPaged
                .Select(x => _mapper.Map<Owner, OwnerResource>(x))
                .ToList();

            return page;
        }

        public async Task<PaginationResource<PropertyResource>> GetPropertiesByOwner(PaginationFilter pagination, int IdOwner)
        {
            var page = new PaginationResource<PropertyResource>(pagination.PageNumber, pagination.PageSize);

            Expression<Func<Property, bool>> filter = (x => x.IdOwner == IdOwner);

            var propertiesPaged = await _unitOfWork.Properties
                .GetPagedAsync(pagination.PageNumber, pagination.PageSize, pagination.OrderBy, pagination.IsAscending, filter);

            page.TotalRecords = await _unitOfWork.Properties.Count(filter);

            page.Data = propertiesPaged
                .Select(x => _mapper.Map<Property, PropertyResource>(x))
                .ToList();

            return page;
        }
    }
}
