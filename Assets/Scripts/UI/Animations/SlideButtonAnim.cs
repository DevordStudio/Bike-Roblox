using UnityEngine;
using DG.Tweening;

public class SlideButtonAnim : MonoBehaviour
{
    [SerializeField] private RectTransform _target;
    [SerializeField] private float _targetPosY = -100f;
    [SerializeField] private float _animTime = 1f;

    private float _initialPosY;

    public void PlayAnimOn()
    {
        _target.DOAnchorPosY(_targetPosY, _animTime);
    }
    public void PlayAnimOff()
    {
        _target.DOAnchorPosY(_initialPosY, _animTime);
    }
}

