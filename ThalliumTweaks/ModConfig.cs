namespace ThalliumTweaks
{
    public class ModConfig
    {
        public bool NoCaveEnergyLoss { get; set; } = true;
        public bool PetBowlAutoFill { get; set; } = true;
        public bool TreeSeedGuarantee { get; set; } = true;

        // Time configuration
        public bool TimeSpeed { get; set; } = true;
        public float TimeSpeedFarm { get; set; } = 1.0f;
        public float TimeSpeedMines { get; set; } = 1.0f;
        public float TimeSpeedOutdoors { get; set; } = 1.0f;
        public float TimeSpeedIndoors { get; set; } = 1.0f;
    }
}