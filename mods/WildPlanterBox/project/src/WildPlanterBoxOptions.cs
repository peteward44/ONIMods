using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace PW
{
	[ModInfo("https://github.com/peteward44/ONIMods", "preview.png", collapse: true)]
	[ConfigFile(SharedConfigLocation: true)]
	[JsonObject(MemberSerialization.OptIn)]
	[RestartRequired]
	public sealed class WildPlanterBoxOptions : SingletonOptions<WildPlanterBoxOptions>
	{
		[Option("Power Consumption (Watts)", "How much power each wild plant tile, specify zero for no power consumption", "")]
		[Limit(0, 240)]
		[JsonProperty]
		public float PowerConsumption { get; set; } = 0.0f;

		[Option("Heat Output (kDTU/s)", "How much heat each tile outputs, in kDTU/s", "")]
		[Limit(0, 2)]
		[JsonProperty]
		public float HeatOutput { get; set; } = 0.0f;

		public WildPlanterBoxOptions() { }
	}
}
