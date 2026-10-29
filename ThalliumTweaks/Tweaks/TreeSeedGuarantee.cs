using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.WildTrees;

namespace ThalliumTweaks.Tweaks
{
    public class TreeSeedGuarantee : ITweak
    {
        private readonly Func<ModConfig> getConfig;

        public TreeSeedGuarantee(Func<ModConfig> getConfig)
        {
            this.getConfig = getConfig;
        }

        public void Enable(IModHelper helper, IMonitor monitor)
        {
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Data/WildTrees"))
                return;

            // Late priority so we run after other mods that also edit tree data
            e.Edit(asset =>
            {
                if (!this.getConfig().TreeSeedGuarantee)
                    return;

                var trees = asset.AsDictionary<string, WildTreeData>().Data;
                foreach (WildTreeData tree in trees.Values)
                    tree.SeedOnChopChance = 1f;
            }, AssetEditPriority.Late);
        }
    }
}