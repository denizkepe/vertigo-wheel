using System.Collections.Generic;   
using UnityEngine;

public enum WheelType { Normal, Silver, Golden }

[System.Serializable]
public class WheelSlice
{
    public RewardDefinition Reward;   
    public int Amount;                
    public float Weight = 1f;         
    public bool IsBomb;               
}



[CreateAssetMenu(fileName = "Wheel_", menuName = "Vertigo/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [SerializeField] private WheelType _type;
    [SerializeField] private List<WheelSlice> _slices = new List<WheelSlice>();

    public WheelType Type => _type;
    public IReadOnlyList<WheelSlice> Slices => _slices;
}