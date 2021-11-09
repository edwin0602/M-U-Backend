using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using RestBackend.Core.Repositories;
using RestBackend.Data;
using RestBackend.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestBackend.UnitTest.Repository
{
    public class RepositoryTests
    {
        private ItemsRepository testRepository;
        private List<RepoItem> testItems = new();

        [SetUp]
        public void Setup()
        {
            #region [ Seed data ]

            testItems.Add(new RepoItem { Name = "Ezekiel Hanson", Year = 1991, Code = "3040", Id = 1 });
            testItems.Add(new RepoItem { Name = "Ezekiel Hanson", Year = 1991, Code = "3040", Id = 1 });
            testItems.Add(new RepoItem { Name = "Inga Best", Year = 1997, Code = "624226", Id = 2 });
            testItems.Add(new RepoItem { Name = "Elaine Serrano", Year = 1995, Code = "61-986", Id = 3 });
            testItems.Add(new RepoItem { Name = "Ezekiel Hanson", Year = 1991, Code = "3040", Id = 1 });
            testItems.Add(new RepoItem { Name = "Jeremy Crane", Year = 1995, Code = "532075", Id = 4 });
            testItems.Add(new RepoItem { Name = "Jonas Clarke", Year = 1992, Code = "23G 8C4", Id = 5 });
            testItems.Add(new RepoItem { Name = "Malachi Hayden", Year = 1993, Code = "N11 5NF", Id = 6 });
            testItems.Add(new RepoItem { Name = "Simon Chapman", Year = 1991, Code = "86525", Id = 7 });
            testItems.Add(new RepoItem { Name = "Quentin Golden", Year = 1997, Code = "752157", Id = 8 });
            testItems.Add(new RepoItem { Name = "Kirk Thornton", Year = 1999, Code = "75711", Id = 9 });
            testItems.Add(new RepoItem { Name = "Hyatt Santos", Year = 1996, Code = "48516", Id = 10 });

            #endregion

            var contextMock = new Mock<DbContext>();
            contextMock
                .Setup(x => x.Set<RepoItem>())
                .ReturnsDbSet(testItems);

            contextMock
                .Setup(x => x.Set<RepoItem>().AddAsync(It.IsAny<RepoItem>(), It.IsAny<CancellationToken>()))
                .Callback((RepoItem model, CancellationToken token) => { testItems.Add(model); });

            contextMock
                .Setup(x => x.Set<RepoItem>().Remove(It.IsAny<RepoItem>()))
                .Callback((RepoItem model) => { testItems.Remove(model); });

            contextMock
                .Setup(x => x.Set<RepoItem>().AddRangeAsync(It.IsAny<List<RepoItem>>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<RepoItem> models, CancellationToken token) => { testItems.AddRange(models); });

            contextMock
                .Setup(x => x.Set<RepoItem>().RemoveRange(It.IsAny<List<RepoItem>>()))
                .Callback((IEnumerable<RepoItem> models) =>
                {
                    foreach (var item in models)
                        testItems.Remove(item);
                });

            testRepository = new ItemsRepository(contextMock.Object);
        }

        [Test]
        public async Task TestAddAndRemove()
        {
            var testItem = new RepoItem() { Id = 999, Code = "999Code", Name = "999Name", Year = 1991 };
            await testRepository.AddAsync(testItem);

            var addedItem = testItems.FirstOrDefault(x => x.Id == testItem.Id);
            Assert.IsTrue(addedItem != default);

            testRepository.Remove(testItem);
            Assert.IsTrue(testItems.FirstOrDefault(x => x.Id == testItem.Id) == default);
        }

        [Test]
        public async Task TestAddRange()
        {
            var testItem1 = new RepoItem() { Id = 999, Code = "999Code", Name = "999Name", Year = 1991 };
            var testItem2 = new RepoItem() { Id = 888, Code = "888Code", Name = "888Name", Year = 1991 };
            var testArray = new List<RepoItem> { testItem2, testItem1 };

            await testRepository.AddRangeAsync(testArray);

            var addedItem = testItems.Where(x => x.Id == testItem1.Id || x.Id == testItem2.Id);
            Assert.IsTrue(addedItem.Count() == 2);

            testRepository.RemoveRange(testArray);
            Assert.IsTrue(!testItems.Any(x => x.Id == testItem1.Id || x.Id == testItem2.Id));
        }

        [Test]
        public async Task TestCount()
        {
            var testCount = testItems.Count(x => x.Year > 1990);
            var result = await testRepository.Count(x => x.Year > 1990);

            Assert.IsTrue(result == testCount);
        }

        [Test]
        public async Task TestGetAllOrdered()
        {
            var result = await testRepository.GetAllAsync(x => x.OrderBy(y => y.Id));

            var LastIdx = testItems.OrderBy(o => o.Id).LastOrDefault().Id;
            var FirstIdx = testItems.OrderBy(o => o.Id).FirstOrDefault().Id;

            Assert.IsTrue(result.Count() == testItems.Count);
            Assert.IsTrue(result.FirstOrDefault().Id == FirstIdx);
            Assert.IsTrue(result.LastOrDefault().Id == LastIdx);
        }

        [Test]
        public async Task TestFirstOrDefault()
        {
            string testCode = testItems.FirstOrDefault().Code;

            var result = await testRepository
                .FirstOrDefaultAsync(x => x.Code == testCode);

            Assert.IsTrue(result != default);
            Assert.IsTrue(result.Code == testCode);
        }

        [Test]
        public async Task TestFind()
        {
            var testData = testItems.Where(d => d.Year > 1995);
            var result = await testRepository
                .FindAsync(x => x.Year > 1995);

            Assert.IsTrue(result.All(testData.Contains));
        }

        [Test]
        public async Task TestGetPagedOrderedAsc()
        {
            int page = 1;
            int limit = 5;
            string filterColum = "Year";

            var pageData = testItems
                .OrderBy(x => x.Year)
                .Take(limit)
                .ToList();
            var result = await testRepository
                        .GetPagedAsync(page, limit, filterColum, true, null);

            Assert.IsTrue(result.All(pageData.Contains));
        }

        [Test]
        public async Task TestGetPagedOrderedDesc()
        {
            int page = 1;
            int limit = 5;
            string filterColum = "Year";

            var pageData = testItems
                .OrderByDescending(x => x.Year)
                .Take(limit)
                .ToList();

            var result = await testRepository
                        .GetPagedAsync(page, limit, filterColum, false, null);

            Assert.IsTrue(result.All(pageData.Contains));
        }

        [Test]
        public async Task TestGetPagedFilter()
        {
            int page = 1;
            int limit = 5;

            var pageData = testItems
                .Where(f => f.Year > 1995)
                .Take(limit)
                .ToList();

            var result = await testRepository
                        .GetPagedAsync(page, limit, string.Empty, true, f => f.Year > 1995);

            Assert.IsTrue(result.All(pageData.Contains));
        }
    }

    public class ItemsRepository : Repository<RepoItem>, IRepository<RepoItem>
    {
        public ItemsRepository(DbContext context)
          : base(context)
        { }
    }

    public class RepoItem
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}