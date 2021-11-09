using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestBackend.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ILogger<PropertyService> _logger;

        private readonly IAuditService _auditService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaxService _taxService;

        private readonly IMapper _mapper;
        private readonly IFileStoreService _fileStore;

        private readonly string[] PermitedFileTypes = { ".jpg", ".jpeg", ".png" };

        public PropertyService(
            ILogger<PropertyService> logger,
            IMapper mapper,
            IFileStoreService fileStore,
            IAuditService auditService,
            IUnitOfWork unitOfWork,
            ITaxService taxService)
        {
            this._logger = logger;

            this._unitOfWork = unitOfWork;
            this._fileStore = fileStore;
            this._auditService = auditService;

            this._mapper = mapper;

            this._taxService = taxService;
        }

        public async Task<PropertyResource> Create(CreatePropertyResource toCreate)
        {
            var property = _mapper.Map<CreatePropertyResource, Property>(toCreate);

            var owner = await _unitOfWork.Owners.FirstOrDefaultAsync(x => x.IdOwner == toCreate.IdOwner);
            if (owner == default)
                throw new BusinessException("Owner not found.");

            var codeUsed = await _unitOfWork.Properties
                .FirstOrDefaultAsync(x => x.CodeInternal == toCreate.CodeInternal.ToUpper());
            if (codeUsed != default)
                throw new BusinessException("Property code was used.");

            property.CodeInternal = property.CodeInternal.ToUpper();

            await _unitOfWork.Properties.AddAsync(property);
            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.CREATED, $"{property.IdProperty}", property);
            _logger.LogInformation($"Property created! ({property.IdProperty})");

            return _mapper.Map<Property, PropertyResource>(property);
        }

        public async Task Update(int id, PropertyResource property)
        {
            var itemToUpdate = await _unitOfWork.Properties.FirstOrDefaultAsync(x => x.IdProperty == id);
            if (itemToUpdate == default)
                throw new Exception("Property not found.");

            if (property.CodeInternal != itemToUpdate.CodeInternal)
            {
                var codeUsed = await _unitOfWork.Properties
                    .FirstOrDefaultAsync(x => x.CodeInternal == property.CodeInternal.ToUpper());
                if (codeUsed != default)
                    throw new BusinessException("Property code was used.");
            }

            var itemSource = _mapper.Map<PropertyResource, Property>(property);
            itemToUpdate.SetForUpdate(itemSource);

            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.UPDATED, $"{itemToUpdate.IdProperty}", itemToUpdate);
            _logger.LogInformation($"Property updated! ({itemToUpdate.IdProperty})");
        }

        public async Task<PaginationResource<PropertyResource>> GetAll(PaginationFilter pagination, PropertyFilterResource filter)
        {
            var page = new PaginationResource<PropertyResource>(pagination.PageNumber, pagination.PageSize);

            Expression<Func<Property, bool>> filterFunc = (x =>
                    (string.IsNullOrEmpty(filter.CodeInternal) || x.CodeInternal == filter.CodeInternal) &&
                    (string.IsNullOrEmpty(filter.LikeName) || x.Name.Contains(filter.LikeName)) &&
                    (!filter.MaxPrice.HasValue || x.Price <= filter.MaxPrice.Value) &&
                    (!filter.MinPrice.HasValue || x.Price >= filter.MinPrice.Value) &&
                    (!filter.MaxYear.HasValue || x.Year <= filter.MaxYear.Value) &&
                    (!filter.MinYear.HasValue || x.Year >= filter.MinYear.Value)
                );

            var propertiesPaged = await _unitOfWork.Properties
                .GetPagedAsync(pagination.PageNumber, pagination.PageSize, pagination.OrderBy, pagination.IsAscending, filterFunc);

            page.TotalRecords = await _unitOfWork.Properties.Count(filterFunc);

            page.Data = propertiesPaged
                .Select(x => _mapper.Map<Property, PropertyResource>(x))
                .ToList();

            return page;
        }

        public async Task<PropertyResource> GetById(int id)
        {
            var property = await _unitOfWork.Properties
                .FirstOrDefaultAsync(x => x.IdOwner == id);
            if (property == default)
                return null;

            var resource = _mapper.Map<Property, PropertyResource>(property);
            resource.Images = await GetImages(id);

            return resource;
        }

        public async Task UpdatePrice(int idProperty, decimal price)
        {
            var property = await _unitOfWork.Properties.FirstOrDefaultAsync(x => x.IdProperty == idProperty);
            if (property == default)
                throw new Exception("Property not found.");

            property.Price = price;
            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.PRICE_CHANGE, $"{property.IdProperty}", property);
            _logger.LogInformation($"Property price updated! ({idProperty})");
        }

        public async Task SellProperty(int idProperty, decimal price, int buyerUserId)
        {
            var currentTax = await _taxService.GetCurrentTax();

            var property = await _unitOfWork.Properties.FirstOrDefaultAsync(x => x.IdProperty == idProperty);
            if (property == default)
                throw new Exception("Property not found.");

            var owner = await _unitOfWork.Owners.FirstOrDefaultAsync(x => x.IdOwner == buyerUserId);
            if (property == default)
                throw new Exception("New Owner not found.");

            property.Price = price;
            property.IdOwner = buyerUserId;

            await _unitOfWork.PropertiesTraces.AddAsync(new PropertyTrace()
            {
                DateSale = DateTime.Now,
                IdProperty = property.IdProperty,
                Name = owner.Name,
                Value = property.Price,
                Tax = property.Price * currentTax
            });

            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.SOLD, $"{property.IdProperty}", property);
            _logger.LogInformation($"Property sold! ({idProperty})");
        }

        public async Task<IEnumerable<PropertyTraceResource>> GetTrace(int idProperty)
        {
            var property = await _unitOfWork.PropertiesTraces.FindAsync(x => x.IdProperty == idProperty);
            if (property == default)
                return null;

            return _mapper.Map<IEnumerable<PropertyTrace>, IEnumerable<PropertyTraceResource>>(property);
        }

        public async Task<IEnumerable<PropertyImageResource>> GetImages(int idProperty)
        {
            var property = await _unitOfWork.PropertiesImages.FindAsync(x => x.IdProperty == idProperty && x.Enabled == true);
            if (property == default)
                return null;

            return _mapper.Map<IEnumerable<PropertyImage>, IEnumerable<PropertyImageResource>>(property);
        }

        public async Task AddImage(int idProperty, IFormFile image)
        {
            var property = await _unitOfWork.Properties.FirstOrDefaultAsync(x => x.IdProperty == idProperty);
            if (property == default)
                throw new BusinessException("Property not found.");

            if (!PermitedFileTypes.Contains(Path.GetExtension(image.FileName)))
                throw new BusinessException("File extension not valid.");

            var urlFile = await _fileStore.SaveFile(image);

            var propertyImage = new PropertyImage
            {
                Enabled = true,
                File = urlFile,
                IdProperty = idProperty
            };
            await _unitOfWork.PropertiesImages.AddAsync(propertyImage);
            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.CREATED, $"{propertyImage.IdProperyImage}", propertyImage);
            _logger.LogInformation($"Property image added! ({propertyImage.IdProperyImage})");
        }

        public async Task RemoveImage(int idProperty, int idImage)
        {
            var property = await _unitOfWork.Properties.FirstOrDefaultAsync(x => x.IdProperty == idProperty);
            if (property == default)
                throw new BusinessException("Property not found.");

            var image = await _unitOfWork.PropertiesImages.FirstOrDefaultAsync(x => x.IdProperyImage == idImage);
            if (image == default)
                throw new BusinessException("Image not found.");

            image.Enabled = false;
            await _unitOfWork.CommitAsync();

            await _auditService.Add(AuditActions.DISABLED, $"{idImage}", image);
            _logger.LogInformation($"Property image disabled! ({idImage})");
        }
    }
}
