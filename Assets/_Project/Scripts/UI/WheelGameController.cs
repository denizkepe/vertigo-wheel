using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelGameController : MonoBehaviour
{
    [SerializeField] private WheelView _wheelView;

    [SerializeField] private WheelConfig _normalWheelConfig;
    [SerializeField] private WheelConfig _safeWheelConfig;
    [SerializeField] private WheelConfig _superWheelConfig;

    [SerializeField] private Button _spinButton;
    [SerializeField] private Button _collectButton;
    [SerializeField] private Button _giveUpButton;
    [SerializeField] private Button _reviveButton;

    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private HudView _hud;

    [SerializeField] private float _rewardGrowth = 1.08f;

    private System.Random _random;
    private SpinResolver _spinResolver;
    private RewardWallet _wallet;
    private ZoneService _zones;
    private PlayerProfile _profile;
    private BombWeightPolicy _bombPolicy;
    private RewardScaler _rewardScaler;

    private IReadOnlyList<WheelSlice> _currentSlices;

    private Dictionary<RewardDefinition, int> _bombBackup =
        new Dictionary<RewardDefinition, int>();

    private void Awake()
    {
        _random = new System.Random();

        _spinResolver = new SpinResolver(_random);
        _wallet = new RewardWallet();
        _zones = new ZoneService();
        _profile = new PlayerProfile();
        _bombPolicy = new BombWeightPolicy();
        _rewardScaler = new RewardScaler(_rewardGrowth);

        _gameOverPanel.SetActive(false);

        _hud.Bind(_wallet);
        _hud.SetZone(_zones.CurrentZone);

        BuildWheel();
    }

    private void OnEnable()
    {
        _spinButton.onClick.AddListener(OnSpinClicked);
        _collectButton.onClick.AddListener(OnCollectClicked);
        _giveUpButton.onClick.AddListener(OnGiveUpClicked);
        _reviveButton.onClick.AddListener(OnReviveClicked);
    }

    private void OnDisable()
    {
        _spinButton.onClick.RemoveListener(OnSpinClicked);
        _collectButton.onClick.RemoveListener(OnCollectClicked);
        _giveUpButton.onClick.RemoveListener(OnGiveUpClicked);
        _reviveButton.onClick.RemoveListener(OnReviveClicked);
    }

    private void OnSpinClicked()
    {
        if (_wheelView.IsSpinning)
            return;

        ZoneType zoneType = _zones.CurrentZoneType;
        int zone = _zones.CurrentZone;

        float[] weights;

        if (zoneType == ZoneType.Normal)
        {
            weights = _bombPolicy.GetWeights(
                _currentSlices,
                zone,
                _profile.FailStreak
            );
        }
        else
        {
            weights = new float[_currentSlices.Count];

            for (int i = 0; i < _currentSlices.Count; i++)
                weights[i] = _currentSlices[i].Weight;
        }

        SetButtons(false);

        int selectedIndex = _spinResolver.ResolveByWeights(weights);

        _wheelView.SpinTo(selectedIndex, () =>
        {
            OnSpinComplete(selectedIndex);
        });
    }

    private void OnSpinComplete(int selectedIndex)
    {
        WheelSlice selectedSlice = _currentSlices[selectedIndex];

        if (selectedSlice.IsBomb)
        {
            _bombBackup.Clear();

            foreach (var item in _wallet.Amounts)
                _bombBackup[item.Key] = item.Value;

            _wallet.Clear();
            _profile.RegisterBombFailure();

            _gameOverPanel.SetActive(true);
            return;
        }

        _wallet.Add(selectedSlice.Reward, selectedSlice.Amount);

        _zones.Advance();
        _hud.SetZone(_zones.CurrentZone);

        ZoneType newZoneType = _zones.CurrentZoneType;

        if (newZoneType == ZoneType.Safe || newZoneType == ZoneType.Super)
            _profile.OnSafeZoneReached();

        BuildWheel();
        SetButtons(true);
    }

    private void OnCollectClicked()
    {
        if (_wheelView.IsSpinning)
            return;

        ResetGame(true);
    }

    private void OnGiveUpClicked()
    {
        _bombBackup.Clear();
        _gameOverPanel.SetActive(false);

        ResetGame(false);
    }

    private void OnReviveClicked()
    {
        foreach (var item in _bombBackup)
            _wallet.Add(item.Key, item.Value);

        _bombBackup.Clear();
        _gameOverPanel.SetActive(false);

        BuildWheel();
        SetButtons(true);
    }

    private void ResetGame(bool resetFailStreak)
    {
        _wallet.Clear();
        _zones = new ZoneService();

        if (resetFailStreak)
            _profile.OnSafeZoneReached();

        _hud.SetZone(_zones.CurrentZone);

        BuildWheel();
        SetButtons(true);
    }

    private void BuildWheel()
    {
        ZoneType zoneType = _zones.CurrentZoneType;
        int zone = _zones.CurrentZone;

        _currentSlices = CreateRandomSlices(zoneType, zone);

        _wheelView.Build(
            _currentSlices,
            GetWheelType(zoneType)
        );
    }

    private List<WheelSlice> CreateRandomSlices(
        ZoneType zoneType,
        int zone
    )
    {
        WheelConfig config = GetWheelConfig(zoneType);

        List<WheelSlice> rewardPool = new List<WheelSlice>();
        WheelSlice bombSlice = null;

        for (int i = 0; i < config.Slices.Count; i++)
        {
            WheelSlice slice = config.Slices[i];

            if (slice.IsBomb)
                bombSlice = slice;
            else
                rewardPool.Add(slice);
        }

        List<WheelSlice> newSlices = new List<WheelSlice>();

        int rewardSlotCount = config.Slices.Count;

        if (zoneType == ZoneType.Normal && bombSlice != null)
            rewardSlotCount--;

        for (int i = 0; i < rewardSlotCount; i++)
        {
            WheelSlice reward = rewardPool[
                _random.Next(rewardPool.Count)
            ];

            newSlices.Add(new WheelSlice
            {
                Reward = reward.Reward,
                Amount = _rewardScaler.ScaleAmount(reward.Amount, zone),
                Weight = reward.Weight,
                IsBomb = false
            });
        }

        if (zoneType == ZoneType.Normal && bombSlice != null)
        {
            newSlices.Add(new WheelSlice
            {
                Reward = null,
                Amount = 0,
                Weight = bombSlice.Weight,
                IsBomb = true
            });
        }

        ShuffleSlices(newSlices);

        return newSlices;
    }

    private void ShuffleSlices(List<WheelSlice> slices)
    {
        for (int i = slices.Count - 1; i > 0; i--)
        {
            int randomIndex = _random.Next(i + 1);

            WheelSlice temp = slices[i];
            slices[i] = slices[randomIndex];
            slices[randomIndex] = temp;
        }
    }

    private WheelConfig GetWheelConfig(ZoneType zoneType)
    {
        if (zoneType == ZoneType.Safe)
            return _safeWheelConfig;

        if (zoneType == ZoneType.Super)
            return _superWheelConfig;

        return _normalWheelConfig;
    }

    private WheelType GetWheelType(ZoneType zoneType)
    {
        if (zoneType == ZoneType.Safe)
            return WheelType.Silver;

        if (zoneType == ZoneType.Super)
            return WheelType.Golden;

        return WheelType.Normal;
    }

    private void SetButtons(bool active)
    {
        _spinButton.interactable = active;
        _collectButton.interactable = active;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_spinButton == null)
        {
            GameObject spinObject = GameObject.Find("ui_button_spin");

            if (spinObject != null)
                _spinButton = spinObject.GetComponent<Button>();
        }

        if (_collectButton == null)
        {
            GameObject collectObject = GameObject.Find("ui_button_collect");

            if (collectObject != null)
                _collectButton = collectObject.GetComponent<Button>();
        }

        if (_giveUpButton == null)
        {
            GameObject giveUpObject = GameObject.Find("ui_button_giveup");

            if (giveUpObject != null)
                _giveUpButton = giveUpObject.GetComponent<Button>();
        }

        if (_reviveButton == null)
        {
            GameObject reviveObject = GameObject.Find("ui_button_revive");

            if (reviveObject != null)
                _reviveButton = reviveObject.GetComponent<Button>();
        }
    }
#endif
}