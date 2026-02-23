// Log.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : error

namespace KernelData.Entities.Kernel;

public class Log : BaseEntity
{
	public required string Message { get; set; }
	public required string MessageTemplate { get; set; }
	public required string Level { get; set; }
	public DateTime TimeStamp { get; set; }
	public string? Exception { get; set; }
	public string? Properties { get; set; }
	public string? LogEvent { get; set; }
}
