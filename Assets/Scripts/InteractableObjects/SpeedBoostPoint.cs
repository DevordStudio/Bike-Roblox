using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class SpeedBoostPoint : MonoBehaviour
{
    [Header("—крипт дл€ бонуса скорости")]
    [HorizontalLine(color: EColor.Pink)]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _boostCoff;
    [SerializeField] private float _boostCoolDownTime;
    [SerializeField] private float _boostTime;
    [SerializeField] private SoundController _sound;
    [SerializeField] private ParticleSystem _getParticles;
    [SerializeField] private GameObject _glow;

    public static event Action<bool> OnBoosted;

    private bool _isCanBoost;
    private void Start()
    {
        Reload();
        _sound = FindAnyObjectByType<SoundController>();

        SpeedBoost.OnConditionChanged += Check;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out bicycle_code player))
            Boost(player);
    }

    private void OnDestroy()
    {
        SpeedBoost.OnConditionChanged -= Check;
    }

    private void Check(bool isActive)
    {
        if (isActive)
        {
            StopAllCoroutines();
            _isCanBoost = false;
        }
        else
            _isCanBoost = true;
    }

    private void Boost(bicycle_code player)
    {
        if (_isCanBoost && !player.IsBoosted)
        {
            _getParticles.Play();
            _isCanBoost = false;
            _glow.SetActive(true);
            player.IsBoosted = true;
            player.LegsPower *= _boostCoff;
            _sound.PlayBoostSound();
            StartCoroutine(SpeedCor(player));
            OnBoosted?.Invoke(true);
            Invoke(nameof(Reload), _boostCoolDownTime);
            Debug.Log("»грок получил бонус скорости");
        }
    }
    private IEnumerator SpeedCor(bicycle_code player)
    {
        yield return new WaitForSeconds(0.1F);
        _model.SetActive(false);
        yield return new WaitForSeconds(_boostTime);
        player.LegsPower /= _boostCoff;
        player.IsBoosted = false;
        _glow.SetActive(false);
        OnBoosted?.Invoke(false);
        Debug.Log("Ёффект скорости закончилс€");
        StopAllCoroutines();
    }
    private void Reload()
    {
        _isCanBoost = true;
        _model.SetActive(true);
        Debug.Log("Ѕуст скорости на карте восстановлен");
    }
}
