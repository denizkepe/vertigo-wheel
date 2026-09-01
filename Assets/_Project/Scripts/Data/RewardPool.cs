using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardPool", menuName = "Vertigo/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private List<RewardDefinition> _rewards = new List<RewardDefinition>();
    public IReadOnlyList<RewardDefinition> Rewards => _rewards;
}