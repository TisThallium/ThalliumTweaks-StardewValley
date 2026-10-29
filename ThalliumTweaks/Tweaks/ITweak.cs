using StardewModdingAPI;

namespace ThalliumTweaks.Tweaks
{
    public interface ITweak
    {
        /// <summary>Hook up this tweak's event handlers.</summary>
        void Enable(IModHelper helper, IMonitor monitor);
    }
}