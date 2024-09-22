using DG.Tweening;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private float _animScale = 1.3F;
    [SerializeField] private float _animTime = 0.6F;
    public void OnEnter()
    {
        transform.DOScale(_animScale, _animTime / 2);
    }
    public void OnExit()
    {
        transform.DOScale(1F, _animTime / 2);
    }
    private void OnDisable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
