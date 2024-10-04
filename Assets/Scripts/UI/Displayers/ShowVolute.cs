using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ShowVolute : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private bool _playAnimation;
    [SerializeField, ShowIf(nameof(_playAnimation))] private float _animScale = 1.3F;
    [SerializeField, ShowIf(nameof(_playAnimation))] private float _animTime = 0.5F;
    private void UpdateVolute(int money)
    {
        _moneyText.text = money.ToString();
        Animation();
    }
    private void Start() => BankVolute.OnMoneyValueChanged += UpdateVolute;
    private void OnDestroy() => BankVolute.OnMoneyValueChanged -= UpdateVolute;
    private void Animation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_moneyText.transform.DOScale(_animScale, _animTime / 2));
        sequence.Append(_moneyText.transform.DOScale(1, _animTime / 2));
        sequence.Play();
    }
}
