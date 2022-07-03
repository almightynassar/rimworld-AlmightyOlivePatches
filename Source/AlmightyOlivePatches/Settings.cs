using UnityEngine;
using Verse;

namespace AlmightyOlivePatches
{
    public class SettingsData : ModSettings
    {
        /// <summary>
        /// Variable to store the value for the AbilityDef Convert's comp property convertPowerFactor
        /// </summary>
        public float conversionPower = 2f;

        /// <summary>
        /// Writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            Scribe_Values.Look(ref conversionPower, "AOP_Ideology.ConversionPower", 2f);
            
            // Apply the settings once saved to disk 
            Startup.ApplySettingsToDefs();

            // Run the parent function
            base.ExposeData();
        }
    }
    public class SettingsController : Mod
    {
        /// <summary>
        /// Settings object
        /// </summary>
        public static SettingsData Settings;

        /// <summary>
        /// Mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public SettingsController(ModContentPack content) : base(content)
        {
            Settings = GetSettings<SettingsData>();
        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory()
        {
            return "AOP_Main.ModName".Translate();
        }

        /// <summary>
        /// The GUI part to set your settings.
        /// </summary>
        /// <param name="r">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect r)
        {
            // Set up the window
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(r);

            // Conversion Power setting
            string conversionPower_Buffer = Settings.conversionPower.ToString();
            listingStandard.TextFieldNumericLabeled("AOP_Ideology.ConversionPower".Translate(), ref Settings.conversionPower, ref conversionPower_Buffer, 1f, 100f);

            // End the window
            listingStandard.End();
            base.DoSettingsWindowContents(r);
        }
    }
}
