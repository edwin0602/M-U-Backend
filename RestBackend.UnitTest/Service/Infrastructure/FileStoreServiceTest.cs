using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using RestBackend.Infrastructure.FileStore;
using RestBackend.UnitTest.Utils;
using System.IO;
using System.Threading.Tasks;

namespace RestBackend.UnitTest.Service.Infrastructure
{
    public class FileStoreServiceTest
    {
        Mock<IFormFile> formFileMock;

        [SetUp]
        public void Setup()
        {
            formFileMock = MockUtils.FormFileMock();
        }

        [Test]
        public async Task TestSaveFile()
        {
            var fileStoreService = new FileStoreService();
            var filePatch = await fileStoreService.SaveFile(formFileMock.Object);

            Assert.True(File.Exists(filePatch));

            File.Delete(filePatch);
        }
    }
}
