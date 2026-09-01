using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Vertigo/Reward Definition")]
public class RewardDefinition : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _baseAmount = 1;
    [SerializeField] private int _minZone = 1;     // bu zone'dan itibaren cikabilir
    [SerializeField] private float _weight = 1f;

    public string Id => _id;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public int BaseAmount => _baseAmount;
    public int MinZone => _minZone;
    public float Weight => _weight;
}