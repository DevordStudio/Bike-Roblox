using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MoneyBoost : MonoBehaviour
{
    [SerializeField] private BankVolute _bank;
    [SerializeField] private float _boostTime = 60F;

    private bool _isBoosted;
    private float _timer;

    private void Start()
    {
        _isBoosted = YandexGame.savesData.Is2XMoney;
        _bank.Is2X = _isBoosted;
        if (_isBoosted)
        {
            _timer = YandexGame.savesData.MoneyBoostTimer;
        }
    }
    public void GetBoost()
    {
        if (!_isBoosted)
        {
            _isBoosted = true;
            _bank.Is2X = _isBoosted;
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
            }
            else _timer += Time.deltaTime;
        }
    }
    private void OnDisable()
    {
        YandexGame.savesData.Is2XMoney = _isBoosted;
        YandexGame.savesData.SpeedBoostTimer = _timer;
    }
}