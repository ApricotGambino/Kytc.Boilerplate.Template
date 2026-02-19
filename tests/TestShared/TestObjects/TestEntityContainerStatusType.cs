namespace TestShared.TestObjects;

using KernelData.Entities;

// TestEntityContainerStatusType is the Principal (parent) of TestEntityToTestEntityContainer which is the  Dependent (child)
public class TestEntityContainerStatusType : BaseEntity
{
    public required string StatusName { get; set; }


}
