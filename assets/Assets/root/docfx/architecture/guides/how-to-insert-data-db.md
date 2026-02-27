# How to insert data into the database

Because I've never heard of a read-only database...[wait](how-to-fetch-data-db.md)

---

## Inserting isn't interesting

For the most part, there's really no difference with this project and almost any other project regarding data insertion.

There's a few interesting things that happen in the background when entities are created/updated/deleted, but that's covered in details in the [auditable entity](../articles/auditable-entity.md) article.

For the most part, you're going to be inserting data like this:

```csharp
var studentToAdd = new Student() { Name = "Mr. Big Brain" };

dbContext.Students.Add(studentToAdd);
dbContext.SaveChanges();
```

There's a few more interesting things you can do, like one-to-many insertions:

```csharp
//Adding an entity the traditional way.
var msFrizzle = new Teacher() { Name = "Ms. Frizzle" };
db.Teachers.Add(msFrizzle);
db.SaveChanges();

var firstCourse = new Course() { Name = "Exploring Uranus", TeacherId = msFrizzle.Id };
var secondCourse = new Course() { Name = "Germ Theory", TeacherId = msFrizzle.Id };
db.Courses.Add(firstCourse);
db.Courses.Add(secondCourse);
db.SaveChanges();
```

```csharp
//Adding by related data.
var msFrizzle = new Teacher() { Name = "Ms. Frizzle" };
var firstCourse = new Course() { Name = "Exploring Uranus", Teacher = msFrizzle };
var secondCourse = new Course() { Name = "Germ Theory", Teacher = msFrizzle };

db.Teachers.Add(msFrizzle);
db.SaveChanges();
```

There are more examples covered in `tests\Sandbox\Concept Demos\HowDoIAddData\HowDoIAddData.cs`

## General Guidance

Nothing is absolute, but in general:

- We generally do not delete data. Delete methods tend to just set the `IsSoftDeleted` property of the entity to `true`
- When adding data, or interacting at all with the db really, prefer `async` methods such as `db.Entity.AddAsync();`
- `addAsync()` vs `addRangeAsync()` are nearly identical in performance (see: `tests\Benchmarks\Tests\EFCoreAddBenchmarks.cs`)
- Audit columns that are on the `BaseEntity` class like `CreatedDate, Version, etc..`  are automatically managed.

## Additional Resources

### Documentation

- [How To Fetch Data Db](how-to-fetch-data-db.md)
- (../articles/ef-relationships.md)
- [Benchmark Testing](../articles/benchmark-testing.md)
- [ReadOnly Repo](xref:KernelInfrastructure.Repositories.ReadOnlyEntityRepo`2) <!-- markdownlint-disable-line MD061 -->
- [Microsoft's documentation](https://learn.microsoft.com/en-us/ef/core/saving/related-data)

### Code Examples

- `\tests\Sandbox\Concept Demos\HowDoIGetData\HowDoIAddData.cs`
- `\tests\Sandbox\Concept Demos\EFRelationship\EFRelationship.cs`
- `\tests\Sandbox\Concept Demos\HowDoIAddData\HowDoIAddData.cs`
- `\tests\KernelTests\Kernel.Integration.Tests\RepositoryTests\ReadOnlyEntityRepoTests.cs`
- `\tests\KernelTests\Kernel.Infrastructure.Unit.Tests\ExtensionTests\PaginationExtensionTests.cs`
