using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.LocationContexts;

namespace ThalliumTweaks.Tweaks
{
    public class NoPassOutPenaltyOnFarm : ITweak
    {
        private readonly Func<ModConfig> getConfig;
        private IModHelper helper = null!;

        /// <summary>Whether we're currently waiving the fee (the player is in a farm area).</summary>
        private bool waiveFee;

        public NoPassOutPenaltyOnFarm(Func<ModConfig> getConfig)
        {
            this.getConfig = getConfig;
        }

        public void Enable(IModHelper helper, IMonitor monitor)
        {
            this.helper = helper;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            // only check four times a second
            if (!e.IsMultipleOf(15))
                return;

            bool shouldWaive =
                Context.IsWorldReady
                && this.getConfig().NoPassOutPenaltyOnFarm
                && LocationUtils.IsFarmArea(Game1.currentLocation);

            if (shouldWaive == this.waiveFee)
                return;

            this.waiveFee = shouldWaive;

            // make the game reload the data so our edit turns on or off
            this.helper.GameContent.InvalidateCache("Data/LocationContexts");
        }

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Data/LocationContexts"))
                return;

            e.Edit(asset =>
            {
                if (!this.waiveFee)
                    return;

                var contexts = asset.AsDictionary<string, LocationContextData>().Data;
                foreach (LocationContextData context in contexts.Values)
                    context.MaxPassOutCost = 0;
            }, AssetEditPriority.Late);
        }
    }
}