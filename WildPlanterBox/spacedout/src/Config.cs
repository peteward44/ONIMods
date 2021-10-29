using Newtonsoft.Json;

public class Config
{
	[JsonProperty]
	public float PowerConsumption { get; set; } = 1;
	
	[JsonProperty]
	public float HeatOutput { get; set; } = 1;
}
