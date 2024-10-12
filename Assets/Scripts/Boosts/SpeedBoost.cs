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

    private bool _is2X;
    private float _timer;

    private void Start()
    {
        _is2X = YandexGame.savesData.Is2XSpeed;
        if (_is2X)
        {
            _bikeCode.LegsPower *= _speedBoostCoff;
            _timer = YandexGame.savesData.SpeedBoostTimer;
        }
        YandexGame.RewardVideoEvent += GetBoost;
        //_reloadImage.fillAmount = 0;
    }
    public void RewardShow()
    {
        if (_is2X) return;
        YandexGame.RewVideoShow(2);
    }
    private void GetBoost(int id)
    {
        if (id != 2) return;
        if (!_is2X)
        {
            _bikeCode.LegsPower *= _speedBoostCoff;
            _is2X = true;
        }
    }
    private void Update()
    {
        if (_is2X)
        {
            if (_timer >= _boostTime)
            {
                _is2X = false;
                _timer = 0;
                _bikeCode.LegsPower /= _speedBoostCoff;
            }
            else
            {
                _timer += Time.deltaTime;
                _reloadImage.fillAmount = 1 - (_timer / _boostTime);
            }
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
    }
}
