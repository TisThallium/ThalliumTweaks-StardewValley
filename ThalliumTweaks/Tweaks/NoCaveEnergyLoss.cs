using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace ThalliumTweaks.Tweaks
{
    public class NoCaveEnergyLoss : ITweak
    {
        private readonly Func<ModConfig> getConfig;
        private float lastStamina;

        public NoCaveEnergyLoss(Func<ModConfig> getConfig)
        {
            this.getConfig = getConfig;
        }

        public void Enable(IModHelper helper, IMonitor monitor)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            Farmer player = Game1.player;
            bool inCave = Game1.currentLocation is MineShaft;

            if (this.getConfig().NoCaveEnergyLoss && inCave && player.Stamina < this.lastStamina)
                player.Stamina = this.lastStamina;

            // always track energy, even when the tweak is off, so toggling it on mid-game works correctly
            this.lastStamina = player.Stamina;
        }
    }
}