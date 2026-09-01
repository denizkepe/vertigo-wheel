using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSliceView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Sprite _bombSprite;

    public void Setup(WheelSlice slice)
    {
        Sprite icon = slice.IsBomb
            ? _bombSprite
            : (slice.Reward != null ? slice.Reward.Icon : null);

        _iconImage.enabled = (icon != null);
        _iconImage.sprite = icon;
        _iconImage.preserveAspect = true;   
        _amountText.text = slice.IsBomb ? "" : ("x" + slice.Amount);
    }
}
