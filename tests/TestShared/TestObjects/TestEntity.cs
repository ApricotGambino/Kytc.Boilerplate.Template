namespace TestShared.TestObjects;

using KernelData.Entities;

public class TestEntity : BaseEntity
{
    public string AString { get; set; } = string.Empty;
    public string AStringWithNumbers { get; set; } = string.Empty;
    public int ANumber { get; set; }
    public bool ABool { get; set; }
    public DateTimeOffset ADateTimeOffset { get; set; }
}

public static class TestEntityHelper
{
    public static List<TestEntity> CreateTestEntityList(int numberOfRecordsToCreate)
    {
        var now = DateTime.Now;
        var listOfObjects = new List<TestEntity>();

        for (var i = 1; i <= numberOfRecordsToCreate; i++)
        {
            listOfObjects.Add(new TestEntity
            {
                //The 'AString' code is stole from here: https://codereview.stackexchange.com/questions/148506/incrementing-a-sequence-of-letters-by-one
                //It just builds up a string like A->B->C...X->Y->Z->AA->BB->CC etc.
                //So that we can actually have unique values we can test order with. 
                AString = new string((char)('A' + ((i - 1) % 26)), ((i - 1) / 26) + 1),
                AStringWithNumbers = $"Test String {i}",
                ANumber = i + 100,
                ABool = i % 2 == 0,
                ADateTimeOffset = now.AddDays(i - numberOfRecordsToCreate)
            });
        }

        return listOfObjects;
    }
}
