

### **Creating More Integration Tests for `Extract` and Related Methods**
Now that we are focusing entirely on **integration tests**, we will:
✅ Use a **real database** (SQLite or an actual test database).  
✅ Ensure **data is inserted, retrieved, updated, and reloaded correctly**.  
✅ Validate **end-to-end behavior**, including persistence and data integrity.  

---

## **🛠️ Integration Test Strategy**
We will add tests for:
1. **Extracting a valid record** (`Extract_ValidId_ShouldReturnRecord`).
2. **Extracting all records** (`ExtractAll_ShouldReturnAllRecords`).
3. **Modifying an extracted record and verifying persistence** (`ExtractAndUpdate_ShouldSaveChanges`).
4. **Ensuring that data reloads correctly from an external source** (`ExtractAndReload_ShouldRestoreOriginalData`).
5. **Performance validation for large datasets** (`ExtractAll_LargeDataset_ShouldNotTimeout`).

---

## **1️⃣ Extract a Valid Record**
#### **Purpose:**
- Ensures `Extract` correctly retrieves a **real record from the database**.

```csharp
[Test]
public async Task Extract_ValidId_ShouldReturnRecord()
{
    // Arrange
    var dbFactory = TestSetup.GetNewSqlitePolicyNetDbContextFactory();
    var db = dbFactory.CreateDbContext();
    db.Database.Migrate(); // Ensure the database is set up

    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-014-30-020"; // ID of a real test record

    // Act
    var extracted = await source.Extract(validId);

    // Assert
    Assert.That(extracted, Is.Not.Null, "Extracted record should not be null.");
    Assert.Multiple(() =>
    {
        Assert.That(extracted.filename, Is.Not.Null.And.Not.Empty, "Filename should not be null or empty.");
        Assert.That(extracted.action, Is.EqualTo("version"), "Action should match expected value.");
        Assert.That(extracted.@type, Is.EqualTo("section"), "Type should match expected value.");
        Assert.That(extracted.lastUpdated, Is.GreaterThan(DateTime.MinValue), "LastUpdated should have a valid timestamp.");
    });
}
```
✅ **Uses a real database** (`db.Database.Migrate()`).  
✅ **Extracts a real record and validates its fields**.  

---

## **2️⃣ Extract All Records**
#### **Purpose:**
- Ensures `ExtractAll` retrieves **all available records**.

```csharp
[Test]
public async Task ExtractAll_ShouldReturnAllRecords()
{
    // Arrange
    var dbFactory = TestSetup.GetNewSqlitePolicyNetDbContextFactory();
    var db = dbFactory.CreateDbContext();
    db.Database.Migrate(); // Ensure database is initialized

    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);

    // Act
    var extractedRecords = await source.ExtractAll(CancellationToken.None);

    // Assert
    Assert.That(extractedRecords, Is.Not.Null, "Extracted records should not be null.");
    Assert.That(extractedRecords.Count(), Is.GreaterThan(0), "Extracted records list should not be empty.");
}
```
✅ **Ensures multiple records are retrieved.**  
✅ **Confirms that the database is returning data correctly.**  

---

## **3️⃣ Modify an Extracted Record and Save Changes**
#### **Purpose:**
- Ensures that changes to extracted records **persist in the database**.

```csharp
[Test]
public async Task ExtractAndUpdate_ShouldSaveChanges()
{
    // Arrange
    var dbFactory = TestSetup.GetNewSqlitePolicyNetDbContextFactory();
    var db = dbFactory.CreateDbContext();
    db.Database.Migrate(); 

    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-014-30-020";

    // Act
    var extracted = await source.Extract(validId);
    Assert.That(extracted, Is.Not.Null, "Extracted record should not be null.");

    // Modify the record
    extracted.filename = "UpdatedFilename.json";
    db.SaveChanges();
    db.ChangeTracker.Clear(); // Clear tracking to force reload

    // Retrieve again from database
    var updatedRecord = await source.Extract(validId);

    // Assert
    Assert.That(updatedRecord.filename, Is.EqualTo("UpdatedFilename.json"), "Filename update should be persisted.");
}
```
✅ **Verifies that updates persist in the database.**  
✅ **Ensures changes are saved and reloaded correctly.**  

---

## **4️⃣ Reload Data from External Source**
#### **Purpose:**
- Ensures that data **reloads correctly from an external source**.

```csharp
[Test]
public async Task ExtractAndReload_ShouldRestoreOriginalData()
{
    // Arrange
    var dbFactory = TestSetup.GetNewSqlitePolicyNetDbContextFactory();
    var db = dbFactory.CreateDbContext();
    db.Database.Migrate(); 

    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var validId = "HA-014-30-020";

    // Act - Extract and modify
    var extracted = await source.Extract(validId);
    Assert.That(extracted, Is.Not.Null, "Extracted record should not be null.");

    var originalFilename = extracted.filename;
    extracted.filename = "TemporaryChange.json";
    db.SaveChanges();
    db.ChangeTracker.Clear();

    // Reload the record from the external source
    var taskManager = new PolicyNetTaskManager(new PpsOwnerDbContextFactory(), dbFactory, new CommunicationDbContextFactory(), null, null, null, PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave);
    extracted.ReloadFromSource(taskManager).Wait();
    db.ChangeTracker.Clear();

    var reloadedRecord = await source.Extract(validId);

    // Assert
    Assert.That(reloadedRecord.filename, Is.EqualTo(originalFilename), "Reload should restore the original filename.");
}
```
✅ **Ensures that data reloads correctly from the external source.**  
✅ **Verifies that temporary modifications are reverted when reloaded.**  

---

## **5️⃣ Extracting a Large Dataset**
#### **Purpose:**
- Ensures that `ExtractAll` **can handle large datasets efficiently**.

```csharp
[Test]
public async Task ExtractAll_LargeDataset_ShouldNotTimeout()
{
    // Arrange
    var dbFactory = TestSetup.GetNewSqlitePolicyNetDbContextFactory();
    var db = dbFactory.CreateDbContext();
    db.Database.Migrate(); 

    var source = new HallexDataMigratorSource(PolicyNetHtmlOrigin.LegacyWebsiteWithLocalAutosave, null);
    var stopwatch = new System.Diagnostics.Stopwatch();

    // Act
    stopwatch.Start();
    var extractedRecords = await source.ExtractAll(CancellationToken.None);
    stopwatch.Stop();

    // Assert
    Assert.That(extractedRecords.Count(), Is.GreaterThan(500), "Extracted records should be more than 500 for large dataset testing.");
    Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(10000), "ExtractAll took too long to execute.");
}
```
✅ **Ensures that `ExtractAll` does not cause timeouts.**  
✅ **Verifies performance for large datasets.**  

---

## **🛠️ Key Takeaways**
✅ **Uses a real database (`SQLite`) instead of mocks.**  
✅ **Covers data retrieval (`Extract` and `ExtractAll`).**  
✅ **Validates updates and reload behavior.**  
✅ **Ensures performance and scalability.**  

---

## **🚀 Next Steps**
1. **Add these integration tests to `HallexDataMigratorSourceTests`.**
2. **Run them and confirm:**  
   - ✅ Do valid records extract properly?  
   - ✅ Do updates persist in the database?  
   - ✅ Does data reload from the external source?  
   - ✅ Can `ExtractAll` handle large datasets efficiently?  
3. **Let me know once completed, and we can add more tests if needed!** 🚀
