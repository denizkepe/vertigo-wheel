using System.Collections.Generic;

public class RewardWallet
{
    private readonly Dictionary<RewardDefinition, int> _amounts = new Dictionary<RewardDefinition, int>();

    // observer pattern
    public event System.Action Changed;

    public IReadOnlyDictionary<RewardDefinition, int> Amounts => _amounts;

    public void Add(RewardDefinition reward, int amount)
    {
        if (reward == null || amount <= 0) return;

        if (_amounts.ContainsKey(reward))
            _amounts[reward] += amount;
        else
            _amounts[reward] = amount;

        Changed?.Invoke();   
    }

    public void Clear()
    {
        _amounts.Clear();
        Changed?.Invoke();
    }

    public int GetAmount(RewardDefinition reward)
    {
        return _amounts.TryGetValue(reward, out int amount) ? amount : 0;
    }

    public int TotalItems()
    {
        int total = 0;
        foreach (var pair in _amounts)
            total += pair.Value;
        return total;
    }
}
