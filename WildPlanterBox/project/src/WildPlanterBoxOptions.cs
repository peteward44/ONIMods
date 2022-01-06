using Newtonsoft.Json;
using PeterHan.PLib.Options;

[ModInfo("https://github.com/peteward44/ONIMods", "preview.png", collapse: true)]
[ConfigFile(SharedConfigLocation: true)]
[JsonObject(MemberSerialization.OptIn)]
[RestartRequired]
public sealed class WildPlanterBoxOptions : SingletonOptions<WildPlanterBoxOptions> {
	[Option("Power Consumption", "How much power each wild plant tile, specify zero for no power consumption", "")]
	[JsonProperty]
	public float PowerConsumption { get; set; } = 30.0f;

	[Option("Heat Output", "How much heat each tile outputs, in DTU/s", "")]
	[JsonProperty]
	public float HeatOutput { get; set; } = 0.25f;

	public WildPlanterBoxOptions() { }
}
