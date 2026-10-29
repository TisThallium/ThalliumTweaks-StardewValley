using StardewModdingAPI;
using StardewModdingAPI.Events;
using ThalliumTweaks.Tweaks;

namespace ThalliumTweaks
{
    public class ModEntry : Mod
    {
        private ModConfig Config = new();

        public override void Entry(IModHelper helper)
        {
            // load config.json (SMAPI creates it with defaults on first run)
            this.Config = helper.ReadConfig<ModConfig>();

            ITweak[] tweaks =
            {
                // all tweaks go here
                new NoCaveEnergyLoss(() => this.Config),
                new PetBowlAutoFill(() => this.Config),
                new TreeSeedGuarantee(() => this.Config)
            };

            foreach (ITweak tweak in tweaks)
                tweak.Enable(helper, this.Monitor);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;

            this.Monitor.Log("Thallium Tweaks loaded!", LogLevel.Info);
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (null if the player doesn't have it installed)
            var gmcm = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm is null)
                return;

            // register this mod in the menu
            gmcm.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () =>
                {
                    this.Helper.WriteConfig(this.Config);
                    // make the game reload data that the tweaks edit
                    this.Helper.GameContent.InvalidateCache("Data/WildTrees");
                }
            );

            gmcm.AddSectionTitle(this.ModManifest, () => "Tweaks");

            gmcm.AddBoolOption(
                mod: this.ModManifest,
                getValue: () => this.Config.NoCaveEnergyLoss,
                setValue: value => this.Config.NoCaveEnergyLoss = value,
                name: () => "No Cave Energy Loss",
                tooltip: () => "You don't lose energy while inside the Mines or Skull Cavern."
            );

            gmcm.AddBoolOption(
                mod: this.ModManifest,
                getValue: () => this.Config.PetBowlAutoFill,
                setValue: value => this.Config.PetBowlAutoFill = value,
                name: () => "Pet Bowl Auto-Fill",
                tooltip: () => "The pet bowl is automatically filled each day."
            );

            gmcm.AddBoolOption(
                mod: this.ModManifest,
                getValue: () => this.Config.TreeSeedGuarantee,
                setValue: value => this.Config.TreeSeedGuarantee = value,
                name: () => "Tree Seed Guarantee",
                tooltip: () => "Chopping down a fully grown tree always drops a seed."
            );
        }
    }
}