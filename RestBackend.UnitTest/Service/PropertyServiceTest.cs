using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestBackend.Core.Models.Business;
using RestBackend.Core.Models.Exceptions;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using RestBackend.Data;
using RestBackend.Services;
using RestBackend.UnitTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestBackend.UnitTest.Service
{
    public class PropertyServiceTest
    {
        private readonly List<Property> seedData = new();
        private readonly List<Owner> ownersData = new();
        private readonly List<PropertyImage> imagesData = new();
        private readonly List<PropertyTrace> traceData = new();
        private PropertyService propertyService;

        [SetUp]
        public void Setup()
        {
            seedData.AddRange(new List<Property>{
                new Property{ IdProperty = 1, Name = "Claudia Mckinney", Address = "8707 Adipiscing, St.", Price = 561, CodeInternal = "U1Q 6X1", Year = 1992, IdOwner = 1 },
                new Property{ IdProperty = 2, Name = "Addison Gardner", Address = "P.O. Box 812, 8324 Sem Ave", Price = 465, CodeInternal = "R2W 2T0", Year = 1994, IdOwner = 3 },
                new Property{ IdProperty = 3, Name = "Holmes Sparks", Address = "6808 Orci Street", Price = 263, CodeInternal = "P9S 9L4", Year = 1990, IdOwner = 5 },
                new Property{ IdProperty = 4, Name = "Amela Briggs", Address = "P.O. Box 923, 6953 Lobortis Street", Price = 141, CodeInternal = "P0Y 5J9", Year = 1991, IdOwner = 5 },
                new Property{ IdProperty = 5, Name = "Ruth Wagner", Address = "523-5983 Justo St.", Price = 332, CodeInternal = "L2K 3L1", Year = 1997, IdOwner = 4 },
                new Property{ IdProperty = 6, Name = "Grant Jefferson", Address = "101-5228 Malesuada Road", Price = 379, CodeInternal = "P7J 1E2", Year = 1992, IdOwner = 1 },
                new Property{ IdProperty = 7, Name = "Madison Thomas", Address = "915-3513 Nisi St.", Price = 599, CodeInternal = "Q7F 9L4", Year = 1992, IdOwner = 2 },
                new Property{ IdProperty = 8, Name = "Vincent Boyer", Address = "950-4160 Integer St.", Price = 236, CodeInternal = "B8N 7T5", Year = 1996, IdOwner = 5 },
                new Property{ IdProperty = 9, Name = "Odessa Middleton", Address = "365-5543 Quam, Street", Price = 218, CodeInternal = "L8K 9M5", Year = 1995, IdOwner = 2 },
                new Property{ IdProperty = 10, Name = "Rudyard Castaneda", Address = "980-8838 Diam St.", Price = 110, CodeInternal = "S8O 3K8", Year = 1999, IdOwner = 5 }
            });

            ownersData.AddRange(new List<Owner>{
                new Owner() { Name = "FakeOwner", IdOwner = 3, Address = "980-8838 Diam St." },
                new Owner() { Name = "FakeBuyer", IdOwner = 5, Address = "980-8838 Diam St." },
            });

            imagesData.AddRange(new List<PropertyImage> {
                new PropertyImage() { Enabled = true, File = "Fake_URL", IdProperty = 1, IdProperyImage = 1 }
            });

            var MockContext = new Mock<RestBackendDbContext>();
            MockContext
                .Setup(x => x.Set<Property>())
                .ReturnsDbSet(seedData);

            MockContext
                .Setup(x => x.Set<Owner>())
                .ReturnsDbSet(ownersData);

            MockContext
                .Setup(x => x.Set<PropertyImage>())
                .ReturnsDbSet(imagesData);

            MockContext
               .Setup(x => x.Set<Property>().AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
               .Callback((Property model, CancellationToken token) => { seedData.Add(model); });

            MockContext
               .Setup(x => x.Set<Owner>().AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()))
               .Callback((Owner model, CancellationToken token) => { ownersData.Add(model); });

            MockContext
               .Setup(x => x.Set<PropertyImage>().AddAsync(It.IsAny<PropertyImage>(), It.IsAny<CancellationToken>()))
               .Callback((PropertyImage model, CancellationToken token) => { imagesData.Add(model); });

            MockContext
               .Setup(x => x.Set<PropertyTrace>().AddAsync(It.IsAny<PropertyTrace>(), It.IsAny<CancellationToken>()))
               .Callback((PropertyTrace model, CancellationToken token) => { traceData.Add(model); });

            var taxService = new Mock<ITaxService>();
            taxService
                .Setup(x => x.GetCurrentTax())
                .ReturnsAsync(0.19m);

            IMapper mapper = MockUtils.IMapperInstance();
            IFileStoreService fileStoreService = Mock.Of<IFileStoreService>();
            IAuditService auditService = Mock.Of<IAuditService>();
            ILogger<PropertyService> logger = Mock.Of<ILogger<PropertyService>>();
            UnitOfWork unitOfWork = new UnitOfWork(MockContext.Object);

            propertyService = new PropertyService(logger, mapper, fileStoreService, auditService, unitOfWork, taxService.Object);
        }

        [Test]
        public async Task TestCreateProperty()
        {
            var beforeItems = seedData.Count;
            var fakeProperty = new Core.Resources.CreatePropertyResource
            {
                Name = "FakeName",
                CodeInternal = "FAKE",
                Price = 2000,
                Address = "FakeAdress",
                Year = 1991,
                IdOwner = ownersData.FirstOrDefault().IdOwner
            };
            await propertyService.Create(fakeProperty);

            Assert.True(seedData.Count > beforeItems);
        }

        [Test]
        public async Task TestUpdateProperty()
        {
            var fakeOwnerId = ownersData.FirstOrDefault().IdOwner;
            var beforeItem = seedData.FirstOrDefault(x => x.IdOwner == fakeOwnerId);
            var fakeCode = $"{beforeItem.CodeInternal}_FAKE";
            var fakePrice = 5000;

            var fakeProperty = new Core.Resources.PropertyResource
            {
                Name = beforeItem.Name,
                CodeInternal = fakeCode,
                Price = fakePrice,
                Address = beforeItem.Address,
                Year = beforeItem.Year,
                IdOwner = beforeItem.IdOwner,
                IdProperty = beforeItem.IdProperty
            };

            await propertyService.Update(beforeItem.IdProperty, fakeProperty);

            var afterItem = seedData.FirstOrDefault(x => x.IdProperty == beforeItem.IdProperty);

            Assert.True(afterItem.CodeInternal == fakeCode);
            Assert.True(afterItem.Year == beforeItem.Year);
            Assert.True(afterItem.Price != fakePrice);
        }

        [Test]
        public async Task TestUpdatePriceProperty()
        {
            var fakeOwnerId = ownersData.FirstOrDefault().IdOwner;
            var beforeItem = seedData.FirstOrDefault(x => x.IdOwner == fakeOwnerId);

            var fakePrice = 5200;

            await propertyService.UpdatePrice(beforeItem.IdProperty, fakePrice);

            var afterItem = seedData.FirstOrDefault(x => x.IdProperty == beforeItem.IdProperty);
            Assert.True(afterItem.Price == fakePrice);
        }

        [Test]
        public async Task TestSellProperty()
        {
            var beforeTrace = traceData.Count;

            var fakeBuyerId = ownersData.LastOrDefault().IdOwner;
            var fakeOwnerId = ownersData.FirstOrDefault().IdOwner;
            var beforeItem = seedData.FirstOrDefault(x => x.IdOwner == fakeOwnerId);

            var fakePrice = 5100;
            await propertyService.SellProperty(beforeItem.IdProperty, fakePrice, fakeBuyerId);

            var afterItem = seedData.FirstOrDefault(x => x.IdProperty == beforeItem.IdProperty);
            Assert.True(afterItem.Price == fakePrice);
            Assert.True(afterItem.IdOwner == fakeBuyerId);

            Assert.True(traceData.Count > beforeTrace);
        }

        [Test]
        public async Task TestAddPermitedImageProperty()
        {
            var formFileMock = MockUtils.FormFileMock("jpg");

            var beforeImageItems = imagesData.Count;
            var property = seedData.FirstOrDefault();

            await propertyService.AddImage(property.IdProperty, formFileMock.Object);
            Assert.True(imagesData.Count > beforeImageItems);
        }

        [Test]
        public async Task TestAddDenyImageProperty()
        {
            try
            {
                var formFileMock = MockUtils.FormFileMock();
                var property = seedData.FirstOrDefault();

                await propertyService.AddImage(property.IdProperty, formFileMock.Object);
            }
            catch (BusinessException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public async Task TestAddImageMissingProperty()
        {
            try
            {
                var formFileMock = MockUtils.FormFileMock();
                await propertyService.AddImage(int.MaxValue, formFileMock.Object);
            }
            catch (BusinessException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public async Task TestRemoveImageMissingProperty()
        {
            var imageItem = imagesData.FirstOrDefault();
            await propertyService.RemoveImage(imageItem.IdProperty, imageItem.IdProperyImage);

            var removedImage = imagesData.FirstOrDefault(x => x.IdProperyImage == imageItem.IdProperyImage);
            Assert.True(!removedImage.Enabled);
        }
    }
}
