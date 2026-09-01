using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudRewardItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _count;

    public void Setup(Sprite icon, int count)
    {
        _icon.sprite = icon;
        _icon.preserveAspect = true;   
        _count.text = "x" + count;
    }
}