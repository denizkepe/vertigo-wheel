using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WheelView : MonoBehaviour
{
    [SerializeField] private RectTransform _sliceContainer;
    [SerializeField] private WheelSliceView _slicePrefab;
    [SerializeField] private Image _baseImage;
    [SerializeField] private Sprite _bronzeSprite;
    [SerializeField] private Sprite _silverSprite;
    [SerializeField] private Sprite _goldenSprite;
    [SerializeField] private float _radius = 120f;
    [SerializeField] private float _itemSize = 55f;
    [SerializeField] private float _spinDuration = 3f;
    [SerializeField] private int _extraSpins = 5;

    private readonly List<WheelSliceView> _slices =
        new List<WheelSliceView>();

    private bool _isSpinning;

    public bool IsSpinning => _isSpinning;

    public void Build(IReadOnlyList<WheelSlice> slices, WheelType type)
    {
        for (int i = 0; i < _slices.Count; i++)
            Destroy(_slices[i].gameObject);

        _slices.Clear();

        if (type == WheelType.Silver)
            _baseImage.sprite = _silverSprite;
        else if (type == WheelType.Golden)
            _baseImage.sprite = _goldenSprite;
        else
            _baseImage.sprite = _bronzeSprite;

        int count = slices.Count;

        for (int i = 0; i < count; i++)
        {
            WheelSliceView slice = Instantiate(_slicePrefab, _sliceContainer);
            RectTransform rectTransform = slice.GetComponent<RectTransform>();

            float angleDegrees = i * (360f / count);
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            rectTransform.anchoredPosition =
                new Vector2(Mathf.Sin(angleRadians), Mathf.Cos(angleRadians))
                * _radius;

            rectTransform.localRotation =
                Quaternion.Euler(0f, 0f, -angleDegrees);

            rectTransform.sizeDelta = new Vector2(_itemSize, _itemSize);

            slice.Setup(slices[i]);
            _slices.Add(slice);
        }
    }

    public void SpinTo(int index, System.Action onComplete)
    {
        if (_isSpinning)
            return;

        _isSpinning = true;

        float anglePerSlice = 360f / _slices.Count;
        float currentRotation =
            ((_sliceContainer.localEulerAngles.z % 360f) + 360f) % 360f;

        float delta = (index * anglePerSlice) - currentRotation;

        if (delta < 0f)
            delta += 360f;

        float totalDelta = (_extraSpins * 360f) + delta;

        _sliceContainer
            .DOLocalRotate(
                new Vector3(0f, 0f, totalDelta),
                _spinDuration,
                RotateMode.LocalAxisAdd
            )
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                _isSpinning = false;
                onComplete?.Invoke();
            });
    }
}