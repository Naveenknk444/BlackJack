### **📌 Commit Message**  
**Commit Title:**  
🔧 **Refactored `HallexDataMigratorSource` to Support Dependency Injection and Added Unit Tests for `Extract` Method**  

**Commit Description:**  
- Introduced **`IHallexDataRepository`** as an abstraction for data retrieval.  
- Modified `HallexDataMigratorSource` to accept `IHallexDataRepository` via **dependency injection**.  
- Created **unit tests** for the `Extract` method using **Moq** to mock data retrieval.  
- Ensured `Extract` correctly handles:  
  - ✅ Valid IDs (returns a fully populated `HallexRecord`).  
  - ✅ Invalid IDs (returns `null`).  
  - ✅ `null` IDs (throws `ArgumentException`).  
- Improved **testability** by removing direct database or API dependencies in unit tests.  

---

### **📝 Simple Explanation for Team Lead**  
We refactored `HallexDataMigratorSource` to make it **more testable** by introducing an **interface (`IHallexDataRepository`)** to handle data retrieval. Instead of `Extract` directly calling a database or API, it now **relies on this interface**, which we can **mock** in unit tests.  

This change allows us to:  
✅ Write **unit tests** without requiring a real database.  
✅ Improve **modularity** by decoupling `Extract` from its data source.  
✅ Ensure `Extract` works **correctly for valid, invalid, and null inputs**.  

Let me know if you need adjustments before pushing the commit! 🚀
