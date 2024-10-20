using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField, ShowIf(nameof(IsScale)), HideIf(nameof(IsScale))] private float _animScale = 1.3F;
    [SerializeField, ShowIf(nameof(IsSlide)), HideIf(nameof(IsNone))] private float _targetYPosition = -100;
    [SerializeField] private float _animTime = 0.6F;
    [SerializeField] private AnimType _animation;

    private float _tempPosY;
    private bool IsNone()
    {
        return _animation == AnimType.None;
    }
    private bool IsScale()
    {
        return _animation == AnimType.Scale;
    }
    private bool IsSlide()
    {
        return _animation == AnimType.DownSlide;
    }
    private RectTransform _rect;
    private bool _isMoving;

    private enum AnimType
    {
        None,
        Scale,
        DownSlide,
    }

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _tempPosY = _rect.anchoredPosition.y;
    }

    public void OnEnter()
    {
        switch (_animation)
        {
            case AnimType.None:
                return;
            case AnimType.Scale:
                ScaleOnEnter();
                break;
            case AnimType.DownSlide:
                MoveToTargetY();
                break;
        }
    }

    public void OnExit()
    {
        switch (_animation)
        {
            case AnimType.None:
                return;
            case AnimType.Scale:
                ScaleOnExit();
                break;
            case AnimType.DownSlide:
                MoveBackToOriginalY();
                break;
        }
    }

    private void MoveToTargetY()
    {
        _isMoving = true;
        _rect.DOAnchorPosY(_targetYPosition, _animTime / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _isMoving = false);
    }

    private void MoveBackToOriginalY()
    {
        _isMoving = true;
        _rect.DOAnchorPosY(_tempPosY, _animTime / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => _isMoving = false);
    }

    private void ScaleOnEnter() => transform.DOScale(_animScale, _animTime / 2);
    private void ScaleOnExit() => transform.DOScale(1F, _animTime / 2);

    private void OnDisable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        if (_isMoving)
        {
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _tempPosY);
        }
    }
}
