using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace ThalliumTweaks.Tweaks
{
    public class TimeSpeed : ITweak
    {
        private readonly Func<ModConfig> getConfig;

        /// <summary>The game's time counter at the end of the previous tick.</summary>
        private int lastInterval;

        /// <summary>Leftover fraction of a millisecond, so slow speeds stay accurate.</summary>
        private double carry;

        public TimeSpeed(Func<ModConfig> getConfig)
        {
            this.getConfig = getConfig;
        }

        public void Enable(IModHelper helper, IMonitor monitor)
        {
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            int current = Game1.gameTimeInterval;

            // Not in a save, not the host, or tweak turned off: just keep tracking
            if (!Context.IsWorldReady || !Context.IsMainPlayer || !this.getConfig().TimeSpeed)
            {
                this.lastInterval = current;
                this.carry = 0;
                return;
            }

            // How much the game's counter grew this tick (zero or negative when the
            // clock just ticked over, or time is paused by a menu or event)
            int delta = current - this.lastInterval;
            if (delta > 0)
            {
                double speed = this.GetSpeed(Game1.currentLocation);
                double scaled = (delta * speed) + this.carry;
                int newDelta = (int)Math.Floor(scaled);
                this.carry = scaled - newDelta;

                Game1.gameTimeInterval = this.lastInterval + newDelta;
            }

            this.lastInterval = Game1.gameTimeInterval;
        }

        private double GetSpeed(GameLocation? location)
        {
            if (location is null)
                return 1;

            ModConfig config = this.getConfig();

            float speed =
                location is MineShaft ? config.TimeSpeedMines
                : LocationUtils.IsFarmArea(location) ? config.TimeSpeedFarm
                : location.IsOutdoors ? config.TimeSpeedOutdoors
                : config.TimeSpeedIndoors;

            // guards against bad values typed into config.json
            return Math.Clamp(speed, 0f, 10f);
        }
    }
}