using StardewValley;
using StardewValley.Locations;

namespace ThalliumTweaks
{
    public static class LocationUtils
    {
        /// <summary>Whether a location is the farm, or a building or room that belongs to it.</summary>
        public static bool IsFarmArea(GameLocation? location)
        {
            if (location is null)
                return false;

            return location.IsFarm
                || location is FarmHouse   // includes cabins
                || location is Cellar
                || location.IsGreenhouse
                || location.ParentBuilding != null;   // barns, coops, sheds, etc.
        }
    }
}