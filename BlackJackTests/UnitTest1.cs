[Test]
public async Task ExtractAll_HA_014_50_001_ShouldReturnAllHallexRecords()
{
    // Arrange: Create an instance of HallexDataMigratorSource
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Act: Call ExtractAll directly
    var extractedRecords = await source.ExtractAll(true);

    // Assert: Check that results are not null or empty
    Assert.NotNull(extractedRecords);
    Assert.That(extractedRecords, Is.Not.Empty);

    // Assert: Validate at least one record exists
    var firstRecord = extractedRecords.FirstOrDefault();
    Assert.NotNull(firstRecord);
    Assert.That(firstRecord.AppProfile, Is.EqualTo("HALLEX"));
}





[Test]
public async Task ExtractAll_HA_014_50_001_ShouldReturnAllHallexRecords()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    var mockHtmlDoc = new HtmlDocument(); // Simulate an HTML document
    var expectedRecords = new List<HallexRecordNew>
    {
        new HallexRecordNew { Id = "101", Uname = "record1", AppProfile = "HALLEX" },
        new HallexRecordNew { Id = "102", Uname = "record2", AppProfile = "HALLEX" }
    };

    _mockService.Setup(s => s.GetHallexListDoc())
                .ReturnsAsync(mockHtmlDoc);

    _mockService.Setup(s => s.ExtractAllHallexRecordsFromJson(It.IsAny<HtmlDocument>()))
                .Returns(expectedRecords);

    // Act
    var extractedRecords = await source.ExtractAll(true);

    // Assert
    Assert.NotNull(extractedRecords);
    Assert.That(extractedRecords.Count, Is.EqualTo(2));
    Assert.That(extractedRecords[0].Id, Is.EqualTo("101"));
    Assert.That(extractedRecords[1].Id, Is.EqualTo("102"));
}





[Test]
public async Task Extract_HA_014_40_003_ShouldReturnNull_ForInvalidId()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    string invalidId = "INVALID_ID";

    // Act
    var extracted = await source.Extract(invalidId, true);

    // Assert
    Assert.Null(extracted);
}




[Test]
public async Task Extract_HA_014_40_004_ShouldHandleInvalidJson()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Mocking an invalid JSON response
    _mockService.Setup(s => s.GetHallexListDoc())
                .Throws(new JsonException());

    // Act & Assert
    Assert.ThrowsAsync<JsonException>(async () => await source.Extract(HallexIds.Prod.Active.SomeValidId, true));
}




[Test]
public async Task Extract_HA_014_40_005_ShouldHandleMissingHtmlDocument()
{
    // Arrange
    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Mocking null HTML document
    _mockService.Setup(s => s.GetHallexListDoc()).ReturnsAsync((HtmlDocument)null);

    // Act
    var extracted = await source.Extract(HallexIds.Prod.Active.SomeValidId, true);

    // Assert
    Assert.Null(extracted);
}
anaveen

---

### **2️⃣ Create `HallexDataRepository.cs`**
📌 **Add this new file: `HallexDataRepository.cs`**
```csharp
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PolicyNet.Services.DataMigration.HallexRecords
{
    public class HallexDataRepository : IHallexDataRepository
    {
        private readonly string _dataDirectory;

        public HallexDataRepository(string dataDirectory)
        {
            _dataDirectory = dataDirectory;
        }

        public async Task<HallexRecord?> GetHallexRecordById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty.", nameof(id));

            var filePath = Path.Combine(_dataDirectory, $"{id}.json");

            if (!File.Exists(filePath))
                return null;

            var jsonData = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<HallexRecord>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
```

---

### **3️⃣ Modify `HallexDataMigratorSource.cs`**
📌 **Modify the constructor in `HallexDataMigratorSource.cs`**
```csharp
public class HallexDataMigratorSource
{
    private readonly IHallexDataRepository _repository;

    public HallexDataMigratorSource(IHallexDataRepository repository)
    {
        _repository = repository;
    }

    public async Task<HallexRecord?> Extract(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID cannot be null or empty.", nameof(id));

        return await _repository.GetHallexRecordById(id);
    }
}
```

---

### **4️⃣ Modify `HallexDataMigratorSourceTests.cs`**
📌 **Add these unit test methods**
```csharp
using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;

namespace IntTests.Lib.DataMigration.HallexRecords
{
    [TestFixture]
    public class HallexDataMigratorSourceTests
    {
        private Mock<IHallexDataRepository> _mockRepository;
        private HallexDataMigratorSource _source;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IHallexDataRepository>();
            _source = new HallexDataMigratorSource(_mockRepository.Object);
        }

        [Test]
        public async Task Extract_ValidId_ShouldReturnMockedHallexRecord()
        {
            // Arrange
            var validId = "HA-014-30-020";
            var mockRecord = new HallexRecord
            {
                filename = "HA-014-30-020.json",
                action = "version",
                @type = "section",
                lastUpdated = DateTime.Parse("2025-01-17T07:20:54Z"),
                PolicyNetObjectTypeCode = PolicyNetObjectTypeCodes.HEARINGS_APPEALS_AND_LITIGATION_LAW_LEX_MANUAL
            };

            _mockRepository.Setup(repo => repo.GetHallexRecordById(validId))
                           .ReturnsAsync(mockRecord);

            // Act
            var extracted = await _source.Extract(validId);

            // Assert
            Assert.That(extracted, Is.Not.Null);
            Assert.That(extracted.filename, Is.EqualTo(mockRecord.filename));
        }

        [Test]
        public async Task Extract_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = "INVALID_ID_12345";
            _mockRepository.Setup(repo => repo.GetHallexRecordById(invalidId))
                           .ReturnsAsync((HallexRecord?)null);

            // Act
            var extracted = await _source.Extract(invalidId);

            // Assert
            Assert.That(extracted, Is.Null);
        }

        [Test]
        public void Extract_NullId_ShouldThrowArgumentException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _source.Extract(null));

            Assert.That(ex.Message, Does.Contain("ID cannot be null"));
        }
    }
}
```

---

### **🚀 Next Steps**
1. **Add the new files (`IHallexDataRepository.cs`, `HallexDataRepository.cs`).**  
2. **Modify `HallexDataMigratorSource.cs` to inject `IHallexDataRepository`.**  
3. **Modify `HallexDataMigratorSourceTests.cs` to include new unit tests.**  
4. **Run tests and verify everything works.**
