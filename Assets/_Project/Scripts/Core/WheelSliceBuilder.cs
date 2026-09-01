using System.Collections.Generic;

public class WheelSliceBuilder
{
    private readonly System.Random _random;
    private readonly RewardScaler _scaler;

    public WheelSliceBuilder(System.Random random, RewardScaler scaler)
    {
        _random = random;
        _scaler = scaler;
    }

    public List<WheelSlice> Build(RewardPool pool, int zone, bool includeBomb, int slotCount)
    {
        // Bu zone için uygun oduller
        var eligible = new List<RewardDefinition>();
        foreach (var r in pool.Rewards)
            if (r.MinZone <= zone) eligible.Add(r);

        var slices = new List<WheelSlice>();
        int rewardSlots = includeBomb ? slotCount - 1 : slotCount;

        for (int i = 0; i < rewardSlots; i++)
        {
            var reward = eligible[_random.Next(eligible.Count)];   // rastgele bir odul
            slices.Add(new WheelSlice
            {
                Reward = reward,
                Amount = _scaler.ScaleAmount(reward.BaseAmount, zone),
                Weight = reward.Weight,
                IsBomb = false
            });
        }

        if (includeBomb)
            slices.Add(new WheelSlice { IsBomb = true, Weight = 1f });

        return slices;
    }
}