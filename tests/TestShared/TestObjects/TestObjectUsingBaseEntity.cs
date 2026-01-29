namespace TestShared.TestObjects;

using Domain.Entities.Common;

public class TestObjectUsingBaseEntity : BaseEntity
{
    public required string AString { get; set; }
    public required int ANumber { get; set; }
    public required bool ABool { get; set; }
}