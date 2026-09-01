public class PlayerProfile
{
    // Art arda kac kez bombaya basip safe zone'a ulasamadi
    public int FailStreak { get; private set; }

    public void RegisterBombFailure()
    {
        FailStreak++;
    }

    public void OnSafeZoneReached()
    {
        FailStreak = 0;   // safe zone'a ulasildi -> sifirla
    }
}