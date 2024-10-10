using NaughtyAttributes;
using TMPro;
using UnityEngine;
using YG;

public class RewardedAd : MonoBehaviour
{
    [SerializeField] private int _moneyForAd = 1000;
    [SerializeField] private RewardIncreaseType _rewardIncreaseType;
    [SerializeField, ShowIf(nameof(IsAddition))] private int _rewardIncreaseValue = 1000;
    [SerializeField, ShowIf(nameof(IsMultiplication))] private float _rewardMultiplier = 2F;
    [SerializeField, HideIf(nameof(IsNone))] private int _maxRewardValue = 100000;
    [SerializeField] private BankVolute _bank;
    [SerializeField] private TMP_Text _textReward;
    private enum RewardIncreaseType
    {
        None,
        Addition,
        Multiplication
    }
    #region BoolMarks
    private bool IsNone()
    {
        return _rewardIncreaseType == RewardIncreaseType.None;
    }
    private bool IsAddition()
    {
        return _rewardIncreaseType == RewardIncreaseType.Addition;
    }
    private bool IsMultiplication()
    {
        return _rewardIncreaseType == RewardIncreaseType.Multiplication;
    }
    #endregion
    private void Start()
    {
        YandexGame.RewardVideoEvent += GetRewarded;
        UpdateText();
    }
    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= GetRewarded;
    }
    private void GetRewarded(int id)
    {
        _bank.IncreaseMoney(_moneyForAd);
        if (IsAddition() && _moneyForAd + _rewardIncreaseValue <= _maxRewardValue)
        {
            _moneyForAd += _rewardIncreaseValue;
            UpdateText();
        }
        else if (IsMultiplication())
        {
            _moneyForAd = (int)(_rewardMultiplier * _moneyForAd);
            UpdateText();
        }
    }
    private void UpdateText() => _textReward.text = $"Посмотри рекламу и получи {_moneyForAd} монет";
}
