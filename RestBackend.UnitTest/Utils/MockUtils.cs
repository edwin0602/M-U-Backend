using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Security.Claims;

namespace RestBackend.UnitTest.Utils
{
    public static class MockUtils
    {
        public static Mock<IHttpContextAccessor> AuthenticatedHttpContextAccessorMock()
        {
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, new Guid().ToString())
            }, "Basic"));

            var httpContext = new DefaultHttpContext() { User = user };

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor
                .Setup(_ => _.HttpContext)
                .Returns(httpContext);

            mockHttpContextAccessor
                .Setup(h => h.HttpContext.User)
                .Returns(user);

            mockHttpContextAccessor
                .Setup(x => x.HttpContext.Request.Path)
                .Returns(It.IsAny<string>());

            return mockHttpContextAccessor;
        }

        public static Mock<IHttpContextAccessor> AnonymousHttpContextAccessorMock()
        {
            var httpContext = new DefaultHttpContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor
                .Setup(_ => _.HttpContext)
                .Returns(httpContext);

            mockHttpContextAccessor
                .Setup(x => x.HttpContext.Request.Path)
                .Returns(It.IsAny<string>());

            return mockHttpContextAccessor;
        }

        public static IMapper IMapperInstance()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new RestBackend.Core.Mapping.MappingProfile()));
            return mockMapper.CreateMapper();
        }

        public static Mock<IFormFile> FormFileMock(string ext = "pdf")
        {
            var formFileMock = new Mock<IFormFile>();

            var content = "Hello World from a Fake File";
            var fileName = $"test.{ext}";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            formFileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            formFileMock.Setup(_ => _.FileName).Returns(fileName);
            formFileMock.Setup(_ => _.Length).Returns(ms.Length);

            return formFileMock;
        }
    }
}
