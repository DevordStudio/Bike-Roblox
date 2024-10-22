using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ShowVolute : MonoBehaviour
{
    [SerializeField] private BankVolute _bank;
    [SerializeField, HideIf(nameof(_isMultiple))] private TMP_Text _moneyText;
    [SerializeField] private bool _playAnimation;
    [Tooltip("Обновляет ли данный скрипт лишь один текст или сразу несколько?")]
    [SerializeField] private bool _isMultiple;
    [SerializeField, ShowIf(nameof(_isMultiple))] private TMP_Text[] _moneyTexts;
    [SerializeField, ShowIf(nameof(_playAnimation))] private float _animScale = 1.3F;
    [SerializeField, ShowIf(nameof(_playAnimation))] private float _animTime = 0.5F;
    [SerializeField] private GameObject[] _glows;
    [SerializeField] private Color _boostedColor;
    private void UpdateVolute()
    {
        if (_isMultiple)
        {
            foreach (var item in _moneyTexts)
            {
                item.text = _bank.GetMoney().ToString();
                Animation(item.transform);
            }
        }
        else
        {
            _moneyText.text = _bank.GetMoney().ToString();
            Animation(_moneyText.transform);
        }
    }
    public void UpdateBoost(bool active)
    {
        if (active)
        {
            if (_isMultiple)
            {
                foreach (var item in _moneyTexts)
                    item.color = _boostedColor;
            }
            else _moneyText.color = _boostedColor;
        }
        else
        {
            if (_isMultiple)
            {
                foreach (var item in _moneyTexts)
                    item.color = Color.white;
            }
            else _moneyText.color = Color.white;
        }
        foreach (var item in _glows)
            item.SetActive(active);
    }
    private void Start()
    {
        BankVolute.OnMoneyValueChanged += UpdateVolute;
        MoneyBoost.OnBoostChanged += UpdateBoost;
        UpdateVolute();
    }
    private void OnDestroy()
    {
        BankVolute.OnMoneyValueChanged -= UpdateVolute;
        MoneyBoost.OnBoostChanged -= UpdateBoost;
    }
    private void Animation(Transform trans)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(trans.DOScale(_animScale, _animTime / 2));
        sequence.Append(trans.DOScale(1, _animTime / 2));
        sequence.Play();
    }
}
