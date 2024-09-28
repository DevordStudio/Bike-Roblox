using UnityEngine;

public class GiftBoxCode : MonoBehaviour
{
    [SerializeField] private int _reward = 1000;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _particle;

    private bool _claimed;

    public BoxPointCode OwnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out bicycle_code player) && !_claimed)
        {
            _claimed = true;
            BankVolute.Instance.Money += _reward;
            Debug.Log($"Коробка открыта. Игрок получил {_reward} денег");
            OwnPoint.InvokeSpawn();
            PlayAnim();
        }
    }
    private void PlayAnim()
    {
        _animator.Play("OpenBox");
        _particle.SetActive(true);
        ParticleSystem PS = _particle.GetComponent<ParticleSystem>();
        PS.Play();
    }
}
