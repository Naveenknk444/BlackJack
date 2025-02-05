using Moq;
using NUnit.Framework;  // Using NUnit for testing
using PolicyNet.Services.DataMigration.HallexRecords;  // Replace with actual namespace
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PolicyNet.Services.DataMigration.Tests  // Replace with actual test namespace
{
    // Mocked service that interacts with the enum
    public interface IHtmlDocumentService
    {
        Task<HtmlDocument> GetHtmlDocument(string url, CancellationToken cancellationToken);
    }

    // The service that uses the enum to fetch HTML documents
    public class HtmlDocumentService : IHtmlDocumentService
    {
        private readonly PolicyNetHtmlOrigin _htmlOrigin;

        public HtmlDocumentService(PolicyNetHtmlOrigin htmlOrigin)
        {
            _htmlOrigin = htmlOrigin;
        }

        public async Task<HtmlDocument> GetHtmlDocument(string url, CancellationToken cancellationToken)
        {
            if (_htmlOrigin == PolicyNetHtmlOrigin.Local)
            {
                // Simulating local HTML document loading
                return await Task.FromResult(new HtmlDocument());
            }
            else
            {
                // Simulating remote HTML document loading
                var htmlWeb = new HtmlWeb();
                return await htmlWeb.LoadFromWebAsync(url, cancellationToken);
            }
        }
    }

    [TestFixture]
    public class HallexDataMigratorSourceTests
    {
        private readonly Mock<IHtmlDocumentService> _mockHtmlDocumentService;  // Mocking the service for GetHtmlDocument
        private readonly HallexDataMigratorSource _hallexDataMigratorSource;

        // Constructor for setting up the mock service and HallexDataMigratorSource
        public HallexDataMigratorSourceTests()
        {
            _mockHtmlDocumentService = new Mock<IHtmlDocumentService>();  // Mocking the service
            _hallexDataMigratorSource = new HallexDataMigratorSource(_mockHtmlDocumentService.Object);  // Class under test
        }

        // Test for Extract method when valid ID is passed
        [Test]
        public async Task Extract_ShouldReturnData_WhenValidIdIsPassed()
        {
            // Arrange: Setting up the mock service behavior for GetHallexListDoc
            var mockHallexList = new HtmlDocument();  // Mocking the document returned by GetHallexListDoc
            _mockHtmlDocumentService.Setup(service => service.GetHtmlDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockHallexList);

            // Mocking ExtractSingleHallexRecordFromJson to return a sample record
            var expectedRecord = new HallexRecordNew { Id = "1", Name = "TestRecord" };
            _mockHtmlDocumentService.Setup(service => service.ExtractSingleHallexRecordFromJson(mockHallexList, "1")).Returns(expectedRecord);

            // Act: Calling the Extract method
            var result = await _hallexDataMigratorSource.Extract("1", false);

            // Assert: Verifying the result using Assert.That
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("TestRecord"));
        }

        // Test for Extract method when invalid ID is passed
        [Test]
        public async Task Extract_ShouldReturnNull_WhenInvalidIdIsPassed()
        {
            // Arrange: Mocking the Extract method to return null for invalid ID
            var mockHallexList = new HtmlDocument();  // Mocking the document returned by GetHallexListDoc
            _mockHtmlDocumentService.Setup(service => service.GetHtmlDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockHallexList);

            _mockHtmlDocumentService.Setup(service => service.ExtractSingleHallexRecordFromJson(mockHallexList, "999")).Returns((HallexRecordNew)null);

            // Act: Calling the Extract method with an invalid ID
            var result = await _hallexDataMigratorSource.Extract("999", false);

            // Assert: Verifying that the result is null
            Assert.That(result, Is.Null);
        }

        // Test for ExtractAll method when data is available
        [Test]
        public async Task ExtractAll_ShouldReturnData_WhenDataIsAvailable()
        {
            // Arrange: Mocking the ExtractAll method to return a list of records
            var mockHallexList = new HtmlDocument();  // Mocking the document returned by GetHallexListDoc
            _mockHtmlDocumentService.Setup(service => service.GetHtmlDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockHallexList);

            var mockRecordList = new List<HallexRecordNew>
            {
                new HallexRecordNew { Id = "1", Name = "Record 1" },
                new HallexRecordNew { Id = "2", Name = "Record 2" }
            };

            _mockHtmlDocumentService.Setup(service => service.ExtractSingleHallexRecordFromJson(mockHallexList, "1")).Returns(mockRecordList[0]);
            _mockHtmlDocumentService.Setup(service => service.ExtractSingleHallexRecordFromJson(mockHallexList, "2")).Returns(mockRecordList[1]);

            // Act: Calling the ExtractAll method
            var result = await _hallexDataMigratorSource.ExtractAll();

            // Assert: Verifying the result using Assert.That
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));  // Verifying two records are returned
            Assert.That(result[0].Name, Is.EqualTo("Record 1"));
        }

        // Test for ExtractAll method when no data is available
        [Test]
        public async Task ExtractAll_ShouldReturnEmptyList_WhenNoDataIsAvailable()
        {
            // Arrange: Mocking the ExtractAll method to return an empty list
            var mockHallexList = new HtmlDocument();  // Mocking the document returned by GetHallexListDoc
            _mockHtmlDocumentService.Setup(service => service.GetHtmlDocument(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockHallexList);

            _mockHtmlDocumentService.Setup(service => service.ExtractSingleHallexRecordFromJson(mockHallexList, It.IsAny<string>())).Returns((HallexRecordNew)null);

            // Act: Calling the ExtractAll method
            var result = await _hallexDataMigratorSource.ExtractAll();

            // Assert: Verifying that the result is an empty list using Assert.That
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
