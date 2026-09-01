using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour
{
    [SerializeField] private Transform _rewardContainer;
    [SerializeField] private HudRewardItem _itemPrefab;
    [SerializeField] private TextMeshProUGUI _zoneText;
    [SerializeField] private GameObject _collectedLabel;

    private RewardWallet _wallet;
    private List<HudRewardItem> _items = new List<HudRewardItem>();

    public void Bind(RewardWallet wallet)
    {
        _wallet = wallet;
        _wallet.Changed += Refresh;

        Refresh();
    }

    private void OnDestroy()
    {
        if (_wallet != null)
            _wallet.Changed -= Refresh;
    }

    public void SetZone(int zone)
    {
        if (_zoneText != null)
            _zoneText.text = "ZONE " + zone;
    }

    private void Refresh()
    {
        for (int i = 0; i < _items.Count; i++)
            Destroy(_items[i].gameObject);

        _items.Clear();

        if (_collectedLabel != null)
            _collectedLabel.SetActive(false);

        foreach (var item in _wallet.Amounts)
        {
            HudRewardItem rewardItem = Instantiate(
                _itemPrefab,
                _rewardContainer
            );

            rewardItem.Setup(item.Key.Icon, item.Value);
            _items.Add(rewardItem);
        }

        if (_collectedLabel != null && _items.Count > 0)
            _collectedLabel.SetActive(true);

        Canvas.ForceUpdateCanvases();

        RectTransform rewardRect = _rewardContainer as RectTransform;
        RectTransform panelRect = _rewardContainer.parent as RectTransform;

        if (rewardRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rewardRect);

        if (panelRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
    }
}