using RimWorld;
using Verse;

namespace AlmightyOlivePatches
{
	/// <summary>
	/// The decorator makes the static constructor get called AFTER defs are loaded.
	/// 
	/// See <a href='https://ludeon.com/forums/index.php?topic=50262.0'>this</a> and <a href='https://ludeon.com/forums/index.php?topic=50275.0'>this</a>
	/// </summary>
	[StaticConstructorOnStartup]
	public static class Startup
	{
		public static AbilityDef convertAbility;

		static Startup()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
                convertAbility = DefDatabase<AbilityDef>.GetNamed("Convert");

				ApplySettingsToDefs();
			});
		}

		/// <summary>
		/// Find the defs and comps and apply the settings
		/// </summary>
		public static void ApplySettingsToDefs()
		{
			if (convertAbility != null)
			{
				CompProperties_AbilityConvert convertComp = (CompProperties_AbilityConvert)convertAbility.comps.Find(x => x is CompProperties_AbilityConvert);
				convertComp.convertPowerFactor = SettingsController.Settings.conversionPower;
			}
		}
	}
}