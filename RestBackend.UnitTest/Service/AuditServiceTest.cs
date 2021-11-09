using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestBackend.Core.Models.Security;
using RestBackend.Data;
using RestBackend.Services;
using RestBackend.UnitTest.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestBackend.UnitTest.Service
{
    public class AuditServiceTest
    {
        private readonly List<Audit> seedData = new();
        private ILogger<AuditService> logger;
        private UnitOfWork unitOfWork;

        [SetUp]
        public void Setup()
        {
            var MockContext = new Mock<RestBackendDbContext>();
            MockContext
                .Setup(x => x.Set<Audit>())
                .ReturnsDbSet(seedData);

            MockContext
               .Setup(x => x.Set<Audit>().AddAsync(It.IsAny<Audit>(), It.IsAny<CancellationToken>()))
               .Callback((Audit model, CancellationToken token) => { seedData.Add(model); });

            logger = Mock.Of<ILogger<AuditService>>();
            unitOfWork = new UnitOfWork(MockContext.Object);
        }

        [Test]
        public async Task TestAddAuditAuthenticated()
        {
            var mockHttpContextAccessor = MockUtils.AuthenticatedHttpContextAccessorMock();
            AuditService auditService = new AuditService(logger, mockHttpContextAccessor.Object, unitOfWork);

            var currentItems = seedData.Count;
            await auditService.Add(new Audit() { Action = "Fake", CreatedAt = DateTime.Now, Entity = "AuditServiceTest" });
            Assert.True(++currentItems == seedData.Count);

            currentItems = seedData.Count;
            await auditService.Add("FakeAction", "FakeTarget", currentItems);
            Assert.True(++currentItems == seedData.Count);
        }

        [Test]
        public async Task TestAddAuditAnonymous()
        {
            var mockHttpContextAccessor = MockUtils.AnonymousHttpContextAccessorMock();
            AuditService auditService = new AuditService(logger, mockHttpContextAccessor.Object, unitOfWork);

            var currentItems = seedData.Count;
            await auditService.Add(new Audit() { Action = "Fake", CreatedAt = DateTime.Now, Entity = "AuditServiceTest" });
            Assert.True(++currentItems == seedData.Count);

            currentItems = seedData.Count;
            await auditService.Add("FakeAction", "FakeTarget", currentItems);
            Assert.True(++currentItems == seedData.Count);
        }
    }
}
