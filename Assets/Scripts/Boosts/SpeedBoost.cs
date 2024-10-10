using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SpeedBoost : MonoBehaviour
{
    [Tooltip("������ ����������")]
    [SerializeField] private bicycle_code _bikeCode;
    [Tooltip("����� �� ������� ���������� ��������")]
    [SerializeField] private float _speedBoostCoff = 2F;
    [SerializeField] private float _boostTime = 60F;

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
    }
    public void GetBoost()
    {
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
            else _timer += Time.deltaTime;
        }
    }
    private void OnDisable()
    {
        YandexGame.savesData.Is2XSpeed = _is2X;
        YandexGame.savesData.SpeedBoostTimer = _timer;
    }
}
