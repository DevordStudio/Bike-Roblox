using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SpeedBoost : MonoBehaviour
{
    [Tooltip("Скрипт велосипеда")]
    [SerializeField] private bicycle_code _bikeCode;
    [Tooltip("Число на которое умножается скорость")]
    [SerializeField] private float _speedBoostCoff = 2F;
    [SerializeField] private float _boostTime = 60F;
    [SerializeField] private Image _reloadImage;
    [SerializeField] private GameObject _glow;
    [SerializeField] private SoundController _soundController;

    private bool _is2X;
    private float _timer;

    public static event Action<bool> OnConditionChanged;

    private void Start()
    {
        _is2X = YandexGame.savesData.Is2XSpeed;
        if (_is2X)
        {
            _bikeCode.LegsPower *= _speedBoostCoff;
            _timer = YandexGame.savesData.SpeedBoostTimer;
            GetBoost(2);
            StartCoroutine(SpeedBoostCoroutine());
        }
        YandexGame.RewardVideoEvent += GetBoost;
    }

    public void RewardShow()
    {
        if (_is2X) return;
        YandexGame.RewVideoShow(2);
    }

    private void GetBoost(int id)
    {
        if (id != 2) return;
        if (!_is2X && !_bikeCode.IsBoosted)
        {
            _bikeCode.LegsPower *= _speedBoostCoff;
            _bikeCode.IsBoosted = true;
            _is2X = true;
            _soundController.PlayBoostSound();
            OnConditionChanged?.Invoke(_is2X);
            _glow.SetActive(true);
            StartCoroutine(SpeedBoostCoroutine());
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        while (_is2X)
        {
            if (_timer >= _boostTime)
            {
                _is2X = false;
                _bikeCode.IsBoosted = false;
                _timer = 0;
                _bikeCode.LegsPower /= _speedBoostCoff;
                _reloadImage.fillAmount = 0;
                _glow.SetActive(false);
                OnConditionChanged?.Invoke(_is2X);
            }
            else
            {
                _timer += Time.deltaTime;
                _reloadImage.fillAmount = 1 - (_timer / _boostTime);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        YandexGame.savesData.Is2XSpeed = _is2X;
        YandexGame.savesData.SpeedBoostTimer = _timer;
    }

    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= GetBoost;
        if (SpeedBoostCoroutine() != null)
        {
            StopCoroutine(SpeedBoostCoroutine());
        }
    }
}