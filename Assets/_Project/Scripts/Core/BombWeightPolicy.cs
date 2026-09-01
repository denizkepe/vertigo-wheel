using System.Collections.Generic;

public class BombWeightPolicy
{
    private readonly int _safeInterval;
    private readonly float _maxBombMultiplier;  
    private readonly float _spikeExponent;      
    private readonly float _mercyPerFail;
    private readonly float _minMercy;

    public BombWeightPolicy(int safeInterval = 5, float maxBombMultiplier = 20f,
                            float spikeExponent = 3f, float mercyPerFail = 0.15f, float minMercy = 0.3f)
    {
        _safeInterval = safeInterval;
        _maxBombMultiplier = maxBombMultiplier;
        _spikeExponent = spikeExponent;
        _mercyPerFail = mercyPerFail;
        _minMercy = minMercy;
    }

    // gerilim: erken zone'lar düsük, safe/super'e son adimda buyuk spike
    public float GetZoneMultiplier(int zone)
    {
        int position = zone % _safeInterval;
        if (position == 0) return 1f;

        float t = (float)position / (_safeInterval - 1);
        float curved = (float)System.Math.Pow(t, _spikeExponent);
        return 1f + (_maxBombMultiplier - 1f) * curved;
    }

    // merh: art arda basarisizlik arttikca 1 -> taban azalir
    public float GetMercyMultiplier(int failStreak)
    {
        float mercy = 1f - failStreak * _mercyPerFail;
        return mercy < _minMercy ? _minMercy : mercy;
    }

    // gerilim x merhamet
    public float GetBombMultiplier(int zone, int failStreak)
    {
        return GetZoneMultiplier(zone) * GetMercyMultiplier(failStreak);
    }

    public float[] GetWeights(IReadOnlyList<WheelSlice> slices, int zone, int failStreak)
    {
        float multiplier = GetBombMultiplier(zone, failStreak);
        var weights = new float[slices.Count];
        for (int i = 0; i < slices.Count; i++)
        {
            weights[i] = slices[i].Weight;
            if (slices[i].IsBomb)
                weights[i] *= multiplier;
        }
        return weights;
    }
}