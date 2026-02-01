namespace TestShared.TestObjects;

using Domain.Entities.Common;

public class TestObjectUsingBaseEntity : BaseEntity
{
    public string AString { get; set; } = string.Empty;
    public string AStringWithNumbers { get; set; } = string.Empty;
    public int ANumber { get; set; }
    public bool ABool { get; set; }
    public DateTimeOffset ADateTimeOffset { get; set; }
}
