using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class SpeedBoostPoint : MonoBehaviour
{
    [Header("������ ��� ������ ��������")]
    [HorizontalLine(color: EColor.Pink)]
    [SerializeField] private GameObject _model;
    [SerializeField] private float _boostCoff;
    [SerializeField] private float _boostCoolDownTime;
    [SerializeField] private float _boostTime;

    private bool _isCanBoost;
    private void Start()
    {
        Reload();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out bicycle_code player))
            Boost(player);
    }

    private void Boost(bicycle_code player)
    {
        if (_isCanBoost)
        {
            _isCanBoost = false;
            _model.SetActive(false);
            player.LegsPower *= _boostCoff;
            StartCoroutine(SpeedCor(player));
            Invoke(nameof(Reload), _boostCoolDownTime);
            Debug.Log("����� ������� ����� ��������");
        }
    }
    private IEnumerator SpeedCor(bicycle_code player)
    {
        yield return new WaitForSeconds(_boostTime);
        player.LegsPower /= _boostCoff;
        Debug.Log("������ �������� ����������");
        StopAllCoroutines();
    }
    private void Reload()
    {
        _isCanBoost = true;
        _model.SetActive(true);
        Debug.Log("���� �������� �� ����� ������������");
    }
}
