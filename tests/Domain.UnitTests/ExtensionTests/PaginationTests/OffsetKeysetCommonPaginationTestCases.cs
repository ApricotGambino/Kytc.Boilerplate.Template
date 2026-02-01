namespace Domain.UnitTests.ExtensionTests.PaginationTests;
/// <summary>
/// This class contains Parameterized test cases to be used in the pagination tests.  These tests are shared for all types of pagination.
/// </summary>
public static class OffsetKeysetCommonPaginationTestCases
{
    /// <summary>
    /// These cases will provide custom page sizes, to test that no matter what page size we use, we get the results we expect. 
    /// </summary>
    public static readonly object[] CustomPageSizeCases =
    [
        3,1,10,25,50,100
    ];
}

