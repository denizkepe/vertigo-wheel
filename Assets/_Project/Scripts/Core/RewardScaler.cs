public class RewardScaler
{
    private readonly float _growthPerZone;

    public RewardScaler(float growthPerZone = 1.3f)
    {
        _growthPerZone = growthPerZone;
    }

    public int ScaleAmount(int baseAmount, int zone)
    {
        double scaled = baseAmount * System.Math.Pow(_growthPerZone, zone - 1);
        return (int)System.Math.Round(scaled);
    }
}