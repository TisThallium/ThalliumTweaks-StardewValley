using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;

namespace ThalliumTweaks.Tweaks
{
    public class PetBowlAutoFill : ITweak
    {
        private readonly Func<ModConfig> getConfig;
        private IMonitor monitor = null!;

        public PetBowlAutoFill(Func<ModConfig> getConfig)
        {
            this.getConfig = getConfig;
        }

        public void Enable(IModHelper helper, IMonitor monitor)
        {
            this.monitor = monitor;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            if (!this.getConfig().PetBowlAutoFill)
                return;

            // in multiplayer, only the host changes shared world state
            if (!Context.IsMainPlayer)
                return;

            foreach (Building building in Game1.getFarm().buildings)
            {
                if (building is PetBowl bowl && !bowl.watered.Value)
                {
                    bowl.watered.Value = true;
                    this.monitor.Log("Filled the pet bowl.", LogLevel.Debug);
                }
            }
        }
    }
}