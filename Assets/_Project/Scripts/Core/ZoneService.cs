public enum ZoneType { Normal, Safe, Super }

public class ZoneService
{
    private readonly int _safeInterval;
    private readonly int _superInterval;

    public int CurrentZone { get; private set; }

    public ZoneService(int safeInterval = 5, int superInterval = 30)
    {
        _safeInterval = safeInterval;
        _superInterval = superInterval;
        CurrentZone = 1;
    }

    public void Advance()
    {
        CurrentZone++;
    }

    public ZoneType GetZoneType(int zone)
    {
        
        if (zone % _superInterval == 0) return ZoneType.Super;
        if (zone % _safeInterval == 0) return ZoneType.Safe;
        return ZoneType.Normal;
    }

    public ZoneType CurrentZoneType => GetZoneType(CurrentZone);
}
