using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestBackend.Core.Models.Business;
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
    public class OwnerServiceTest
    {
        private readonly List<Property> propertiesData = new();
        private readonly List<Owner> ownersData = new();
        private readonly List<PropertyImage> imagesData = new();
        private readonly List<PropertyTrace> traceData = new();
        private OwnerService ownerService;

        [SetUp]
        public void Setup()
        {

            propertiesData.AddRange(new List<Property>{
                new Property{ IdProperty = 1, Name = "Claudia Mckinney", Address = "8707 Adipiscing, St.", Price = 561, CodeInternal = "U1Q 6X1", Year = 1992, IdOwner = 1 },
                new Property{ IdProperty = 2, Name = "Addison Gardner", Address = "P.O. Box 812, 8324 Sem Ave", Price = 465, CodeInternal = "R2W 2T0", Year = 1994, IdOwner = 3 },
                new Property{ IdProperty = 3, Name = "Holmes Sparks", Address = "6808 Orci Street", Price = 263, CodeInternal = "P9S 9L4", Year = 1990, IdOwner = 5 },
                new Property{ IdProperty = 4, Name = "Amela Briggs", Address = "P.O. Box 923, 6953 Lobortis Street", Price = 141, CodeInternal = "P0Y 5J9", Year = 1991, IdOwner = 5 },
                new Property{ IdProperty = 5, Name = "Ruth Wagner", Address = "523-5983 Justo St.", Price = 332, CodeInternal = "L2K 3L1", Year = 1997, IdOwner = 4 }
            });

            ownersData.AddRange(new List<Owner>{
                new Owner() { Name = "Claudia Mckinney", IdOwner = 3, Address = "980-8838 Diam St." },
                new Owner() { Name = "Addison Gardner", IdOwner = 5, Address = "523-5983 Justo St." }
            });

            var MockContext = new Mock<RestBackendDbContext>();
            MockContext
                .Setup(x => x.Set<Property>())
                .ReturnsDbSet(propertiesData);

            MockContext
                .Setup(x => x.Set<Owner>())
                .ReturnsDbSet(ownersData);

            MockContext
               .Setup(x => x.Set<Property>().AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
               .Callback((Property model, CancellationToken token) => { propertiesData.Add(model); });

            MockContext
               .Setup(x => x.Set<Owner>().AddAsync(It.IsAny<Owner>(), It.IsAny<CancellationToken>()))
               .Callback((Owner model, CancellationToken token) => { ownersData.Add(model); });

            IMapper mapper = MockUtils.IMapperInstance();
            IAuditService auditService = Mock.Of<IAuditService>();
            ILogger<OwnerService> logger = Mock.Of<ILogger<OwnerService>>();
            UnitOfWork unitOfWork = new UnitOfWork(MockContext.Object);

            ownerService = new OwnerService(logger, auditService, mapper, unitOfWork);
        }

        [Test]
        public async Task TestCreateOwner()
        {
            var beforeItems = ownersData.Count;
            var fakeProperty = new Core.Resources.CreateOwnerResource
            {
                Name = "FakeName",
                Photo = "URL",
                Address = "FakeAdress",
                Birthday = DateTime.Now
            };
            await ownerService.Create(fakeProperty);

            Assert.True(ownersData.Count > beforeItems);
        }

        [Test]
        public async Task TestUpdateOwner()
        {
            var beforeItem = ownersData.FirstOrDefault();
            var fakeAddress = $"{beforeItem.Address}_FAKE";
            var fakePhoto = $"{beforeItem.Photo}_FAKE";

            var fakeProperty = new Core.Resources.OwnerResource
            {
                Birthday = DateTime.Now.AddDays(-5),
                Name = beforeItem.Name,
                Address = fakeAddress,
                Photo = fakePhoto,
                IdOwner = beforeItem.IdOwner
            };

            await ownerService.Update(beforeItem.IdOwner, fakeProperty);

            var afterItem = ownersData.FirstOrDefault(x => x.IdOwner == beforeItem.IdOwner);
            Assert.True(afterItem.Address == fakeAddress);
            Assert.True(afterItem.Photo == fakePhoto);
        }
    }
}
