using Kernel.Data.Entities;

namespace Data.Entities.Example;

//NOTE: This example entity showcases how to create your own entity, and how that flows all the way up through
//the service layer, into an endpoint. You're welcome to remove this whenever you feel you have a grasp on how to do things
//on your own.
//Unless you're certain of what you're doing, you shouldn't modify or add anything to the Kernel section of the source code.
//Which is why following this ExampleEntity is going to represent what you need to do for your application.
public class ExampleEntity : BaseEntity
{
    public string AString { get; set; } = string.Empty;
    public string AStringWithNumbers { get; set; } = string.Empty;
    public int ANumber { get; set; }
    public bool ABool { get; set; }
    public DateTimeOffset ADateTimeOffset { get; set; }
}
