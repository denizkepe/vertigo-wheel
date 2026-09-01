using System.Collections.Generic;

public class SpinResolver
{
    private readonly System.Random _random;

    public SpinResolver(System.Random random)
    {
        _random = random;
    }

    // Dilimlerin kendi agirliklariyla secer 
    public int ResolveSliceIndex(IReadOnlyList<WheelSlice> slices)
    {
        var weights = new float[slices.Count];
        for (int i = 0; i < slices.Count; i++)
            weights[i] = slices[i].Weight;
        return ResolveByWeights(weights);
    }

    // Dogrudan verilen agirliklardan secer (dinamik agirliklar icin)
    public int ResolveByWeights(IReadOnlyList<float> weights)
    {
        float total = 0f;
        for (int i = 0; i < weights.Count; i++)
            total += weights[i];

        float roll = (float)(_random.NextDouble() * total);

        float cumulative = 0f;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (roll < cumulative)
                return i;
        }
        return weights.Count - 1;
    }
}
