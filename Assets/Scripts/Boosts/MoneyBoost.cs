using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MoneyBoost : MonoBehaviour
{
    [SerializeField] private BankVolute _bank;
    [SerializeField] private float _boostTime = 60F;
    [SerializeField] private Image _relodImage;
    [SerializeField] private SoundController _soundController;

    private bool _isBoosted;
    private float _timer;

    public static event Action<bool> OnBoostChanged;

    private void Start()
    {
        _isBoosted = YandexGame.savesData.Is2XMoney;
        _bank.Is2X = _isBoosted;
        if (_isBoosted)
        {
            _timer = YandexGame.savesData.MoneyBoostTimer;
            OnBoostChanged?.Invoke(true);
        }
        YandexGame.RewardVideoEvent += GetBoost;
        //_relodImage.fillAmount = 0;
    }
    public void RewardShow()
    {
        if (_isBoosted) return;
        YandexGame.RewVideoShow(1);
    }
    private void GetBoost(int id)
    {
        if (id != 1) return;
        if (!_isBoosted)
        {
            _isBoosted = true;
            _bank.Is2X = _isBoosted;
            _soundController.PlayBoostSound();
            OnBoostChanged?.Invoke(true);
        }
    }
    private void Update()
    {
        if (_isBoosted)
        {
            if (_timer >= _boostTime)
            {
                _timer = 0;
                _isBoosted = false;
                _bank.Is2X = _isBoosted;
                OnBoostChanged?.Invoke(false);
            }
            else
            {
                _timer += Time.deltaTime;
                _relodImage.fillAmount = 1 - (_timer / _boostTime);
            }
        }
    }
    private void OnDisable()
    {
        YandexGame.savesData.Is2XMoney = _isBoosted;
        YandexGame.savesData.SpeedBoostTimer = _timer;
    }
    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= GetBoost;
    }
}